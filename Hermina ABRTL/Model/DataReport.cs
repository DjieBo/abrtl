namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DataReport")]
    public partial class DataReport
    {
        public int ID { get; set; }

        [StringLength(8)]
        public string Periode { get; set; }

        [StringLength(8)]
        public string IDRS { get; set; }

        [StringLength(10)]
        public string Round { get; set; }

        [StringLength(15)]
        public string Data { get; set; }
    }
}
