using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class ValidatorVM
    {
        public List<RegionVm> ListRegion { get; set; }
        public List<BiodataRSValVM> ListRS { get; set; }
        public List<ValidasiRound> ValidasiVal { get; set; }
        public List<ValidateCondition> ValidasiRAB { get; set; }
        public List<SPKCondition> listSPK { get; set; }
        public List<ReportSPK> listReportSPK { get; set; }
        public List<validatePeriodeEX> ListPeriode { get; set; }
        public List<validatePeriodeRAB> ListPeriodeRAB { get; set; }
        public List<validatePeriodeSPK> ListPeriodeSPK { get; set; }
        public string TimeStart { get; set; }
    }
    public class validatePeriodeSPK
    {
        public string Periode { get; set; }
    }
    public class validatePeriodeRAB {
        public string Periode { get; set; }
    }
    public class validatePeriodeEX
    {
        public string Periode { get; set; }
    }
    public class ReportSPK
    {
        public int ID { get; set; }
        public string Round { get; set; }
        public string SubArea { get; set; }
        public string Item { get; set; }
        public string Penilaian { get; set; }
        public string Nilai { get; set; }
        public string Status { get; set; }
        public string Keterangan { get; set; }
        public string PictureBf { get; set; }
        public string PictureAf { get; set; }
        public string Periode { get; set; }
        public string KomponenPerbaiki { get; set; }
        public string ItemPerbaiki { get; set; }
        public string DeskripsiPerbaiki { get; set; }
        public string HargaDasar { get; set; }
        public string HargaInput { get; set; }
        public string SelisihHarga { get; set; }
    }
    public class SPKCondition {
        public string IDRS { get; set; }
        public string Round { get; set; }
        public string Periode { get; set; }
        public string Status { get; set; }
    }
    public class ValidateCondition {
        public string PeriodeRAB { get; set; }
        public string RoundRAB { get; set; }
        public string IDRSRAB { get; set; }
    }
    public class ValidasiRound {
        public string Periode { get; set; }
        public string Round { get; set; }
        public string IDRS { get; set; }
    }
    public class RegionVm
    {
        public string IDRegion { get; set; }
        public string Region { get; set; }
    }
    public class BiodataRSValVM
    {
        public string Region { get; set; }
        public string IDRS { get; set; }
        public string NamaRS { get; set; }
        public string Periode { get; set; }
    }
    public class DataApproveValVM
    {
        public string IDRS { get; set; }
        public string IDRound { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Status3 { get; set; }
    }
}