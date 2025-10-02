using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class RABVM
    {
        public List<SubmitRABVM> listSubmitRAB { get; set; }

    }
    public class RABListVM
    {
        public List<RABCheck> listCheck { get; set; }
        public List<CheckRound> listCheckerVal { get; set; }
        public string Akses { get; set; }
    }
    public class CheckRound
    {
        public string Round { get; set; }
        public string StatusChecker { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Periode { get; set; }
    }
    public class RABCheck
    {
        public string KodeAkses { get; set; }
        public string Round { get; set; }
        public string StatusChecker { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Verifikasi2Date { get; set; }
        public string Periode { get; set; }
        public string StatusUmum { get; set; }
        public string StatusPenilaian1 { get; set; }
        public string StatusPenilaian2 { get; set; }
        public string StatusPenilaian3 { get; set; }
        public string Approve { get; set; }
        public string ApproveDate { get; set; }

    }
    public class SubmitRABVM
    {
        public int ID { get; set; }
        public string Round { get; set; }
        public string SubArea { get; set; }
        public string Item { get; set; }
        public string Penilaian { get; set; }
        public string Nilai { get; set; }
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
        public List<DtKategoriBahanVM> LsKategoriBrg { get; set; }
    }

    public class ExtReportRABVM
    {
        public List<DataReportRABVM> listData { get; set; }
        public List<DataImageRAB> listDataImage { get; set; }
        public string Round { get; set; }
        public string IDRS { get; set; }
        public string NamaRS { get; set; }
        public string Periode { get; set; }
        public string Checker { get; set; }
        public string WakilDir { get; set; }
        public string Direktur { get; set; }
        public string StatusVer1 { get; set; }
        public string StatusVer2 { get; set; }
        public string Validator1 { get; set; }
        public string Validator2 { get; set; }
        public string Validator3 { get; set; }
        public string Approve1 { get; set; }
        public string Approve2 { get; set; }
        public string Approve3 { get; set; }
        public string UserOnLogin { get; set; }
    }
    public class DataImageRAB {
        public string RoundIMG { get; set; }
        public string SubAreaIMG { get; set; }
        public string ItemIMG { get; set; }
        public string KeteranganIMG { get; set; }
        public string PenilaianIMG { get; set; }
        public string BeforeIMG { get; set; }
        public string AfterIMG { get; set; }
    }

    public class DataReportRABVM
    {
        public int ID { get; set; }
        public string Round { get; set; }
        public string SubArea { get; set; }
        public string Item { get; set; }
        public string Penilaian { get; set; }
        public string Nilai { get; set; }
        public string IDUnix { get; set; }
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
    public class BackRABRowDataVM
    {
        public int ID { get; set; }
        public string Round { get; set; }
        public string SubArea { get; set; }
        public string Item { get; set; }
        public string Penilaian { get; set; }
        public string Nilai { get; set; }
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

    public class DtKategoriBahanVM
    {
        public string IDKategori { get; set; }
        public string Kategori { get; set; }
    }

    public class DtItemBahanVM
    {
        public string IDKategori { get; set; }
        public string Kategori { get; set; }
        public string IDItem { get; set; }
        public string Item { get; set; }
    }

    public class DtHargaBahanVM
    {
        public string IDKategori { get; set; }
        public string Kategori { get; set; }
        public string IDItem { get; set; }
        public string Item { get; set; }
        public string TypeDes { get; set; }
        public decimal Harga { get; set; }
        public int ID { get; set; }
    }

    public class RABValue {
        public string Budget { get; set; }
        public string PayOut { get; set; }
        public string Quareel { get; set; }
    }
    public class DataSPK {
        public List<ListDataSPK> ListSPK { get; set; }
        public List<ListApproveSPK> listAppSPK { get; set; }
        public string NamaRS { get; set; }
        public string Round { get; set; }
        public string Periode { get; set; }
        public string TimeStart { get; set; }
    }
    public class ListApproveSPK {
        public string RoundApp { get; set; }
        public string PeriodeApp { get; set; }
        public string TimeStartApp { get; set; }
    }
    public class ListDataSPK {
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
        public string IDUnix { get; set; }
        public string KomponenPerbaiki { get; set; }
        public string ItemPerbaiki { get; set; }
        public string DeskripsiPerbaiki { get; set; }
        public string HargaDasar { get; set; }
        public string HargaInput { get; set; }
        public string SelisihHarga { get; set; }
    }
}