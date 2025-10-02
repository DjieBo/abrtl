namespace Hermina_ABRTL.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BuildingContext : DbContext
    {
        public BuildingContext()
            : base("name=BuildingContext")
        {
        }

        public virtual DbSet<ApprovalRAB> ApprovalRAB { get; set; }
        public virtual DbSet<DaftarRSIA> DaftarRSIA { get; set; }
        public virtual DbSet<DataAkumulasi> DataAkumulasi { get; set; }
        public virtual DbSet<DataDinamis> DataDinamis { get; set; }
        public virtual DbSet<DataHarga> DataHarga { get; set; }
        public virtual DbSet<DataLog> DataLog { get; set; }
        public virtual DbSet<DataReport> DataReport { get; set; }
        public virtual DbSet<DataStatis> DataStatis { get; set; }
        public virtual DbSet<Lokasi> Lokasi { get; set; }
        public virtual DbSet<MUser> MUser { get; set; }
        public virtual DbSet<OverTime> OverTime { get; set; }
        public virtual DbSet<PenilaianRAB> PenilaianRAB { get; set; }
        public virtual DbSet<RValueApproval> RValueApproval { get; set; }
        public virtual DbSet<RValueVerifikator> RValueVerifikator { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataHarga>()
                .Property(e => e.IDKategori)
                .IsUnicode(false);

            modelBuilder.Entity<DataHarga>()
                .Property(e => e.Kategori)
                .IsUnicode(false);

            modelBuilder.Entity<DataHarga>()
                .Property(e => e.IDItem)
                .IsUnicode(false);

            modelBuilder.Entity<DataHarga>()
                .Property(e => e.Item)
                .IsUnicode(false);

            modelBuilder.Entity<DataHarga>()
                .Property(e => e.TypeDes)
                .IsUnicode(false);

            modelBuilder.Entity<DataHarga>()
                .Property(e => e.HargaDKI)
                .HasPrecision(18, 3);

            modelBuilder.Entity<DataHarga>()
                .Property(e => e.HargaJawa)
                .HasPrecision(18, 3);

            modelBuilder.Entity<DataHarga>()
                .Property(e => e.HargaNJawa)
                .HasPrecision(18, 3);

            modelBuilder.Entity<MUser>()
                .Property(e => e.IDRegister)
                .IsUnicode(false);

            modelBuilder.Entity<MUser>()
                .Property(e => e.Nama)
                .IsUnicode(false);

            modelBuilder.Entity<MUser>()
                .Property(e => e.KodeAkses)
                .IsUnicode(false);

            modelBuilder.Entity<MUser>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<MUser>()
                .Property(e => e.NoHP)
                .IsUnicode(false);

            modelBuilder.Entity<MUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<MUser>()
                .Property(e => e.KetReg)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.IDRS)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.NamaRS)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.Round)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.Tanggal)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.Submit)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.IDVer1)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.TimeVer1)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.Hitung)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.IDVer2)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.TimeVer2)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.OverTime1)
                .IsUnicode(false);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.OverTime2)
                .IsUnicode(false);

            modelBuilder.Entity<PenilaianRAB>()
                .Property(e => e.KetVerifikator1)
                .IsUnicode(false);

            modelBuilder.Entity<PenilaianRAB>()
                .Property(e => e.KetVerifikator2)
                .IsUnicode(false);

            modelBuilder.Entity<RValueVerifikator>()
                .Property(e => e.KetVerifikator1)
                .IsUnicode(false);

            modelBuilder.Entity<RValueVerifikator>()
                .Property(e => e.KetVerifikator2)
                .IsUnicode(false);
        }
    }
}
