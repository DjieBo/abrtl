namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DaftarRSIA")]
    public partial class DaftarRSIA
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string IDProvinsi { get; set; }

        public int? IDKota { get; set; }

        [StringLength(50)]
        public string Regional { get; set; }

        [StringLength(500)]
        public string Alamat { get; set; }

        [StringLength(10)]
        public string IDRS { get; set; }

        [StringLength(300)]
        public string NamaRS { get; set; }

        [StringLength(20)]
        public string Telephon { get; set; }
    }
}
