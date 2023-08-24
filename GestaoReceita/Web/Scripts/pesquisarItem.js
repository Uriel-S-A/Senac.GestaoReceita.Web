window.onload = function () {
    var url = "/Empresa/Pesquisar";
    var json = "";//$("#inputCNPJ").val();

    $.ajax({
        method: 'POST',
        url: url,
        data: { inputCNPJ: "teste" },
        success: (e) => {
            console.log(e)
        },
        error: (e) => {
            console.log('nao foi')
        }
    })

}