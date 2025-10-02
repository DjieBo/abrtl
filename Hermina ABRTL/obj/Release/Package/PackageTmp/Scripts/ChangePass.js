function CheckRegister() {
    let IDReg = document.getElementById("chgRegister").value;
    $.ajax({
        url: "/LandingPage/CheckRegister",
        type: "POST",
        data: { IDRegister: IDReg},
        success: function (result) {
            if (result.Values == "Registed") {
                let IDRgfield = document.getElementById("chgRegister");
                IDRgfield.setAttribute("disabled", "");

                $("#TitlePanel").text("Change your Password!");

                let fieldPW = document.getElementById("fldPw");
                let pw = document.createElement("input");
                pw.setAttribute("name", "newpassword");
                pw.setAttribute("type", "password");
                pw.setAttribute("id", "NewPassword");
                pw.setAttribute("placeholder", "Insert new password!");
                fieldPW.appendChild(pw);

                $("#BtnCheck").remove();
                let wrpbtn = document.getElementById("ButtonForm");
                let btn = document.createElement("button");
                btn.setAttribute("id", "resetPassword");
                btn.setAttribute("onclick", "ResetPassword()");
                btn.innerText = "Save";
                wrpbtn.appendChild(btn);
            } else if (result.Values == "Unregisted"){
                alert('Maaf ID Register tidak di temukan, silahkan cek kembali ID Register anda!');
            }
        }
    });
}
function ResetPassword() {
    let IDReg = document.getElementById("chgRegister").value;
    let NewPw = document.getElementById("NewPassword").value;
    $.ajax({
        url: "/LandingPage/ChangePassword",
        type: "POST",
        data: { IDRegister: IDReg, Password: NewPw },
        success: function (result) {
            console.log(result.Values)
            if (result.Values == "Saved") {
                alert("Password akun anda telah diganti, Silahkan lanjutkan di halaman Login!");
                window.location = "/LandingPage/Login";
            } else if (result.Values == "Unsaved"){
                alert("Password akun anda gagal di Update, Silakan hubungi developer untuk tindak lanjut, Terima kasih!");
                window.location = "/LandingPage/Login";
            }
        }
    });
}