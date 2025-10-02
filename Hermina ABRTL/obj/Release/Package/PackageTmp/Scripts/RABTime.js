$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var monthNow = monthNames[today.getMonth()];
    $("#MonthReady").text(monthNow + " " + yyyy);
    let PeriodDate = yyyy + mm;

    let Data = [];
    const DataList = document.querySelectorAll(".ActRound");
    DataList.forEach(n => Data.push(
        {
            Round: n.dataset.round,
            Periode: n.dataset.periode,
            SVerifikasi: n.dataset.statusverifikasi,
            Status: n.dataset.status,
            Nilai1: n.dataset.nilai1
        }));
    let DataIn = Data.filter(m => m.Periode === PeriodDate);

    let ArrApprv = [];
    const DataApp = document.querySelectorAll(".ApproveConds");
    DataApp.forEach(m => ArrApprv.push({
        RoundApp: m.dataset.roundapp,
        PeriodApp: m.dataset.appperiod,
        StatusApp: m.dataset.approve,
        DateApp: m.dataset.approvedate
    }));

    for (let i = 0; i < DataIn.length; i++) {
        if (DataIn[i].SVerifikasi == "Verify") {
            let num = DataIn[i].Round;
            let Nm = num.replace("Round ", "");

            $("#RABtitle" + Nm).removeClass("Minactive");
            $("#RABtitle" + Nm).addClass("Mactive");
            $("#RAB" + Nm + "Btn").addClass("active");
            $("#RAB" + Nm + "info P").remove();

            const p1 = document.createElement("P");
            p1.setAttribute("class", "hm-rab-link");
            p1.style.cursor = "pointer";
            p1.style.color = "#019343";
            p1.style.fontWeight = "bold";
            if (DataIn[i].Status == "Insert" || DataIn[i].Status == "Disetujui" && DataIn[i].Nilai1 == "Disetujui" || DataIn[i].Status == "Diketahui" && DataIn[i].Nilai1 == "Disetujui" || DataIn[i].Status == "Finish" && DataIn[i].Nilai1 == "Disetujui") {
                p1.setAttribute("onclick", "ViewCheckerRAB('" + DataIn[i].Round + "', '" + DataIn[i].Periode + "', 'Finish')");
                p1.innerHTML = "View Report RAB Here";
            } else if (DataIn[i].Status == "Insert" || DataIn[i].Status == "Disetujui") {
                p1.setAttribute("onclick", "ViewCheckerRAB('" + DataIn[i].Round + "', '" + DataIn[i].Periode + "', 'View')");
                p1.innerHTML = "View Report RAB Here";
            } else if (DataIn[i].Status == "Verify" || DataIn[i].Status == "SaveR") {
                p1.setAttribute("onclick", "RAB('" + DataIn[i].Round + "', '" + DataIn[i].Periode + "')");
                p1.innerHTML = "Submit RAB Here";
            } else if (DataIn[i].Status == "" || DataIn[i].Status == null) {
                p1.setAttribute("onclick", "alert('" + DataIn[i].Round + " tidak memiliki Data RAB')");
                p1.innerHTML = "View Status RAB Here";
            }
            $("#RAB" + Nm + "info").append(p1);

            const p3 = document.createElement("p");
            p3.innerHTML = "Job Report";
            p3.style.color = "black";
            p3.style.cursor = "pointer";
            p3.setAttribute("class", "hm-rab-link");
            //p3.setAttribute("onclick", "ExePort('" + DataIn[i].Round + "', '" + yyyy + mm + "')");
            p3.setAttribute("onclick", "alert('Under Construction')");
            $("#RAB" + Nm + "info").append(p3);

            $("#RAB" + Nm + "Status").removeClass('inactived');
            $("#RAB" + Nm + "Status").addClass('actived');
            $("#StatInfoRAB" + Nm).text("Status Active");
            $("#IconRAB" + Nm).removeClass('fa');
            $("#IconRAB" + Nm).removeClass('fa-clock-o');
            $("#IconRAB" + Nm).addClass('far');
            $("#IconRAB" + Nm).addClass('fa-check-circle');
        }
    }
});

function ViewCheckerRAB(Round, Periode, Status) {
    $.ajax({
        url: "/Checker/PrintRAB/",
        type: "POST",
        data: { Round: Round, Periode: Periode },
        success: function (x) {
            $('#hmPartView').html(x);
            if (Status == "Finish" || Status == "View") {
                $("#PrintBtnCh").css("display", "block");
            }
        }
    });
}
function SPKChekers(r, dt) {
    $.ajax({
        url: "/Checker/SPK/",
        type: "POST",
        data: { Round: r, Periode: dt },
        success: function (result) {
            $('#hmPartView').html(result);
        }
    });
}