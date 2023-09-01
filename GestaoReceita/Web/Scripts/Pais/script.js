
//ver sobre sigla, se confirmar que não terá
//daí remover do front e back todos as "siglas".


//Button Create
function buttonCreate() {
    $('#createBtn').on('click', function () {
        var paisNome = $('#pais-nome').val();

        if (!paisNome) { 
            mostrarMensagemErro("Por favor, preencha a descrição do País.");
            return false;
        }

        var data = {
            descricaoPais: paisNome,
            
        };

        enviarRequisicaoCreate(data);
    });
}
function enviarRequisicaoCreate(data) {
    $.ajax({
        url: '/Pais/AdicionarPais',
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
}

//Button Editar
function buttonEditar() {
    $(".btn-modal-edit").on('click', function () {
        var rowSelected = this.parentNode.parentNode;

        var id = $(rowSelected).find("td.paisId").text();
        var paisNome = $(rowSelected).find("td.paisNome").text();
        
        $("#pais-nome-update").val(paisNome);

        $('#updateBtn').on('click', function () {
            var updatedPaisNome = $("#pais-nome-update").val();

            if (!updatedPaisNome) {
                mostrarMensagemErro("Por favor, preencha a descrição do País.");
                return;
            }

            var data = {
                id: id,
                descricaoPais: updatedPaisNome
            };

            enviarRequisicaoEditar(data);
        });
    });
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

function mostrarMensagemErro(mensagem) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: mensagem,
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
        success: function (response) {
            $('#confirmModal').modal('hide');
            if (response.mensagemRetorno != null && response.mensagemRetorno != "") {
                alert(response.mensagemRetorno)
            }
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


