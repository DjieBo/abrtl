using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class PeerGroupVM
    {
        public List<LogVerifyVM> DataVerifikasi { get; set; }
        public List<ListDataRS> DataRS { get; set; }
    }
    public class ListDataRS
    {
        public string IDRS { get; set; }
        public string NamaRS { get; set; }

    }
    public class LogVerifyVM
    {
        public string IDRS { get; set; }
        public string Periode { get; set; }
        public string Round { get; set; }
        public string DateSubmit { get; set; }
        public string StatusData { get; set; }
        public string IDVerify1 { get; set; }
        public string DateVerify1 { get; set; }
        public string KetVerify1 { get; set; }
        public string Status1 { get; set; }
        public string IDVerify2 { get; set; }
        public string DateVerify2 { get; set; }
        public string KetVerify2 { get; set; }
        public string Status2 { get; set; }
    }
}