class Igrica
{
    constructor(id, naziv, cena, onSale, novaCena)
    {
        this.id = id;
        this.naziv = naziv;
        this.cena = cena;
        this.onSale = onSale;
        this.novaCena = novaCena;
    }

    crtajIgricuZaPocetnu()
    {
        let divColMb5 = document.createElement("div");
        divColMb5.classList.add("col");
        divColMb5.classList.add("mb-5");
        gde.appendChild(divColMb5);

        let divCardH100 = document.createElement("div");
        divCardH100.classList.add("card");
        divCardH100.classList.add("h-100")
        divColMb5.appendChild(divCardH100);

        let divSaleBadge = document.createElement("div");
        divSaleBadge.classList.add("badge");
        divSaleBadge.classList.add("bg-dark");
        divSaleBadge.classList.add("text-white");
        divSaleBadge.classList.add("position-absolute");
        divSaleBadge.style="top: 0.5rem; right: 0.5rem"
        divSaleBadge.innerHTML = "Sale";
        if(this.onSale == true)
        {
            divSaleBadge.style.visibility = "visible";
        }
        else
        {
            divSaleBadge.style.visibility = "hidden";
        }

        divCardH100.appendChild(divSaleBadge);

        let divCardBodyP4 = document.createElement("div");
        divCardBodyP4.classList.add("card-body");
        divCardBodyP4.classList.add("p-4");
        divCardH100.appendChild(divCardBodyP4);

        let divTextCenter = document.createElement("div");
        divTextCenter.classList.add("text-center");
        divCardBodyP4.appendChild(divTextCenter);

        let h5el = document.createElement("h5");
        h5el.innerText = this.naziv;
        divTextCenter.appendChild(h5el);

        let textCena = document.createElement("span");
        textCena.innerHTML = "Cena: " + this.cena;
        divTextCenter.appendChild(textCena);
        
        let textNovaCena = document.createElement("span");
        textNovaCena.innerHTML = "  " + this.novaCena;
        textNovaCena.style.visibility = "hidden";
        divTextCenter.appendChild(textNovaCena);

        if(this.onSale == true)
        {
            textCena.classList.add("text-muted");
            textCena.classList.add("text-decoration-line-through");

            textNovaCena.style.visibility = "visible";
        }
        else{
            textCena.classList.remove("text-muted");
            textCena.classList.remove("text-decoration-line-through");

            textNovaCena.style.visibility = "hidden";         
        }

        let divBtn = document.createElement("div");
        divBtn.classList.add("card-footer");
        divBtn.classList.add("p-4");
        divBtn.classList.add("pt-0");
        divBtn.classList.add("border-top-0");
        divBtn.classList.add("bg-transparent");
        divCardH100.appendChild(divBtn);

        let parentDivBtn = document.createElement("div");
        parentDivBtn.classList.add("text-center");
        divBtn.appendChild(parentDivBtn);

        let elementBtn = document.createElement("button");
        elementBtn.classList.add("btn", "btn-outline-dark", "mt-auto");
        elementBtn.innerHTML = "Add to wishlist";
        elementBtn.style.visibility = "hidden";

        parentDivBtn.appendChild(elementBtn);

        
    }
    
    crtajIgricuZaLogovan()
    {
        let divColMb5 = document.createElement("div");
        divColMb5.classList.add("col");
        divColMb5.classList.add("mb-5");
        gde.appendChild(divColMb5);

        let divCardH100 = document.createElement("div");
        divCardH100.classList.add("card");
        divCardH100.classList.add("h-100")
        divColMb5.appendChild(divCardH100);

        let divSaleBadge = document.createElement("div");
        divSaleBadge.classList.add("badge");
        divSaleBadge.classList.add("bg-dark");
        divSaleBadge.classList.add("text-white");
        divSaleBadge.classList.add("position-absolute");
        divSaleBadge.style="top: 0.5rem; right: 0.5rem"
        divSaleBadge.innerHTML = "Sale";
        if(this.onSale == true)
        {
            divSaleBadge.style.visibility = "visible";
        }
        else
        {
            divSaleBadge.style.visibility = "hidden";
        }

        divCardH100.appendChild(divSaleBadge);

        let divCardBodyP4 = document.createElement("div");
        divCardBodyP4.classList.add("card-body");
        divCardBodyP4.classList.add("p-4");
        divCardH100.appendChild(divCardBodyP4);

        let divTextCenter = document.createElement("div");
        divTextCenter.classList.add("text-center");
        divCardBodyP4.appendChild(divTextCenter);

        let h5el = document.createElement("h5");
        h5el.id = "h5el";
        h5el.innerText = this.naziv;
        divTextCenter.appendChild(h5el);

        let textCena = document.createElement("span");
        textCena.innerHTML = "Cena: " + this.cena;
        divTextCenter.appendChild(textCena);
        
        let textNovaCena = document.createElement("span");
        textNovaCena.innerHTML = "  " + this.novaCena;
        textNovaCena.style.visibility = "hidden";
        divTextCenter.appendChild(textNovaCena);

        if(this.onSale == true)
        {
            textCena.classList.add("text-muted");
            textCena.classList.add("text-decoration-line-through");

            textNovaCena.style.visibility = "visible";
        }
        else{
            textCena.classList.remove("text-muted");
            textCena.classList.remove("text-decoration-line-through");

            textNovaCena.style.visibility = "hidden";         
        }

        let divBtn = document.createElement("div");
        divBtn.classList.add("card-footer");
        divBtn.classList.add("p-4");
        divBtn.classList.add("pt-0");
        divBtn.classList.add("border-top-0");
        divBtn.classList.add("bg-transparent");
        divCardH100.appendChild(divBtn);

        let parentDivBtn = document.createElement("div");
        parentDivBtn.classList.add("text-center");
        divBtn.appendChild(parentDivBtn);

        let elementBtn = document.createElement("button");
        elementBtn.id = "btnAdd" + this.id;
        elementBtn.classList.add("btn", "btn-outline-dark", "mt-auto");
        elementBtn.innerHTML = "Add to wishlist";

        parentDivBtn.appendChild(elementBtn);
        
    }

    
    crtajIgricuZaProfil()
    {
        let divColMb5 = document.createElement("div");
        divColMb5.classList.add("col");
        divColMb5.classList.add("mb-5");
        gde.appendChild(divColMb5);

        let divCardH100 = document.createElement("div");
        divCardH100.classList.add("card");
        divCardH100.classList.add("h-100")
        divColMb5.appendChild(divCardH100);

        let divSaleBadge = document.createElement("div");
        divSaleBadge.classList.add("badge");
        divSaleBadge.classList.add("bg-dark");
        divSaleBadge.classList.add("text-white");
        divSaleBadge.classList.add("position-absolute");
        divSaleBadge.style="top: 0.5rem; right: 0.5rem"
        divSaleBadge.innerHTML = "Sale";
        if(this.onSale == true)
        {
            divSaleBadge.style.visibility = "visible";
        }
        else
        {
            divSaleBadge.style.visibility = "hidden";
        }

        divCardH100.appendChild(divSaleBadge);

        let divCardBodyP4 = document.createElement("div");
        divCardBodyP4.classList.add("card-body");
        divCardBodyP4.classList.add("p-4");
        divCardH100.appendChild(divCardBodyP4);

        let divTextCenter = document.createElement("div");
        divTextCenter.classList.add("text-center");
        divCardBodyP4.appendChild(divTextCenter);

        let h5el = document.createElement("h5");
        h5el.innerText = this.naziv;
        divTextCenter.appendChild(h5el);

        let textCena = document.createElement("span");
        textCena.innerHTML = "Cena: " + this.cena;
        divTextCenter.appendChild(textCena);
        
        let textNovaCena = document.createElement("span");
        textNovaCena.innerHTML = "  " + this.novaCena;
        textNovaCena.style.visibility = "hidden";
        divTextCenter.appendChild(textNovaCena);

        if(this.onSale == true)
        {
            textCena.classList.add("text-muted");
            textCena.classList.add("text-decoration-line-through");

            textNovaCena.style.visibility = "visible";
        }
        else{
            textCena.classList.remove("text-muted");
            textCena.classList.remove("text-decoration-line-through");

            textNovaCena.style.visibility = "hidden";         
        }

        let divBtn = document.createElement("div");
        divBtn.classList.add("card-footer");
        divBtn.classList.add("p-4");
        divBtn.classList.add("pt-0");
        divBtn.classList.add("border-top-0");
        divBtn.classList.add("bg-transparent");
        divCardH100.appendChild(divBtn);

        let parentDivBtn = document.createElement("div");
        parentDivBtn.classList.add("text-center");
        divBtn.appendChild(parentDivBtn);

        let elementBtn = document.createElement("button");
        elementBtn.id = "btnRemove" + this.id;
        elementBtn.classList.add("btn", "btn-outline-dark", "mt-auto");
        elementBtn.innerHTML = "Remove from wishlist";

        parentDivBtn.appendChild(elementBtn);
        
    }
}