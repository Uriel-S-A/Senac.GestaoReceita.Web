

function buttonCreate() {
    $('#createBtn').on('click', function () {
        var estadoPais = $('#estado-pais').val();
        var estadoNome = $('#estado-nome').val();
        var estadoSigla = $('#estado-sigla').val();

        if (!estadoPais || !estadoNome || !estadoSigla) {
            swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'por favor, preencha todos os campos.',
            });
            return false;
        }

        var data = {
            Pais : estadoPais,
            Estado: estadoNome,
            Sigla: estadoSigla
        };

        $.ajax({
            url: '/Estado/AdicionarNovoEstado',
            type: 'POST',
            data: data,
            success: function () {
                console.log('Dados enviados com sucesso!');
                $('#exampleModal').modal('hide');
                window.location.href = "/Estado/Index";
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

        var id = $(rowSelected).find("td.estadoId").text();
        var estadoPais = $(rowSelected).find("td.estadoPais").text();
        var estadoNome = $(rowSelected).find("td.estadoNome").text();
        var estadoSigla = $(rowSelected).find("td.estadoSigla").text();

        preencherCamposEdicao(id, estadoPais, estadoNome, estadoSigla);
    });
}

function preencherCamposEdicao(id, estadoPais, estadoNome, estadoSigla) {
    //preenchendo valores na modal.
    $("#estado-pais-update").val(estadoPais);
    $("#estado-nome-update").val(estadoNome);
    $("#estado-sigla-update").val(estadoSigla)

    //quando clicar em update.
    $('#updateBtn').on('click', function () {
        //quando validar campos, deve vir diferente de falso.
        if (!validarCamposEdicao()) {
            return false;
        }

        var data = {
            Id: id,
            Pais: $('#estado-pais-update').val(),
            Estado: $('#estado-nome-update').val(),
            Sigla: $('#estado-sigla-update').val()
        };

        enviarRequisicaoEditar(data);
    });
}

function validarCamposEdicao() {
    //valores que estão na modal
    var estadoPaisUpdate = $("#estado-pais-update").val();
    var estadoNomeUpdate = $("#estado-nome-update").val();
    var estadoSiglaUpdate = $("#estado-sigla-update").val();

    if (!estadoPaisUpdate || !estadoNomeUpdate || !estadoSiglaUpdate) {

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
        url: '/Estado/EditarEstado',
        type: 'POST',
        data: data,
        success: function () {
            $('#exampleModal').modal('hide');
            window.location.href = "/Estado/Index";
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
        var id = $(rowSelected).find("td.estadoId").text();

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
        url: '/Estado/DeletarEstado',
        type: 'POST',
        data: data,
        success: function (response) {
            $('#confirmModal').modal('hide');
            if (response.mensagemRetorno != null && response.mensagemRetorno != "") {
                alert(response.mensagemRetorno)
            }
            window.location.href = "/Estado/Index";
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


