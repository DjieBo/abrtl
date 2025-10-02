using Hermina_ABRTL.clsLogict;
using Hermina_ABRTL.Model;
using Hermina_ABRTL.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.DAL
{
    public class UserDAL
    {
        public static string Connection = ConfigurationManager.ConnectionStrings["BuildingContext"].ToString();

        public static loginVM cekUser(string UserName, string Pass, out string strErr, out int dt)
        {
            loginVM data = new loginVM();
            strErr = ""; dt = 0;
            //string sqlCommand = ""; DataSet ds = new DataSet();
            //bool res = false;
            try
            {
                #region CaraLQ
                using (BuildingContext db = new BuildingContext())
                {
                    data = (from a in db.MUser
                            join b in db.DaftarRSIA on a.IDRS equals b.IDRS
                            where a.IDRegister == UserName & a.Password == Pass
                            select new loginVM()
                            {
                                ID = a.ID,
                                Status = a.Status,
                                IDRS = a.IDRS,
                                NamaRS = b.NamaRS,
                                Email = a.Email,
                                IDRegister = a.IDRegister,
                                KodeAkses = a.KodeAkses,
                                LogLogin = a.LogLogin,
                                Regional = a.KetReg
                            }).FirstOrDefault();

                    if (data == null)
                    {
                        strErr = "Incorrect UserID or Password.!";
                        return data;
                    }
                    else if (data.Status == "Login")
                    {
                        strErr = "Maaf ID anda berstatus Aktif, silahkan Reset terlebih dahulu.!";
                        return data;
                    }
                    else if (data.IDRS == "00000")
                    {
                        return data;
                    }
                    else if (data.IDRS != null)
                    {
                        string sqlCommand = "select count(ID) as dt from RSIA" + data.IDRS;
                        DataSet ds = new DataSet();
                        bool blnRes = false;
                        using (clsDBSQLConnection dbs = new clsDBSQLConnection())
                        {
                            blnRes = dbs.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                        }
                        if (blnRes)
                        {
                            if (ds != null & ds.Tables.Count > 0 & ds.Tables[0].Rows.Count > 0)
                            {
                                dt = int.Parse(ds.Tables[0].Rows[0]["dt"].ToString());
                            }
                        }
                        return data;
                    }
                }
                #endregion       
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
                return data;
            }
            return data;
        }
        public static bool UpdateDataLogin(int ID, string Sesion, out string strErr)
        {
            bool res = false;
            strErr = "";
            using (BuildingContext db = new BuildingContext())
            {
                var item = db.MUser.Find(ID);
                item.Status = "Login";
                item.LogLogin = Sesion;
                try
                {
                    db.SaveChanges();
                    res = true;
                }
                catch (Exception ex)
                {
                    strErr = "Error #100: " + ex.Message;
                    res = false;
                }
            }
            return res;
        }
        public static bool resertLogin(string IDReg)
        {
            bool result = false;
            string sqlQommand = "", err = "";

            sqlQommand = " update MUser set Status = null where IDRegister = '"+ IDReg+"'";
            using (clsDBSQLConnection db = new clsDBSQLConnection())
            {
                result = db.QueryCommand(Connection, sqlQommand, out err);
            }
            return result;
        }
        public static bool registerAccount(string Provinsi, string Region, string IDRS, string Nama, string Email,
                                           string IDRegister, string Jabatan, string Phone, string Password, out string strErr)
        {
            strErr = "";
            string sqlCommand = "";
            bool Result = false;
            DataSet ds = new DataSet();
            try
            {
                sqlCommand = @"
                                declare @intID int,
		                                @strError varchar(115),
		                                @strKodeAkses varchar(50),
                                        @intVer int

                                set @intID = (select count(IDRegister) from MUser where IDRegister = @idRegister )
                                
                                if @intID != 0
                                 begin
                                  set @strError = 'ID Register sudah terdaftar'
                                  goto ERROR
                                 end

                                set @strKodeAkses = @kodeaks

                                if @strKodeAkses != 'Checker'
	                                 begin
	                                  set @intVer = (select count(KodeAkses) from MUser where IDRS = @idrs and KodeAkses = @kodeaks)
	                                  if @intVer != 0
		                                begin
			                                set @strError = 'Jabatan sudah terdaftar'
			                                goto ERROR
		                                end
	                                 end

                                if @strKodeAkses = 'Validator 2'
                                 begin 
                                  insert into MUser(IDRegister, IDRS, Nama, KodeAkses, Email, NoHP, Password, KetReg)
                                  values(@idRegister , @idrs, @nama, @kodeaks, @email, @hp, @pass, @region)
                                 end
                                else
                                 begin
                                  insert into MUser(IDRegister, IDRS, Nama, KodeAkses, Email, NoHP, Password)
                                  values(@idRegister , @idrs, @nama, @kodeaks, @email, @hp, @pass)
                                 end

                                ERROR:
                                select @strError as Validasi
                                ";

                SqlParameter[] dbParam = new SqlParameter[8];
                dbParam[0] = new SqlParameter("@idRegister", IDRegister);
                dbParam[1] = new SqlParameter("@idrs", IDRS);
                dbParam[2] = new SqlParameter("@nama", Nama);
                dbParam[3] = new SqlParameter("@kodeaks", Jabatan);
                dbParam[4] = new SqlParameter("@email", Email);
                dbParam[5] = new SqlParameter("@hp", Phone);
                dbParam[6] = new SqlParameter("@pass", Password);
                dbParam[7] = new SqlParameter("@region", Region);

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    Result = db.QueryCommand(Connection, sqlCommand, ref dbParam, out ds, out strErr);
                }

                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["Validasi"].ToString() == "ID Register sudah terdaftar")
                    {
                        Result = false;
                        strErr = "ID Register : " + IDRegister + " sudah terdaftar";
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
            }
            return Result;
        }
        public static bool updateRegisterAccount(string Email, string IDRegister, string Phone, string Password, out string strErr)
        {
            strErr = "";
            string sqlCommand = "";
            bool Result = false;
            try
            {
                sqlCommand = "update Muser set Email = '" + Email +"', NoHP = '" + Phone +"', Password = '" + Password+"' where IDRegister = '" + IDRegister+"'";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    Result = db.QueryCommand(Connection, sqlCommand, out strErr);
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
            }
            return Result;
        }
        public static bool deleteRegisterAccount(string IDRegister, out string strErr)
        {
            strErr = "";
            string sqlCommand = "";
            bool Result = false;
            DataSet ds = new DataSet();
            try
            {
                sqlCommand = "Delete from MUser where IDRegister = @idRegister";

                SqlParameter[] dbParam = new SqlParameter[1];
                dbParam[0] = new SqlParameter("@idRegister", IDRegister);

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    Result = db.QueryCommand(Connection, sqlCommand, ref dbParam, out ds, out strErr);
                }

            }
            catch (Exception ex)
            {
                strErr = ex.Message;
            }
            return Result;
        }
        public static bool registerRS(string ProvinsiRS, string RegionRS, string CityRS, string AddressRS, string IDRSRS,
                                    string RSNameRS, string PhoneRS, out string strErr)
        {
            strErr = "";
            string sqlCommand = "";
            bool Result = false;
            DataSet ds = new DataSet();
            try
            {
                #region Querry
                sqlCommand = @"         
                                declare @intID int,
		                                @strError varchar(115)

                                set @intID = (select count(IDRS) from DaftarRSIA where IDRS = @idrs)
                                if @intID != 0
                                 begin
	                                set @strError = 'ID Rumah Sakit sudah terdaftar'
                                    goto ERROR
                                 end

                                CREATE TABLE [dbo].[RSIA" + IDRSRS + @"](
	                                [ID] [int] IDENTITY(1,1) NOT NULL,
	                                [Round] [nvarchar](50) NULL,
	                                [IDRoundArea] [nvarchar](50) NULL,
	                                [RoundArea] [nvarchar](100) NULL,
	                                [IDArea] [nvarchar](50) NULL,
	                                [Area] [nvarchar](100) NULL,
	                                [IDSubArea] [nvarchar](50) NULL,
	                                [SubArea] [nvarchar](100) NULL,
	                                [IDType] [nvarchar](50) NULL,
	                                [Type] [nvarchar](100) NULL,
	                                [IDOption] [nvarchar](50) NULL,
	                                [OptionArea] [nvarchar](100) NULL,
	                                [IDItem] [nvarchar](50) NULL,
	                                [Item] [nvarchar](200) NULL,
	                                [Komponen] [nvarchar](max) NULL,
                                    [Parameter] [nvarchar](100) NULL,
	                                [IDChecker] [varchar](50) NULL,
	                                [Nilai] [varchar](50) NULL,
	                                [Status] [varchar](50) NULL,
                                    [ParamValue] [nvarchar](100) NULL,
	                                [Ket] [varchar](max) NULL,
	                                [IDUnix] [varchar](75) NULL,
	                                [Periode] [nvarchar](8) NULL,
                                 CONSTRAINT [PK_RSIA" + IDRSRS + @"] PRIMARY KEY CLUSTERED 
                                (
	                                [ID] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                                --GO                                

                                CREATE TABLE [dbo].[RABRS" + IDRSRS + @"](
	                                [ID] [int] IDENTITY(1,1) NOT NULL,
	                                [Round] [nvarchar](50) NULL,
	                                [IDRoundArea] [nvarchar](50) NULL,
	                                [RoundArea] [nvarchar](100) NULL,
	                                [IDArea] [nvarchar](50) NULL,
	                                [Area] [nvarchar](100) NULL,
	                                [IDSubArea] [nvarchar](50) NULL,
	                                [SubArea] [nvarchar](100) NULL,
	                                [IDType] [nvarchar](50) NULL,
	                                [Type] [nvarchar](100) NULL,
	                                [IDOption] [nvarchar](50) NULL,
	                                [OptionArea] [nvarchar](100) NULL,
	                                [IDItem] [nvarchar](50) NULL,
	                                [Item] [nvarchar](200) NULL,
	                                [Komponen] [nvarchar](max) NULL,
                                    [Parameter] [nvarchar](100) NULL,
	                                [IDChecker] [varchar](50) NULL,
	                                [Nilai] [varchar](50) NULL,
	                                [Status] [varchar](50) NULL,
                                    [ParamValue] [nvarchar](100) NULL,
	                                [Ket] [varchar](max) NULL,
	                                [IDUnix] [varchar](75) NOT NULL,
	                                [JPG] [varchar](max) NULL,
                                    [JPGAfter] [varchar](max) NULL,
                                    [KomponenPerbaikan] [varchar](900) NULL,
                                    [ItemPerbaikan] [varchar](900) NULL,
                                    [DeskripsiPerbaikan] [varchar](900) NULL,
                                    [HargaDasar] [decimal](18, 2) NULL, 
                                    [HargaInput] [decimal](18, 2) NULL,
                                    [SelisihHarga] [decimal](18, 2) NULL,
	                                [Periode] [varchar](8) NULL,
                                 CONSTRAINT [PK_RABRS" + IDRSRS + @"] PRIMARY KEY CLUSTERED 
                                (
	                                [ID] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                                --GO                               

                                insert into DaftarRSIA(IDProvinsi, IDKota, Regional, Alamat, IDRS, NamaRS, Telephon)
                                values(@idProv, @idKota, @Region, @alamat, @idrs, @nama, @telp)

                                ERROR:
                                 select @strError as Validasi
                                ";
                #endregion

                SqlParameter[] dbParam = new SqlParameter[7];
                dbParam[0] = new SqlParameter("@idProv", ProvinsiRS);
                dbParam[1] = new SqlParameter("@idKota", CityRS);
                dbParam[2] = new SqlParameter("@Region", RegionRS);
                dbParam[3] = new SqlParameter("@alamat", AddressRS);
                dbParam[4] = new SqlParameter("@idrs", IDRSRS);
                dbParam[5] = new SqlParameter("@nama", RSNameRS);
                dbParam[6] = new SqlParameter("@telp", PhoneRS);

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    Result = db.QueryCommand(Connection, sqlCommand, ref dbParam, out ds, out strErr);
                }

                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0][""].ToString() == "ID Rumah Sakit sudah terdaftar")
                    {
                        strErr = "ID Rumah Sakit : " + IDRSRS + " sudah terdaftar";
                        Result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
            }
            return Result;
        }
        public static bool updateRegisterRS(string MAddressRS, string MIDRSRS, string MRSNameRS, string MPhoneRS, out string strErr)
        {
            strErr = "";
            string sqlCommand = "";
            bool Result = false;
            try
            {
                sqlCommand = "update DaftarRSIA set Alamat = '" + MAddressRS + "', NamaRS = '" + MRSNameRS + "', Telephon = '" + MPhoneRS + "' where IDRS = '" + MIDRSRS + "'";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    Result = db.QueryCommand(Connection, sqlCommand, out strErr);
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
            }
            return Result;
        }
        public static bool deleteRegisterRS(string IDRS, out string strErr)
        {
            bool result = false;
            strErr = "";
            string sqlCommand = "";
            try
            {
                sqlCommand = @"
                                DROP TABLE RSIA"+ IDRS + @" 
                                DROP TABLE RABRS" + IDRS + @" 
                                DELETE FROM MUser WHERE IDRS = '" + IDRS + @"' 
                                DELETE FROM RValueApproval WHERE IDRS = '" + IDRS + @"' 
                                DELETE FROM RValueVerifikator WHERE IDRS = '" + IDRS + @"' 
                                DELETE FROM DataLog WHERE IDRS = '" + IDRS + @"' 
                                DELETE FROM DataAkumulasi WHERE IDRS = '" + IDRS + @"' 
                                DELETE FROM DataReport WHERE IDRS = '" + IDRS + @"' 
                                DELETE FROM OverTime WHERE IDRS = '" + IDRS + @"' 
                                DELETE FROM DaftarRSIA WHERE IDRS = '" + IDRS + @"'
                                ";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out strErr);
                }
            }
            catch (Exception ex)
            {
                result = false;
                strErr = ex.Message;
            }
            return result;
        }
        public static List<AlamatVM> GetProvinsi()
        {
            string conn = ConfigurationManager.ConnectionStrings["BuildingContext"].ToString();
            string strErr = "", sqlCommand = "";
            DataSet dsOut = new DataSet();
            List<AlamatVM> result = new List<AlamatVM>();
            try
            {
                bool blnResult = false;
                sqlCommand = "select distinct IDProvinsi, Provinsi from dbo.Lokasi ";
                using (clsDBSQLConnection _clsDBSQLConnection = new clsDBSQLConnection())
                {
                    blnResult = _clsDBSQLConnection.QueryCommand(conn, sqlCommand, out dsOut, out strErr);
                }

                if (blnResult == true && dsOut != null)
                {
                    AlamatVM tmpAlamat = new AlamatVM();
                    result = new List<AlamatVM>();
                    foreach (DataRow data in dsOut.Tables[0].Rows)
                    {
                        tmpAlamat = new AlamatVM();
                        tmpAlamat.IDProvinsi = data["IDProvinsi"].ToString();
                        tmpAlamat.Provinsi = data["Provinsi"].ToString();
                        result.Add(tmpAlamat);
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message + Environment.NewLine + ex.StackTrace.ToString();
                result = new List<AlamatVM>();
            }
            return result;
        }
        public static List<AlamatVM> GetKota(int IDProvinsi)
        {
            string conn = ConfigurationManager.ConnectionStrings["BuildingContext"].ToString();
            string strErr = "", sqlCommand = "";
            DataSet dsOut = new DataSet();
            List<AlamatVM> result = new List<AlamatVM>();
            try
            {
                bool blnResult = false;
                if (IDProvinsi == 0)
                {
                    sqlCommand = "select IDProvinsi, ID, Kota from dbo.Lokasi";
                }
                else
                {
                    sqlCommand = "select IDProvinsi, ID, Kota from dbo.Lokasi  where IDProvinsi = @IDProvinsi";
                }

                SqlParameter[] dbParam = new SqlParameter[1];
                dbParam[0] = new SqlParameter("@IDProvinsi", SqlDbType.Int);
                dbParam[0].Value = IDProvinsi;
                dbParam[0].Direction = ParameterDirection.Input;

                using (clsDBSQLConnection _clsDBSQLConnection = new clsDBSQLConnection())
                {
                    blnResult = _clsDBSQLConnection.QueryCommand(conn, sqlCommand, ref dbParam, out dsOut, out strErr);
                }

                if (blnResult == true && dsOut != null)
                {
                    AlamatVM tmpAlamat = new AlamatVM();
                    result = new List<AlamatVM>();
                    foreach (DataRow data in dsOut.Tables[0].Rows)
                    {
                        tmpAlamat = new AlamatVM();
                        tmpAlamat.IDProvinsi = data["IDProvinsi"].ToString();
                        tmpAlamat.IDKota = int.Parse(data["ID"].ToString());
                        tmpAlamat.Kota = data["Kota"].ToString();
                        result.Add(tmpAlamat);
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message + Environment.NewLine + ex.StackTrace.ToString();
                result = new List<AlamatVM>();
            }
            return result;
        }
        public static IdentitasRSVM GetIdentitasRS(string IDRS)
        {
            IdentitasRSVM Ressult = new IdentitasRSVM();
            using (BuildingContext db = new BuildingContext())
            {
                Ressult = (from a in db.DaftarRSIA
                           join b in db.Lokasi on a.IDKota equals b.ID
                           where a.IDRS == IDRS
                           select new IdentitasRSVM()
                           {
                               Alamat = a.Alamat,
                               IDKota = a.IDKota,
                               IDProvinsi = a.IDProvinsi,
                               IDRS = IDRS,
                               Kota = b.Kota,
                               NamaRS = a.NamaRS,
                               Phone = a.Telephon,
                               Provinsi = b.Provinsi,
                               region = a.Regional
                           }).FirstOrDefault();
            }
            return Ressult;
        }
        public static AccountVM getListDataAcountRS()
        {
            AccountVM result = new AccountVM();
            List<AccountRSVM> listModel = new List<AccountRSVM>();
            List<accVM> listAcc = new List<accVM>();
            using (BuildingContext db = new BuildingContext())
            {
                listModel = (from a in db.DaftarRSIA
                             join b in db.Lokasi on a.IDKota equals b.ID
                             select new AccountRSVM()
                             {
                                 IDRS = a.IDRS,
                                 IDProvinsi = a.IDProvinsi,
                                 Provinsi = b.Provinsi,
                                 IDKota = b.ID,
                                 Kota = b.Kota,
                                 Alamat = a.Alamat,
                                 NamaRS = a.NamaRS,
                                 Phone = a.Telephon
                             }).ToList();

                result.listAcountRS = listModel;

                listAcc = (from a in db.MUser
                           where a.IDRS != null
                           select new accVM()
                           {
                               AksesAcc = a.KodeAkses,
                               EmailAcc = a.Email,
                               IDRegAcc = a.IDRegister,
                               NamaAcc = a.Nama,
                               PhoneAcc = a.NoHP,
                               IDRS = a.IDRS
                           }).ToList();
                result.listAcc = listAcc;
            }
            return result;
        }
        public static modifyRS GetModifyRS(string IDRS)
        {
            modifyRS result = new modifyRS();
            using (BuildingContext db = new BuildingContext())
            {
                result = (from a in db.DaftarRSIA
                          join b in db.Lokasi on a.IDKota equals b.ID
                          where a.IDRS == IDRS
                          select new modifyRS()
                          {
                              MaddressRS = a.Alamat,
                              McityRS = b.Kota,
                              MnameRS = a.NamaRS,
                              MnikRS = a.IDRS,
                              MphoneRS = a.Telephon,
                              MprovinceRS = b.Provinsi,
                              MregionRS = a.Regional
                          }).FirstOrDefault();
            }
            return result;
        }
        public static modifyAcc GetModifyAcc(string IDReg)
        {
            modifyAcc result = new modifyAcc();
            using (BuildingContext db = new BuildingContext())
            {
                result = (from a in db.MUser
                          join b in db.DaftarRSIA on a.IDRS equals b.IDRS
                          join c in db.Lokasi on b.IDKota equals c.ID
                          where a.IDRegister == IDReg
                          select new modifyAcc()
                          {
                              emailMDF = a.Email,
                              nameMDF = a.Nama,
                              nikMDF = a.IDRegister,
                              parameterMDF = a.IDRS,
                              passwordMDF = a.Password,
                              phoneMDF = a.NoHP,
                              positionMDF = a.KodeAkses,
                              provinceMDF = c.Provinsi,
                              regionMDF = c.Kota
                          }).FirstOrDefault();
            }
            return result;
        }
        public static bool getUserVerify(string IDRS, out int result)
        {
            result = 0;
            DataSet ds = new DataSet(); string sqlCommand = "", strErr = ""; bool blnResult = false;

            sqlCommand = "select count(Nama) as Jumlah from MUser where KodeAkses like '%Verifikator%' and IDRS = '"+IDRS+"' ";
            using (clsDBSQLConnection db = new clsDBSQLConnection())
            {
                blnResult = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
            }
            if (blnResult)
            {
                if (ds.Tables[0].Rows[0]["Jumlah"].ToString() != "0")
                {
                    result = int.Parse(ds.Tables[0].Rows[0]["Jumlah"].ToString());
                }
            }
            return blnResult;
        }
        public static bool CheckDataUser(string IDRegister, out string Mesage,out string Err)
        {
            Err = "";
            Mesage = "";
            bool result = true;
            string sqlCommand = "";
            DataSet ds = new DataSet();
            sqlCommand = "select * from MUser where IDRegister = '"+ IDRegister + "' ";
            using (clsDBSQLConnection db = new clsDBSQLConnection())
            {
                result = db.QueryCommand(Connection, sqlCommand, out ds, out Err);
            }

            if (result) {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0){
                    Mesage = "Registed";
                }
                else {
                    Mesage = "Unregisted";
                }
            }

            return result;
        }
        public static bool ChangePassword(string IDRegister, string Password, out string Message, out string Err)
        {
            Err = ""; Message = "";
            bool result = true;
            DataSet ds = new DataSet();
            string sqlCommand = "";
            sqlCommand = "update MUser set Password = '"+Password+"' where IDRegister = '"+IDRegister+"' ";
            using (clsDBSQLConnection db = new clsDBSQLConnection())
            {
                result = db.QueryCommand(Connection, sqlCommand, out ds, out Err);
            }
            if (result)
            {
                Message = "Saved";
            }
            else {
                Message = "Unsaved";
            }

            return result;
        }

    }
}