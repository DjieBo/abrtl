let Time = document.querySelector("#SPKCd");
let DataTime = Time.dataset.sttime;
let SDay = DataTime.substring(0, 2);
let SMonth = DataTime.substring(3, 5);
let SYear = DataTime.substring(6, 10);
let STime = DataTime.substring(11, 19).replaceAll('.', ':');
let DateTimeApp = SMonth + "/" + SDay + "/" + SYear + " " + STime;

let System = new Date(DateTimeApp);
System.setHours(System.getHours() + 168);
//===========================================================
let countDownDate = new Date(System).getTime();
let xx = setInterval(function () {
    let now = new Date().getTime();
    let distance = countDownDate - now;
    let days = Math.floor(distance / (1000 * 60 * 60 * 24));
    let hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    let minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    let seconds = Math.floor((distance % (1000 * 60)) / 1000);
    //====================================================================
    $("#SPKCd").text("" + days + " Hari " + hours + " Jam " + minutes + " Menit " + seconds + " Detik ");
    if (distance < 0) {
        $.ajax({
            url: "/Checker/SPKSubmit",
            type: "POST",
            data: { "IDRS": IDRS, "Round": Round, "AutoValue": "Submit"},
            succsess: function (result) {
                $("#hmPartView").html(result);
            }
        });
        $("#SPKCd").text("Waktu Telah Habis");
        $("#SubmitSPK").remove();
        $("#printPDFSPK").removeAttr("style");
        $("#printPDFSPK").css("display", "inline-block");
    }
}, 1000);