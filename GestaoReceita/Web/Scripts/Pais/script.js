
function mostrarMensagemErro(mensagem, redirecionarURL) {
    Swal.fire({
        timer: 3000,
        icon: 'error',
        text: mensagem,
        showConfirmButton: false,
        allowOutsideClick: false
    }).then(() => {
        window.location.href = redirecionarURL;
    });
}

function mostrarMensagemSucesso(mensagem, redirecionarURL) {
    Swal.fire({
        icon: 'success',
        text: mensagem,
        timer: 2000,
        showConfirmButton: false,
        allowOutsideClick: false
    }).then(() => {
        window.location.href = redirecionarURL;
    });
}


//Button Create
function buttonCreate() {
    $('#createBtn').on('click', function () {
        var paisNome = $('#pais-nome').val();

        if (!paisNome) { 
            mostrarMensagemErro("Por favor, preencha a descrição do País.", "/Pais/Index");
            return false;
        }

        var data = {
            descricaoPais: paisNome,
            
        };

        $('#exampleModal').modal('hide');

        enviarRequisicaoCreate(data);
    });
}
function enviarRequisicaoCreate(data) {
    $.ajax({
        url: '/Pais/AdicionarPais',
        type: 'POST',
        data: data,
        success: function (response) {
            if (response.success === false) {
                mostrarMensagemErro("O País que deseja cadastrar já existe.", "/Pais/Index");
            } else {
                mostrarMensagemSucesso("País adicionado com sucesso.", "/Pais/Index");
            }                                    
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
                mostrarMensagemErro("Por favor, preencha a descrição do País.", "/Pais/Index");
                return;
            }

            var data = {
                id: id,
                descricaoPais: updatedPaisNome
            };

            $('#editModal').modal('hide');

            enviarRequisicaoEditar(data);
        });
    });
}
function enviarRequisicaoEditar(data) {
    $.ajax({
        url: '/Pais/EditarPais',
        type: 'POST',
        data: data,
        success: function (response) {           
            if (response.success === false) {
                mostrarMensagemErro("O País que deseja editar já existe.", "/Pais/Index");
            } else {
                mostrarMensagemSucesso("País alterado com sucesso.", "/Pais/Index");
            }
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


