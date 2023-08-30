function editarIngrediente(button) {

    document.getElementById("limparCamposButton").style.display = "none";

    var linhaItemClicado = button.closest("tr");

    var inputId = linhaItemClicado.querySelector("input[data-item='item_id']");
    var inputNome = linhaItemClicado.querySelector("input[data-item='item_nome']");
    var inputQuantidade = linhaItemClicado.querySelector("input[data-item='item_quantidade']")
    var inputUnidadeMedida = linhaItemClicado.querySelector("input[data-item='item_unidade_medida']");
    var inputPreco = linhaItemClicado.querySelector("input[data-item='item_preco']");
    var inputEmpresa = linhaItemClicado.querySelector("input[data-item='item_empresa']");

    /*console.log(`Id: ${inputId.value} Nome: ${inputNome.value} Quantidade: ${inputQuantidade.value} Unidade Medida: ${inputUnidadeMedida.value} Preço: ${inputPreco.value} Empresa: ${inputEmpresa.value}`);*/

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
    modalPreco.value = inputPreco.value;
}