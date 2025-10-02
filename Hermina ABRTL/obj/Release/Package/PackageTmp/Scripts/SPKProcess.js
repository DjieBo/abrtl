$(document).ready(function () {
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    listSPK = [];
    let DataSPK = document.querySelectorAll(".SPKList");
    DataSPK.forEach(m => listSPK.push({
        Round: m.dataset.round,
        Periode: m.dataset.periode,
        Status: m.dataset.status
    }));   

    let SelcP = document.getElementById("SPKperiode");
    for (let i = 0; i < listSPK.length; i++) {
        let SPKBln = monthNames[parseInt(listSPK[i].Periode.substring(6, 4) - 1)];
        let SPKTahun = listSPK[i].Periode.substring(0, 4);
        let NewOption = document.createElement("option");
        NewOption.setAttribute("value", listSPK[i].Periode + listSPK[i].Status);
        NewOption.innerText = SPKBln + " " + SPKTahun;
        SelcP.appendChild(NewOption);
    }
    SelcP.setAttribute("onchange", "SelectPriodeSPK(this.value)");
});

function SelectPriodeSPK(value) {
    let PeriodeGet = value.substring(0, 6);
    let StatusGet = value.replace(PeriodeGet, "");

    ListApp = [];
    let DataApp = document.querySelectorAll(".SPKApproveList");
    DataApp.forEach(m => ListApp.push({
        Round: m.dataset.round,
        Periode: m.dataset.periode,
        Time: m.dataset.time
    }));
    let DataSpRound = ListApp.filter(m => m.Periode == PeriodeGet);
    let SelcRound = document.getElementById("SPKround");
    SelcRound.removeAttribute("disabled");
    for (let i = 0; i < DataSpRound.length; i++) {
        let Rnd = DataSpRound[i].Round.replace("Round ", "");
        let NewOption = document.createElement("option");
        NewOption.setAttribute("value", DataSpRound[i].Periode + Rnd + StatusGet);
        NewOption.innerText = DataSpRound[i].Round;
        SelcRound.appendChild(NewOption);
    }
    SelcRound.setAttribute("onchange", "SelectSPK(this.value)");
}
function SelectSPK(value) {
    let Periode = value.substring(0, 6);
    let Round = "Round " + value.substring(6, 7);
    let trimer = Periode + value.substring(6, 7);
    let Status = value.replace(trimer, "");
    $.ajax({
        url: "/Checker/SPK",
        type: "POST",
        data: { Round: Round, Periode: Periode },
        success: function (result) {
            $('#tmpSPK').html(result);
            if (Status == "Finish") {
                $("#SubmitSPK").css("display", "none");
                $("#printPDFSPK").css("display", "inline-block");
                $("#SPKtimer").remove();
                //$("#SPKCd").text("SPK Telah Selesai");
            }
        }
    });
}