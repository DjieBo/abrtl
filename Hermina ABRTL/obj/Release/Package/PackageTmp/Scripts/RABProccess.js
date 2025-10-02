function StatusList(id, round, periode) {
    //console.log(`${id} | ${round} | ${periode}`);
}

function KategoriRAB(id, round, periode) {
    
    let IDKat = "Kat" + id + round + periode;
    let Katvalues = document.getElementById(IDKat).value;

    if (Katvalues != null) {
        $("#Status" + id + round + periode).removeClass("hm-rab-status-yellow");
        $("#Status" + id + round + periode).addClass("hm-rab-status-blue")
    } else if (Katvalues == null) {
        $("#Status" + id + round + periode).removeClass("hm-rab-status-blue");
        $("#Status" + id + round + periode).addClass("hm-rab-status-yellow")
    }

    $(".hm-hid-itm-" + id + round + periode).remove();
    $(".OpID" + id + round + periode).remove();
    $(".hm-hid-dec-" + id + round + periode).remove();
    $(".Merk" + id + round + periode).remove();
    $("#pyout" + id + round + periode).val("");
    $("#budget" + id + round + periode).val("");
    $("#QAl" + id + round + periode).val("");

    let CallItem = "Item" + id + round + periode;
    $.ajax({
        url: "/Checker/PasstoGet/",
        type: "POST",
        cache:false,
        data: { IDItem: Katvalues},
        success: function (result) {
            if (result.DataArray) {
                let ListArray = result.DataArray;
                for (let i = 0; i < ListArray.length; i++) {
                    var el = document.createElement("p");
                    el.setAttribute("class", "hm-hid-itm-" + id + round + periode);
                    el.style.display = "none";
                    el.setAttribute("data-idttitem", ListArray[i]);
                    el.textContent = ListArray[i];
                    $("#" + CallItem).append(el);
                }
                const ArrayDataItem = Array.from(document.querySelectorAll('.hm-hid-itm-' + id + round + periode));
                let AllItem = ArrayDataItem.map(m => m.dataset.idttitem);
                let ItemID = ArrayDataItem.filter(a => a.textContent.includes("MI0")).map(m => m.dataset.idttitem);
                let Item = AllItem.filter(x => !ItemID.includes(x));
                let MapData = ItemID.map(function (_, i) {
                    return {
                        DataID: ItemID[i],
                        DataItem: Item[i]
                    }
                });
                const ElSelect = document.getElementById("Slc" + id + round + periode);
                ElSelect.removeAttribute("disabled");
                ElSelect.setAttribute("onchange", "ItemRAB('" + id + "', '" + round + "', '" + periode + "')");
                for (let i = 0; i < MapData.length; i++) {
                    const ElOp = document.createElement("option");
                    ElOp.setAttribute("class", "OpID" + id + round + periode);
                    ElOp.setAttribute("value", MapData[i].DataID);
                    ElOp.innerHTML = MapData[i].DataItem;
                    $("#Slc" + id + round + periode).append(ElOp);
                }

            }
        }

    });
}

function ItemRAB(id, round, periode) {
    var IDItem = "Slc" + id + round + periode;
    var IDDesc = "Dec" + id + round + periode;
    var ItemValue = document.getElementById(IDItem).value;

    $(".hm-hid-dec-" + id + round + periode).remove();
    $(".Merk" + id + round + periode).remove();
    $("#pyout" + id + round + periode).val("");
    $("#budget" + id + round + periode).val("");
    $("#QAl" + id + round + periode).val("");
    $.ajax({
        url: "/Checker/PassItem/",
        type: "POST",
        cache: false,
        data: { ValItem: ItemValue },
        success: function (result) {
            if (result.DataArrayItem) {
                let ListArrayDesc = result.DataArrayItem;
                for (let i = 0; i < ListArrayDesc.length; i++) {
                    var el = document.createElement("p");
                    el.setAttribute("class", "hm-hid-dec-" + id + round + periode);
                    el.style.display = "none";
                    el.setAttribute("data-idttdec", ListArrayDesc[i]);
                    el.textContent = ListArrayDesc[i];
                    $("#" + IDDesc).append(el);
                }
                const ArrayDataDec = Array.from(document.querySelectorAll('.hm-hid-dec-' + id + round + periode));
                let AllDec = ArrayDataDec.map(m => m.dataset.idttdec);
                let DDec = ArrayDataDec.filter(a => a.textContent.includes("#")).map(m => m.dataset.idttdec);
                let IDDec = [];
                let FoxDex = []
                for (let g = 0; g < DDec.length; g++) {
                    IDDec.push(parseFloat(DDec[g]).toFixed(2));
                    FoxDex.push(DDec[g]);
                }
                let DescType = AllDec.filter(x => !FoxDex.includes(x));
                let MapDesc = IDDec.map(function (_, i) {
                    return {
                        Deskrip: DescType[i],
                        Harga: IDDec[i]
                    }
                });
                const ElementDesc = document.getElementById("DescSlc" + id + round + periode);
                ElementDesc.removeAttribute("disabled");
                ElementDesc.setAttribute("onchange", "Pricing(this)");
                for (let a = 0; a < MapDesc.length; a++) {
                    const ElOp = document.createElement("option");
                    ElOp.setAttribute("class", "Merk" + id + round + periode);
                    ElOp.setAttribute("value", MapDesc[a].Deskrip);
                    ElOp.setAttribute("id", MapDesc[a].Harga);
                    ElOp.innerHTML = MapDesc[a].Deskrip;
                    $("#DescSlc" + id + round + periode).append(ElOp);
                }
                
            }
        }
    });
}

function Pricing(a) {
    let id = a.id;

    let dataID = id.replace("DescSlc", "");
    $("#pyout" + dataID).val("");
    $("#budget" + dataID).val("");
    $("#QAl" + dataID).val("");

    $("#pyout" + dataID).removeAttr("disabled");
    let Harga = a[a.selectedIndex].id;
    let Bugdet = "budget" + dataID;
    document.getElementById(Bugdet).value = Harga;
    
    if (Harga != null || Harga != "") {
        $("#Status" + dataID).removeClass("hm-rab-status-blue");
        $("#Status" + dataID).addClass("hm-rab-status-green");
        $("#Status" + dataID).text("Check");
        $("#Status" + dataID).attr("onclick", "SaveRowRAB('" + dataID +"')");
    } else if (QLSt == null || QLSt == "") {
        $("#Status" + dataID).removeClass("hm-rab-status-green");
        $("#Status" + dataID).addClass("hm-rab-status-blue");
    }
}

function SaveRowRAB(dataID) {
    let TextID = "HidID" + dataID;
    let valueID = document.getElementById(TextID).value;
    let TextRD = "HidRD" + dataID;
    let valueRD = document.getElementById(TextRD).value;
    let TextPR = "HidPR" + dataID;
    let valuePR = document.getElementById(TextPR).value;
    let IDKat = "Kat" + dataID;
    let valueKategori = document.getElementById(IDKat).value;
    let IDTm = "Slc" + dataID;
    let valueItem = document.getElementById(IDTm).value;
    let IDDesc = "DescSlc" + dataID;
    let valueDesc = document.getElementById(IDDesc).value;
    let IDBuget = "budget" + dataID;
    let valueBuget = document.getElementById(IDBuget).value;
    let IDPay = "pyout" + dataID;
    let valuePayout = document.getElementById(IDPay).value;
    let IDQl = "QAl" + dataID;
    let valueQuarel = document.getElementById(IDQl).value;
    $.ajax({
        url: "/Checker/SaveRowRAB/",
        type: "POST",
        cache: false,
        data: { ID: valueID, Round: valueRD, Periode: valuePR, Kategori: valueKategori, Item: valueItem, Deskripsi: valueDesc, Budget: valueBuget, Payout: valuePayout, Quarel: valueQuarel },
        success: function (result) {
            if (result.DataSave) {
                $("#Status" + dataID).text("Saved");
                $("#Status" + dataID).removeAttr("data-status");
                $("#Status" + dataID).attr("data-status", "Saved");
                $("#Status" + dataID).removeAttr("Onclick");
                $("#Status" + dataID).css("cursor", "default");
                $("#Kat" + dataID).attr("disabled", "on");
                $("#Slc" + dataID).attr("disabled", "on");
                $("#DescSlc" + dataID).attr("disabled", "on");
                $("#pyout" + dataID).attr("disabled", "on");

                let dataStatus = document.querySelectorAll(".hm-rab-status-fnl");
                let Status = [];
                var elementHtml = [].map.call(dataStatus, function (el) {
                    return Status.push(el.innerHTML);
                });
                let ArrayStatus = Status.map(m => m.trim());
                let ArraySaved = ArrayStatus.filter(a => a == 'Saved');

                let BtnSubmit = document.querySelector("button");
                BtnSubmit.setAttribute("data-summary", ArrayStatus.length);
                BtnSubmit.setAttribute("data-save", ArraySaved.length);
            }
        }
    });
}

$(document).ready(function () {
    let dataStatus = document.querySelectorAll(".hm-rab-status-fnl");
    let Status = [];
    var elementHtml = [].map.call(dataStatus, function (el) {
        return Status.push(el.innerHTML);
    });
    let ArrayStatus = Status.map(m => m.trim());
    let ArraySaved = ArrayStatus.filter(a => a == 'Saved');

    let BtnSubmit = document.querySelector("button");
    BtnSubmit.setAttribute("data-summary", ArrayStatus.length);
    BtnSubmit.setAttribute("data-save", ArraySaved.length);
});

function SubmitRAB(obj) {
    let DataSave = obj.dataset.save;
    let DataTotal = obj.dataset.summary;
    let DataRound = "Round " + obj.dataset.round;
    let DataPeriode = obj.dataset.periode;

    if (DataTotal > DataSave) {
        alert(`Maaf Data Anda Harus Tersimpan Semua`);
    } else if (DataTotal = DataSave) {
        $.ajax({
            url: "/Checker/SubmitDataRAB/",
            type: "POST",
            cache: false,
            data: { Round: DataRound, Periode: DataPeriode },
            success: function (result) {
                $('#hmPartView').html(result);
            }
        })
    }
}
