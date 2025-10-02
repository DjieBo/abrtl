namespace Hermina_ABRTL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DataStatis
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
        public string IDItem { get; set; }

        [StringLength(100)]
        public string Item { get; set; }

        public string Komponen { get; set; }

        public string Parameter { get; set; }
    }
}
