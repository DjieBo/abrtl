namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MUser")]
    public partial class MUser
    {
        public int ID { get; set; }

        [StringLength(25)]
        public string Status { get; set; }

        [StringLength(50)]
        public string LogLogin { get; set; }

        [Required]
        [StringLength(15)]
        public string IDRegister { get; set; }

        [StringLength(10)]
        public string IDRS { get; set; }

        [StringLength(150)]
        public string Nama { get; set; }

        [StringLength(50)]
        public string KodeAkses { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(25)]
        public string NoHP { get; set; }

        [Required]
        [StringLength(15)]
        public string Password { get; set; }

        [StringLength(100)]
        public string KetReg { get; set; }
    }
}
