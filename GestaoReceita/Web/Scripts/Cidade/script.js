function buttonCreate() {
    $('#createBtn').on('click', function () {
        var cidadeNome = $('#cidade-nome').val();
        var estadoNome = $('#estado-nome').val();

        if (!cidadeNome || !estadoNome) {
            swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'por favor, preencha todos os campos.',
            });
            return false;
        }

        var data = {
            Cidade: cidadeNome,
            Estado: estadoNome,

        };

        $.ajax({
            url: '/Cidade/AdicionarNovoCidade',
            type: 'POST',
            data: data,
            success: function () {
                console.log('Dados enviados com sucesso!');
                $('#exampleModal').modal('hide');
                window.location.href = "/Cidade/Index";
            },
            error: function (error) {
                console.error('Erro ao enviar dados:', error);
            }
        });
    });
}

//Button Update
function buttonEditar() {
    $(".btn-modal-edit").on('click', function () {
        //pegando valores da linha selecionada.

        var rowSelected = this.parentNode.parentNode;

        var id = $(rowSelected).find("td.cidadeId").text();
        var cidadeNome = $(rowSelected).find("td.cidadeNome").text();
        var estadoNome = $(rowSelected).find("td.estadoNome").text();

        preencherCamposEdicao(id, cidadeNome, estadoNome);
    });
}

function preencherCamposEdicao(id, cidadeNome, estadoNome) {
    //preenchendo valores na modal.
    $("#cidade-nome-update").val(cidadeNome);
    $("#estado-nome-update").val(estadoNome);

    //quando clicar em update.
    $('#updateBtn').on('click', function () {
        //quando validar campos, deve vir diferente de falso.
        if (!validarCamposEdicao()) {
            return false;
        }

        var data = {
            Id: id,
            Cidade: $('#cidade-nome-update').val(),
            Estado: $('#estado-nome-update').val(),
        };

        enviarRequisicaoEditar(data);
    });
}

function validarCamposEdicao() {
    //valores que estão na modal
    var cidadeNomeUpdate = $("#cidade-nome-update").val();
    var estadoNomeUpdate = $("#estado-nome-update").val();

    if (!cidadeNomeUpdate) {

        //REMOVER FORMATO DE ERRO, FAZER EXIBIÇÃO VIA MODAL(mensagem de erro com modal de cor vermelha ou ver sobre)

        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Por favor, preencha todos os campos.',
        });
        return false;
    }

    return true;
}

function enviarRequisicaoEditar(data) {

    $.ajax({
        url: '/Cidade/EditarCidade',
        type: 'POST',
        data: data,
        success: function () {
            $('#exampleModal').modal('hide');
            window.location.href = "/Cidade/Index";
        },
        error: function (error) {
            console.error('Erro ao enviar dados:', error);
        }
    });
}

//Button Delete

function buttonDeletar() {
    $('.btn-modal-delete').on('click', function () {

        var rowSelected = this.parentNode.parentNode;
        var id = $(rowSelected).find("td.cidadeId").text();

        $("#btnConfirmDelete").on("click", function () {
            var data = {
                Id: id
            }

            enviarRequisicaoDeletar(data);
        })
    });
}


function enviarRequisicaoDeletar(data) {
    $.ajax({
        url: '/Cidade/DeletarCidade',
        type: 'POST',
        data: data,
        success: function () {
            $('#confirmModal').modal('hide');
            window.location.href = "/Cidade/Index";
        },
        error: function (error) {
            console.error('Erro ao enviar dados:', error);
        }
    });
}

function onLoad() {
    buttonCreate();
    buttonEditar();
    buttonDeletar();
}

$(document).ready(function () {
    onLoad();
});