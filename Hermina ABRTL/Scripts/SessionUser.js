$(document).ready(function () {
    $("#forGt").css("display", "block");
    let form = document.querySelector("#formclrSES");

    let field = document.createElement("input");
    field.setAttribute("name", "IDUser");
    field.setAttribute("id", "IDUser");
    field.setAttribute("value", "");
    field.setAttribute("autocomplete", "off");
    field.setAttribute("placeholder", "Type your ID here!");
    form.appendChild(field);

    let btnyes = document.createElement("button");
    btnyes.setAttribute("id", "reset");
    btnyes.setAttribute("onclick", "SessionReset(this.id)");
    btnyes.innerText = "Reset";
    form.appendChild(btnyes);

    let btnno = document.createElement("button");
    btnno.setAttribute("id", "cancel");
    btnno.setAttribute("onclick", "SessionReset(this.id)");
    btnno.innerText = "Cancel";
    form.appendChild(btnno);
});

function SessionReset(id) {
    if (id == "reset") {
        let IDUser = document.getElementById("IDUser").value;
        $.ajax({
            url: "/LandingPage/SessionClear",
            type: "POST",
            data: { IDReg: IDUser},
            success: function () {
                $("#forGt").css("display", "none");
                alert('ID User telah di Reset Session, silahkan Login kembali!');
            }
        });
    } else if (id == "cancel") {
        $("#forGt").css("display", "none");
    }
}