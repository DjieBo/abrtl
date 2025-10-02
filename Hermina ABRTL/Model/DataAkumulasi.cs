namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DataAkumulasi")]
    public partial class DataAkumulasi
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string IDRS { get; set; }

        [StringLength(50)]
        public string NamaRS { get; set; }

        [StringLength(15)]
        public string TotalData { get; set; }

        [StringLength(15)]
        public string Akumulasi { get; set; }

        [StringLength(50)]
        public string Periode { get; set; }
    }
}
