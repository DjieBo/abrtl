function TSubmit(IDRS, Periode, Round, SSubmit, TSubmit, SVer1, TVer1) {
    var note = "Ver1";
    var IDV1 = note + IDRS + Periode + Round;

    if (SSubmit == "Submit" && SVer1 == "") {
        $("#" + IDV1).css({ "background": "#f0eb5d", "text-align": "center", "padding": ".2% 0"});
        $("#" + IDV1).text("Pending");
        var ro = document.createElement("div");
        ro.setAttribute("class", "hm-row");
        ro.setAttribute("id", "Padding" + IDV1);
        ro.setAttribute("style", "font-weight:bold;font-size:7pt");
        $("#" + IDV1).append(ro);

        var System = new Date(TSubmit);
        //System.setHours(System.getHours() + 24);
        System.setMinutes(System.getMinutes() + 5);
        //===========================================================
        var countDownDate = new Date(System).getTime();
        var xx = setInterval(function () {
            var now = new Date().getTime();
            var distance = countDownDate - now;
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);
            //====================================================================
            $("#Padding" + IDV1).text("( " + hours + " Jam " + minutes + " Menit " + seconds + " Detik )");
            if (distance < 0) {
                alert("Waktu Wakil Direktur Expired")
                //$.ajax({
                //    url: "/PeerGroup/AutoSubmit",
                //    type: "POST",
                //    data: { "IDRS": IDRS, "Round": Round, "AutoValue": "Submit"},
                //    succsess: function (result) {
                //        $("#PeerData").html(result);
                //    }
                //});
            }
        }, 1000);

    } else if (SSubmit == "Submit" && SVer1 == "Verify") {
        $("#" + IDV1).css("background", "transparent");
        $("#" + IDV1).text(SVer1);
        var TNote = document.createElement("b");
        TNote.innerHTML = " ( " + TVer1 + " )";
        $("#" + IDV1).append(TNote);
    } else if (SSubmit == "Verify" && SVer1 == "Verify") {
        $("#" + IDV1).css("background", "transparent");
        $("#" + IDV1).text(SVer1);
        var TNote = document.createElement("b");
        TNote.innerHTML = " ( " + TVer1 + " )";
        $("#" + IDV1).append(TNote);
    }else if (SSubmit == "Reject" && SVer1 == "Reject") {
        $("#" + IDV1).css({ "background": "transparent", "color" : "red" });
        $("#" + IDV1).text(SVer1);
        var TNote = document.createElement("b");
        TNote.innerHTML = " ( " + TVer1 + " )";
        $("#" + IDV1).append(TNote);
    }
}
//==============================================================================
function TVerify(IDRS, Periode, Round, SVer1, TVer1, SVer2, TVer2) {
    var note = "Ver2";
    var IDV2 = note + IDRS + Periode + Round;

    if (SVer1 == "Verify" && SVer2 == "") {
        $("#" + IDV2).css({ "background": "#f0eb5d", "text-align": "center", "padding":".2% 0"});
        $("#" + IDV2).text("Pending");
        var ro = document.createElement("div");
        ro.setAttribute("class", "hm-row");
        ro.setAttribute("id", "Padding" + IDV2);
        ro.setAttribute("style", "font-weight:bold;font-size:7pt");
        $("#" + IDV2).append(ro);

        var System = new Date(TVer1);
        //System.setHours(System.getHours() + 72);
        System.setMinutes(System.getMinutes() + 5);
        //===========================================================
        var countDownDate = new Date(System).getTime();
        var xx = setInterval(function () {
            var now = new Date().getTime();
            var distance = countDownDate - now;
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);
            //====================================================================
            $("#Padding" + IDV2).text("( " + days + " Hari " + hours + " Jam " + minutes + " Menit " + seconds + " Detik )");
            if (distance < 0) {
                alert("Waktu Direktur Expired")
                //$.ajax({
                //    url: "/PeerGroup/AutoVerify",
                //    type: "POST",
                //    data: { "IDRS": IDRS, "Round": Round },
                //    succsess: function (result) {
                //        $("#PeerData").html(result);
                //    }
                //});
            }
        }, 1000);

    } else if (SVer1 == "Verify" && SVer2 == "Verify") {
        $("#" + IDV2).css("background", "transparent");
        $("#" + IDV2).text(SVer2);
        var TNote = document.createElement("b");
        TNote.innerHTML = " ( " + TVer2 + " )";
        $("#" + IDV2).append(TNote);
    } else if (SVer1 == "Verify" && SVer2 == "Reject") {
        $("#" + IDV2).css({ "background": "transparent", "color": "red" });
        $("#" + IDV2).text(SVer2);
        var TNote = document.createElement("b");
        TNote.innerHTML = " ( " + TVer2 + " )";
        $("#" + IDV2).append(TNote);
    } else if (SVer1 == "Reject" && SVer2 == "") {
        $("#" + IDV2).text("");
    }
}
$(function () {
    $('div.hm-time-submit[onload]').trigger('onload');
    $('div.hm-time-verify[onload]').trigger('onload');
});