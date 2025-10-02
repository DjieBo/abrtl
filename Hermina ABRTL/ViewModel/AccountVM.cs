using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class AccountVM
    {
        public List<AccountRSVM> listAcountRS { get; set; }
        public List<accVM> listAcc { get; set; }
    }
    public class AccountRSVM
    {
        public string IDRS { get; set; }
        public string IDProvinsi { get; set; }
        public string Provinsi { get; set; }
        public string Kota { get; set; }
        public string NamaRS { get; set; }
        public string Phone { get; set; }
        public int IDKota { get; set; }
        public string Alamat { get; set; }
    }
    public class RegisterVM
    {
        public int ID { get; set; }
        public string nameAC { get; set; }
        public string nikAC { get; set; }
        public string provinceAC { get; set; }
        public string regionAC { get; set; }
        public int IDKota { get; set; }
        public string KotaAC { get; set; }
        public string parameterAC { get; set; } //IDRS
        public string NamaRS { get; set; }
        public string positionAC { get; set; }
        public string emailAC { get; set; }
        public string phoneAC { get; set; }
        public string passwordAC { get; set; }
    }
    public class RegisterCheckerVM
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string nik { get; set; }
        public string province { get; set; }
        public string kota { get; set; }
        public int IDKota { get; set; }
        public string KotaAC { get; set; }
        public string parameter { get; set; } //IDRS
        public string NamaRS { get; set; }
        public string position { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
    }
    public class accVM
    {
        public string IDRS { get; set; }
        public string NamaAcc { get; set; }
        public string IDRegAcc { get; set; }
        public string EmailAcc { get; set; }
        public string PhoneAcc { get; set; }
        public string AksesAcc { get; set; }
    }
    public class modifyRS
    {
        public string MregionRS { get; set; }
        public string MprovinceRS { get; set; }
        public string McityRS { get; set; }
        public string MaddressRS { get; set; }
        public string MnameRS { get; set; }
        public string MphoneRS { get; set; }
        public string MnikRS { get; set; }
    }
    public class modifyAcc
    {
        public string provinceMDF { get; set; }
        public string regionMDF { get; set; }
        public string parameterMDF { get; set; }
        public string positionMDF { get; set; }
        public string nameMDF { get; set; }
        public string nikMDF { get; set; }
        public string emailMDF { get; set; }
        public string phoneMDF { get; set; }
        public string passwordMDF { get; set; }
    }
}