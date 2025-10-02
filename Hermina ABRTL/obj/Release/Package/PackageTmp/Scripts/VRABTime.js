$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var monthNow = monthNames[today.getMonth()];
    $("#VMonthReady").text(monthNow + " " + yyyy);
    let DateNow = yyyy + mm;

    let ValueData = [];
    const List = document.querySelectorAll(".VerifikatorActRound");
    List.forEach(n => ValueData.push(
        {
            Round: n.dataset.round,
            Periode: n.dataset.periode,
            Akses: n.dataset.akses,
            SCheck: n.dataset.check,
            Status: n.dataset.status,
            Nilai1: n.dataset.nilai1,
            Nilai2: n.dataset.nilai2,
            Nilai3: n.dataset.nilai3,
        }));
    let DataCollect = ValueData.filter(m => m.Periode === DateNow);
    //console.log(DataCollect);

    for (let i = 0; i < DataCollect.length; i++) {
        if (DataCollect[i].SCheck == "Verify") {
            let rn = DataCollect[i].Round;
            let Rnd = rn.replace("Round ", "");

            $("#VRABtitle" + Rnd).removeClass("Minactive");
            $("#VRABtitle" + Rnd).addClass("Mactive");
            $("#VRAB" + Rnd + "Btn").addClass("active");
            $("#VRAB" + Rnd + "info P").remove();

            var p1 = document.createElement("P");
            p1.setAttribute("class", "hm-rab-link");
            p1.style.cursor = "pointer";
            p1.style.color = "#019343";
            p1.style.fontWeight = "bold";
            if (DataCollect[i].SCheck == "Verify" && (DataCollect[i].Nilai1 == "" || DataCollect[i].Nilai1 == "Ditolak" || DataCollect[i].Nilai1 == null)) {
                p1.setAttribute("onclick", "alert('RAB " + DataCollect[i].Round + " saat ini sedang di Proses oleh Checker')");
                p1.innerHTML = "View Status RAB Here";
            } else if (DataCollect[i].SCheck == "Verify" && DataCollect[i].Nilai1 == "Insert" && DataCollect[i].Nilai2 == "" && (DataCollect[i].Nilai3 == "")) {
                if (DataCollect[i].Akses == "Verifikator 1") {
                    p1.setAttribute("onclick", "VerifyRAB('" + DataCollect[i].Round + "', '" + yyyy + mm + "', 'Run')");
                    p1.innerHTML = "Submit RAB Here";
                } else {
                    p1.setAttribute("onclick", "alert('RAB " + DataCollect[i].Round + " saat ini sedang di Proses oleh Wakil Direktur')");
                    p1.innerHTML = "View Status RAB Here";
                }
            } else if (DataCollect[i].SCheck == "Verify" && DataCollect[i].Nilai1 == "Insert" && DataCollect[i].Nilai2 == "Disetujui" && DataCollect[i].Nilai3 == "") {
                if (DataCollect[i].Akses == "Verifikator 2") {
                    p1.setAttribute("onclick", "VerifyRAB('" + DataCollect[i].Round + "', '" + yyyy + mm + "', 'Run')");
                    p1.innerHTML = "Submit RAB Here";
                } else {
                    p1.setAttribute("onclick", "VerifyRAB('" + DataCollect[i].Round + "', '" + yyyy + mm + "', 'Verify')");
                    p1.innerHTML = "View Status RAB Here";
                }
            } else if (DataCollect[i].SCheck == "Verify" && DataCollect[i].Nilai1 == "Insert" && DataCollect[i].Nilai2 == "Disetujui" && (DataCollect[i].Nilai3 == "Ditolak")) {
                if (DataCollect[i].Akses == "Verifikator 1") {
                    p1.setAttribute("onclick", "VerifyRAB('" + DataCollect[i].Round + "', '" + yyyy + mm + "', 'Run')");
                    p1.innerHTML = "Submit RAB Here";
                } else {
                    p1.setAttribute("onclick", "alert('RAB " + DataCollect[i].Round + " telah di Tolak dan dikembalikan ke Wakil Direktur')");
                    p1.innerHTML = "View Status RAB Here";
                }
            } else if (DataCollect[i].SCheck == "Verify" && DataCollect[i].Nilai1 == "Disetujui" && DataCollect[i].Nilai2 == "Disetujui" && (DataCollect[i].Nilai3 == "Disetujui")) {
                p1.setAttribute("onclick", "VerifyRAB('" + DataCollect[i].Round + "', '" + yyyy + mm + "', 'Finish')");
                p1.innerHTML = "View Report RAB Here";

            }
            $("#VRAB" + Rnd + "info").append(p1);

            //var p2 = document.createElement("p");
            //p2.innerHTML = "View SPK";
            //p2.style.color = "black";
            //p2.style.cursor = "pointer";
            //p2.setAttribute("class", "hm-rab-link");
            //p2.setAttribute("onclick", "SPKVerify('" + DataCollect[i].Round + "', '" + yyyy + mm + "')");
            //$("#VRAB" + Rnd + "info").append(p2);

            var p3 = document.createElement("p");
            p3.innerHTML = "Job Report";
            p3.style.color = "black";
            p3.style.cursor = "pointer";
            p3.setAttribute("class", "hm-rab-link");
            p3.setAttribute("onclick", "PrintVerifyRAB('" + DataCollect[i].Round + "', '" + yyyy + mm + "')");
            $("#VRAB" + Rnd + "info").append(p3);

            $("#VRAB" + Rnd + "Status").removeClass('inactived');
            $("#VRAB" + Rnd + "Status").addClass('actived');
            $("#VStatInfoRAB" + Rnd).text("Status Active");
            $("#VIconRAB" + Rnd).removeClass('fa');
            $("#VIconRAB" + Rnd).removeClass('fa-clock-o');
            $("#VIconRAB" + Rnd).addClass('far');
            $("#VIconRAB" + Rnd).addClass('fa-check-circle');
        }
        }
        
});

function VerifyRAB(round, periode, status) {
    if (status == "Run") {
        $.ajax({
            url: "/Verifikator/VerifikatorRABForm/",
            type: "POST",
            data: { Round: round, Periode: periode },
            success: function (result) {
                $('#VrifikatLayout').html(result);
            }
        });
    } else if (status == "Verify") {
        $.ajax({
            url: "/Verifikator/VerifikatorRABForm/",
            type: "POST",
            data: { Round: round, Periode: periode },
            success: function (result) {
                $('#VrifikatLayout').html(result);
                $('#BtnPlc').css("display", "none");
                $('#RABSign').css("display", "block");
                $("#PrintBtnVr").css("display", "block");
            }
        });
    } else if (status == "Finish") {
        $.ajax({
            url: "/Verifikator/VerifikatorRABForm/",
            type: "POST",
            data: { Round: round, Periode: periode },
            success: function (result) {
                $('#VrifikatLayout').html(result);
                $('#BtnPlc').css("display", "none");
                $('#RABSign').css("display", "block");
                $("#PrintBtnVr").css("display", "block");
            }
        });
    }

}

function SPKVerify(round, periode) {
    alert(`Under Construction`);
}

function PrintVerifyRAB(round, periode) {
    alert(`Under Construction`);
}