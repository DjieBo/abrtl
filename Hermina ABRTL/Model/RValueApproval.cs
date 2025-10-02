namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RValueApproval")]
    public partial class RValueApproval
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string IDRS { get; set; }

        [StringLength(50)]
        public string IDRound { get; set; }

        [Required]
        [StringLength(50)]
        public string DateVerify { get; set; }

        [StringLength(20)]
        public string StatusData { get; set; }

        [StringLength(50)]
        public string IDRegApprov1 { get; set; }

        [StringLength(50)]
        public string Approval1Date { get; set; }

        [StringLength(50)]
        public string Status1 { get; set; }

        [StringLength(50)]
        public string IDRegApprov2 { get; set; }

        [StringLength(50)]
        public string Approval2Date { get; set; }

        [StringLength(50)]
        public string Status2 { get; set; }

        [StringLength(50)]
        public string IDRegApprov3 { get; set; }

        [StringLength(50)]
        public string Approval3Date { get; set; }

        [StringLength(50)]
        public string Status3 { get; set; }

        [StringLength(6)]
        public string Periode { get; set; }
    }
}
