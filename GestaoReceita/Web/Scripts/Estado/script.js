
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


//Create
function buttonCreateEstado() {

    var paisNome = $('#paises').val();
    var estadoNome = $('#estado-nome').val();

    if (!estadoNome) {
        $('#createModal').modal('hide');
        mostrarMensagemErro("Por favor, preencha o campo Estado.", "/Estado/Index");
        return;
    }

    $('#createModal').modal('hide');

    var data = {
        descricaoPais: paisNome,
        descricaoEstado: estadoNome
    };

    enviarRequisicaoCreate(data);
}

function enviarRequisicaoCreate(data) {
    $.ajax({
        url: '/Estado/AdicionarEstado',
        type: 'POST',
        data: data,
        success: function (response) {
            if (response.success === false) {
                mostrarMensagemErro("O Estado que deseja criar já existe.", "/Estado/Index");
            } else {
                mostrarMensagemSucesso("Estado criado com sucesso.", "/Estado/Index");
            }
        },
        error: function (error) {
            window.Error('Erro ao enviar dados:', error);
        }
    });
}


//Update
function buttonUpdateEstado() {
    var idPais = $('#paises').val();
    var idEstado = $(".button-update-estado").attr('id');
    var nomeEstado = $(".button-update-estado").val()

    if (!nomeEstado) {
        $('#updateModal').modal('hide');
        mostrarMensagemErro("Por favor, preencha o campo Estado.", "/Estado/Index");
        return;
    }

    $('#updateModal').modal('hide');

    var data = {
        id: idEstado,
        descricaoEstado: nomeEstado,
        idPais: idPais
    };
    
    enviarRequisicaoUpdate(data);
}

function enviarRequisicaoUpdate(data) {
    $.ajax({
        url: '/Estado/EditarEstado',
        type: 'POST',
        data: data,
        success: function (response) {
            if (response.success === false) {
                mostrarMensagemErro("O Estado que deseja editar já existe.", "/Estado/Index");
            } else {
                mostrarMensagemSucesso("Estado alterado com sucesso.", "/Estado/Index");
            }
        },
        error: function (error) {
            window.Error('Erro ao enviar dados:', error);
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

            $('#confirmModal').modal('hide');

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
            if (response.mensagemRetorno != null && response.mensagemRetorno != "") {                                    
                var mensagem = (response.mensagemRetorno)
                mostrarMensagemSucesso(mensagem, "/Estado/Index");
            }            
        },
        error: function (error) {
            console.error('Erro ao enviar dados:', error);
        }
    });
}


//Carrega Modal Create
function ModalCreate() {
    $("#modal-create").click(function () {
        $.ajax({
            url: "/Estado/getModalPaises",
            type: "GET",
            success: function (data) {
                $("#exibe-modal-create").html(data);
                $("#createModal").modal("show");
            },
            error: function (error) {
                window.Error('Erro ao enviar dados:', error);
            }
        });
    });
}

//Carrega Modal Update
function ModalUpdate() {
    $(".btn-modal-edit").click(function () {

        var rowSelected = this.parentNode.parentNode;

        var paisIdTela = $(rowSelected).find("td.paisId").text();
        var estadoIdTela = $(rowSelected).find("td.estadoId").text();
        var estadoDescricaoTela = $(rowSelected).find("td.estadoDescricao").text();

        var data = {
            id: estadoIdTela,
            descricaoEstado: estadoDescricaoTela,
            idPais: paisIdTela
        };

        RequisicaoModalUpdate(data);
    });
}
function RequisicaoModalUpdate(data) {

    $.ajax({
        url: "/Estado/getPaisesAndById",
        type: "GET",
        data: data,
        success: function (data) {
            $("#exibe-modal-update").html(data);
            $("#updateModal").modal("show");
        },
        error: function (error) {
            window.Error('Erro ao enviar dados:', error);
        }
    });
}


function onLoad() {
    //Partial Views    
    ModalCreate();
    ModalUpdate();    
    buttonDeletar();
}

$(document).ready(function () {
    onLoad();
});


