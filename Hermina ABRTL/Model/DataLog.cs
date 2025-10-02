namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DataLog")]
    public partial class DataLog
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string Bulan { get; set; }

        [StringLength(50)]
        public string DateInsert { get; set; }

        [StringLength(10)]
        public string IDRS { get; set; }

        [StringLength(50)]
        public string Round { get; set; }

        public string Data { get; set; }

        [StringLength(50)]
        public string Periode { get; set; }
    }
}
