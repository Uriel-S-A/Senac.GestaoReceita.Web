
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
function buttonCreateCidade() {

    var estadoNome = $('#estados').val();
    var cidadeNome = $('#cidade-nome').val();

    if (!cidadeNome) {
        $('#createModal').modal('hide');
        mostrarMensagemErro("Por favor, preencha o campo Cidade.", "/Cidade/Index");
        return;
    }

    $('#createModal').modal('hide');

    var data = {
        descricaoCidade: cidadeNome,
        descricaoEstado: estadoNome
    };

    enviarRequisicaoCreate(data);
}

function enviarRequisicaoCreate(data) {
    $.ajax({
        url: '/Cidade/AdicionarCidade',
        type: 'POST',
        data: data,
        success: function (response) {
            if (response.success === false) {
                mostrarMensagemErro("A Cidade que deseja criar já existe.", "/Cidade/Index");
            } else {
                mostrarMensagemSucesso("Cidade criada com sucesso.", "/Cidade/Index");
            }
        },
        error: function (error) {
            window.Error('Erro ao enviar dados:', error);
        }
    });
}

function buttonUpdateEstado() {
    var idEstado = $('#estados').val();
    var idCidade = $(".button-update-cidade").attr('id');
    var nomeCidade = $(".button-update-cidade").val()

    if (!nomeCidade) {
        $('#updateModal').modal('hide');
        mostrarMensagemErro("Por favor, preencha o campo Cidade.", "/Cidade/Index");
        return;
    }

    $('#updateModal').modal('hide');

    var data = {
        id: idCidade,
        descricaoCidade: nomeCidade,
        idEstado: idEstado
    };

    enviarRequisicaoUpdate(data);
}

function enviarRequisicaoUpdate(data) {
    $.ajax({
        url: '/Cidade/EditarCidade',
        type: 'POST',
        data: data,
        success: function (response) {
            if (response.success === false) {
                mostrarMensagemErro("A Cidade que deseja editar já existe.", "/Cidade/Index");
            } else {
                mostrarMensagemSucesso("Cidade alterada com sucesso.", "/Cidade/Index");
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
        var id = $(rowSelected).find("td.cidadeId").text();

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
        url: '/Cidade/DeletarCidade',
        type: 'POST',
        data: data,
        success: function (response) {
            $('#confirmModal').modal('hide');
            if (response.mensagemRetorno != null && response.mensagemRetorno != "") {
                var mensagem = (response.mensagemRetorno)
                mostrarMensagemSucesso(mensagem, "/Cidade/Index");       
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
            url: "/Cidade/getModalEstados",
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

        var estadoIdTela = $(rowSelected).find("td.estadoId").text();
        var cidadeIdTela = $(rowSelected).find("td.cidadeId").text();
        var estadoDescricaoTela = $(rowSelected).find("td.descricaoCidade").text();

        var data = {
            id: cidadeIdTela,
            descricaoCidade: estadoDescricaoTela,
            idEstado: estadoIdTela
        };

        RequisicaoModalUpdate(data);
    });
}
function RequisicaoModalUpdate(data) {

    $.ajax({
        url: "/Cidade/getEstadosAndById",
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
    ModalCreate();
    ModalUpdate();
    buttonDeletar();
}

$(document).ready(function () {
    onLoad();
});