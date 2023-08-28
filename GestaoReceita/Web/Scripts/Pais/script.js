//java será organizado corretamente

//Button Create

function buttonCreate() {
    $('#createBtn').on('click', function () {
        var paisNome = $('#pais-nome').val();
        var paisSigla = $('#pais-sigla').val();

        if (!paisNome || !paisSigla) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Por favor, preencha todos os campos.',
            });
            return false;
        }

        var data = {
            PaisNome: paisNome,
            PaisSigla: paisSigla
        };

        $.ajax({
            url: '/Pais/AdicionarNovoPais',
            type: 'POST',
            data: data,
            success: function () {
                console.log('Dados enviados com sucesso!');
                $('#exampleModal').modal('hide');
                window.location.href = "/Pais/Index";
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

        var id = $(rowSelected).find("td.paisId").text();
        var paisNome = $(rowSelected).find("td.paisNome").text();
        var paisSigla = $(rowSelected).find("td.paisSigla").text();

        preencherCamposEdicao(id, paisNome, paisSigla);
    });
}

function preencherCamposEdicao(id, paisNome, paisSigla) {
    //preenchendo valores na modal.
    $("#pais-nome-update").val(paisNome);
    $("#pais-sigla-update").val(paisSigla);

    //quando clicar em update.
    $('#updateBtn').on('click', function () {
        //quando validar campos, deve vir diferente de falso.
        if (!validarCamposEdicao()) {
            return false;
        }

        var data = {
            Id: id,
            PaisNome: $('#pais-nome-update').val(),
            PaisSigla: $('#pais-sigla-update').val()
        };

        enviarRequisicaoEditar(data);
    });
}

function validarCamposEdicao() {
    //valores que estão na modal
    var paisNomeUpdate = $("#pais-nome-update").val();
    var paisSiglaUpdate = $("#pais-sigla-update").val();

    if (!paisNomeUpdate || !paisSiglaUpdate) {

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
        url: '/Pais/EditarPais',
        type: 'POST',
        data: data,
        success: function () {
            $('#exampleModal').modal('hide');
            window.location.href = "/Pais/Index";
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
        var id = $(rowSelected).find("td.paisId").text();

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
        url: '/Pais/DeletarPais',
        type: 'POST',
        data: data,
        success: function () {
            $('#confirmModal').modal('hide');
            window.location.href = "/Pais/Index";
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


