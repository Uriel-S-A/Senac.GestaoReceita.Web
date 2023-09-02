// Função para editar o ingrediente selecionado
function editarIngrediente(button, opcao) {
    // seta a variável de controle para false
    var editar = false;

    // verifica se o botão clicado foi o de cadastro ou o de editar
    opcao == 'cadastro' ? editar = false : editar = true;

    // se foi o de editar, ele vai preencher os campos da modal com os dados do ingrediente selecionado
    if (editar) {
        // esconde o botão de limpar, pois ele não é necessário na tela de editar
        document.getElementById("limparCamposButton").style.display = "none";

        // passa os dados contidos na linha da tabela em que o botão está presente para uma variável
        var linhaItemClicado = button.closest("tr");

        // obtém os dados necessários da linha
        var inputId = linhaItemClicado.querySelector("input[data-item='item_id']");
        var inputNome = linhaItemClicado.querySelector("input[data-item='item_nome']");
        var inputQuantidade = linhaItemClicado.querySelector("input[data-item='item_quantidade']")
        var inputUnidadeMedida = linhaItemClicado.querySelector("input[data-item='item_unidade_medida']");
        var inputPreco = linhaItemClicado.querySelector("input[data-item='item_preco']");
        var inputEmpresa = linhaItemClicado.querySelector("input[data-item='item_empresa']");

        // passa os valores para a modal de editar para ela exibir os campos preenchidos
        var modalId = document.getElementById("idIngredienteModal");
        modalId.value = inputId.value;

        var modalNome = document.getElementById("modalNome");
        modalNome.value = inputNome.value;

        var modalQuantidade = document.getElementById("modalQuantidade");
        modalQuantidade.value = inputQuantidade.value;

        var modalMedida = document.getElementById("selectMedida");
        modalMedida.value = inputUnidadeMedida.value;

        var modalEmpresa = document.getElementById("selectEmpresa");
        modalEmpresa.value = inputEmpresa.value;

        var modalPreco = document.getElementById("modalPreco");
        modalPreco.value = Number(inputPreco.value.replace(",", "."));
    }
    else {// se não, ele vai chamar a tela de cadastro e todos os campos estarão vazios

        // o botão limpar aparece na tela
        document.getElementById("limparCamposButton").style.display = "block";

        // limpa todos os campos
        var modalId = document.getElementById("idIngredienteModal");
        modalId.value = '';

        var modalNome = document.getElementById("modalNome");
        modalNome.value = '';

        var modalQuantidade = document.getElementById("modalQuantidade");
        modalQuantidade.value = '';

        var modalMedida = document.getElementById("selectMedida");
        modalMedida.value = '';

        var modalEmpresa = document.getElementById("selectEmpresa");
        modalEmpresa.value = '';

        var modalPreco = document.getElementById("modalPreco");
        modalPreco.value = '';
    }
}

// Função para excluir o ingrediente selecionado
function excluirIngrediente(button) {
    // passa os dados contidos na linha da tabela em que o botão está presente para uma variável
    var linhaItemClicado = button.closest("tr");

    // obtém os dados necessários da linha
    var id = linhaItemClicado.querySelector("input[data-item='item_id']");
    var ingrediente = linhaItemClicado.querySelector("input[data-item='item_nome']");
    var empresa = linhaItemClicado.querySelector("input[data-item='item_empresa']");
    var unidadeMedida = linhaItemClicado.querySelector("input[data-item='item_unidade_medida']");

    // passa os valores para a modal de confirmação de excluir
    var inputId = document.getElementById("inputId");
    inputId.value = id.value;

    var spanIngrediente = document.getElementById("spanIngrediente");
    spanIngrediente.innerText = ingrediente.value;

    var spanEmpresa = document.getElementById("spanEmpresa");
    spanEmpresa.innerText = empresa.value;

    var spanUnidadeMedida = document.getElementById("spanUnidadeMedida");
    spanUnidadeMedida.innerText = unidadeMedida.value;
}