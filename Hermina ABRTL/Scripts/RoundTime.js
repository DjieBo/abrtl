$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var monthNow = monthNames[today.getMonth()];
    $("#MonthNow").text(monthNow + " " + yyyy);
    let PeriodeRun = yyyy + mm;

    let RoundData = [];
    const Rdata = document.querySelectorAll(".RoundValidation");
    Rdata.forEach(n => RoundData.push(
        {
            Round: n.dataset.round,
            Periode: n.dataset.periode,
            StatusChecker: n.dataset.statuschecker,
            Status1: n.dataset.status1,
            Status2: n.dataset.status2
        }));
    
    const ArrayRoundC = ["Round 1", "Round 2", "Round 3", "Round 4"];
    let TrueRound = RoundData.map(m => m.Round);
    let FalseRound = ArrayRoundC.filter(m => !TrueRound.includes(m));
    if ((dd >= 1 && dd <= 7)) {
        if (TrueRound.length > 0) {
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#Round" + NmrRnd + "Btn").addClass("active");
                $("#Round" + NmrRnd + "info P").remove();
                var p1 = document.createElement("p");
                p1.setAttribute("class", "infoActive");
                if (RoundData[i].StatusChecker == "Submit") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim ke Wakil Direktur')");
                } else if (RoundData[i].StatusChecker == "Reject") {
                    p1.innerHTML = "Repair Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRnd + "')");
                } else if (RoundData[i].StatusChecker == "Verify") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim setujui sampai Direktur.')");
                }
                p1.setAttribute("class", "hm-check-link");
                p1.style.cursor = "pointer";
                p1.style.color = "#019343";
                p1.style.fontWeight = "bold";
                $("#Round" + NmrRnd + "info").append(p1);
                var p2 = document.createElement("p");
                if (RoundData[i].StatusChecker == "Reject") {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Silahkan perbaiki data Checklist terlebih dahulu dan lakukan Submit untuk melihat Report!')");
                } else {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "ExePort('Round " + NmrRnd + "','" + RoundData[i].Periode + "')");
                }
                p2.setAttribute("class", "hm-check-link");
                p2.style.cursor = "pointer";
                p2.style.color = "black";
                $("#Round" + NmrRnd + "info").append(p2);
                $("#Round" + NmrRnd + "Status").removeClass('inactived');
                $("#Round" + NmrRnd + "Status").addClass('actived');
                $("#StatInfoRound" + NmrRnd + "").text("Status Active");
                $("#IconRound" + NmrRnd + "").removeClass('fa');
                $("#IconRound" + NmrRnd + "").removeClass('fa-clock-o');
                $("#IconRound" + NmrRnd + "").addClass('far');
                $("#IconRound" + NmrRnd + "").addClass('fa-check-circle');
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 1") {
                    let NmrRndAct = "1";
                    $("#Round" + NmrRndAct + "Btn").addClass("active");
                    $("#Round" + NmrRndAct + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoActive");
                    p1.innerHTML = "Submit Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRndAct + "')");
                    p1.setAttribute("class", "hm-check-link");
                    p1.style.cursor = "pointer";
                    p1.style.color = "#019343";
                    p1.style.fontWeight = "bold";
                    $("#Round" + NmrRndAct + "info").append(p1);
                    var p2 = document.createElement("p");
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Maaf Report belum tersedia, Silakan Selesaikan Checklist dan Submit data terlebih dahulu!')");
                    p2.setAttribute("class", "hm-check-link");
                    p2.style.cursor = "pointer";
                    p2.style.color = "black";
                    $("#Round" + NmrRndAct + "info").append(p2);
                    $("#Round" + NmrRndAct + "Status").removeClass('inactived');
                    $("#Round" + NmrRndAct + "Status").addClass('actived');
                    $("#StatInfoRound" + NmrRndAct + "").text("Status Active");
                    $("#IconRound" + NmrRndAct + "").removeClass('fa');
                    $("#IconRound" + NmrRndAct + "").removeClass('fa-clock-o');
                    $("#IconRound" + NmrRndAct + "").addClass('far');
                    $("#IconRound" + NmrRndAct + "").addClass('fa-check-circle');
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");

                    $("#Round" + NmrRnd + "Btn").removeClass("active");
                    $("#Round" + NmrRnd + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoInActive");
                    p1.innerHTML = "This round status <br> is inactive";
                    p1.style.cursor = "no-drop";
                    p1.style.color = "#999";
                    $("#Round" + NmrRnd + "info").append(p1);

                    $("#Round" + NmrRnd + "Status").removeClass("active");
                    $("#Round" + NmrRnd + "Status").addClass("inactived");
                    $("#StatInfoRound" + NmrRnd + "").text("Status Inactive");
                    $("#IconRound" + NmrRnd + "").removeClass("far");
                    $("#IconRound" + NmrRnd + "").removeClass("fa-check-circle");
                    $("#IconRound" + NmrRnd + "").addClass("fa");
                    $("#IconRound" + NmrRnd + "").addClass("fa-clock-o");
                }

            }
        }
    }
    if (dd >= 8 && dd <= 14) {
        if (TrueRound.length > 0) {
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#Round" + NmrRnd + "Btn").addClass("active");
                $("#Round" + NmrRnd + "info P").remove();
                var p1 = document.createElement("p");
                p1.setAttribute("class", "infoActive");
                if (RoundData[i].StatusChecker == "Submit") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim ke Wakil Direktur')");
                } else if (RoundData[i].StatusChecker == "Reject") {
                    p1.innerHTML = "Repair Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRnd + "')");
                } else if (RoundData[i].StatusChecker == "Verify") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim setujui sampai Direktur.')");
                }
                p1.setAttribute("class", "hm-check-link");
                p1.style.cursor = "pointer";
                p1.style.color = "#019343";
                p1.style.fontWeight = "bold";
                $("#Round" + NmrRnd + "info").append(p1);
                var p2 = document.createElement("p");
                if (RoundData[i].StatusChecker == "Reject") {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Silahkan perbaiki data Checklist terlebih dahulu dan lakukan Submit untuk melihat Report!')");
                } else {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "ExePort('Round " + NmrRnd + "','" + RoundData[i].Periode + "')");
                }
                p2.setAttribute("class", "hm-check-link");
                p2.style.cursor = "pointer";
                p2.style.color = "black";
                $("#Round" + NmrRnd + "info").append(p2);
                $("#Round" + NmrRnd + "Status").removeClass('inactived');
                $("#Round" + NmrRnd + "Status").addClass('actived');
                $("#StatInfoRound" + NmrRnd + "").text("Status Active");
                $("#IconRound" + NmrRnd + "").removeClass('fa');
                $("#IconRound" + NmrRnd + "").removeClass('fa-clock-o');
                $("#IconRound" + NmrRnd + "").addClass('far');
                $("#IconRound" + NmrRnd + "").addClass('fa-check-circle');
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 2") {
                    let NmrRndAct = "2";
                    $("#Round" + NmrRndAct + "Btn").addClass("active");
                    $("#Round" + NmrRndAct + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoActive");
                    p1.innerHTML = "Submit Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRndAct + "')");
                    p1.setAttribute("class", "hm-check-link");
                    p1.style.cursor = "pointer";
                    p1.style.color = "#019343";
                    p1.style.fontWeight = "bold";
                    $("#Round" + NmrRndAct + "info").append(p1);
                    var p2 = document.createElement("p");
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Maaf Report belum tersedia, Silakan Selesaikan Checklist dan Submit data terlebih dahulu!')");
                    p2.setAttribute("class", "hm-check-link");
                    p2.style.cursor = "pointer";
                    p2.style.color = "black";
                    $("#Round" + NmrRndAct + "info").append(p2);
                    $("#Round" + NmrRndAct + "Status").removeClass('inactived');
                    $("#Round" + NmrRndAct + "Status").addClass('actived');
                    $("#StatInfoRound" + NmrRndAct + "").text("Status Active");
                    $("#IconRound" + NmrRndAct + "").removeClass('fa');
                    $("#IconRound" + NmrRndAct + "").removeClass('fa-clock-o');
                    $("#IconRound" + NmrRndAct + "").addClass('far');
                    $("#IconRound" + NmrRndAct + "").addClass('fa-check-circle');
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");

                    $("#Round" + NmrRnd + "Btn").removeClass("active");
                    $("#Round" + NmrRnd + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoInActive");
                    p1.innerHTML = "This round status <br> is inactive";
                    p1.style.cursor = "no-drop";
                    p1.style.color = "#999";
                    $("#Round" + NmrRnd + "info").append(p1);

                    $("#Round" + NmrRnd + "Status").removeClass("active");
                    $("#Round" + NmrRnd + "Status").addClass("inactived");
                    $("#StatInfoRound" + NmrRnd + "").text("Status Inactive");
                    $("#IconRound" + NmrRnd + "").removeClass("far");
                    $("#IconRound" + NmrRnd + "").removeClass("fa-check-circle");
                    $("#IconRound" + NmrRnd + "").addClass("fa");
                    $("#IconRound" + NmrRnd + "").addClass("fa-clock-o");
                }

            }
        }
    }
    if (dd >= 15 && dd <= 21) {
        if (TrueRound.length > 0) {
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#Round" + NmrRnd + "Btn").addClass("active");
                $("#Round" + NmrRnd + "info P").remove();
                var p1 = document.createElement("p");
                p1.setAttribute("class", "infoActive");
                if (RoundData[i].StatusChecker == "Submit") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim ke Wakil Direktur')");
                } else if (RoundData[i].StatusChecker == "Reject") {
                    p1.innerHTML = "Repair Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRnd + "')");
                } else if (RoundData[i].StatusChecker == "Verify") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim setujui sampai Direktur.')");
                }
                p1.setAttribute("class", "hm-check-link");
                p1.style.cursor = "pointer";
                p1.style.color = "#019343";
                p1.style.fontWeight = "bold";
                $("#Round" + NmrRnd + "info").append(p1);
                var p2 = document.createElement("p");
                if (RoundData[i].StatusChecker == "Reject") {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Silahkan perbaiki data Checklist terlebih dahulu dan lakukan Submit untuk melihat Report!')");
                } else {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "ExePort('Round " + NmrRnd + "','" + RoundData[i].Periode + "')");
                }
                p2.setAttribute("class", "hm-check-link");
                p2.style.cursor = "pointer";
                p2.style.color = "black";
                $("#Round" + NmrRnd + "info").append(p2);
                $("#Round" + NmrRnd + "Status").removeClass('inactived');
                $("#Round" + NmrRnd + "Status").addClass('actived');
                $("#StatInfoRound" + NmrRnd + "").text("Status Active");
                $("#IconRound" + NmrRnd + "").removeClass('fa');
                $("#IconRound" + NmrRnd + "").removeClass('fa-clock-o');
                $("#IconRound" + NmrRnd + "").addClass('far');
                $("#IconRound" + NmrRnd + "").addClass('fa-check-circle');
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 3") {
                    let NmrRndAct = "3";
                    $("#Round" + NmrRndAct + "Btn").addClass("active");
                    $("#Round" + NmrRndAct + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoActive");
                    p1.innerHTML = "Submit Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRndAct + "')");
                    p1.setAttribute("class", "hm-check-link");
                    p1.style.cursor = "pointer";
                    p1.style.color = "#019343";
                    p1.style.fontWeight = "bold";
                    $("#Round" + NmrRndAct + "info").append(p1);
                    var p2 = document.createElement("p");
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Maaf Report belum tersedia, Silakan Selesaikan Checklist dan Submit data terlebih dahulu!')");
                    p2.setAttribute("class", "hm-check-link");
                    p2.style.cursor = "pointer";
                    p2.style.color = "black";
                    $("#Round" + NmrRndAct + "info").append(p2);
                    $("#Round" + NmrRndAct + "Status").removeClass('inactived');
                    $("#Round" + NmrRndAct + "Status").addClass('actived');
                    $("#StatInfoRound" + NmrRndAct + "").text("Status Active");
                    $("#IconRound" + NmrRndAct + "").removeClass('fa');
                    $("#IconRound" + NmrRndAct + "").removeClass('fa-clock-o');
                    $("#IconRound" + NmrRndAct + "").addClass('far');
                    $("#IconRound" + NmrRndAct + "").addClass('fa-check-circle');
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");

                    $("#Round" + NmrRnd + "Btn").removeClass("active");
                    $("#Round" + NmrRnd + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoInActive");
                    p1.innerHTML = "This round status <br> is inactive";
                    p1.style.cursor = "no-drop";
                    p1.style.color = "#999";
                    $("#Round" + NmrRnd + "info").append(p1);

                    $("#Round" + NmrRnd + "Status").removeClass("active");
                    $("#Round" + NmrRnd + "Status").addClass("inactived");
                    $("#StatInfoRound" + NmrRnd + "").text("Status Inactive");
                    $("#IconRound" + NmrRnd + "").removeClass("far");
                    $("#IconRound" + NmrRnd + "").removeClass("fa-check-circle");
                    $("#IconRound" + NmrRnd + "").addClass("fa");
                    $("#IconRound" + NmrRnd + "").addClass("fa-clock-o");
                }

            }
        }
    }
    if (dd >= 22 && dd <= 31) {
        if (TrueRound.length > 0) { 
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#Round" + NmrRnd +"Btn").addClass("active");
                $("#Round" + NmrRnd +"info P").remove();
                var p1 = document.createElement("p");
                p1.setAttribute("class", "infoActive");
                if (RoundData[i].StatusChecker == "Submit") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim ke Wakil Direktur')");
                } else if (RoundData[i].StatusChecker == "Reject") {
                    p1.innerHTML = "Repair Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRnd + "')");
                } else if (RoundData[i].StatusChecker == "Verify") {
                    p1.innerHTML = "View Status Here";
                    p1.setAttribute("onclick", "alert('Checklist " + RoundData[i].Round + " telah dikirim setujui sampai Direktur.')");
                }
                p1.setAttribute("class", "hm-check-link");
                p1.style.cursor = "pointer";
                p1.style.color = "#019343";
                p1.style.fontWeight = "bold";
                $("#Round" + NmrRnd +"info").append(p1);
                var p2 = document.createElement("p");
                if (RoundData[i].StatusChecker == "Reject") {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Silahkan perbaiki data Checklist terlebih dahulu dan lakukan Submit untuk melihat Report!')");
                } else {
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "ExePort('Round " + NmrRnd + "','" + RoundData[i].Periode + "')");
                }
                p2.setAttribute("class", "hm-check-link");
                p2.style.cursor = "pointer";
                p2.style.color = "black";
                $("#Round" + NmrRnd +"info").append(p2);
                $("#Round" + NmrRnd +"Status").removeClass('inactived');
                $("#Round" + NmrRnd +"Status").addClass('actived');
                $("#StatInfoRound" + NmrRnd +"").text("Status Active");
                $("#IconRound" + NmrRnd + "").removeClass('fa');
                $("#IconRound" + NmrRnd + "").removeClass('fa-clock-o');
                $("#IconRound" + NmrRnd + "").addClass('far');
                $("#IconRound" + NmrRnd + "").addClass('fa-check-circle');
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 4") {
                    let NmrRndAct = "4";
                    $("#Round" + NmrRndAct + "Btn").addClass("active");
                    $("#Round" + NmrRndAct + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoActive");
                    p1.innerHTML = "Submit Checklist Here";
                    p1.setAttribute("onclick", "Round('Round " + NmrRndAct + "')");
                    p1.setAttribute("class", "hm-check-link");
                    p1.style.cursor = "pointer";
                    p1.style.color = "#019343";
                    p1.style.fontWeight = "bold";
                    $("#Round" + NmrRndAct + "info").append(p1);
                    var p2 = document.createElement("p");
                    p2.innerHTML = "View Result";
                    p2.setAttribute("onclick", "alert('Maaf Report belum tersedia, Silakan Selesaikan Checklist dan Submit data terlebih dahulu!')");
                    p2.setAttribute("class", "hm-check-link");
                    p2.style.cursor = "pointer";
                    p2.style.color = "black";
                    $("#Round" + NmrRndAct + "info").append(p2);
                    $("#Round" + NmrRndAct + "Status").removeClass('inactived');
                    $("#Round" + NmrRndAct + "Status").addClass('actived');
                    $("#StatInfoRound" + NmrRndAct + "").text("Status Active");
                    $("#IconRound" + NmrRndAct + "").removeClass('fa');
                    $("#IconRound" + NmrRndAct + "").removeClass('fa-clock-o');
                    $("#IconRound" + NmrRndAct + "").addClass('far');
                    $("#IconRound" + NmrRndAct + "").addClass('fa-check-circle');
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");

                    $("#Round" + NmrRnd + "Btn").removeClass("active");
                    $("#Round" + NmrRnd + "info P").remove();
                    var p1 = document.createElement("p");
                    p1.setAttribute("class", "infoInActive");
                    p1.innerHTML = "This round status <br> is inactive";
                    p1.style.cursor = "no-drop";
                    p1.style.color = "#999";
                    $("#Round" + NmrRnd + "info").append(p1);

                    $("#Round" + NmrRnd + "Status").removeClass("active");
                    $("#Round" + NmrRnd + "Status").addClass("inactived");
                    $("#StatInfoRound" + NmrRnd + "").text("Status Inactive");
                    $("#IconRound" + NmrRnd + "").removeClass("far");
                    $("#IconRound" + NmrRnd + "").removeClass("fa-check-circle");
                    $("#IconRound" + NmrRnd + "").addClass("fa");
                    $("#IconRound" + NmrRnd + "").addClass("fa-clock-o");
                }
                
            }
        }
    }
    //if (dd == 31) {
    //    for (let i = 0; i < FalseRound.length; i++) {
    //        let NmrRnd = FalseRound[i].replace("Round ", "");

    //        $("#Round" + NmrRnd + "Btn").removeClass("active");
    //        $("#Round" + NmrRnd + "info P").remove();
    //        var p1 = document.createElement("p");
    //        p1.setAttribute("class", "infoInActive");
    //        p1.innerHTML = "This round status <br> is inactive";
    //        p1.style.cursor = "no-drop";
    //        p1.style.color = "#999";
    //        $("#Round" + NmrRnd + "info").append(p1);

    //        $("#Round" + NmrRnd + "Status").removeClass("active");
    //        $("#Round" + NmrRnd + "Status").addClass("inactived");
    //        $("#StatInfoRound" + NmrRnd + "").text("Status Inactive");
    //        $("#IconRound" + NmrRnd + "").removeClass("far");
    //        $("#IconRound" + NmrRnd + "").removeClass("fa-check-circle");
    //        $("#IconRound" + NmrRnd + "").addClass("fa");
    //        $("#IconRound" + NmrRnd + "").addClass("fa-clock-o");
    //    }
    //}
});