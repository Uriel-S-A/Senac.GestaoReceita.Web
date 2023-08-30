//function controle(opcao) {
//    var editar = false;

//    opcao == 'cadastro' ? editar = false : editar = true;

//    return editar;
//}

function editarIngrediente(button, opcao) {
    var editar = false;

    opcao == 'cadastro' ? editar = false : editar = true;

    if (editar == true) {
        document.getElementById("limparCamposButton").style.display = "none";

        var linhaItemClicado = button.closest("tr");

        var inputId = linhaItemClicado.querySelector("input[data-item='item_id']");
        var inputNome = linhaItemClicado.querySelector("input[data-item='item_nome']");
        var inputQuantidade = linhaItemClicado.querySelector("input[data-item='item_quantidade']")
        var inputUnidadeMedida = linhaItemClicado.querySelector("input[data-item='item_unidade_medida']");
        var inputPreco = linhaItemClicado.querySelector("input[data-item='item_preco']");
        var inputEmpresa = linhaItemClicado.querySelector("input[data-item='item_empresa']");

        var modalId = document.getElementById("idIngredienteModal");
        modalId.value = inputId.value;

        var modalNome = document.getElementById("modalNome");
        modalNome.value = inputNome.value;

        var modalQuantidade = document.getElementById("modalQuantidade");
        modalQuantidade.value = inputQuantidade.value;

        var modalMedida = document.getElementById("selectUnidadeMedida");
        modalMedida.value = inputUnidadeMedida.value;

        var modalEmpresa = document.getElementById("selectEmpresa");
        modalEmpresa.value = inputEmpresa.value;

        var modalPreco = document.getElementById("modalPreco");
        modalPreco.value = Number(inputPreco.value.replace(",", "."));
    }
    else {
        document.getElementById("limparCamposButton").style.display = "block";

        var modalId = document.getElementById("idIngredienteModal");
        modalId.value = '';

        var modalNome = document.getElementById("modalNome");
        modalNome.value = '';

        var modalQuantidade = document.getElementById("modalQuantidade");
        modalQuantidade.value = '';

        var modalMedida = document.getElementById("selectUnidadeMedida");
        modalMedida.value = '';

        var modalEmpresa = document.getElementById("selectEmpresa");
        modalEmpresa.value = '';

        var modalPreco = document.getElementById("modalPreco");
        modalPreco.value = '';
    }
}

function excluirIngrediente(button) {
    var linhaItemClicado = button.closest("tr");

    var Id = linhaItemClicado.querySelector("input[data-item='item_id']");
}