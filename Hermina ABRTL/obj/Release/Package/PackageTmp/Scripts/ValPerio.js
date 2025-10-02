$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var monthNow = monthNames[today.getMonth()] + " " + yyyy;
    let PerData = []
    const valPeriode = document.querySelectorAll(".valuePeriodeLtVal");
    valPeriode.forEach(m => PerData.push({
        Periode: m.dataset.periodecheck
    }));
    
    let PerValNow = PerData.filter(m => m.Periode === yyyy + mm);
    let listMode = PerValNow.find(m => m.Periode);
    let GetMonth = listMode.Periode.substring(6, 4);
    let Bln = monthNames[parseInt(GetMonth) - 1];
    let DateName = Bln + " " + yyyy;

    $("#DateNameNow").text(monthNow);


    const SelectPeriode = document.getElementById("hmValPeriodeSlectd");
    for (let i = 0; i < PerData.length; i++) {
        
        let NamaBln = monthNames[parseInt(PerData[i].Periode.substring(6, 4) - 1)];
        let Tahun = PerData[i].Periode.substring(0, 4);
        let NewOption = document.createElement("option");
        NewOption.setAttribute("value", PerData[i].Periode);
        NewOption.innerText = NamaBln + " " + Tahun;
        SelectPeriode.appendChild(NewOption)
    }
    SelectPeriode.setAttribute("onchange", "SelectPeriode(this.value)");
});

function SelectPeriode(value) {
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    if (value != "") {
        const Yrs = value.substring(0, 4);
        const DateChange = monthNames[parseInt(value.substring(6, 4) - 1)] + " " + Yrs;
        $("#DateNameNow").text(DateChange);
        const getField = document.querySelectorAll("#GtPeriod");
        for (let i = 0; i < getField.length; i++) {
            getField[i].setAttribute("data-periode", value);
        }
    } else {}
}