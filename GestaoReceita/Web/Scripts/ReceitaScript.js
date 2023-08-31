window.onload = function () {


    var receitasTr = [];
    var randomString;


    var url = "/Receita/receitasBuscar";
    $.get(url, null, function (data) {
        console.log(data)
    });

     var inputElement = document.getElementById("verificacao");
        inputElement.addEventListener("paste", function (event) {
            event.preventDefault();
            alerte("Ação de colagem foi bloqueada.", "warning");
        });

    $(".test").on("click", function (e) {
        $(this).toggleClass('bottomhover');
        const divPai = this.parentElement;
        const divPai1 = divPai.parentElement;
        if ($(this).hasClass('bottomhover')) {
            receitasTr.push(divPai1);
        } else {
            const indexToRemove = receitasTr.indexOf(divPai1);
            if (indexToRemove !== -1) {
                receitasTr.splice(indexToRemove, 1);
            }
        }
        console.log(receitasTr);
        toggleMenuContagemDelete();
    });

    $("#EXCLUIR").on("click", function (e) {
        var divsComClasse = document.querySelectorAll(".bottomhover");

        divsComClasse.forEach(function (div) {
            console.log(div);
        });
        sla = receitasTr.length;

        var resultado = obterPrimeirasTds(receitasTr);
        var trs = receitasTr

        console.log(resultado)
        console.log(receitasTr)
        i = 0

        var tbody = document.getElementById("conteudoTbody");
        var numeroDeLinhas = tbody.rows.length;

        var url = "/Receita/Excluir";
        $.post(url, { listaId: resultado }, function (data) {
            if (data == "False") {
                alerte("Não foi possivel excluir as receitas", "warning")
            } else {
                alerte("Receitas foram excluidas")
                while (i <= sla) {

                    if (numeroDeLinhas > 0) {
                        console.log(trs[i])
                        trs[i].remove();
                        $(".menu").removeClass("active");
                        i++;
                        numeroDeLinhas--;
                    } else {
                        document.getElementById("tables").remove();

                        const elemento = document.getElementById('vazio');


                        var container = document.querySelector('#vazio');


                        var novoHTML = `
                            <div class="sla12">
                                <img src="../Imagens/OIG-removebg-preview.png"/>
                            </div>
                            `;

                        container.innerHTML = novoHTML;

                        var myButton = document.getElementById("excluirReceita");
                        myButton.disabled = true;


                        myButton.style.backgroundColor = "#ccc";
                        myButton.style.color = "#666";
                        myButton.style.cursor = "not-allowed";
                        myButton.style.border = "none";

                        elemento.style.removeProperty('overflow');
                        elemento.style.removeProperty('height');
                    }

                }
                if (sla > 1) {
                    alerte(sla + " receitas foram excluidas")
                } else {
                    alerte("Uma receita foi excluida")
                }
            }
        })
        


        receitasTr = [];


    });

    $("#FecharAlert").on("click", function (e) {

        $(this).toggleClass("aaaa");

    });

    $("#excluirReceita").on("click", function (e) {
        document.getElementById('verificacao').value = '';
        var url = "/Receita/GenerateToken";
        $.get(url, null, function (data) {
            a = document.getElementById('testSocial');
            sessionStorage.setItem("codigoValidacao", data);
            a.innerText = data;
            $(a).css('font-weight', 'bold')
            $(a).css('font-size', '30px')
            $(a).css('user-select', 'none')
        });

    });

    $("#EXCLUIRTUDO").on("click", function (e) {
        var botão = this
        botão.disabled = true;

        var url = "/Receita/submeterCodigo";
        var digitado = $("#verificacao").val();

        console.log(sessionStorage.getItem("codigoValidacao"))
        console.log(digitado)

        var listaId = pegarValoresDaColuna(0);
        console.log(listaId)

        $.post(url, { digitado: digitado }, function (data) {
            switch (data) {
                case "Codigo correto":
                    var url = "/Receita/excluirTudo";
                    $.post(url, { listaId: listaId }, function (data) {

                        if (data == "False") {

                            alerte("Erro ao limpar a lista", "warning");
                            console.log("Lista vazia");

                            
                            botão.disabled = false;

                        }
                        else {
                            botão.style.backgroundColor = "#ccc";
                            botão.style.color = "#666";
                            botão.style.cursor = "not-allowed";
                            botão.style.border = "none";


                            alerte("Receitas apagadas", "sucess");
                            closeModal();
                            document.getElementById("tables").remove();

                            const elemento = document.getElementById('vazio');


                            var container = document.querySelector('#vazio');


                            var novoHTML = `
                            <div class="sla12">
                                <img src="../Imagens/OIG-removebg-preview.png"/>
                            </div>
                            `;

                            container.innerHTML = novoHTML;

                            var myButton = document.getElementById("excluirReceita");
                            myButton.disabled = true;


                            myButton.style.backgroundColor = "#ccc";
                            myButton.style.color = "#666";
                            myButton.style.cursor = "not-allowed";
                            myButton.style.border = "none";

                            elemento.style.removeProperty('overflow');
                            elemento.style.removeProperty('height');
                        }
                    });
                    break;
                case "Codigo não bate":
                    alerte("Erro no codigo", "warning");
                    console.log("Erro no codigo");
                    
                    botão.disabled = false;
                    break;
                case "Codigo vazio":
                    alerte("Insira o codigo", "warning");
                    console.log("Insira o codigo");
                    botão.disabled = false;
                    break;
            }
        });

        /**/

    });
};




function toggleMenuContagemDelete() {
    var isOpen = $(".test").hasClass('bottomhover');

    if (isOpen) {
        if (!$(".menu").hasClass("active")) {
            $(".menu").addClass("active");
        }
    } else {
        $(".menu").removeClass("active");
    }
}
function onClick(e) {
    e.preventDefault();
    grecaptcha.enterprise.ready(async () => {
        const token = await grecaptcha.enterprise.execute('6LdD-78nAAAAAFsoghhAlZeIJaJuapMXSopJVt_K', { action: 'LOGIN' });
    });
}


function alerte(mensagem, tipo) {

    var alertElement = document.getElementById("meuAlerta");

    alertElement.classList.add("aaaa");

    var audioWarning = new Audio('../Audio/notification-sound-7062.mp3');
    var audioSucess = new Audio("../Audio/short-success-sound-glockenspiel-treasure-video-game-6346.mp3")


    alertElement.classList.remove("alert-success", "alert-danger", "alert-warning", "alert-info");


    switch (tipo) {
        case "success":
            alertElement.classList.add("alert", "alert-success");
            audioSucess.play();
            break;
        case "error":
            alertElement.classList.add("alert", "alert-danger");
            audioWarning.play();
            break;
        case "warning":
            alertElement.classList.add("alert", "alert-warning");
            audioWarning.play();
            break;
        default:
            alertElement.classList.add("alert", "alert-info");
            audioWarning.play();
            break;
    }
    alertElement.classList.add("fade");
    alertElement.classList.remove("aaaa");


    alertElement.innerHTML = mensagem;

    setTimeout(function () {
        alertElement.classList.add("aaaa");
        alertElement.innerHTML = '';
    }, 8000);

}

function closeModal() {
    var myModal = document.getElementById('staticBackdrop');
    var modal = bootstrap.Modal.getInstance(myModal);
    document.querySelector('.modal-backdrop').remove();
    document.body.classList.remove('modal-open');
    document.body.style.overflow = 'auto';
    modal.hide();
}

function iniciarContador() {
    let contador = 30;
    const tempoDiv = document.getElementById("tempo");

    intervalId = setInterval(function () {
        tempoDiv.textContent = contador;
        contador--;

        if (contador < 0) {
            clearInterval(intervalId);
            tempoDiv.textContent = "Tempo acabou!";
        }
    }, 1000);
}

/*function pararContador() {
    clearInterval(intervalId);
}*/

function pegarValoresDaColuna(numeroColuna) {
    var tabela = document.getElementById("tables");
    var valoresColuna = [];

    for (var i = 1; i < tabela.rows.length; i++) {
        var celula = tabela.rows[i].cells[numeroColuna];
        valoresColuna.push(celula.innerHTML);
    }

    return valoresColuna;
}

function obterPrimeirasTds(receitasTr) {
    var receitasId = []; 

    receitasTr.forEach(tr => {
        const primeiraTd = tr.querySelector('td'); 
        if (primeiraTd) {
            receitasId.push(primeiraTd.textContent); 
        }
    });

    return receitasId; 
}

check = true

function procurarValor() {
    const chaveProcurada = document.getElementById("chaveInput").value.toLowerCase();
    const tbody = document.getElementById("conteudoTbody");

    console.log(chaveProcurada)

    for (let i = 0; i < tbody.rows.length; i++) {
        const Nomereceita = tbody.rows[i].cells[1].textContent.toLowerCase(); 
        console.log(Nomereceita.includes(chaveProcurada))

        const Idreceita = tbody.rows[i].cells[0].textContent.toLowerCase(); 
        console.log(Idreceita.includes(chaveProcurada))

        const PreçoReceita = tbody.rows[i].cells[2].textContent.toLowerCase(); 
        console.log(Idreceita.includes(chaveProcurada))



        if (Nomereceita.includes(chaveProcurada) || PreçoReceita.includes(chaveProcurada) || Idreceita.includes(chaveProcurada)) {
            tbody.rows[i].style.display = "table-row";
            console.log("Opa")
            var container = document.querySelector('#vazio');

            container.style.display = "flex";

            check = true;

            var myButton = document.getElementById("excluirReceita");
            myButton.disabled = false;

            var container = document.querySelector('#defaut');
            container.innerHTML = "";

        } else {
            tbody.rows[i].style.display = "none";
            console.log("Oi")


            
        }

        
    }

    var tabela = document.getElementById("conteudoTbody");
    var linhas = tabela.getElementsByTagName("tr");

    var estiloDesejado = "display: none;";

    var todasLinhasTemEstilo = true;


    for (i = 0; i < linhas.length; i++) {
        var estiloLinha = linhas[i].getAttribute("style");

        if (estiloLinha !== estiloDesejado) {
            todasLinhasTemEstilo = false;
            break;
        }
    }

    if (todasLinhasTemEstilo) {
        var container = document.querySelector('#vazio');

        container.style.display = "none";

        var novoHTML = `
                            <div class="sla12">
                                <img src="../Imagens/OIG-removebg-preview.png"/>
                                <p style="text-align: center;">Não foi possivel achar a receita</p>
                            </div>
                            `;



        var novaDiv = document.createElement('div');
        novaDiv.innerHTML = novoHTML;

        var container = document.querySelector('#defaut');


        if (check == true) {
            container.appendChild(novaDiv);
        }


        var myButton = document.getElementById("excluirReceita");
        myButton.disabled = true;




        check = false;
    } else {
        console.log("Algumas linhas não têm o estilo desejado.");
    }
}

