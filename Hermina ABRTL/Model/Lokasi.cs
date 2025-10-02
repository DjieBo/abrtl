namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lokasi")]
    public partial class Lokasi
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string IDProvinsi { get; set; }

        [StringLength(50)]
        public string Provinsi { get; set; }

        [StringLength(50)]
        public string Kota { get; set; }
    }
}
