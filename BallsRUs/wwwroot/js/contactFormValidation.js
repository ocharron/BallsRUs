function validateContactFormSubmit() {
    // Valide le courriel
    let x = document.forms["contactForm"]["email"].value;
    if (x == "") {
        alert("Vous devez entrer votre courriel.");
        return false;
    }
    let regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!regexEmail.test(x)) {
        alert("Entrez un courriel valide.");
        return false;
    }

    // Valide le nom
    let y = document.forms["contactForm"]["subject"].value;
    if (y == "") {
        alert("Vous devez entrer votre nom.");
        return false;
    }
    let regexNom = /^.{2,20}$/;
    if (!regexNom.test(y)) {
        alert("Entrez un nom valide.");
        return false;
    }

    // Valide le message
    let z = document.forms["contactForm"]["body"].value;
    if (z == "") {
        alert("Vous devez entrer une description pour votre demande.");
        return false;
    }

    return true;
}