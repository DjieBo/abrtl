namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PenilaianRAB")]
    public partial class PenilaianRAB
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string IDRS { get; set; }

        [Required]
        [StringLength(50)]
        public string IDRound { get; set; }

        [Required]
        [StringLength(50)]
        public string DateSubmit { get; set; }

        [StringLength(25)]
        public string StatusData { get; set; }

        [StringLength(50)]
        public string IDRegVerify1 { get; set; }

        [StringLength(50)]
        public string Verifikasi1Date { get; set; }

        [StringLength(50)]
        public string Status1 { get; set; }

        [Column(TypeName = "text")]
        public string KetVerifikator1 { get; set; }

        [StringLength(50)]
        public string IDRegVerify2 { get; set; }

        [StringLength(50)]
        public string Verifikasi2Date { get; set; }

        [StringLength(50)]
        public string Status2 { get; set; }

        [Column(TypeName = "text")]
        public string KetVerifikator2 { get; set; }

        [StringLength(6)]
        public string Periode { get; set; }
    }
}
