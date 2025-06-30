
$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Id').val(obj.Id);
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
        $('#formCadastro #CPF').val(obj.CPF);
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "Id": $(this).find("#Id").val(),
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "CPF": $(this).find("#CPF").val()
            },
            error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success:
            function (r) {
                ModalDialog("Sucesso!", r)
                $("#formCadastro")[0].reset();                                
                window.location.href = urlRetorno;
            }
        });
    })
    
})

$(document).ready(function () {
    $('#btnSalvarBeneficiario').on('click', function () {
        var nome = $('#inputNome').val().trim();
        var cpf = $('#inputCPF').val().trim();
        var idCliente = $('#Id').val();

        if (!nome || !cpf || !idCliente) {
            alert("Preencha todos os campos.");
            return;
        }

        $.ajax({
            type: 'POST',
            url: '/Beneficiario/Incluir',
            data: {
                Nome: nome,
                CPF: cpf,
                IdCliente: idCliente
            },
            success: function (response) {
                if (response.Result === "OK") {
                    alert(response.Message);
                    $('#inputNome').val('');
                    $('#inputCPF').val('');
                    abrirModalBeneficiarios(idCliente);
                } else {
                    alert(response.Message);
                }
            },
            error: function () {
                alert("Erro ao salvar o beneficiário.");
            }
        });

    });
});

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}


function abrirModalBeneficiarios() {
    var idCliente = $('#inputIdCliente').val();
    if (!idCliente) {
        alert("ID do cliente não está definido.");
        return;
    }

    $('#modalBeneficiarios').modal('show');

    $.ajax({
        type: 'POST',
        url: '/Beneficiario/Listar',
        data: { idCliente: idCliente },
        success: function (response) {
            if (response.Result === "OK") {
                // Preencher a tabela dinamicamente
                preencherTabela(response.Records);
            } else {
                alert(response.Message);
            }
        },
        error: function () {
            alert("Erro ao carregar os beneficiários.");
        }
    });
}

function preencherTabela(beneficiarios) {
    var tbody = $("#tabelaBeneficiarios tbody");
    tbody.empty();

    if (beneficiarios.length === 0) {
        tbody.append(`<tr><td colspan="4" class="text-center">Nenhum beneficiário encontrado.</td></tr>`);
        return;
    }

    beneficiarios.forEach(function (b) {
        var linha = `
            <tr>
                <td>${b.Nome}</td>
                <td>${b.CPF}</td>
                <td>
                    <button class="btn btn-sm btn-primary btnAlterar" data-id="${b.Id}">Alterar</button>
                    <button class="btn btn-sm btn-danger btnExcluir" data-id="${b.Id}">Excluir</button>
                </td>
            </tr>
        `;
        tbody.append(linha);
    });
}


$(document).on('click', '.btnAlterar', function () {
    var id = $(this).data('id');

    // Buscar os dados do beneficiário na tabela ou via AJAX
    // Supondo que você já tenha os dados na tabela, pode pegar direto:

    var row = $(this).closest('tr');
    var nome = row.find('td').eq(0).text();
    var cpf = row.find('td').eq(1).text();
    var idCliente = $('#inputIdCliente').val(); // do modal principal

    // Preencher campos do modal alterar
    $('#alterarId').val(id);
    $('#alterarNome').val(nome);
    $('#alterarCPF').val(cpf);
    $('#alterarIdCliente').val(idCliente);

    // Abrir modal
    $('#modalAlterarBeneficiario').modal('show');
});


$(document).on('click', '.btnExcluir', function () {
    var id = $(this).data('id');

    if (confirm("Tem certeza que deseja excluir este beneficiário?")) {
        $.ajax({
            type: 'POST',
            url: '/Beneficiario/Excluir',
            data: { id: id },
            success: function (response) {
                alert(response);
                // Recarrega a lista após exclusão
                abrirModalBeneficiarios();
            },
            error: function () {
                alert("Erro ao excluir o beneficiário.");
            }
        });
    }
});

$('#btnConfirmarAlteracao').on('click', function () {
    var id = $('#alterarId').val();
    var nome = $('#alterarNome').val().trim();
    var cpf = $('#alterarCPF').val().trim();
    var idCliente = $('#Id').val();

    if (!nome || !cpf) {
        alert("Preencha nome e CPF.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Beneficiario/Alterar',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({
            Id: id,
            Nome: nome,
            CPF: cpf,
            IdCliente: idCliente
        }),
        success: function (response) {
            alert(response);
            $('#modalAlterarBeneficiario').modal('hide');
            abrirModalBeneficiarios(); // Recarrega a lista atualizada
        },
        error: function (xhr) {
            alert(xhr.responseJSON || "Erro ao alterar beneficiário.");
        }
    });
});
