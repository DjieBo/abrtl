using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class DataRSVM
    {
        public List<DataRoundAreaVM> listAreaRound { get; set; }
        public List<DataKomponenVM> listKomponen { get; set; }
        public string round { get; set; }
        public string JPG { get; set; }
        public string TotalData { get; set; }
        public string TotalCheck { get; set; }
    }

    public class DataRoundAreaVM
    {
        public int dtCheck { get; set; }
        public int dtTotal { get; set; }
        public string IDRoundArea { get; set; }
        public string RoundArea { get; set; }
        public string Round { get; set; }
    }

    public class DataKomponenVM
    {
        public int IDKomponen { get; set; }
        public string Round { get; set; }
        public string IDRoundArea { get; set; }
        public string RoundArea { get; set; }
        public string IDArea { get; set; }
        public string Area { get; set; }
        public string IDSubArea { get; set; }
        public string SubArea { get; set; }
        public string IDType { get; set; }
        public string Type { get; set; }
        public string IDOption { get; set; }
        public string Option { get; set; }
        public string IDItem { get; set; }
        public string Item { get; set; }
        public string Komponen { get; set; }
        public string Parameter { get; set; }
        public string ParamValue { get; set; }
        public string Nilai { get; set; }
        public string Status { get; set; }
        public string Keterangan { get; set; }
        public string IDUnix { get; set; }
    }

    public class ViewImageVM
    {
        public string Round { get; set; }
        public string SubArea { get; set; }
        public string Item { get; set; }
        public string Komponen { get; set; }
        public string JPG { get; set; }
        public string JPGAfter { get; set; }
        public string Keterangan { get; set; }
    }
    public class ImageRABBVM
    {
        public string Round { get; set; }
        public string SubArea { get; set; }
        public string Item { get; set; }
        public string Komponen { get; set; }
        public string JPG { get; set; }
        public string JPGAfter { get; set; }
    }
    public class ExcReportVM
    {
        public string UserOnLogin { get; set; }
        public string NamaRS { get; set; }
        public string Periode { get; set; }
        public string Bulan { get; set; }
        public string IDRS { get; set; }
        public string Round { get; set; }
        public List<dtAreaVM> listDataArea { get; set; }
        public string Checker { get; set; }
        public string WakilDirektur { get; set; }
        public string DIrektur { get; set; }
        public string Validator1 { get; set; }
        public string Validator2 { get; set; }
        public string Validator3 { get; set; }
        public decimal totalItemPenilaian { get; set; }
        public decimal totalItemA { get; set; }
        public decimal totalItemB { get; set; }
        public decimal totalItemC { get; set; }
        public decimal persenTotalItemA { get; set; }
        public decimal persenTotalItemB { get; set; }
        public decimal persenTotalItemC { get; set; }
        public decimal bobotItemA { get; set; }
        public decimal bobotItemB { get; set; }
        public decimal bobotItemC { get; set; }
        public decimal totalBobot { get; set; }
        public decimal totalAman { get; set; }
        public decimal persenAman { get; set; }
        public decimal Aman { get; set; }
        public decimal tdkAman { get; set; }
        public decimal totalBersih { get; set; }
        public decimal persenBersih { get; set; }
        public decimal Bersih { get; set; }
        public decimal tdkBersih { get; set; }
        public decimal totalRapih { get; set; }
        public decimal persenRapih { get; set; }
        public decimal Rapih { get; set; }
        public decimal tdkRapih { get; set; }
        public decimal totalBaru { get; set; }
        public decimal persenBaru { get; set; }
        public decimal Baru { get; set; }
        public decimal tdkBaru { get; set; }
        public decimal totalRL { get; set; }
        public decimal persenRL { get; set; }
        public decimal RL { get; set; }
        public decimal tdkRL { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Status3 { get; set; }
        public string StatusVer1 { get; set; }
        public string StatusVer2 { get; set; }
        public List<ViewImageVM> ListImage { get; set; }

    }
    public class dtAreaVM
    {
        public string IDRoundArea { get; set; }
        public string RoundArea { get; set; }
        public decimal Total { get; set; }
        public decimal NilaiA { get; set; }
        public decimal NilaiB { get; set; }
        public decimal NilaiC { get; set; }
        public decimal persenA { get; set; }
        public decimal persenB { get; set; }
        public decimal persenC { get; set; }
        public List<dtKeterangan> listKeteranganA { get; set; }
        public List<dtKeterangan> listKeteranganB { get; set; }
        public List<dtKeterangan> listKeteranganC { get; set; }
    }
    public class dtKeterangan
    {
        //public string IDRoundArea { get; set; }
        //public string Nilai { get; set; }
        public string Keterangan { get; set; }
    }
}