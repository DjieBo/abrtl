using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class DashboardVM
    {
        public decimal[] PersenDataRound { get; set; }
        public int[] DataNilaiR1 { get; set; }
        public int[] DataNilaiR2 { get; set; }
        public int[] DataNilaiR3 { get; set; }
        public int[] DataNilaiR4 { get; set; }
        public int[] DataParamR1 { get; set; }
        public int[] DataParamR2 { get; set; }
        public int[] DataParamR3 { get; set; }
        public int[] DataParamR4 { get; set; }
        public List<DataLineVM> dtLine { get; set; }
        public List<DataPieSPK> dtPieSPK { get; set; }
        public string NamaRS { get; set; }
    }

    public class DataLineVM
    {
        public decimal Akumulasi { get; set; }
        public string Periode { get; set; }
    }
    public class DataPieSPK {
        public string IDRS { get; set; }
        public string Periode { get; set; }
        public string Round { get; set; }
        public string sumSPK { get; set; }
        public string doneSPK { get; set; }
        public string NTdoneSPK { get; set; }
    }
}