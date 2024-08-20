let btn = document.getElementById("prijava");
btn.onclick = (ev) => {
    console.log("Button clicked!");
    this.prebaciNaLogin();
};

function prebaciNaLogin() {
    console.log("Function called!");
    setTimeout(function () {
        window.location = 'https://localhost:7051/User/LoginStranica';
    }, 1000);
}

// let btn1 = document.getElementById("odjava");
// btn1.onclick = (ev) => this.odjavi();

function odjavi() {
    console.log("nesto se desava");
    fetch("https://localhost:7051/User/Odjavi", {
        method: "GET"
    }).then(response => { setTimeout(function () {
                window.location = 'https://localhost:7051/User/LoginStranica';
            }, 1000)
    }); 
}
