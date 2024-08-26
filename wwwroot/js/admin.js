let btn1 = document.getElementById("odjava");
btn1.onclick = (ev) => {
    this.prebaciNaLogin();
};

gde2.className = "svasta";
gde3.className = "svasta";

function prebaciNaLogin() {
    fetch("https://localhost:7051/User/Odjavi", {
            method: "GET"
        }).then(response => {

            if (response.status == 200) 
            {
                setTimeout(function () {
                    window.location = 'https://localhost:7051/User/LoginStranica';
                }, 1000)
            }

        });
}

let btn2 = document.getElementById("dugme");
btn2.onclick = (ev) => {
    this.dodajIgru();
}

function dodajIgru()
{
    let n = document.getElementById('naziv').value;
                                        
    let c = document.getElementById('cena').value;

    let s = document.getElementById('onSale').checked;

    let nc = document.getElementById('novaCena').value;
    console.log(n + c + s +nc);

    if(n == null || c == null || s == null || nc || null)
    {
        alert("POLJA NE SMEJU BITI PRAZNA");
    }

    fetch("https://localhost:7051/User/DodajNovuIgricu/" + n + "/" + c + "/" + s + "/" + nc, {
        method: "POST"
    }).then(response => {
        if (response.status == 200) 
            {
                location.reload();
            }
        else{
            alert("GRESKA");
        }
    });
}

let btn3 = document.getElementById("dugmeUpdate");
btn3.onclick = (ev) => {
    this.izmeniIgru();
}

function izmeniIgru()
{
    let id = document.getElementById('nazivLbl').innerHTML;

    let n = document.getElementById('nazivUpdate').value;
                                        
    let c = document.getElementById('cenaUpdate').value;

    let s = document.getElementById('onSaleUpdate').checked;

    let nc = document.getElementById('novaCenaUpdate').value;
    console.log(n + c + s +nc);

    if(n == null || c == null || s == null || nc || null)
    {
        alert("POLJA NE SMEJU BITI PRAZNA");
    }

    fetch("https://localhost:7051/User/IzmeniIgricu/" + id + "/" + n + "/" + c + "/" + s + "/" + nc, {
        method: "PUT"
    }).then(response => {
        if (response.status == 200) 
            {
                location.reload();
            }
        else{
            alert("GRESKA");
        }
    });
}