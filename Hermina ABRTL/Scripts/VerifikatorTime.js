$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    //var mn = String("0" + (this.getMonth() + 1)).slice(-2);
    var yyyy = today.getFullYear();
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var monthNow = monthNames[today.getMonth()];
    $("#MonthDrip").text(monthNow + " " + yyyy);

    let RoundData = [];
    const Rdata = document.querySelectorAll(".RoundVerValidation");
    Rdata.forEach(n => RoundData.push(
        {
            AksesVerifikator: n.dataset.onloginverifikator,
            Round: n.dataset.round,
            Periode: n.dataset.periode,
            StatusChecker: n.dataset.statuschecker,
            Status1: n.dataset.status1,
            Status2: n.dataset.status2
        }));

    const ArrayRoundC = ["Round 1", "Round 2", "Round 3", "Round 4"];
    let TrueRound = RoundData.map(m => m.Round);
    let FalseRound = ArrayRoundC.filter(m => !TrueRound.includes(m));
    if (dd >= 1 && dd <= 7) {
        if (TrueRound.length > 0) {
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#VBtn" + NmrRnd + "").addClass("active");
                $("#VStatus" + NmrRnd + "").removeClass('inactived');
                $("#VStatus" + NmrRnd + "").addClass('actived');
                $("#VInfoStatus" + NmrRnd + "").text('- Status Actived');
                $("#Info" + NmrRnd + " p").remove();
                $("#Info" + NmrRnd + "").css("padding", "10% 0");
                var p1 = document.createElement("p");
                p1.style.color = "black";
                p1.style.cursor = "pointer";
                p1.style.marginBottom = "4%";
                p1.setAttribute("class", "hm-rab-link");
                p1.innerHTML = "Penilaian";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Wakil Direktur') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "") && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Round " + NmrRnd + " saat ini sedang di proses oleh Wakil Direktur ')");
                    }
                }
                $("#Info" + NmrRnd + "").append(p1);
                //var p3 = document.createElement("p");
                //p3.style.color = "black";
                //p3.style.cursor = "pointer";
                //p3.style.marginBottom = "4%";
                //p3.setAttribute("class", "hm-rab-link");
                //p3.innerHTML = "SPK";
                ////p3.setAttribute("onclick", "SPKData('Round " + NmrRnd + "', '" + yyyy + mm + "')");
                //p3.setAttribute("onclick", "alert('Under Construction')");
                //$("#Info" + NmrRnd + "").append(p3);
                var p4 = document.createElement("p");
                p4.style.color = "black";
                p4.style.cursor = "pointer";
                p4.style.marginBottom = "4%";
                p4.setAttribute("class", "hm-rab-link");
                p4.innerHTML = "Job Report";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Direktur telah menolak laporan ini, Mohon untuk di periksa kembali!')");
                    } else if (RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan selesaikan Penilaian terlebih dahulu!')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Anda telah menolak laporan ini, Laporan ke kembalikan pada Wakil Direktur')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan memberikan Penilaian pada " + RoundData[i].Round + " Terlebih dahulu!')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Penilaian pada " + RoundData[i].Round + " Sedang di proses oleh Wakil Direktur')");
                    }
                }
                $("#Info" + NmrRnd + "").append(p4);
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 1") {
                    let NmrRndAct = "1";
                    $("#VBtn" + NmrRndAct + "").addClass("active");
                    $("#VStatus" + NmrRndAct + "").removeClass('inactived');
                    $("#VStatus" + NmrRndAct + "").addClass('actived');
                    $("#VInfoStatus" + NmrRndAct + "").text('- Status Actived');
                    $("#Info" + NmrRndAct + " p").remove();
                    $("#Info" + NmrRndAct + "").css("padding", "10% 0");
                    var p1 = document.createElement("p");
                    p1.style.color = "black";
                    p1.style.cursor = "pointer";
                    p1.style.marginBottom = "4%";
                    p1.setAttribute("class", "hm-rab-link");
                    p1.innerHTML = "Penilaian";
                    p1.setAttribute("onclick", "alert('Data " + FalseRound[i] + " Belum di Submit')");
                    $("#Info" + NmrRndAct + "").append(p1);
                    //var p3 = document.createElement("p");
                    //p3.style.color = "black";
                    //p3.style.cursor = "pointer";
                    //p3.style.marginBottom = "4%";
                    //p3.setAttribute("class", "hm-rab-link");
                    //p3.innerHTML = "SPK";
                    ////p3.setAttribute("onclick", "SPKData('Round " + NmrRndAct + "', '" + yyyy + mm + "')");
                    //p3.setAttribute("onclick", "alert('Under Construction')");
                    //$("#Info" + NmrRndAct + "").append(p3);
                    var p4 = document.createElement("p");
                    p4.style.color = "black";
                    p4.style.cursor = "pointer";
                    p4.style.marginBottom = "4%";
                    p4.setAttribute("class", "hm-rab-link");
                    p4.innerHTML = "Job Report";
                    p4.setAttribute("onclick", "alert('Data " + FalseRound[i] + " Belum di Submit!')");
                    $("#Info" + NmrRndAct + "").append(p4);
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");
                    $("#VBtn" + NmrRnd + "").css("cursor", "no-drop");
                }
            }
        }
    }
    if (dd >= 8 && dd <= 14) {
        if (TrueRound.length > 0) {
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#VBtn" + NmrRnd + "").addClass("active");
                $("#VStatus" + NmrRnd + "").removeClass('inactived');
                $("#VStatus" + NmrRnd + "").addClass('actived');
                $("#VInfoStatus" + NmrRnd + "").text('- Status Actived');
                $("#Info" + NmrRnd + " p").remove();
                $("#Info" + NmrRnd + "").css("padding", "10% 0");
                var p1 = document.createElement("p");
                p1.style.color = "black";
                p1.style.cursor = "pointer";
                p1.style.marginBottom = "4%";
                p1.setAttribute("class", "hm-rab-link");
                p1.innerHTML = "Penilaian";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Wakil Direktur') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "") && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Round " + NmrRnd + " saat ini sedang di proses oleh Wakil Direktur ')");
                    }
                }
                $("#Info" + NmrRnd + "").append(p1);
                //var p3 = document.createElement("p");
                //p3.style.color = "black";
                //p3.style.cursor = "pointer";
                //p3.style.marginBottom = "4%";
                //p3.setAttribute("class", "hm-rab-link");
                //p3.innerHTML = "SPK";
                ////p3.setAttribute("onclick", "SPKData('Round " + NmrRnd + "', '" + yyyy + mm + "')");
                //p3.setAttribute("onclick", "alert('Under Construction')");
                //$("#Info" + NmrRnd + "").append(p3);
                var p4 = document.createElement("p");
                p4.style.color = "black";
                p4.style.cursor = "pointer";
                p4.style.marginBottom = "4%";
                p4.setAttribute("class", "hm-rab-link");
                p4.innerHTML = "Job Report";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Direktur telah menolak laporan ini, Mohon untuk di periksa kembali!')");
                    } else if (RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan selesaikan Penilaian terlebih dahulu!')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Anda telah menolak laporan ini, Laporan ke kembalikan pada Wakil Direktur')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan memberikan Penilaian pada " + RoundData[i].Round + " Terlebih dahulu!')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Penilaian pada " + RoundData[i].Round + " Sedang di proses oleh Wakil Direktur')");
                    }
                }
                $("#Info" + NmrRnd + "").append(p4);
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 2") {
                    let NmrRndAct = "2";
                    $("#VBtn" + NmrRndAct + "").addClass("active");
                    $("#VStatus" + NmrRndAct + "").removeClass('inactived');
                    $("#VStatus" + NmrRndAct + "").addClass('actived');
                    $("#VInfoStatus" + NmrRndAct + "").text('- Status Actived');
                    $("#Info" + NmrRndAct + " p").remove();
                    $("#Info" + NmrRndAct + "").css("padding", "10% 0");
                    var p1 = document.createElement("p");
                    p1.style.color = "black";
                    p1.style.cursor = "pointer";
                    p1.style.marginBottom = "4%";
                    p1.setAttribute("class", "hm-rab-link");
                    p1.innerHTML = "Penilaian";
                    p1.setAttribute("onclick", "alert('Data " + FalseRound[i] + " Belum di Submit')");
                    $("#Info" + NmrRndAct + "").append(p1);
                    //var p3 = document.createElement("p");
                    //p3.style.color = "black";
                    //p3.style.cursor = "pointer";
                    //p3.style.marginBottom = "4%";
                    //p3.setAttribute("class", "hm-rab-link");
                    //p3.innerHTML = "SPK";
                    ////p3.setAttribute("onclick", "SPKData('Round " + NmrRndAct + "', '" + yyyy + mm + "')");
                    //p3.setAttribute("onclick", "alert('Under Construction')");
                    $("#Info" + NmrRndAct + "").append(p3);
                    var p4 = document.createElement("p");
                    p4.style.color = "black";
                    p4.style.cursor = "pointer";
                    p4.style.marginBottom = "4%";
                    p4.setAttribute("class", "hm-rab-link");
                    p4.innerHTML = "Job Report";
                    p4.setAttribute("onclick", "alert('Data " + FalseRound[i] + " Belum di Submit!')");
                    $("#Info" + NmrRndAct + "").append(p4);
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");
                    $("#VBtn" + NmrRnd + "").css("cursor", "no-drop");
                }
            }
        }
    }
    if (dd >= 15 && dd <= 21) {
        if (TrueRound.length > 0) {
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#VBtn" + NmrRnd + "").addClass("active");
                $("#VStatus" + NmrRnd + "").removeClass('inactived');
                $("#VStatus" + NmrRnd + "").addClass('actived');
                $("#VInfoStatus" + NmrRnd + "").text('- Status Actived');
                $("#Info" + NmrRnd + " p").remove();
                $("#Info" + NmrRnd + "").css("padding", "10% 0");
                var p1 = document.createElement("p");
                p1.style.color = "black";
                p1.style.cursor = "pointer";
                p1.style.marginBottom = "4%";
                p1.setAttribute("class", "hm-rab-link");
                p1.innerHTML = "Penilaian";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Wakil Direktur') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "") && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Round " + NmrRnd + " saat ini sedang di proses oleh Wakil Direktur ')");
                    }
                }
                $("#Info" + NmrRnd + "").append(p1);
                //var p3 = document.createElement("p");
                //p3.style.color = "black";
                //p3.style.cursor = "pointer";
                //p3.style.marginBottom = "4%";
                //p3.setAttribute("class", "hm-rab-link");
                //p3.innerHTML = "SPK";
                ////p3.setAttribute("onclick", "SPKData('Round " + NmrRnd + "', '" + yyyy + mm + "')");
                //p3.setAttribute("onclick", "alert('Under Construction')");
                //$("#Info" + NmrRnd + "").append(p3);
                var p4 = document.createElement("p");
                p4.style.color = "black";
                p4.style.cursor = "pointer";
                p4.style.marginBottom = "4%";
                p4.setAttribute("class", "hm-rab-link");
                p4.innerHTML = "Job Report";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Direktur telah menolak laporan ini, Mohon untuk di periksa kembali!')");
                    } else if (RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan selesaikan Penilaian terlebih dahulu!')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Anda telah menolak laporan ini, Laporan ke kembalikan pada Wakil Direktur')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan memberikan Penilaian pada " + RoundData[i].Round + " Terlebih dahulu!')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Penilaian pada " + RoundData[i].Round + " Sedang di proses oleh Wakil Direktur')");
                    }
                }
                $("#Info" + NmrRnd + "").append(p4);
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 3") {
                    let NmrRndAct = "3";
                    $("#VBtn" + NmrRndAct + "").addClass("active");
                    $("#VStatus" + NmrRndAct + "").removeClass('inactived');
                    $("#VStatus" + NmrRndAct + "").addClass('actived');
                    $("#VInfoStatus" + NmrRndAct + "").text('- Status Actived');
                    $("#Info" + NmrRndAct + " p").remove();
                    $("#Info" + NmrRndAct + "").css("padding", "10% 0");
                    var p1 = document.createElement("p");
                    p1.style.color = "black";
                    p1.style.cursor = "pointer";
                    p1.style.marginBottom = "4%";
                    p1.setAttribute("class", "hm-rab-link");
                    p1.innerHTML = "Penilaian";
                    p1.setAttribute("onclick", "alert('Data " + FalseRound[i] + " Belum di Submit')");
                    $("#Info" + NmrRndAct + "").append(p1);
                    //var p3 = document.createElement("p");
                    //p3.style.color = "black";
                    //p3.style.cursor = "pointer";
                    //p3.style.marginBottom = "4%";
                    //p3.setAttribute("class", "hm-rab-link");
                    //p3.innerHTML = "SPK";
                    ////p3.setAttribute("onclick", "SPKData('Round " + NmrRndAct + "', '" + yyyy + mm + "')");
                    //p3.setAttribute("onclick", "alert('Under Construction')");
                    $("#Info" + NmrRndAct + "").append(p3);
                    var p4 = document.createElement("p");
                    p4.style.color = "black";
                    p4.style.cursor = "pointer";
                    p4.style.marginBottom = "4%";
                    p4.setAttribute("class", "hm-rab-link");
                    p4.innerHTML = "Job Report";
                    p4.setAttribute("onclick", "alert('Data " + FalseRound[i] + " Belum di Submit!')");
                    $("#Info" + NmrRndAct + "").append(p4);
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");
                    $("#VBtn" + NmrRnd + "").css("cursor", "no-drop");
                }
            }
        }
    }
    if (dd >= 22 && dd <= 31) {
        if (TrueRound.length > 0) {
            for (let i = 0; i < RoundData.length; i++) {
                let NmrRnd = RoundData[i].Round.replace("Round ", "");

                $("#VBtn" + NmrRnd + "").addClass("active");
                $("#VStatus" + NmrRnd + "").removeClass('inactived');
                $("#VStatus" + NmrRnd + "").addClass('actived');
                $("#VInfoStatus" + NmrRnd + "").text('- Status Actived');
                $("#Info" + NmrRnd + " p").remove();
                $("#Info" + NmrRnd + "").css("padding", "10% 0");
                var p1 = document.createElement("p");
                p1.style.color = "black";
                p1.style.cursor = "pointer";
                p1.style.marginBottom = "4%";
                p1.setAttribute("class", "hm-rab-link");
                p1.innerHTML = "Penilaian";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].StatusChecker == "Verify" && RoundData[i].Status2 == "Verify") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " sudah dilakukan, Silahkan lihat di Job Report')");
                    } else if (RoundData[i].StatusChecker == "Reject" && RoundData[i].Status1 == "Reject" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Manager Penunjang Umum') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status2 == "Reject") {
                        p1.setAttribute("onclick", "alert('Penilaian " + RoundData[i].Round + " telah di Reject dan dikembalikan ke Wakil Direktur') ");
                    } else if (RoundData[i].StatusChecker == "Submit" && RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "Penilaian('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].StatusChecker == "Submit" && (RoundData[i].Status1 == "") && (RoundData[i].Status2 == "")) {
                        p1.setAttribute("onclick", "alert('Round " + NmrRnd + " saat ini sedang di proses oleh Wakil Direktur ')");
                    }
                }                
                $("#Info" + NmrRnd + "").append(p1);
                //var p3 = document.createElement("p");
                //p3.style.color = "black";
                //p3.style.cursor = "pointer";
                //p3.style.marginBottom = "4%";
                //p3.setAttribute("class", "hm-rab-link");
                //p3.innerHTML = "SPK";
                ////p3.setAttribute("onclick", "SPKData('Round " + NmrRnd + "', '" + yyyy + mm + "')");
                //p3.setAttribute("onclick", "alert('Under Construction')");
                //$("#Info" + NmrRnd + "").append(p3);
                var p4 = document.createElement("p");
                p4.style.color = "black";
                p4.style.cursor = "pointer";
                p4.style.marginBottom = "4%";
                p4.setAttribute("class", "hm-rab-link");
                p4.innerHTML = "Job Report";
                if (RoundData[i].AksesVerifikator == "Verifikator 1") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Direktur telah menolak laporan ini, Mohon untuk di periksa kembali!')");
                    } else if (RoundData[i].Status1 == "Verify" && (RoundData[i].Status2 == "")) {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan selesaikan Penilaian terlebih dahulu!')");
                    }
                } else if (RoundData[i].AksesVerifikator == "Verifikator 2") {
                    if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Verify") {
                        p4.setAttribute("onclick", "JobReport('Round " + NmrRnd + "', '" + RoundData[i].Periode + "')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "Reject") {
                        p4.setAttribute("onclick", "alert('Anda telah menolak laporan ini, Laporan ke kembalikan pada Wakil Direktur')");
                    } else if (RoundData[i].Status1 == "Reject" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Laporan telah dikembalikan ke Manager Penunjang Umum')");
                    } else if (RoundData[i].Status1 == "Verify" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Silahkan memberikan Penilaian pada " + RoundData[i].Round + " Terlebih dahulu!')");
                    } else if (RoundData[i].Status1 == "" && RoundData[i].Status2 == "") {
                        p4.setAttribute("onclick", "alert('Penilaian pada " + RoundData[i].Round + " Sedang di proses oleh Wakil Direktur')");
                    }
                }
                $("#Info" + NmrRnd + "").append(p4);
            }
        }
        if (FalseRound.length > 0) {
            for (let i = 0; i < FalseRound.length; i++) {
                if (FalseRound[i] == "Round 4") {
                    let NmrRndAct = "4";
                    $("#VBtn" + NmrRndAct + "").addClass("active");
                    $("#VStatus" + NmrRndAct + "").removeClass('inactived');
                    $("#VStatus" + NmrRndAct + "").addClass('actived');
                    $("#VInfoStatus" + NmrRndAct + "").text('- Status Actived');
                    $("#Info" + NmrRndAct + " p").remove();
                    $("#Info" + NmrRndAct + "").css("padding", "10% 0");
                    var p1 = document.createElement("p");
                    p1.style.color = "black";
                    p1.style.cursor = "pointer";
                    p1.style.marginBottom = "4%";
                    p1.setAttribute("class", "hm-rab-link");
                    p1.innerHTML = "Penilaian";
                    p1.setAttribute("onclick", "alert('Data " + FalseRound[i] +" Belum di Submit')");
                    $("#Info" + NmrRndAct + "").append(p1);
                    //var p3 = document.createElement("p");
                    //p3.style.color = "black";
                    //p3.style.cursor = "pointer";
                    //p3.style.marginBottom = "4%";
                    //p3.setAttribute("class", "hm-rab-link");
                    //p3.innerHTML = "SPK";
                    ////p3.setAttribute("onclick", "SPKData('Round " + NmrRndAct + "', '" + yyyy + mm + "')");
                    //p3.setAttribute("onclick", "alert('Under Construction')");
                    //$("#Info" + NmrRndAct + "").append(p3);
                    var p4 = document.createElement("p");
                    p4.style.color = "black";
                    p4.style.cursor = "pointer";
                    p4.style.marginBottom = "4%";
                    p4.setAttribute("class", "hm-rab-link");
                    p4.innerHTML = "Job Report";
                    p4.setAttribute("onclick", "alert('Data " + FalseRound[i] +" Belum di Submit!')");
                    $("#Info" + NmrRndAct + "").append(p4);
                } else {
                    let NmrRnd = FalseRound[i].replace("Round ", "");
                    $("#VBtn" + NmrRnd + "").css("cursor", "no-drop");
                }
            }
        }
    }
});