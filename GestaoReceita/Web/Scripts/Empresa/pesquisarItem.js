window.onload = function () {
    var obj = {
        emails: 'teste'
    }

    var json = JSON.stringify(obj)

    $.ajax({
        type: 'POST',
        url: '/Empresa/getEmpresa',
        data: json,
        contentType: 'application/json',
        success: (e) => {
            let inputCNPJ = document.getElementById("inputCNPJ").value;
            console.log(inputCNPJ);
            console.log(e);
            //confirmarCadastro(e);
        },
        error: (e) => {
            console.log('nao foi')
        }
    })
}

function confirmarCadastro(listaEmpresas) {

    let inputCNPJ = document.getElementById("inputCNPJ").value;
    let empresasCadastrada = false;

    for (let i = 0; i < listaEmpresas.lenght; i++) {

        if (listaEmpresas[i] === inputCNPJ) {
            empresasCadastrada = true;
        }
    }

    if (empresasCadastrada === false) {
        //abri uma modal
    }
}


function setIdDelete(id) {
    document.getElementById("idItemDeletar").value = id;
}