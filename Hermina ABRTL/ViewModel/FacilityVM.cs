using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.ViewModel
{
    public class FacilityVM
    {
        public List<DataTableVM> DataRoundArea { get; set; }
        public List<DataTableVM> DataArea { get; set; }
        public List<DataTableVM> DataSubAreaTypeNNull { get; set; }
        public List<DataTableVM> DataSubAreaTypeNull { get; set; }
        public List<DataTableVM> DataTypeNNull { get; set; }
        public List<DataTableVM> DataOptionNull { get; set; }
        public List<DataTableVM> DataOptionNNull { get; set; }
    }
    public class DataTableVM
    {
        public string RoundArea { get; set; }
        public string IDRoundArea { get; set; }
        public string Area { get; set; }
        public string IDArea { get; set; }
        public string SubArea { get; set; }
        public string IDSubArea { get; set; }
        public string Type { get; set; }
        public string IDType { get; set; }
        public string OptionArea { get; set; }
        public string IDOption { get; set; }
        public string Item { get; set; }
        public string IDItem { get; set; }

    }

    public class DataRoomRLVM
    {
        public string IDType { get; set; }
        public int Lantai { get; set; }
        public int Ruangan { get; set; }
    }
}