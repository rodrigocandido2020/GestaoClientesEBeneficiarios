
$(document).ready(function () {

    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable({
            title: 'Clientes',
            paging: true, //Enable paging
            pageSize: 5, //Set page size (default: 10)
            sorting: true, //Enable sorting
            defaultSorting: 'Nome ASC', //Set default sorting
            actions: {
                listAction: urlClienteList,
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
                        return '<button onclick="window.location.href=\'' + urlAlteracao + '/' + data.record.Id + '\'" class="btn btn-primary btn-sm">Alterar</button>';
                    }
                },
                Excluir: {
                    title: '',
                    display: function (data) {
                        return '<button class="btn btn-danger btn-sm" onclick="excluirCliente(' + data.record.Id + ')"> <i class="fa fa-trash"></i> Excluir</button>';
                    }
                }
            }
        });

    //Load student list from server
    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable('load');
})

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
