let btn = document.getElementById("profil");
btn.onclick = (ev) => this.prebaciNaProfil();

function prebaciNaProfil()
{
    setTimeout(function () {
        window.location = 'https://localhost:7051/User/ProfilStranica';
    }, 1000);
}

let btn1 = document.getElementById("odjava");
btn1.onclick = (ev) => {
    this.prebaciNaLogin();
};

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

function dodajUOmiljenje()
{
    let nazivIgre = document.getElementById("h5el").innerHTML;

    console.log(nazivIgre);
}