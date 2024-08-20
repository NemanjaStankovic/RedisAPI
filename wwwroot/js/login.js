let btn = document.getElementById("login");
btn.onclick = (ev) => this.login();

function login() {

    
    let user = document.getElementById("user").value;
    let pass = document.getElementById("pass").value;

        fetch("https://localhost:7051/User/Login/" + user + "/" + pass, {
            method: "POST"
        }).then(response => {


            if (response.status == 401) {
                alert("Pogresan username");
            }
    
            if (response.status == 402) {
                alert("Pogresna lozinka");
            }

            if (response.status == 200) {
                console.log("USPESNO");
                setTimeout(function () {

                    window.location = 'https://localhost:7051/User/LogovanStranica';
                }, 1000)
            }

            if(response.status == 201)
            {
                console.log("VEC JE LOGOVAN");
            }
        }); 
}