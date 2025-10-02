$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var monthNow = monthNames[today.getMonth()] + " " + yyyy;
    $("#RABDateNow").text(monthNow);
    
    let RABData = []
    const valPeriode = document.querySelectorAll(".RABListPeriode");
    valPeriode.forEach(m => RABData.push({
        Periode: m.dataset.periodecheck
    }));
    console.log(RABData);
    let SelectPeriode = document.getElementById("RABSelectPeriode");
    for (let i = 0; i < RABData.length; i++) {

        let NamaBln = monthNames[parseInt(RABData[i].Periode.substring(6, 4) - 1)];
        let Tahun = RABData[i].Periode.substring(0, 4);
        let NewOption = document.createElement("option");
        NewOption.setAttribute("value", RABData[i].Periode);
        NewOption.innerText = NamaBln + " " + Tahun;
        SelectPeriode.appendChild(NewOption)
    }
    SelectPeriode.setAttribute("onchange", "SelectPeriodeRAB(this.value)");

});

function SelectPeriodeRAB(value) {
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    if (value != "") {
        const Yrs = value.substring(0, 4);
        const DateChange = monthNames[parseInt(value.substring(6, 4) - 1)] + " " + Yrs;
        $("#RABDateNow").text(DateChange);
        const getField = document.querySelectorAll("#RABRnd");
        for (let i = 0; i < getField.length; i++) {
            getField[i].setAttribute("data-periode", value);
        }
    } else { }
}

function GetRABReport(obj) {
    let today = new Date();
    let mm = String(today.getMonth() + 1).padStart(2, '0');
    let yyyy = today.getFullYear();
    let DateNow = yyyy + mm;

    let ParamIDRS = obj.dataset.idrs;
    let ParamRound = obj.dataset.round;
    let ParamPeriode = obj.dataset.periode;

    let ValidData = []
    const valPeriode = document.querySelectorAll(".RABListdata");
    valPeriode.forEach(m => ValidData.push({
        Periode: m.dataset.periodecheck,
        IDRS: m.dataset.idrscheck,
        Round: m.dataset.roundcheck
    }));
    let Data = ValidData.filter(m => m.IDRS == ParamIDRS && m.Round == ParamRound);
    if (Data.length >= 1) {
        if (ParamPeriode == "" || ParamPeriode == null) {
            let GetData = Data.find(n => n.Periode == DateNow);
            if (GetData == null) {
                alert("Maaf Data yang anda cari tidak ditemukan");
            } else {
                $.ajax({
                    url: "/Validator/ExeportRABLayout/",
                    type: "GET",
                    data: { Round: ParamRound, IDRS: ParamIDRS, Periode: DateNow },
                    success: function (result) {
                        $('#VldtrLayout').html(result);
                    }
                });
            }
        } else {
            let GetData = Data.find(n => n.Periode == ParamPeriode);
            if (GetData == null) {
                alert("Maaf Data yang anda cari tidak ditemukan");
            } else {
                $.ajax({
                    url: "/Validator/ExeportRABLayout/",
                    type: "GET",
                    data: { Round: ParamRound, IDRS: ParamIDRS, Periode: ParamPeriode },
                    success: function (result) {
                        $('#VldtrLayout').html(result);
                    }
                });
            }
        }
    } else if (Data.length < 1) {
        alert("Maaf Data yang anda cari tidak ditemukan");
    }
}