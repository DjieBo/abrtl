using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class UserVM
    {
    }

    public class loginVM
    {
        public int ID { get; set; }
        public string IDRegister { get; set; }
        public string IDRS { get; set; }
        public string NamaRS { get; set; }
        public string Email { get; set; }
        public string KodeAkses { get; set; }
        public string Regional { get; set; }
        public string Status { get; set; }
        public string LogLogin { get; set; }
    }
    public class AlamatVM
    {
        public string IDProvinsi { get; set; }
        public int IDKota { get; set; }
        public string IDCabang { get; set; }
        public string IDRS { get; set; }
        public string Provinsi { get; set; }
        public string Kota { get; set; }
        public string Cabang { get; set; }
        public string NamaRS { get; set; }
    }
    public class IdentitasRSVM
    {
        public string IDRS { get; set; }
        public string IDProvinsi { get; set; }
        public string Provinsi { get; set; }
        public string Kota { get; set; }
        public string NamaRS { get; set; }
        public string Phone { get; set; }
        public int? IDKota { get; set; }
        public string Alamat { get; set; }
        public string region { get; set; }
    }
}