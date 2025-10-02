$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var monthNow = monthNames[today.getMonth()] + " " + yyyy;
    $("#SPKDateNow").text(monthNow);

    let SPKData = []
    const valPeriode = document.querySelectorAll(".SPKListPeriode");
    valPeriode.forEach(m => SPKData.push({
        Periode: m.dataset.periodecheck
    }));

    let SelectPeriode = document.getElementById("SPKSelectPeriode");
    for (let i = 0; i < SPKData.length; i++) {

        let NamaBln = monthNames[parseInt(SPKData[i].Periode.substring(6, 4) - 1)];
        let Tahun = SPKData[i].Periode.substring(0, 4);
        let NewOption = document.createElement("option");
        NewOption.setAttribute("value", SPKData[i].Periode);
        NewOption.innerText = NamaBln + " " + Tahun;
        SelectPeriode.appendChild(NewOption)
    }
    SelectPeriode.setAttribute("onchange", "SelectPeriodeSPK(this.value)");
});

function SelectPeriodeSPK(value) {
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    if (value != "") {
        const Yrs = value.substring(0, 4);
        const DateChange = monthNames[parseInt(value.substring(6, 4) - 1)] + " " + Yrs;
        $("#SPKDateNow").text(DateChange);
        const getField = document.querySelectorAll("#SPKnd");
        for (let i = 0; i < getField.length; i++) {
            getField[i].setAttribute("data-periode", value);
        }
    } else { }
}
function GetSPKReport(obj) {
    let today = new Date();
    let mm = String(today.getMonth() + 1).padStart(2, '0');
    let yyyy = today.getFullYear();
    let DateNow = yyyy + mm;

    let ParamIDRS = obj.dataset.idrs;
    let ParamRound = obj.dataset.round;
    let ParamPeriode = obj.dataset.periode;

    let SPKData = []
    const valPeriode = document.querySelectorAll(".SPKListdata");
    valPeriode.forEach(m => SPKData.push({
        Periode: m.dataset.periodecheck,
        IDRS: m.dataset.idrscheck,
        Round: m.dataset.roundcheck
    }));

    if (ParamPeriode == "" || ParamPeriode == null) {
        let ParamPeriode = DateNow;
        let Data = SPKData.filter(m => m.IDRS == ParamIDRS && m.Round == ParamRound && m.Periode == ParamPeriode);
        console.log(Data);
        if (Data.length >= 1) {
            $.ajax({
                url: "/Validator/LayoutSPK",
                type: "GET",
                data: { IDRS: ParamIDRS, Round: ParamRound, Periode: ParamPeriode },
                success: function (dataSPK) {
                    $('#VldtrLayout').html(dataSPK);
                }
            });
        } else { alert("Maaf Data yang anda cari tidak ditemukan"); }
        
    } else {
        let Data = SPKData.filter(m => m.IDRS == ParamIDRS && m.Round == ParamRound && m.Periode == ParamPeriode);
        if (Data.length >= 1) {
            $.ajax({
                url: "/Validator/LayoutSPK",
                type: "GET",
                data: { IDRS: ParamIDRS, Round: ParamRound, Periode: ParamPeriode },
                success: function (dataSPK) {
                    $('#VldtrLayout').html(dataSPK);
                }
            });
        } else { alert("Maaf Data yang anda cari tidak ditemukan"); }
    }

}