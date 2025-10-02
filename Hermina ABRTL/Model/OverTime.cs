namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OverTime")]
    public partial class OverTime
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string IDRS { get; set; }

        [StringLength(200)]
        public string NamaRS { get; set; }

        [StringLength(100)]
        public string Round { get; set; }

        [StringLength(50)]
        public string Tanggal { get; set; }

        [StringLength(100)]
        public string Submit { get; set; }

        [StringLength(20)]
        public string IDVer1 { get; set; }

        [StringLength(50)]
        public string TimeVer1 { get; set; }

        [StringLength(50)]
        public string Hitung { get; set; }

        [StringLength(20)]
        public string IDVer2 { get; set; }

        [StringLength(50)]
        public string TimeVer2 { get; set; }

        [StringLength(50)]
        public string OverTime1 { get; set; }

        [StringLength(50)]
        public string OverTime2 { get; set; }

        [StringLength(6)]
        public string Periode { get; set; }
    }
}
