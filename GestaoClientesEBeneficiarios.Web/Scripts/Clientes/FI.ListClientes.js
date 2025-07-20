
$(document).ready(function () {
    const $gridClientes = $('#gridClientes');

    if ($gridClientes.length) {
        $gridClientes.jtable({
            title: 'Clientes',
            paging: true,
            pageSize: 5,
            sorting: true,
            defaultSorting: 'Nome ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.ajax({
                        url: urlClienteList,
                        type: 'GET',
                        dataType: 'json',
                        data: jtParams
                    });
                }
            },
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '50%'
                },
                Email: {
                    title: 'Email',
                    width: '35%'
                },
                Alterar: {
                    title: '',
                    display: function (data) {
                        return '<button onclick="window.location.href=\'/cliente/editar/' + data.record.Id + '\'" class="btn btn-primary btn-sm">Alterar</button>';
                    }
                },
                Excluir: {
                    title: '',
                    display: function (data) {
                        return '<button class="btn btn-danger btn-sm" onclick="excluirCliente(' + data.record.Id + ')"><i class="fa fa-trash"></i> Excluir</button>';
                    }
                }
            }
        });

        $gridClientes.jtable('load');
    }
});

function excluirCliente(id) {
    if (confirm('Confirma exclusão?')) {
        $.post(urlClienteExcluir, { id: id })
            .done(function (response) {
                if (response.Result === 'OK') {
                    $('#gridClientes').jtable('load');
                } else {
                    alert('Erro: ' + response.Message);
                }
            })
            .fail(function () {
                alert('Erro ao excluir cliente');
            });
    }
}
