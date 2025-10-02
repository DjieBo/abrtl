$(document).ready(function () {
    let dataSubmit = document.querySelector(".ConsdDataRound");
    let DataTotal = dataSubmit.dataset.total;
    let DataCheck = dataSubmit.dataset.check;
    if (DataTotal == DataCheck) {
        const btnpost = document.getElementById("SubmitBTNchecker");
        const tombol = document.createElement("Button");
        tombol.setAttribute("id", "submitRound");
        tombol.setAttribute("onclick", "SubmitRound(this.id)");
        tombol.innerText = "Submit";
        btnpost.appendChild(tombol);
    }
});