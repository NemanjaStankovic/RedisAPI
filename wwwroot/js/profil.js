let btn = document.getElementById("pocetna");
btn.onclick = (ev) => this.prebaciNaPocetnu();

function prebaciNaPocetnu()
{
    setTimeout(function () {
        window.location = 'https://localhost:7051/User/LogovanStranica';
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


            if (response.status == 200) {setTimeout(function () {
                    window.location = 'https://localhost:7051/User/LoginStranica';
                }, 1000)
            }
        });
}