namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DataDinamis
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(50)]
        public string Round { get; set; }

        [StringLength(50)]
        public string IDRoundArea { get; set; }

        [StringLength(100)]
        public string RoundArea { get; set; }

        [StringLength(50)]
        public string IDArea { get; set; }

        [StringLength(100)]
        public string Area { get; set; }

        [StringLength(50)]
        public string IDSubArea { get; set; }

        [StringLength(100)]
        public string SubArea { get; set; }

        [StringLength(50)]
        public string IDType { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        [StringLength(50)]
        public string IDOption { get; set; }

        [StringLength(100)]
        public string OptionArea { get; set; }

        [StringLength(50)]
        public string IDItem { get; set; }

        [StringLength(200)]
        public string Item { get; set; }

        public string Komponen { get; set; }

        public string Parameter { get; set; }
    }
}
