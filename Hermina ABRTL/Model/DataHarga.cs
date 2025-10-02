namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DataHarga")]
    public partial class DataHarga
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string IDKategori { get; set; }

        [StringLength(150)]
        public string Kategori { get; set; }

        [Required]
        [StringLength(20)]
        public string IDItem { get; set; }

        [StringLength(150)]
        public string Item { get; set; }

        [StringLength(150)]
        public string TypeDes { get; set; }

        public decimal? HargaDKI { get; set; }

        public decimal? HargaJawa { get; set; }

        public decimal? HargaNJawa { get; set; }
    }
}
