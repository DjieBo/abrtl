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
    public class RSDAL
    {
        public static string Connection = ConfigurationManager.ConnectionStrings["BuildingContext"].ToString();

        public static List<DataRoundAreaVM> getDistRoundArea(string IDRS, string Round, out string strErr)
        {
            List<DataRoundAreaVM> result = new List<DataRoundAreaVM>();
            DataRoundAreaVM data = new DataRoundAreaVM();
            strErr = "";
            string strCommand = ""; bool res = false; DataSet dsOut = new DataSet();

            try
            {
                strCommand = "select distinct Round, RoundArea, IDRoundArea from RSIA" + IDRS + " where Round = '" + Round + "' ";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    res = db.QueryCommand(Connection, strCommand, out dsOut, out strErr);
                }
                if (res & dsOut != null)
                {
                    foreach (DataRow item in dsOut.Tables[0].Rows)
                    {
                        data = new DataRoundAreaVM();
                        data.IDRoundArea = item["IDRoundArea"].ToString();
                        data.RoundArea = item["RoundArea"].ToString();
                        data.Round = item["Round"].ToString();
                        var dtCount = getCountDataArea(IDRS, Round, data.IDRoundArea);
                        data.dtCheck = dtCount.dtCheck;
                        data.dtTotal = dtCount.dtTotal;
                        result.Add(data);
                    }
                }
                else
                {
                    strErr = "Error #101: " + strErr;
                }
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
                return result;
            }

            return result;
        }
        public static DataRoundAreaVM getCountDataArea(string IDRS, string Round, string IDRArea)
        {
            DataRoundAreaVM result = new DataRoundAreaVM();
            string sqlCommand = ""; bool res = false; DataSet dsOut = new DataSet();
            string Err = "";

            sqlCommand = "select count(Komponen) as Jumlah from RSIA" + IDRS + " where Round = '" + Round + "' " +
                "and IDRoundArea = '" + IDRArea + "' and Status in ('Checked', 'Reject') ";
            sqlCommand += " select count(Komponen) as Jumlah from RSIA" + IDRS + " where Round = '" + Round + "' " +
                "and IDRoundArea = '" + IDRArea + "'";

            using (clsDBSQLConnection db = new clsDBSQLConnection())
            {
                res = db.QueryCommand(Connection, sqlCommand, out dsOut, out Err);
            }

            if (res & dsOut != null)
            {
                result.dtCheck = int.Parse(dsOut.Tables[0].Rows[0]["Jumlah"].ToString());
                result.dtTotal = int.Parse(dsOut.Tables[1].Rows[0]["Jumlah"].ToString());
            }
            return result;
        }
        public static List<DataKomponenVM> getAllDataKomponen(string IDRS, string Round, string IDRoundArea, out string total, out string jumlah, out string strErr)
        {
            List<DataKomponenVM> result = new List<DataKomponenVM>();
            string sqlCommand = ""; bool res = false; DataSet dsOut = new DataSet(); strErr = ""; total = ""; jumlah = "";
            string Err = "";

            sqlCommand = "select * from RSIA" + IDRS + " where Round = '" + Round + "' and IDRoundArea = '" + IDRoundArea + "' " +
                         "select Count(*) as 'TotalData', Count(Status) as 'TotalCheck'  from RSIA" + IDRS + " where Round = '" + Round + "' ";
            try
            {
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    res = db.QueryCommand(Connection, sqlCommand, out dsOut, out Err);
                }

                if (res)
                {
                    if (dsOut != null)
                    {
                        total = dsOut.Tables[1].Rows[0]["TotalData"].ToString();
                        jumlah = dsOut.Tables[1].Rows[0]["TotalCheck"].ToString();
                        var data = new DataKomponenVM();
                        foreach (DataRow item in dsOut.Tables[0].Rows)
                        {
                            data = new DataKomponenVM();
                            data.IDKomponen = int.Parse(item["ID"].ToString());
                            data.Round = item["Round"].ToString();
                            data.IDRoundArea = item["IDRoundArea"].ToString();
                            data.RoundArea = item["RoundArea"].ToString();
                            data.IDArea = item["IDArea"].ToString();
                            data.Area = item["Area"].ToString();
                            data.IDSubArea = item["IDSubArea"].ToString();
                            data.SubArea = item["SubArea"].ToString();
                            if (item["IDType"].ToString() != "")
                            {
                                data.SubArea += " " + item["Type"].ToString();
                            }
                            if (item["IDOption"].ToString() != "")
                            {
                                data.SubArea += " " + item["OptionArea"].ToString();
                            }
                            //data.IDType = item["IDType"].ToString();
                            //data.Type = item["Type"].ToString();
                            //data.IDOption = item["IDOption"].ToString();
                            //data.Option = item["OptionArea"].ToString();
                            data.IDItem = item["IDItem"].ToString();
                            data.Item = item["Item"].ToString();
                            data.Komponen = item["Komponen"].ToString();
                            data.Parameter = item["Parameter"].ToString().Trim();
                            data.ParamValue = item["ParamValue"].ToString();
                            data.Nilai = item["Nilai"].ToString();
                            data.Status = item["Status"].ToString();
                            data.Keterangan = item["Ket"].ToString();
                            data.IDUnix = item["IDUnix"].ToString();
                            result.Add(data);
                        }
                        
                    }
                    else
                    {
                        strErr = "Error #103: Data tidak ditemukan.";
                        return result;
                    }
                }
                else
                {
                    strErr = "Error #101: " + Err;
                }
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
                return result;
            }
            return result;
        }
        public static bool saveDataPenilaian(string IDRS, string IDUser, string ID, string Nilai,
                                             string ValParameter, string Keterangan, string base64image,
                                             out string IDRoundArea, out string Round, out string strErr)
        {
            bool result = false; strErr = ""; IDRoundArea = ""; Round = "";
            string sqlCommand = ""; DataSet dsOut = new DataSet();
            if (string.IsNullOrEmpty(Keterangan))
            {
                Keterangan = "";
            }
            if (string.IsNullOrEmpty(base64image))
            {
                base64image = "";
            }
            try
            {
                sqlCommand = @"
                                declare @CodeUnix varchar(75),
		                                @nilai varchar(5),
		                                @strID varchar(10),
		                                @period varchar(10),
		                                @action varchar(25),
		                                @statusIDUnix varchar(25)

                                set @period = (select Periode from RSIA" + IDRS + @" where ID = @ID)
                                if @period is null
                                 begin
	                                set @period = @waktu
	                                set @action = 'new'
                                 end
                                else
                                 begin
	                                set @action = 'update'
                                 end

                                set @CodeUnix = (select IDUnix from RSIA" + IDRS + @" where ID = @ID)
                                if @CodeUnix is null
                                 begin
	                                set @strID = '.' + (select CONVERT(varchar(10), ID) from RSIA" + IDRS + @" where ID = @ID)
	                                set @CodeUnix = (select @IDRS + '.' + @period + '.' + Round + @strID from RSIA" + IDRS + @" where ID = @ID)
	                                set @statusIDUnix = 'new'
                                 end
                                else
                                 begin
	                                set @statusIDUnix = 'update'
                                 end

                                set @nilai = @Nilaii

                                if @nilai = 'A'
                                 begin
	                                if @action = 'new'
	                                 begin
		                                if @statusIDUnix = 'new'
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = '',
			                                ParamValue = @Parameter, IDUnix = null where ID = @ID
			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
		                                else
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = '',
			                                ParamValue = @Parameter, IDUnix = null where ID = @ID
			                                delete from RABRS" + IDRS + @" where IDUnix = @CodeUnix
			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
	                                 end
	                                else
	                                 begin
		                                if @statusIDUnix = 'new'
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = '',
			                                ParamValue = @Parameter, IDUnix = null where ID = @ID
			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
		                                else
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = '',
			                                ParamValue = @Parameter, IDUnix = null where ID = @ID
			                                delete from RABRS" + IDRS + @" where IDUnix = @CodeUnix
			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
	                                 end
                                 end
                                else
                                 begin 
	                                if @action = 'new'
	                                 begin
		                                if @statusIDUnix = 'new'
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = @Ket,
				                                ParamValue = @Parameter, IDUnix = @CodeUnix where ID = @ID
			
			                                insert into RABRS" + IDRS + @"(Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption, OptionArea, IDItem, 
			                                Item, Komponen, IDChecker,Nilai, Status, ParamValue, Ket, IDUnix, JPG)
			                                select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption, OptionArea, IDItem, Item,
			                                Komponen, IDChecker, Nilai, Status,ParamValue, Ket, IDUnix, @jpg from RSIA" + IDRS + @" where ID = @ID	

			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
		                                else
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = @Ket,
				                                ParamValue = @Parameter, IDUnix = @CodeUnix where ID = @ID

			                                update RABRS" + IDRS + @" set IDChecker = @IDChecker, Status = 'Checked', Nilai = @nilai, ParamValue = @Parameter, Ket = @Ket, JPG = @jpg where IDUnix = @CodeUnix
			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
	                                 end
	                                else
	                                 begin
		                                if @statusIDUnix = 'new'
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = @Ket,
				                                ParamValue = @Parameter, IDUnix = @CodeUnix where ID = @ID
			
			                                insert into RABRS" + IDRS + @"(Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption, OptionArea, IDItem, 
			                                Item, Komponen, IDChecker, Nilai, Status, ParamValue, Ket, IDUnix, JPG, Periode)
			                                select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption, OptionArea, IDItem, Item,
			                                Komponen, IDChecker, Nilai, Status, ParamValue, Ket, IDUnix, @jpg, Periode from RSIA" + IDRS + @" where ID = @ID

			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
		                                else
		                                 begin
			                                update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked', Ket = @Ket,
				                                ParamValue = @Parameter, IDUnix = @CodeUnix where ID = @ID

			                                update RABRS" + IDRS + @" set IDChecker = @IDChecker, Nilai = @nilai, Status = 'Checked',ParamValue = @Parameter, Ket = @Ket, JPG = @jpg where IDUnix = @CodeUnix
			                                select Round, IDRoundArea as data from RSIA" + IDRS + @" where ID = @ID
		                                 end
	                                 end
                                 end
                                ";

                #region Comment Save Data awal
                //if (Nilai == "A")
                //{
                //    sqlCommand = @"update RSIA" + IDRS + " set IDChecker = '" + IDUser +"', Nilai = '" + Nilai + "', Status = 'Checked'," +
                //                    " Ket = '', Parameter = '"+ ValParameter + "', IDUnix = '' where ID = " + ID +
                //                    "   select Round, IDRoundArea as data from RSIA" + IDRS + " where ID = " + ID;

                //    using (clsDBSQLConnection db = new clsDBSQLConnection())
                //    {
                //        result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                //    }
                //}
                //else
                //{
                //    //BULAN DAN TAHUN MASIH HARDCODE
                //    sqlCommand = @"declare @CodeUnix varchar(75),
                //           @strID varchar(max)

                //                set @strID = '.' + (select CONVERT(varchar(max), ID) from RSIA" + IDRS + @" where ID = @ID)
                //                set @CodeUnix = (select (@IDRS + '.01.2020.'+ Round + @strID) from RSIA" + IDRS + @" where ID = @ID) 
                //                " +

                //                " update RSIA" + IDRS + @" set IDChecker = @IDChecker, Nilai = @Nilai, Status = 'Checked', Ket = @Ket, 
                //                Parameter = @Parameter, IDUnix = @CodeUnix where ID = @ID  

                //                " + 

                //                " insert into RABRS" + IDRS + @"(Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption, OptionArea, IDItem, 
                //                Item, Komponen, IDChecker, Status, Ket, IDUnix, JPG)
                //                select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption, OptionArea, IDItem, Item,
                //                Komponen, IDChecker, Status, Ket, IDUnix, @jpg from RSIA" + IDRS + @" where ID = @ID 

                //                " +

                //                " select Round, IDRoundArea as data from RSIA" + IDRS + " where ID = @ID ";
                //    
                //}

                #endregion
                SqlParameter[] dbParam = new SqlParameter[8];
                dbParam[0] = new SqlParameter("@IDRS", IDRS);
                dbParam[1] = new SqlParameter("@ID", ID);
                dbParam[2] = new SqlParameter("@IDChecker", IDUser);
                dbParam[3] = new SqlParameter("@Nilaii", Nilai);
                dbParam[4] = new SqlParameter("@Ket", Keterangan);
                dbParam[5] = new SqlParameter("@Parameter", ValParameter);
                dbParam[6] = new SqlParameter("@jpg", base64image);
                dbParam[7] = new SqlParameter("@waktu", DateTime.Now.ToString("yyyyMM"));

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, ref dbParam, out dsOut, out strErr);
                }

                if (result & dsOut != null)
                {
                    IDRoundArea = dsOut.Tables[0].Rows[0]["data"].ToString();
                    Round = dsOut.Tables[0].Rows[0]["Round"].ToString();
                }
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
                return result;
            }

            return result;
        }
        public static bool getImage(string IDRS, string ID, out ViewImageVM model)
        {
            bool result = false; model = new ViewImageVM();
            DataSet dsOut = new DataSet(); string sqlCommand = "", Error = "";

            try
            {
                sqlCommand = "select a.IDUnix, b.JPG, b.Round, b.SubArea, b.Item, b.Komponen from RSIA" + IDRS + " a " +
                              "join RABRS" + IDRS + " b on a.IDUnix = b.IDUnix " +
                              "where a.ID = " + ID;

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out Error);
                }

                if (result & dsOut != null)
                {
                    model.Round = dsOut.Tables[0].Rows[0]["Round"].ToString();
                    model.SubArea = dsOut.Tables[0].Rows[0]["SubArea"].ToString();
                    model.Item = dsOut.Tables[0].Rows[0]["Item"].ToString();
                    model.Komponen = dsOut.Tables[0].Rows[0]["Komponen"].ToString();
                    model.JPG = dsOut.Tables[0].Rows[0]["JPG"].ToString();
                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message;
                throw;
            }

            return result;
        }
        public static bool getRABBeforeImage(string IDRS, string ID, string Round, string Periode, out ImageRABBVM model)
        {
            bool result = false; model = new ImageRABBVM();
            DataSet dsOut = new DataSet(); string sqlCommand = "", Error = "";

            try
            {
                sqlCommand = "select * from RABRS" + IDRS + " where ID = '" + ID + "' and Round = '" + Round + "' and Periode = '" + Periode + "'";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out Error);
                }

                if (result & dsOut != null)
                {

                    model.Round = dsOut.Tables[0].Rows[0]["Round"].ToString();
                    model.SubArea = dsOut.Tables[0].Rows[0]["SubArea"].ToString();
                    model.Item = dsOut.Tables[0].Rows[0]["Item"].ToString();
                    model.Komponen = dsOut.Tables[0].Rows[0]["Komponen"].ToString();
                    model.JPG = dsOut.Tables[0].Rows[0]["JPG"].ToString();

                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message;
                throw;
            }

            return result;
        }
        public static bool getImageSPK(string IDRS, string IDUnix, string Round, string Periode, out ImageRABBVM model)
        {
            bool result = true;
            model = new ImageRABBVM();
            DataSet dsOut = new DataSet(); string sqlCommand = "", Error = "";
            try
            {
                sqlCommand = "select * from RABRS" + IDRS + " where IDUnix = '" + IDUnix + "' and Round = '" + Round + "' and Periode = '" + Periode + "'";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out Error);
                }

                if (result & dsOut != null)
                {

                    model.Round = dsOut.Tables[0].Rows[0]["Round"].ToString();
                    model.SubArea = dsOut.Tables[0].Rows[0]["SubArea"].ToString();
                    model.Item = dsOut.Tables[0].Rows[0]["Item"].ToString();
                    model.Komponen = dsOut.Tables[0].Rows[0]["Komponen"].ToString();
                    model.JPG = dsOut.Tables[0].Rows[0]["JPG"].ToString();
                    model.JPGAfter = dsOut.Tables[0].Rows[0]["JPGAfter"].ToString();
                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message;
                throw;
            }
            return result;

        }
        public static bool saveDataSubmit(string IDRS, string Round, string Date, out string Periode,  out string strErr)
        {
            bool result = false;
            strErr = "";
            string sqlCommad = ""; DataSet dsOut = new DataSet();
            Periode = DateTime.Now.ToString("yyyyMM");
            string bulan = Periode.Substring(4, 2);
            try
            {
                sqlCommad = @" declare @strError varchar(550),
		                               @cntChecked int,
		                               @cntKomponen int,
                                       @masa varchar(8)

                            set @masa = (select top 1 Periode from RSIA" + IDRS + @" where Round = @round)
                                if @masa is not null
	                                begin
                                        update RSIA" + IDRS + @" set Status = 'Submit' where Status in ('Checked', 'Reject') and Round = @round
                                        update RABRS" + IDRS + @" set Status = 'Submit' where Status in ('Checked', 'Reject') and Round = @round and Periode = @masa
		                                update RValueVerifikator set DateSubmit = @date, StatusData = 'Submit', IDRegVerify1 = '',
				                                Verifikasi1Date = '', Status1 = '', KetVerifikator1 = '', IDRegVerify2 = '', Verifikasi2Date = '',
				                                Status2 = '', KetVerifikator2 = '' where Periode = @masa and IDRS = '" + IDRS + @"' and IDRound = @round 		
	                                end
                                else
	                                begin
		                                update RSIA" + IDRS + @" set Status = 'Submit' where Status in ('Checked', 'Reject') and Round = @round
			                            update RSIA" + IDRS + @" set Periode = @period where  Round = @round
			                            update RABRS" + IDRS + @" set Status = 'Submit', Periode = @period where Status in ('Checked', 'Reject') and Round = @round and Periode is null
		                                insert into RValueVerifikator values (@idrs, @round, @date, 'Submit','','','','','','','','',@period)
                                        declare @intAkumulasi int
                                         set @intAkumulasi = ( select count(Periode) from DataAkumulasi where IDRS = '" + IDRS + @"' and Periode = @period)
                                         if	@intAkumulasi = 0
                                          begin
                                            insert DataReport values(@period,'" + IDRS + @"','Round 1',null)
	                                        insert DataReport values(@period,'" + IDRS + @"','Round 2',null)
	                                        insert DataReport values(@period,'" + IDRS + @"','Round 3',null)
	                                        insert DataReport values(@period,'" + IDRS + @"','Round 4',null)
                                            insert Datalog values('" + bulan + @"', null, @idrs, 'Round 1', null, @period)
                                            insert Datalog values('" + bulan + @"', null, @idrs, 'Round 2', null, @period)
                                            insert Datalog values('" + bulan + @"', null, @idrs, 'Round 3', null, @period)
                                            insert Datalog values('" + bulan + @"', null, @idrs, 'Round 4', null, @period)
                                            insert DataAkumulasi values('" + IDRS + @"','','0','0',@period)                                         
                                          end                                        
	                                end
                                ";

                SqlParameter[] dbParam = new SqlParameter[4];
                dbParam[0] = new SqlParameter("@round", Round);
                dbParam[1] = new SqlParameter("@idrs", IDRS);
                dbParam[2] = new SqlParameter("@date", Date);
                dbParam[3] = new SqlParameter("@period", Periode);

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommad, ref dbParam, out dsOut, out strErr);
                }

                if (result)
                {
                    if (dsOut.Tables[0].Rows[0]["error"].ToString() != "")
                    {
                        strErr = dsOut.Tables[0].Rows[0]["error"].ToString();
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
                return result;
            }
            return result;
        }
        public static bool getDataExecReport(string IDRS, string Round, string Periode, out ExcReportVM model, out string strErr)
        {
            bool Result = false; model = new ExcReportVM(); strErr = "";
            string sqlCommand = ""; DataSet dsOut = new DataSet();
            string blnBerjalan = DateTime.Now.ToString("yyyyMM");

            try
            {
                #region sqlCommand
                sqlCommand = @"
                                declare @strData varchar(max),
                                        @strStatusUtama varchar(50)

                                set @strData = (select Data from DataLog where IDRS = '" + IDRS + @"' and Round = '" + Round + @"' and Periode = '" + Periode + @"') 
                                if @strData is null
                                 begin
                                  Create table #temp1
                                    (	
	                                    ID int IDENTITY(1, 1) primary key ,
	                                    IDRoundArea varchar(25),
	                                    RoundArea varchar(350)
                                    )

                                    Create table #tmpData
                                    (	
	                                    ID int IDENTITY(1, 1) primary key ,
	                                    IDRoundArea varchar(25),
	                                    RoundArea varchar(350),
	                                    Total int,
	                                    NialiA int,
	                                    NilaiB int,
	                                    NilaiC int
                                    )

                                    Create table #listKet
                                    (	
	                                    ID int IDENTITY(1, 1) primary key ,
	                                    IDRoundArea varchar(25),
	                                    Nilai varchar(5),
	                                    Ket varchar(max)
                                    )

                                    create table #listUser
                                    (
	                                    KodeAkses varchar(50),
	                                    Nama varchar(250),
	                                    Email varchar(300)
                                    )

                                    Create Table #tmpParam
                                    ( 
	                                    ID		int IDENTITY(1, 1) primary key,
	                                    Parameter	varchar(50),
	                                    Jumlah	int,
	                                    Yes		int,
	                                    Dont	int
                                    )

                                    insert into #tmpParam(Parameter) select distinct Parameter from RSIA" + IDRS + @" where Round = '" + Round + @"'

                                    insert into #temp1(IDRoundArea, RoundArea)
                                    select distinct IDRoundArea, RoundArea from RSIA" + IDRS + " where Round = '" + Round + @"'

                                    insert into #tmpData(IDRoundArea, RoundArea)
                                    select distinct IDRoundArea, RoundArea from RSIA" + IDRS + " where Round = '" + Round + @"'

                                    insert into #listKet(IDRoundArea, Nilai, Ket)
                                    select IDRoundArea, Nilai, Ket from RSIA" + IDRS + @" where Ket != ''

                                    insert into #listUser(KodeAkses, Nama, Email)
                                    select KodeAkses, Nama, Email from MUser where IDRS = '" + IDRS + @"' 
                                    union 
                                    select KodeAkses, Nama, Email from MUser where  KodeAkses in ('Validator 1' , 'Validator 3')
                                    union 
                                    select a.KodeAkses, a.Nama, a.Email from Muser a join DaftarRSIA b on a.KetReg = b.Regional 
                                    where b.IDRS =  '" + IDRS + @"' 
        
                                    declare @int int, @jmlh int, @intHt int, @intJHt int		

                                    set @int = 1
                                    set @jmlh = (select COUNT(RoundArea) from #temp1)

                                    while @int <= @jmlh	
	                                    begin
		                                    update #tmpData set NialiA = (select count(Komponen)
		                                    from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='A')
		                                    where ID = @int

		                                    update #tmpData set NilaiB = (select count(Komponen)
		                                    from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='B')
		                                    where ID = @int
		
		                                    update #tmpData set NilaiC = (select count(Komponen)
		                                    from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='C')
		                                    where ID = @int

		                                    update #tmpData set Total = (select COUNT(Komponen) from RSIA" + IDRS + @" a
									                                    join #temp1 b on a.IDRoundArea = b.IDRoundArea where a.Round = '" + Round + @"' and b.ID = @int)
		                                    where ID = @int

		                                    set @int = @int + 1		
	                                    end

                                    set @intHt = 1
                                    set @intJHt = (select count(Parameter) from #tmpParam)

                                    while @intHt <= @intJHt
                                     begin
	                                    update #tmpParam set Jumlah = (select count(a.Parameter) from RSIA" + IDRS + @" a 
	                                    join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.Round = '" + Round + @"') where ID = @intHt

	                                    Update #tmpParam set Yes = (select count(a.ParamValue) from RSIA" + IDRS + @" a 
	                                    join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.ParamValue = b.Parameter and a.Round = '" + Round + @"') where ID = @intHt

	                                    Update #tmpParam set Dont = (select count(a.ParamValue) from RSIA" + IDRS + @" a 
	                                    join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.ParamValue like '%Tidak%' and a.Round = '" + Round + @"') where ID = @intHt

	                                    set @intHt = @intHt + 1
                                     end

                                    select * from #temp1
                                    select * from #tmpData
                                    select * from #listKet
                                    select * from #listUser
                                    select NamaRS from DaftarRSIA where IDRS = '" + IDRS + @"' 
                                    select * from RValueVerifikator where IDRS = '" + IDRS + "' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"' 
                                    select * from #tmpParam
                                    select * from RABRS" + IDRS + @" where Round = '" + Round + @"' and Periode = '" + Periode + @"' 
                                 end
                                else
                                 begin
                                  select @strData as Data 
                                  select * from RABRS" + IDRS + @" where Round = '" + Round + @"' and Periode = '" + Periode + @"' 
                                  select * from RValueApproval where IDRS = '" + IDRS + "' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"' 
                                 end                              
                                ";

                #endregion

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    Result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }

                if (Result)
                {
                    if (dsOut != null)
                    {
                        if (dsOut.Tables.Count == 3)
                        {
                            DataSet data = new DataSet();
                            System.IO.StringReader XML = new System.IO.StringReader(dsOut.Tables[0].Rows[0]["Data"].ToString());
                            data.ReadXml(XML);
                            List<dtAreaVM> tmpListData = new List<dtAreaVM>();
                            dtAreaVM tmpdata = new dtAreaVM();
                            foreach (DataRow item in data.Tables["Table"].Rows)
                            {
                                string IDArea = item["IDRoundArea"].ToString();
                                foreach (DataRow dataArea in data.Tables["Table1"].Rows)
                                {
                                    if (dataArea["IDRoundArea"].ToString() == IDArea)
                                    {
                                        tmpdata = new dtAreaVM();
                                        tmpdata.IDRoundArea = dataArea["IDRoundArea"].ToString();
                                        tmpdata.RoundArea = dataArea["RoundArea"].ToString();
                                        tmpdata.Total = decimal.Parse(dataArea["Total"].ToString());
                                        tmpdata.NilaiA = decimal.Parse(dataArea["NialiA"].ToString());
                                        tmpdata.NilaiB = decimal.Parse(dataArea["NilaiB"].ToString());
                                        tmpdata.NilaiC = decimal.Parse(dataArea["NilaiC"].ToString());
                                        tmpdata.persenA = Math.Round((tmpdata.NilaiA / tmpdata.Total) * 100, 2);
                                        tmpdata.persenB = Math.Round((tmpdata.NilaiB / tmpdata.Total) * 100, 2);
                                        tmpdata.persenC = Math.Round((tmpdata.NilaiC / tmpdata.Total) * 100, 2);
                                        var dtKet = new dtKeterangan();
                                        var listKetA = new List<dtKeterangan>();
                                        var listKetB = new List<dtKeterangan>();
                                        var listKetC = new List<dtKeterangan>();
                                        if (data.Tables.Count == 7)
                                        {
                                            foreach (DataRow tmpKet in data.Tables["Table2"].Rows)
                                            {
                                                if (tmpKet["IDRoundArea"].ToString() == IDArea & tmpKet["Nilai"].ToString() == "A")
                                                {
                                                    dtKet = new dtKeterangan();
                                                    dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                    listKetA.Add(dtKet);
                                                }
                                                else if (tmpKet["IDRoundArea"].ToString() == IDArea & tmpKet["Nilai"].ToString() == "B")
                                                {
                                                    dtKet = new dtKeterangan();
                                                    dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                    listKetB.Add(dtKet);
                                                }
                                                else if (tmpKet["IDRoundArea"].ToString() == IDArea & tmpKet["Nilai"].ToString() == "C")
                                                {
                                                    dtKet = new dtKeterangan();
                                                    dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                    listKetC.Add(dtKet);
                                                }
                                            }
                                        }
                                        tmpdata.listKeteranganA = listKetA;
                                        tmpdata.listKeteranganB = listKetB;
                                        tmpdata.listKeteranganC = listKetC;

                                        tmpListData.Add(tmpdata);
                                        model.totalItemPenilaian += tmpdata.Total;
                                        model.totalItemA += tmpdata.NilaiA;
                                        model.totalItemB += tmpdata.NilaiB;
                                        model.totalItemC += tmpdata.NilaiC;
                                    }
                                }
                            }
                            model.listDataArea = tmpListData;
                            model.persenTotalItemA = Math.Round((model.totalItemA / model.totalItemPenilaian) * 100, 2);
                            model.persenTotalItemB = Math.Round((model.totalItemB / model.totalItemPenilaian) * 100, 2);
                            model.persenTotalItemC = Math.Round((model.totalItemC / model.totalItemPenilaian) * 100, 2);

                            model.bobotItemA = Math.Round((model.persenTotalItemA * 1), 3);
                            model.bobotItemB = Math.Round((model.persenTotalItemB * decimal.Parse("0.5")), 3);
                            model.bobotItemC = Math.Round((model.persenTotalItemC * decimal.Parse("0.25")), 3);

                            model.totalBobot = model.bobotItemA + model.bobotItemB + model.bobotItemC;
                            model.NamaRS = data.Tables["Table4"].Rows[0]["NamaRS"].ToString();
                            string periode = data.Tables["Table5"].Rows[0]["Periode"].ToString().Trim().Substring(4, 2);
                            model.Bulan = RSDAL.convertPeriode(periode) + " " + data.Tables["Table5"].Rows[0]["Periode"].ToString().Trim().Substring(0, 4);
                            model.IDRS = IDRS;
                            model.Round = Round;
                            model.Periode = data.Tables["Table5"].Rows[0]["Periode"].ToString().Trim();

                            foreach (DataRow item in data.Tables["Table3"].Rows)
                            {
                                if (item["KodeAkses"].ToString() == "Checker")
                                {
                                    model.Checker = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Validator 1")
                                {
                                    model.Validator1 = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Validator 2")
                                {
                                    model.Validator2 = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Validator 3")
                                {
                                    model.Validator3 = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Verifikator 1")
                                {
                                    model.WakilDirektur = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Verifikator 2")
                                {
                                    model.DIrektur = item["Nama"].ToString();
                                }
                            }
                            foreach (DataRow param in data.Tables["Table6"].Rows)
                            {
                                if (param["Parameter"].ToString().Trim() == "Aman")
                                {
                                    model.totalAman = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Aman = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkAman = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenAman = Math.Round((model.Aman / model.totalAman) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Rapih")
                                {
                                    model.totalRapih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Rapih = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkRapih = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenRapih = Math.Round((model.Rapih / model.totalRapih) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Tampak baru")
                                {
                                    model.totalBaru = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Baru = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkBaru = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenBaru = Math.Round((model.Baru / model.totalBaru) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Ramah Lingkungan")
                                {
                                    model.totalRL = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.RL = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkRL = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenRL = Math.Round((model.RL / model.totalRL) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Bersih")
                                {
                                    model.totalBersih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Bersih = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkBersih = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenBersih = Math.Round((model.Bersih / model.totalBersih) * 100, 2);
                                }
                            }
                            if (dsOut.Tables[1].Rows.Count > 0)
                            {
                                List<ViewImageVM> lsIMG = new List<ViewImageVM>();
                                ViewImageVM IMG = new ViewImageVM();
                                foreach (DataRow dtRAB in dsOut.Tables[1].Rows)
                                {
                                    IMG = new ViewImageVM();
                                    IMG.Round = Round;
                                    IMG.Item = dtRAB["Item"].ToString();
                                    IMG.JPG = dtRAB["JPG"].ToString();
                                    IMG.Komponen = dtRAB["Komponen"].ToString();
                                    IMG.SubArea = dtRAB["SubArea"].ToString();
                                    IMG.Keterangan = dtRAB["Ket"].ToString();
                                    lsIMG.Add(IMG);
                                }
                                model.ListImage = lsIMG;
                            }

                            //STATUS VER
                            model.StatusVer1 = "Verify";
                            model.StatusVer2 = "Verify";
                            if (dsOut.Tables[2].Rows.Count > 0)
                            {
                                model.Status1 = dsOut.Tables[2].Rows[0]["Status1"].ToString();
                                model.Status2 = dsOut.Tables[2].Rows[0]["Status2"].ToString();
                                model.Status3 = dsOut.Tables[2].Rows[0]["Status3"].ToString();
                            }
                        }
                        else
                        {
                            List<dtAreaVM> lstRoundArea = new List<dtAreaVM>();
                            foreach (DataRow item in dsOut.Tables[0].Rows)
                            {
                                var strIDRA = item["IDRoundArea"].ToString();
                                dtAreaVM dtRoundArea = new dtAreaVM();

                                foreach (DataRow tmpData in dsOut.Tables[1].Rows)
                                {
                                    if (tmpData["IDRoundArea"].ToString() == strIDRA)
                                    {
                                        dtRoundArea = new dtAreaVM();
                                        dtRoundArea.IDRoundArea = tmpData["IDRoundArea"].ToString();
                                        dtRoundArea.RoundArea = tmpData["RoundArea"].ToString();
                                        dtRoundArea.Total = decimal.Parse(tmpData["Total"].ToString());
                                        dtRoundArea.NilaiA = decimal.Parse(tmpData["NialiA"].ToString());
                                        dtRoundArea.NilaiB = decimal.Parse(tmpData["NilaiB"].ToString());
                                        dtRoundArea.NilaiC = decimal.Parse(tmpData["NilaiC"].ToString());
                                        dtRoundArea.persenA = Math.Round((dtRoundArea.NilaiA / dtRoundArea.Total) * 100, 2);
                                        dtRoundArea.persenB = Math.Round((dtRoundArea.NilaiB / dtRoundArea.Total) * 100, 2);
                                        dtRoundArea.persenC = Math.Round((dtRoundArea.NilaiC / dtRoundArea.Total) * 100, 2);

                                        var dtKet = new dtKeterangan();
                                        var listKetA = new List<dtKeterangan>();
                                        var listKetB = new List<dtKeterangan>();
                                        var listKetC = new List<dtKeterangan>();
                                        foreach (DataRow tmpKet in dsOut.Tables[2].Rows)
                                        {
                                            if (tmpKet["IDRoundArea"].ToString() == strIDRA & tmpKet["Nilai"].ToString() == "A")
                                            {
                                                dtKet = new dtKeterangan();
                                                dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                listKetA.Add(dtKet);
                                            }
                                            else if (tmpKet["IDRoundArea"].ToString() == strIDRA & tmpKet["Nilai"].ToString() == "B")
                                            {
                                                dtKet = new dtKeterangan();
                                                dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                listKetB.Add(dtKet);
                                            }
                                            else if (tmpKet["IDRoundArea"].ToString() == strIDRA & tmpKet["Nilai"].ToString() == "C")
                                            {
                                                dtKet = new dtKeterangan();
                                                dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                listKetC.Add(dtKet);
                                            }
                                        }
                                        dtRoundArea.listKeteranganA = listKetA;
                                        dtRoundArea.listKeteranganB = listKetB;
                                        dtRoundArea.listKeteranganC = listKetC;

                                        lstRoundArea.Add(dtRoundArea);
                                        //model.listDataArea = dtRoundArea;
                                        model.totalItemPenilaian += dtRoundArea.Total;
                                        model.totalItemA += dtRoundArea.NilaiA;
                                        model.totalItemB += dtRoundArea.NilaiB;
                                        model.totalItemC += dtRoundArea.NilaiC;
                                    }
                                }
                            }
                            model.listDataArea = lstRoundArea;
                            model.persenTotalItemA = Math.Round((model.totalItemA / model.totalItemPenilaian) * 100, 2);
                            model.persenTotalItemB = Math.Round((model.totalItemB / model.totalItemPenilaian) * 100, 2);
                            model.persenTotalItemC = Math.Round((model.totalItemC / model.totalItemPenilaian) * 100, 2);

                            model.bobotItemA = Math.Round((model.persenTotalItemA * 1), 3);
                            model.bobotItemB = Math.Round((model.persenTotalItemB * decimal.Parse("0.5")), 3);
                            model.bobotItemC = Math.Round((model.persenTotalItemC * decimal.Parse("0.25")), 3);

                            model.totalBobot = model.bobotItemA + model.bobotItemB + model.bobotItemC;
                            model.NamaRS = dsOut.Tables[4].Rows[0]["NamaRS"].ToString();
                            //string periode = dsOut.Tables[5].Rows[0]["Periode"].ToString().Trim().Substring(4, 2);
                            model.Bulan = convertPeriode(Periode.Substring(4, 2)) + " " + Periode.Substring(0, 4);
                            model.IDRS = IDRS;
                            model.Round = Round;
                            model.StatusVer1 = dsOut.Tables[5].Rows[0]["Status1"].ToString();
                            model.StatusVer2 = dsOut.Tables[5].Rows[0]["Status2"].ToString();

                            foreach (DataRow item in dsOut.Tables[3].Rows)
                            {
                                if (item["KodeAkses"].ToString() == "Checker")
                                {
                                    model.Checker = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Validator 1")
                                {
                                    model.Validator1 = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Validator 2")
                                {
                                    model.Validator2 = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Validator 3")
                                {
                                    model.Validator3 = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Verifikator 1")
                                {
                                    model.WakilDirektur = item["Nama"].ToString();
                                }
                                else if (item["KodeAkses"].ToString() == "Verifikator 2")
                                {
                                    model.DIrektur = item["Nama"].ToString();
                                }
                            }

                            foreach (DataRow param in dsOut.Tables[6].Rows)
                            {
                                if (param["Parameter"].ToString().Trim() == "Aman")
                                {
                                    model.totalAman = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Aman = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkAman = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenAman = Math.Round((model.Aman / model.totalAman) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Rapih")
                                {
                                    model.totalRapih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Rapih = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkRapih = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenRapih = Math.Round((model.Rapih / model.totalRapih) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Tampak Baru")
                                {
                                    model.totalBaru = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Baru = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkBaru = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenBaru = Math.Round((model.Baru / model.totalBaru) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Ramah Lingkungan")
                                {
                                    model.totalRL = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.RL = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkRL = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenRL = Math.Round((model.RL / model.totalRL) * 100, 2);
                                }
                                else if (param["Parameter"].ToString().Trim() == "Bersih")
                                {
                                    model.totalBersih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                    model.Bersih = decimal.Parse(param["Yes"].ToString().Trim());
                                    model.tdkBersih = decimal.Parse(param["Dont"].ToString().Trim());
                                    model.persenBersih = Math.Round((model.Bersih / model.totalBersih) * 100, 2);
                                }
                            }

                            if (dsOut.Tables[7].Rows.Count > 0)
                            {
                                List<ViewImageVM> lsIMG = new List<ViewImageVM>();
                                ViewImageVM IMG = new ViewImageVM();
                                foreach (DataRow dtRAB in dsOut.Tables[7].Rows)
                                {
                                    IMG = new ViewImageVM();
                                    IMG.Round = Round;
                                    IMG.Item = dtRAB["Item"].ToString();
                                    IMG.JPG = dtRAB["JPG"].ToString();
                                    IMG.Komponen = dtRAB["Komponen"].ToString();
                                    IMG.SubArea = dtRAB["SubArea"].ToString();
                                    IMG.Keterangan = dtRAB["Ket"].ToString();
                                    lsIMG.Add(IMG);
                                }
                                model.ListImage = lsIMG;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
                return Result;
            }

            return Result;
        }
        public static bool getDataPenilaianVer(string IDRS, string Round, string Periode, out ExcReportVM model, out string strErr)
        {
            bool Result = false; model = new ExcReportVM(); strErr = "";
            string sqlCommand = ""; DataSet dsOut = new DataSet();
            string blnBerjalan = DateTime.Now.ToString("yyyyMM");

            try
            {
                #region sqlCommand
                sqlCommand = @"
                                Create table #temp1
                                (	
	                                ID int IDENTITY(1, 1) primary key ,
	                                IDRoundArea varchar(25),
	                                RoundArea varchar(350)
                                )

                                Create table #tmpData
                                (	
	                                ID int IDENTITY(1, 1) primary key ,
	                                IDRoundArea varchar(25),
	                                RoundArea varchar(350),
	                                Total int,
	                                NialiA int,
	                                NilaiB int,
	                                NilaiC int
                                )

                                Create table #listKet
                                (	
	                                ID int IDENTITY(1, 1) primary key ,
	                                IDRoundArea varchar(25),
	                                Nilai varchar(5),
	                                Ket varchar(max)
                                )

                                create table #listUser
                                (
	                                KodeAkses varchar(50),
	                                Nama varchar(250),
	                                Email varchar(300)
                                )

                                Create Table #tmpParam
                                ( 
	                                ID		int IDENTITY(1, 1) primary key,
	                                Parameter	varchar(50),
	                                Jumlah	int,
	                                Yes		int,
	                                Dont	int
                                )

                                insert into #tmpParam(Parameter) select distinct Parameter from RSIA" + IDRS + @" where Round = '" + Round + @"'

                                insert into #temp1(IDRoundArea, RoundArea)
                                select distinct IDRoundArea, RoundArea from RSIA" + IDRS + " where Round = '" + Round + @"'

                                insert into #tmpData(IDRoundArea, RoundArea)
                                select distinct IDRoundArea, RoundArea from RSIA" + IDRS + " where Round = '" + Round + @"'

                                insert into #listKet(IDRoundArea, Nilai, Ket)
                                select IDRoundArea, Nilai, Ket from RSIA" + IDRS + @" where Ket != ''

                                insert into #listUser(KodeAkses, Nama, Email)
                                select KodeAkses, Nama, Email from MUser where IDRS = '" + IDRS + @"' 
                                union 
                                select KodeAkses, Nama, Email from MUser where  KodeAkses in ('Validator 1' , 'Validator 3')
                                union 
                                select a.KodeAkses, a.Nama, a.Email from Muser a join DaftarRSIA b on a.KetReg = b.Regional 
                                where b.IDRS =  '" + IDRS + @"' 
        
                                declare @int int, @jmlh int, @intHt int, @intJHt int		

                                set @int = 1
                                set @jmlh = (select COUNT(RoundArea) from #temp1)

                                while @int <= @jmlh	
	                                begin
		                                update #tmpData set NialiA = (select count(Komponen)
		                                from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='A')
		                                where ID = @int

		                                update #tmpData set NilaiB = (select count(Komponen)
		                                from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='B')
		                                where ID = @int
		
		                                update #tmpData set NilaiC = (select count(Komponen)
		                                from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='C')
		                                where ID = @int

		                                update #tmpData set Total = (select COUNT(Komponen) from RSIA" + IDRS + @" a
									                                join #temp1 b on a.IDRoundArea = b.IDRoundArea where a.Round = '" + Round + @"' and b.ID = @int)
		                                where ID = @int

		                                set @int = @int + 1		
	                                end

                                set @intHt = 1
                                set @intJHt = (select count(Parameter) from #tmpParam)

                                while @intHt <= @intJHt
                                 begin
	                                update #tmpParam set Jumlah = (select count(a.Parameter) from RSIA" + IDRS + @" a 
	                                join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.Round = '" + Round + @"') where ID = @intHt

	                                Update #tmpParam set Yes = (select count(a.ParamValue) from RSIA" + IDRS + @" a 
	                                join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.ParamValue = b.Parameter and a.Round = '" + Round + @"') where ID = @intHt

	                                Update #tmpParam set Dont = (select count(a.ParamValue) from RSIA" + IDRS + @" a 
	                                join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.ParamValue like '%Tidak%' and a.Round = '" + Round + @"') where ID = @intHt

	                                set @intHt = @intHt + 1
                                 end

                                select * from #temp1
                                select * from #tmpData
                                select * from #listKet
                                select * from #listUser
                                select NamaRS from DaftarRSIA where IDRS = '" + IDRS + @"' 
                                select top 1 * from RValueVerifikator where IDRS = '" + IDRS + "' and IDRound = '" + Round + @"' and Periode =  '" + Periode + @"' 
                                select * from #tmpParam
                                ";

                #endregion

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    Result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }

                if (Result)
                {
                    if (dsOut != null & dsOut.Tables.Count > 0)
                    {
                        List<dtAreaVM> lstRoundArea = new List<dtAreaVM>();
                        foreach (DataRow item in dsOut.Tables[0].Rows)
                        {
                            var strIDRA = item["IDRoundArea"].ToString();
                            dtAreaVM dtRoundArea = new dtAreaVM();

                            foreach (DataRow tmpData in dsOut.Tables[1].Rows)
                            {
                                if (tmpData["IDRoundArea"].ToString() == strIDRA)
                                {
                                    dtRoundArea = new dtAreaVM();
                                    dtRoundArea.IDRoundArea = tmpData["IDRoundArea"].ToString();
                                    dtRoundArea.RoundArea = tmpData["RoundArea"].ToString();
                                    dtRoundArea.Total = decimal.Parse(tmpData["Total"].ToString());
                                    dtRoundArea.NilaiA = decimal.Parse(tmpData["NialiA"].ToString());
                                    dtRoundArea.NilaiB = decimal.Parse(tmpData["NilaiB"].ToString());
                                    dtRoundArea.NilaiC = decimal.Parse(tmpData["NilaiC"].ToString());
                                    dtRoundArea.persenA = Math.Round((dtRoundArea.NilaiA / dtRoundArea.Total) * 100, 2);
                                    dtRoundArea.persenB = Math.Round((dtRoundArea.NilaiB / dtRoundArea.Total) * 100, 2);
                                    dtRoundArea.persenC = Math.Round((dtRoundArea.NilaiC / dtRoundArea.Total) * 100, 2);

                                    var dtKet = new dtKeterangan();
                                    var listKetA = new List<dtKeterangan>();
                                    var listKetB = new List<dtKeterangan>();
                                    var listKetC = new List<dtKeterangan>();
                                    foreach (DataRow tmpKet in dsOut.Tables[2].Rows)
                                    {
                                        if (tmpKet["IDRoundArea"].ToString() == strIDRA & tmpKet["Nilai"].ToString() == "A")
                                        {
                                            dtKet = new dtKeterangan();
                                            dtKet.Keterangan = tmpKet["Ket"].ToString();
                                            listKetA.Add(dtKet);
                                        }
                                        else if (tmpKet["IDRoundArea"].ToString() == strIDRA & tmpKet["Nilai"].ToString() == "B")
                                        {
                                            dtKet = new dtKeterangan();
                                            dtKet.Keterangan = tmpKet["Ket"].ToString();
                                            listKetB.Add(dtKet);
                                        }
                                        else if (tmpKet["IDRoundArea"].ToString() == strIDRA & tmpKet["Nilai"].ToString() == "C")
                                        {
                                            dtKet = new dtKeterangan();
                                            dtKet.Keterangan = tmpKet["Ket"].ToString();
                                            listKetC.Add(dtKet);
                                        }
                                    }
                                    dtRoundArea.listKeteranganA = listKetA;
                                    dtRoundArea.listKeteranganB = listKetB;
                                    dtRoundArea.listKeteranganC = listKetC;

                                    lstRoundArea.Add(dtRoundArea);
                                    //model.listDataArea = dtRoundArea;
                                    model.totalItemPenilaian += dtRoundArea.Total;
                                    model.totalItemA += dtRoundArea.NilaiA;
                                    model.totalItemB += dtRoundArea.NilaiB;
                                    model.totalItemC += dtRoundArea.NilaiC;
                                }
                            }
                        }
                        model.listDataArea = lstRoundArea;
                        model.persenTotalItemA = Math.Round((model.totalItemA / model.totalItemPenilaian) * 100, 2);
                        model.persenTotalItemB = Math.Round((model.totalItemB / model.totalItemPenilaian) * 100, 2);
                        model.persenTotalItemC = Math.Round((model.totalItemC / model.totalItemPenilaian) * 100, 2);

                        model.bobotItemA = Math.Round((model.persenTotalItemA * 1), 3);
                        model.bobotItemB = Math.Round((model.persenTotalItemB * decimal.Parse("0.5")), 3);
                        model.bobotItemC = Math.Round((model.persenTotalItemC * decimal.Parse("0.25")), 3);

                        model.totalBobot = model.bobotItemA + model.bobotItemB + model.bobotItemC;
                        model.NamaRS = dsOut.Tables[4].Rows[0]["NamaRS"].ToString();
                        string periode = dsOut.Tables[5].Rows[0]["Periode"].ToString().Trim().Substring(4, 2);
                        model.Bulan = convertPeriode(periode) + " " + dsOut.Tables[5].Rows[0]["Periode"].ToString().Trim().Substring(0, 4);
                        model.StatusVer1 = dsOut.Tables[5].Rows[0]["Status1"].ToString();
                        model.StatusVer2 = dsOut.Tables[5].Rows[0]["Status2"].ToString();
                        model.IDRS = IDRS;
                        model.Round = Round;

                        foreach (DataRow item in dsOut.Tables[3].Rows)
                        {
                            if (item["KodeAkses"].ToString() == "Checker")
                            {
                                model.Checker = item["Nama"].ToString();
                            }
                            else if (item["KodeAkses"].ToString() == "Validator 1")
                            {
                                model.Validator1 = item["Nama"].ToString();
                            }
                            else if (item["KodeAkses"].ToString() == "Validator 2")
                            {
                                model.Validator2 = item["Nama"].ToString();
                            }
                            else if (item["KodeAkses"].ToString() == "Validator 3")
                            {
                                model.Validator3 = item["Nama"].ToString();
                            }
                            else if (item["KodeAkses"].ToString() == "Verifikator 1")
                            {
                                model.WakilDirektur = item["Nama"].ToString();
                            }
                            else if (item["KodeAkses"].ToString() == "Verifikator 2")
                            {
                                model.DIrektur = item["Nama"].ToString();
                            }
                        }

                        foreach (DataRow param in dsOut.Tables[6].Rows)
                        {
                            if (param["Parameter"].ToString().Trim() == "Aman")
                            {
                                model.totalAman = decimal.Parse(param["Jumlah"].ToString().Trim());
                                model.Aman = decimal.Parse(param["Yes"].ToString().Trim());
                                model.tdkAman = decimal.Parse(param["Dont"].ToString().Trim());
                                model.persenAman = Math.Round((model.Aman / model.totalAman) * 100, 2);
                            }
                            else if (param["Parameter"].ToString().Trim() == "Rapih")
                            {
                                model.totalRapih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                model.Rapih = decimal.Parse(param["Yes"].ToString().Trim());
                                model.tdkRapih = decimal.Parse(param["Dont"].ToString().Trim());
                                model.persenRapih = Math.Round((model.Rapih / model.totalRapih) * 100, 2);
                            }
                            else if (param["Parameter"].ToString().Trim() == "Tampak Baru")
                            {
                                model.totalBaru = decimal.Parse(param["Jumlah"].ToString().Trim());
                                model.Baru = decimal.Parse(param["Yes"].ToString().Trim());
                                model.tdkBaru = decimal.Parse(param["Dont"].ToString().Trim());
                                model.persenBaru = Math.Round((model.Baru / model.totalBaru) * 100, 2);
                            }
                            else if (param["Parameter"].ToString().Trim() == "Ramah Lingkungan")
                            {
                                model.totalRL = decimal.Parse(param["Jumlah"].ToString().Trim());
                                model.RL = decimal.Parse(param["Yes"].ToString().Trim());
                                model.tdkRL = decimal.Parse(param["Dont"].ToString().Trim());
                                model.persenRL = Math.Round((model.RL / model.totalRL) * 100, 2);
                            }
                            else if (param["Parameter"].ToString().Trim() == "Bersih")
                            {
                                model.totalBersih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                model.Bersih = decimal.Parse(param["Yes"].ToString().Trim());
                                model.tdkBersih = decimal.Parse(param["Dont"].ToString().Trim());
                                model.persenBersih = Math.Round((model.Bersih / model.totalBersih) * 100, 2);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
                return Result;
            }

            return Result;
        }
        public static string convertPeriode(string periode)
        {
            string Bulan = "";
            switch (periode)
            {
                case "01":
                    Bulan = "Januari";
                    break;
                case "02":
                    Bulan = "Februari";
                    break;
                case "03":
                    Bulan = "Maret";
                    break;
                case "04":
                    Bulan = "April";
                    break;
                case "05":
                    Bulan = "Mei";
                    break;
                case "06":
                    Bulan = "Juni";
                    break;
                case "07":
                    Bulan = "Juli";
                    break;
                case "08":
                    Bulan = "Agustus";
                    break;
                case "09":
                    Bulan = "September";
                    break;
                case "10":
                    Bulan = "Oktober";
                    break;
                case "11":
                    Bulan = "November";
                    break;
                case "12":
                    Bulan = "Desember";
                    break;
            }
            return Bulan;
        }
        public static string rollBackBulan(string bulan)
        {
            string Periode = "";
            switch (bulan)
            {
                case "Januari":
                    Periode = "01";
                    break;
                case "Februari":
                    Periode = "02";
                    break;
                case "Maret":
                    Periode = "03";
                    break;
                case "April":
                    Periode = "04";
                    break;
                case "Mei":
                    Periode = "05";
                    break;
                case "Juni":
                    Periode = "06";
                    break;
                case "Juli":
                    Periode = "07";
                    break;
                case "Agustus":
                    Periode = "08";
                    break;
                case "September":
                    Periode = "09";
                    break;
                case "Oktober":
                    Periode = "10";
                    break;
                case "November":
                    Periode = "11";
                    break;
                case "Desember":
                    Periode = "12";
                    break;
            }
            return Periode;
        }
        public static bool saveFasilitasRS(string[] Area, string[] SubArea, string[] Type, string[] OptionRoom, string[] OptionRL, string[] QTYRoom, string IDRS)
        {
            bool Result = false;
            string sqlCommand = "";

            sqlCommand = @"
                            declare @intDta int
                            set @intDta = (select count(Komponen) from RSIA" + IDRS + @")
                            select @intDta
                            if @intDta = 0
                             begin
                              insert into RSIA" + IDRS + @"(Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDItem, Item, Komponen, Parameter)
                              select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDItem, Item, Komponen, Parameter from DataStatis
                             end
                            ";

            if (Area != null)
            {
                foreach (var item in Area)
                {
                    sqlCommand += "insert into dbo.RSIA" + IDRS + @"(
                                Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDItem, Item, Komponen, Parameter
                                )
                                select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, 
                                IDItem, Item, Komponen, Parameter from DataDinamis where IDArea = '" + item + "' " + Environment.NewLine;
                }
            }

            if (SubArea != null)
            {
                foreach (var item in SubArea)
                {
                    sqlCommand += "insert into dbo.RSIA" + IDRS + @"(
                                Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDItem, Item, Komponen, Parameter
                                )
                                select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDItem, Item, 
                                Komponen, Parameter from DataDinamis where IDSubArea = '" + item + "' " + Environment.NewLine;
                }
            }

            if (Type != null)
            {
                foreach (var item in Type)
                {
                    sqlCommand += " insert into dbo.RSIA" + IDRS + @"(
                                Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDItem, Item, Komponen, Parameter
                                )
                                select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type,
                                IDItem, Item, Komponen, Parameter from DataDinamis where IDType = '" + item + "' " + Environment.NewLine;
                }
            }

            if (OptionRoom != null) // Hanya punya ruangan
            {
                foreach (var item in OptionRoom)
                {

                    string room = item.ToString();
                    string room1 = room.Replace("[", "");
                    string room2 = room1.Replace("]", "");
                    int lengRoom = room2.Length;
                    string jmlhRoom = room2.Substring(5, lengRoom - 5);
                    string type = room2.Substring(0, 5);

                    for (int i = 1; i <= int.Parse(jmlhRoom); i++)
                    {
                        sqlCommand += @"
                                        insert into dbo.RSIA" + IDRS + @"(Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, 
                                        IDType, Type, IDOption, OptionArea, IDItem, Item, Komponen, Parameter)
                                        select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption + '(" + i + @")' ,
                                        OptionArea + '(" + i + ")', IDItem + '(" + i + @")', Item, 
                                        Komponen, Parameter from DataDinamis where IDType = '" + type + "' " + Environment.NewLine;
                    }
                }
            }

            if (OptionRL != null & QTYRoom != null) // Lantai + ruangan
            {
                for (int i = 0; i < OptionRL.Length; i++)
                {
                    string room = OptionRL[i].ToString();
                    string room1 = room.Replace("[", "");
                    string room2 = room1.Replace("]", "");
                    int lengRoom = room2.Length;
                    string IDType = room2.Substring(0, 5);
                    int Lantai = int.Parse(room2.Substring(5, lengRoom - 5));
                    int Ruangan = int.Parse(QTYRoom[i].ToString());

                    for (int j = 1; j <= Ruangan; j++)
                    {
                        string noRuang = "";
                        if (j < 10)
                        {
                            noRuang = "00" + j;
                        }
                        else if (j >= 10 & j < 100)
                        {
                            noRuang = "0" + j;
                        }

                        sqlCommand += " insert into dbo.RSIA" + IDRS +
                                "(Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDOption, OptionArea, IDItem, Item, Komponen, Parameter)" +
                                "select Round, IDRoundArea, RoundArea, IDArea, Area, IDSubArea, SubArea, IDType, Type, IDType + '" + Lantai + "(" + noRuang + ")', 'Lantai " + Lantai + " Kamar " + noRuang + "'," +
                                " IDType + '[" + Lantai + "](" + noRuang + ")' + IDItem, Item, " +
                                "Komponen, Parameter from DataDinamis where IDType = '" + IDType + "' " + Environment.NewLine;
                    }
                }
            }

            string strErr = ""; DataSet ds = new DataSet();
            using (clsDBSQLConnection db = new clsDBSQLConnection())
            {
                Result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
            }

            return Result;
        }
        public static bool validasiVerifyReport(string KodeAkses, string IDRS, string Round, out string strError)
        {
            bool result = false; strError = "";
            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = @"
                                declare @strKodeAkses varchar(50),
		                                @strValidasi varchar(225),
		                                @intDtSubmit int,
                                        @strStatusVer varchar(50),
                                        @strStatus1 varchar(50),
		                                @strStatus2 varchar(50)


                                set @intDtSubmit = (select count(ID) from RSIA" + IDRS + @" where Status in ('Submit','Reject'))
                                if @intDtSubmit = 0
	                                begin
		                                set @strValidasi = 'Belum ada data yang disubmit Checker'
		                                goto ERROR
	                                end
                                set @strStatusVer = (select top 1 StatusData from RValueVerifikator where IDRS = '" + IDRS + @"' 
                                and IDRound = '" + Round + @"' order by ID desc )
                                set @strStatus1 = (select top 1 Status1 from RValueVerifikator where IDRS = '" + IDRS + @"' 
                                and IDRound = '" + Round + @"' order by ID desc )
                                set @strStatus2 = (select top 1 Status2 from RValueVerifikator where IDRS = '" + IDRS + @"' 
                                and IDRound = '" + Round + @"' order by ID desc )

                                set @strKodeAkses = '" + KodeAkses + @"'
                                if @strKodeAkses = 'Verifikator 1'
	                                begin 
		                                if @strStatusVer = 'Reject'
		                                begin
			                                set @strValidasi = 'Data dikembalikan ke Checker'
			                                goto ERROR
		                                end		
                                        if @strStatus1 = 'Verify' and @strStatus2 = ''
		                                 begin
			                                set @strValidasi = 'Anda sudah melakukan autorisasi untuk data ini.'
			                                goto ERROR
		                                 end	
	                                end
                                if @strKodeAkses = 'Verifikator 2'
                                 begin
                                    if @strStatusVer = 'Reject'
		                                begin
			                                set @strValidasi = 'Data dikembalikan ke Checker'
			                                goto ERROR
		                                end	
                                    if @strStatus2 = 'Verify'
		                             begin
			                            set @strValidasi = 'Anda sudah melakukan autorisasi untuk data ini.'
			                            goto ERROR
		                             end
	                                if (select top 1 Status1 from RValueVerifikator where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' order by ID desc) != 'Verify'
	                                 begin
	                                  set @strValidasi = 'Data belum  di Verifiksi oleh Wakil Direktur'
                                      goto ERROR
	                                 end
                                 end

                                ERROR:
                                select @strValidasi as Validasi
                                ";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strError);
                }

                if (result & ds != null & ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Validasi"].ToString() != "")
                        {
                            result = false;
                            strError = ds.Tables[0].Rows[0]["Validasi"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                result = false;
            }
            return result;
        }
        public static bool authVerifikator(string IDRS, string IDReg, string Akses, string Round, string Submit,
                                            string Comment, string Periode, out string strErr)
        {
            bool result = false; strErr = "";
            string sqlQommand = ""; DataSet dsOut = new DataSet(); var model = new ExcReportVM();
            try
            {
                int leng = Periode.Length;
                string tahun = Periode.Substring(leng - 4, 4);
                string bulan = Periode.Substring(0, leng - 5);
                string masa = Periode;
                string Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //if (string.IsNullOrEmpty(Comment))
                //{
                //    Comment = "";
                //}

                #region SQLCommand
                sqlQommand = @"
                                declare	@strKodeAkses varchar(50),
		                                @strNilai varchar(50)		

                                set @strKodeAkses = '" + Akses + @"'
                                set @strNilai = '" + Submit + @"'
                                if @strKodeAkses = 'Verifikator 1'
                                 begin
                                  if @strNilai = 'Verify'
                                   begin
                                    update RValueVerifikator set IDRegVerify1 = '" + IDReg + @"', Verifikasi1Date = '" + Date + @"', Status1 = 'Verify', 
	                                KetVerifikator1 = '" + Comment + @"', IDRegVerify2 = '', Verifikasi2Date = '', Status2 = '', KetVerifikator2 = ''
	                                where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + masa + @"'
                                   end
                                  else --reject
                                   begin
                                    update RValueVerifikator set StatusData = 'Reject', IDRegVerify1 = '" + IDReg + @"', Verifikasi1Date = '" + Date + @"',
	                                Status1 = 'Reject', KetVerifikator1 = '" + Comment + @"',
	                                IDRegVerify2 = '', Verifikasi2Date = '', Status2 = '', KetVerifikator2 = ''
	                                where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + masa + @"'
	                                update RSIA" + IDRS + @" set Status = 'Reject' where Round = '" + Round + @"' and Periode = '" + masa + @"' and Status = 'Submit'
	                                update RABRS" + IDRS + @" set Status = 'Reject' where Round = '" + Round + @"' and Periode = '" + masa + @"' and Status = 'Submit'
                                   end
                                 end
                                else if @strKodeAkses = 'Verifikator 2'
                                 begin 
                                  if @strNilai = 'Verify'
                                   begin
                                    update RValueVerifikator set StatusData = 'Verify', IDRegVerify2 = '" + IDReg + @"', Verifikasi2Date = '" + Date + @"', Status2 = 'Verify', KetVerifikator2 = '" + Comment + @"'
	                                where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + masa + @"'
	                                --tambahkan logict ambil data excReport

                                Create table #temp1
                                (	
	                                ID int IDENTITY(1, 1) primary key ,
	                                IDRoundArea varchar(25),
	                                RoundArea varchar(350)
                                )

                                Create table #tmpData
                                (	
	                                ID int IDENTITY(1, 1) primary key ,
	                                IDRoundArea varchar(25),
	                                RoundArea varchar(350),
	                                Total int,
	                                NialiA int,
	                                NilaiB int,
	                                NilaiC int
                                )

                                Create table #listKet
                                (	
	                                ID int IDENTITY(1, 1) primary key ,
	                                IDRoundArea varchar(25),
	                                Nilai varchar(5),
	                                Ket varchar(max)
                                )

                                create table #listUser
                                (
	                                KodeAkses varchar(50),
	                                Nama varchar(250),
	                                Email varchar(300)
                                )

                                Create Table #tmpParam
                                ( 
	                                ID		int IDENTITY(1, 1) primary key,
	                                Parameter	varchar(50),
	                                Jumlah	int,
	                                Yes		int,
	                                Dont	int
                                )

                                insert into #tmpParam(Parameter) select distinct Parameter from RSIA" + IDRS + @" where Round = '" + Round + @"'
                            
                                insert into #temp1(IDRoundArea, RoundArea)
                                select distinct IDRoundArea, RoundArea from RSIA" + IDRS + " where Round = '" + Round + @"'

                                insert into #tmpData(IDRoundArea, RoundArea)
                                select distinct IDRoundArea, RoundArea from RSIA" + IDRS + " where Round = '" + Round + @"'

                                insert into #listKet(IDRoundArea, Nilai, Ket)
                                select IDRoundArea, Nilai, Ket from RSIA" + IDRS + @" where Ket != ''

                                insert into #listUser(KodeAkses, Nama, Email)
                                select KodeAkses, Nama, Email from MUser where IDRS = '" + IDRS + @"' 
                                union 
                                select KodeAkses, Nama, Email from MUser where  KodeAkses in ('Validator 1' , 'Validator 3')
                                union 
                                select a.KodeAkses, a.Nama, a.Email from Muser a join DaftarRSIA b on a.KetReg = b.Regional 
                                where b.IDRS =  '" + IDRS + @"' 
        
                                declare @int int, @jmlh int, @intHt int, @intJHt int

                                set @int = 1
                                set @jmlh = (select COUNT(RoundArea) from #temp1)

                                while @int <= @jmlh	
	                                begin
		                                update #tmpData set NialiA = (select count(Komponen)
		                                from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='A')
		                                where ID = @int

		                                update #tmpData set NilaiB = (select count(Komponen)
		                                from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='B')
		                                where ID = @int
		
		                                update #tmpData set NilaiC = (select count(Komponen)
		                                from RSIA" + IDRS + @" a join #temp1 b on a.IDRoundArea = b.IDRoundArea where b.ID = @int and a.Nilai ='C')
		                                where ID = @int

		                                update #tmpData set Total = (select COUNT(Komponen) from RSIA" + IDRS + @" a
									                                join #temp1 b on a.IDRoundArea = b.IDRoundArea where a.Round = '" + Round + @"' and b.ID = @int)
		                                where ID = @int

		                                set @int = @int + 1		
	                                end
                                set @intHt = 1
                                set @intJHt = (select count(Parameter) from #tmpParam)

                                while @intHt <= @intJHt
                                 begin
	                                update #tmpParam set Jumlah = (select count(a.Parameter) from RSIA" + IDRS + @" a 
	                                join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.Round = '" + Round + @"') where ID = @intHt

	                                Update #tmpParam set Yes = (select count(a.ParamValue) from RSIA" + IDRS + @" a 
	                                join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.ParamValue = b.Parameter and a.Round = '" + Round + @"') where ID = @intHt

	                                Update #tmpParam set Dont = (select count(a.ParamValue) from RSIA" + IDRS + @" a 
	                                join #tmpParam b on a.Parameter = b.Parameter where b.ID = @intHt and a.ParamValue like '%Tidak%' and a.Round = '" + Round + @"') where ID = @intHt

	                                set @intHt = @intHt + 1
                                 end

                                select * from #temp1
                                select * from #tmpData
                                select * from #listKet
                                select * from #listUser
                                select NamaRS from DaftarRSIA where IDRS = '" + IDRS + @"' 
                                select top 1 Periode from RValueVerifikator where IDRS = '" + IDRS + "' and IDRound = '" + Round + @"' order by Periode desc
                                select * from #tmpParam

                                    update RSIA" + IDRS + @" set Status = 'Verify' where Round = '" + Round + @"' and Periode = '" + masa + @"' and Status = 'Submit'
	                                update RABRS" + IDRS + @" set Status = 'Verify' where Round = '" + Round + @"' and Periode = '" + masa + @"' and Status = 'Submit'
                                   end
                                  else --reject
                                   begin
	                                update RValueVerifikator set IDRegVerify2 = '" + IDReg + @"', Verifikasi2Date = '" + Date + @"', Status2 = 'Reject', KetVerifikator2 = '" + Comment + @"'
	                                where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + masa + @"'
                                   end
                                 end
                                ";
                #endregion

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlQommand, out dsOut, out strErr);
                }

                if (result)
                {
                    if (Akses == "Verifikator 2")
                    {
                        if (Submit == "Verify")
                        {
                            if (dsOut != null)
                            {
                                var dsXML = dsOut.GetXml().ToString();
                                #region convData
                                List<dtAreaVM> lstRoundArea = new List<dtAreaVM>();
                                foreach (DataRow item in dsOut.Tables[0].Rows)
                                {
                                    var strIDRA = item["IDRoundArea"].ToString();
                                    dtAreaVM dtRoundArea = new dtAreaVM();

                                    foreach (DataRow tmpData in dsOut.Tables[1].Rows)
                                    {
                                        if (tmpData["IDRoundArea"].ToString() == strIDRA)
                                        {
                                            dtRoundArea = new dtAreaVM();
                                            dtRoundArea.IDRoundArea = tmpData["IDRoundArea"].ToString();
                                            dtRoundArea.RoundArea = tmpData["RoundArea"].ToString();
                                            dtRoundArea.Total = decimal.Parse(tmpData["Total"].ToString());
                                            dtRoundArea.NilaiA = decimal.Parse(tmpData["NialiA"].ToString());
                                            dtRoundArea.NilaiB = decimal.Parse(tmpData["NilaiB"].ToString());
                                            dtRoundArea.NilaiC = decimal.Parse(tmpData["NilaiC"].ToString());
                                            dtRoundArea.persenA = Math.Round((dtRoundArea.NilaiA / dtRoundArea.Total) * 100, 2);
                                            dtRoundArea.persenB = Math.Round((dtRoundArea.NilaiB / dtRoundArea.Total) * 100, 2);
                                            dtRoundArea.persenC = Math.Round((dtRoundArea.NilaiC / dtRoundArea.Total) * 100, 2);

                                            model.totalItemPenilaian += dtRoundArea.Total;
                                            model.totalItemA += dtRoundArea.NilaiA;
                                            model.totalItemB += dtRoundArea.NilaiB;
                                            model.totalItemC += dtRoundArea.NilaiC;
                                        }
                                    }
                                }
                                model.persenTotalItemA = Math.Round((model.totalItemA / model.totalItemPenilaian) * 100, 2);
                                model.persenTotalItemB = Math.Round((model.totalItemB / model.totalItemPenilaian) * 100, 2);
                                model.persenTotalItemC = Math.Round((model.totalItemC / model.totalItemPenilaian) * 100, 2);

                                model.bobotItemA = Math.Round((model.persenTotalItemA * 1), 3);
                                model.bobotItemB = Math.Round((model.persenTotalItemB * decimal.Parse("0.5")), 3);
                                model.bobotItemC = Math.Round((model.persenTotalItemC * decimal.Parse("0.25")), 3);

                                model.totalBobot = model.bobotItemA + model.bobotItemB + model.bobotItemC;
                                model.NamaRS = dsOut.Tables[4].Rows[0]["NamaRS"].ToString();
                                string periode = dsOut.Tables[5].Rows[0]["Periode"].ToString().Trim();
                                string month = dsOut.Tables[5].Rows[0]["Periode"].ToString().Trim().Substring(4, 2);
                                //float bobot = float.Parse(model.totalBobot.ToString());
                                //cara bodong repace koma jadi titik. ujicoba string jadi angka ngak ke SQL tanpa ''
                                #endregion

                                sqlQommand = @"
                                            declare @strRound varchar(25),
		                                            @fldTotalData float,
		                                            @fldAkumul float
                                                
                                                set @fldTotalData = (select TotalData from DataAkumulasi where IDRS = '" + IDRS + @"' and Periode = '" + masa + @"') + @fload --Total dari depan
	                                            set @fldAkumul = @fldTotalData / 4
                                                
                                                update DataLog set DateInsert = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"', Data = '" + dsXML + @"' where IDRS = '" + IDRS + @"' and Periode = '" + masa + @"' and Round = '" + Round + @"'
                                                update DataAkumulasi set NamaRS = '" + model.NamaRS + @"', TotalData = @fldTotalData, Akumulasi = @fldAkumul where IDRS = '" + IDRS + @"' and Periode = '" + masa + @"'
	                                            update DataReport set Data = '" + model.totalBobot + @"' where IDRS = '" + IDRS + @"' and Periode = '" + masa + @"' and Round = '" + Round + @"' --total data dari depan
                                                update RSIA" + IDRS + @" set IDChecker = null, Nilai = null, Status = null, Ket = null, 
	                                            ParamValue = null, IDUnix = null, Periode = null where Round = '" + Round + @"'
                                                insert RValueApproval(IDRS, IDRound, DateVerify, StatusData, Periode)
                                                values('" + IDRS + @"','" + Round + @"','" + Date + @"','Verify','" + masa + @"')
                                         ";

                                SqlParameter[] dbParam = new SqlParameter[1];
                                dbParam[0] = new SqlParameter("@fload", SqlDbType.Float);
                                dbParam[0].Value = model.totalBobot;
                                DataSet dt = new DataSet();
                                using (clsDBSQLConnection dbb = new clsDBSQLConnection())
                                {
                                    result = dbb.QueryCommand(Connection, sqlQommand, ref dbParam, out dt, out strErr);
                                }
                            }
                            else
                            {
                                strErr = "Terdapat kesalahan dalam proses Inquery data";
                                result = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                strErr = ex.Message;
            }

            return result;
        }
        public static bool getDataLogForVer(string IDRS, string Round, string Period, out ExcReportVM model, out string strErr)
        {
            model = new ExcReportVM(); strErr = "";
            bool result = false;
            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = @"
                            declare @strDataLog varchar(max)
                            set @strDataLog = (select Data from DataLog where IDRS = '" + IDRS + @"' and Round = '" + Round + @"' and Periode = '" + Period + @"')
                            if @strDataLog is null
                             begin
                              select 'Data belum di verify' as Data 
                             end
                            else
                             begin
                              select @strDataLog as Data
                             end
                            ";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }
                if (result)
                {
                    if (ds != null & ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["Data"].ToString() == "Data belum di verify")
                            {
                                result = false;
                                strErr = "Data belum di verify";
                            }
                            else
                            {
                                DataSet data = new DataSet();
                                System.IO.StringReader XML = new System.IO.StringReader(ds.Tables[0].Rows[0]["Data"].ToString());
                                data.ReadXml(XML);
                                List<dtAreaVM> tmpListData = new List<dtAreaVM>();
                                dtAreaVM tmpdata = new dtAreaVM();
                                foreach (DataRow item in data.Tables["Table"].Rows)
                                {
                                    string IDArea = item["IDRoundArea"].ToString();
                                    foreach (DataRow dataArea in data.Tables["Table1"].Rows)
                                    {
                                        if (dataArea["IDRoundArea"].ToString() == IDArea)
                                        {
                                            tmpdata = new dtAreaVM();
                                            tmpdata.IDRoundArea = dataArea["IDRoundArea"].ToString();
                                            tmpdata.RoundArea = dataArea["RoundArea"].ToString();
                                            tmpdata.Total = decimal.Parse(dataArea["Total"].ToString());
                                            tmpdata.NilaiA = decimal.Parse(dataArea["NialiA"].ToString());
                                            tmpdata.NilaiB = decimal.Parse(dataArea["NilaiB"].ToString());
                                            tmpdata.NilaiC = decimal.Parse(dataArea["NilaiC"].ToString());
                                            tmpdata.persenA = Math.Round((tmpdata.NilaiA / tmpdata.Total) * 100, 2);
                                            tmpdata.persenB = Math.Round((tmpdata.NilaiB / tmpdata.Total) * 100, 2);
                                            tmpdata.persenC = Math.Round((tmpdata.NilaiC / tmpdata.Total) * 100, 2);
                                            var dtKet = new dtKeterangan();
                                            var listKetA = new List<dtKeterangan>();
                                            var listKetB = new List<dtKeterangan>();
                                            var listKetC = new List<dtKeterangan>();
                                            if (data.Tables.Count == 7)
                                            {
                                                foreach (DataRow tmpKet in data.Tables["Table2"].Rows)
                                                {
                                                    if (tmpKet["IDRoundArea"].ToString() == IDArea & tmpKet["Nilai"].ToString() == "A")
                                                    {
                                                        dtKet = new dtKeterangan();
                                                        dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                        listKetA.Add(dtKet);
                                                    }
                                                    else if (tmpKet["IDRoundArea"].ToString() == IDArea & tmpKet["Nilai"].ToString() == "B")
                                                    {
                                                        dtKet = new dtKeterangan();
                                                        dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                        listKetB.Add(dtKet);
                                                    }
                                                    else if (tmpKet["IDRoundArea"].ToString() == IDArea & tmpKet["Nilai"].ToString() == "C")
                                                    {
                                                        dtKet = new dtKeterangan();
                                                        dtKet.Keterangan = tmpKet["Ket"].ToString();
                                                        listKetC.Add(dtKet);
                                                    }
                                                }
                                            }
                                            tmpdata.listKeteranganA = listKetA;
                                            tmpdata.listKeteranganB = listKetB;
                                            tmpdata.listKeteranganC = listKetC;

                                            tmpListData.Add(tmpdata);
                                            model.totalItemPenilaian += tmpdata.Total;
                                            model.totalItemA += tmpdata.NilaiA;
                                            model.totalItemB += tmpdata.NilaiB;
                                            model.totalItemC += tmpdata.NilaiC;
                                        }
                                    }
                                }
                                model.listDataArea = tmpListData;
                                model.persenTotalItemA = Math.Round((model.totalItemA / model.totalItemPenilaian) * 100, 2);
                                model.persenTotalItemB = Math.Round((model.totalItemB / model.totalItemPenilaian) * 100, 2);
                                model.persenTotalItemC = Math.Round((model.totalItemC / model.totalItemPenilaian) * 100, 2);

                                model.bobotItemA = Math.Round((model.persenTotalItemA * 1), 3);
                                model.bobotItemB = Math.Round((model.persenTotalItemB * decimal.Parse("0.5")), 3);
                                model.bobotItemC = Math.Round((model.persenTotalItemC * decimal.Parse("0.25")), 3);

                                model.totalBobot = model.bobotItemA + model.bobotItemB + model.bobotItemC;
                                model.NamaRS = data.Tables["Table4"].Rows[0]["NamaRS"].ToString();
                                string periode = data.Tables["Table5"].Rows[0]["Periode"].ToString().Trim().Substring(4, 2);
                                model.Bulan = convertPeriode(periode) + " " + data.Tables["Table5"].Rows[0]["Periode"].ToString().Trim().Substring(0, 4);
                                model.IDRS = IDRS;
                                model.Round = Round;
                                model.Periode = Period;

                                foreach (DataRow item in data.Tables["Table3"].Rows)
                                {
                                    if (item["KodeAkses"].ToString() == "Checker")
                                    {
                                        model.Checker = item["Nama"].ToString();
                                    }
                                    else if (item["KodeAkses"].ToString() == "Validator 1")
                                    {
                                        model.Validator1 = item["Nama"].ToString();
                                    }
                                    else if (item["KodeAkses"].ToString() == "Validator 2")
                                    {
                                        model.Validator2 = item["Nama"].ToString();
                                    }
                                    else if (item["KodeAkses"].ToString() == "Validator 3")
                                    {
                                        model.Validator3 = item["Nama"].ToString();
                                    }
                                    else if (item["KodeAkses"].ToString() == "Verifikator 1")
                                    {
                                        model.WakilDirektur = item["Nama"].ToString();
                                    }
                                    else if (item["KodeAkses"].ToString() == "Verifikator 2")
                                    {
                                        model.DIrektur = item["Nama"].ToString();
                                    }
                                }
                                foreach (DataRow param in data.Tables["Table6"].Rows)
                                {
                                    if (param["Parameter"].ToString().Trim() == "Aman")
                                    {
                                        model.totalAman = decimal.Parse(param["Jumlah"].ToString().Trim());
                                        model.Aman = decimal.Parse(param["Yes"].ToString().Trim());
                                        model.tdkAman = decimal.Parse(param["Dont"].ToString().Trim());
                                        model.persenAman = Math.Round((model.Aman / model.totalAman) * 100, 2);
                                    }
                                    else if (param["Parameter"].ToString().Trim() == "Rapih")
                                    {
                                        model.totalRapih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                        model.Rapih = decimal.Parse(param["Yes"].ToString().Trim());
                                        model.tdkRapih = decimal.Parse(param["Dont"].ToString().Trim());
                                        model.persenRapih = Math.Round((model.Rapih / model.totalRapih) * 100, 2);
                                    }
                                    else if (param["Parameter"].ToString().Trim() == "Tampak baru")
                                    {
                                        model.totalBaru = decimal.Parse(param["Jumlah"].ToString().Trim());
                                        model.Baru = decimal.Parse(param["Yes"].ToString().Trim());
                                        model.tdkBaru = decimal.Parse(param["Dont"].ToString().Trim());
                                        model.persenBaru = Math.Round((model.Baru / model.totalBaru) * 100, 2);
                                    }
                                    else if (param["Parameter"].ToString().Trim() == "Ramah Lingkungan")
                                    {
                                        model.totalRL = decimal.Parse(param["Jumlah"].ToString().Trim());
                                        model.RL = decimal.Parse(param["Yes"].ToString().Trim());
                                        model.tdkRL = decimal.Parse(param["Dont"].ToString().Trim());
                                        model.persenRL = Math.Round((model.RL / model.totalRL) * 100, 2);
                                    }
                                    else if (param["Parameter"].ToString().Trim() == "Bersih")
                                    {
                                        model.totalBersih = decimal.Parse(param["Jumlah"].ToString().Trim());
                                        model.Bersih = decimal.Parse(param["Yes"].ToString().Trim());
                                        model.tdkBersih = decimal.Parse(param["Dont"].ToString().Trim());
                                        model.persenBersih = Math.Round((model.Bersih / model.totalBersih) * 100, 2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                strErr = ex.Message;
            }

            return result;
        }
        public static bool validasiCheck(string IDRS, string Round, out string strErr)
        {
            strErr = "";
            bool result = false;
            string sqlCommand = ""; DataSet ds = new DataSet();
            string periode = DateTime.Now.ToString("yyyyMM");
            try
            {
                sqlCommand = "select * from RValueVerifikator where IDRS = '" + IDRS + "' and IDRound = '" + Round + "' and Periode = '" + periode + "' and StatusData in ('Submit', 'Verify')";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                    if (result)
                    {
                        if (ds.Tables.Count > 0 & ds.Tables[0].Rows.Count > 0)
                        {
                            result = false;
                            strErr = "Data diproses oleh Verifikator";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
                throw;
            }

            return result;
        }

        #region RAB
        //BAGIAN RAB-->
        //========================== Cek Data RAB
        public static bool ValidasiRound(string IDRS, string KodeAkses, string Periode, out List<RABCheck> model, out List<CheckRound> model2, out string strErr)
        {
            strErr = "";
            bool result = false;
            string sqlCommand = ""; DataSet ds = new DataSet();
            model = new List<RABCheck>();
            model2 = new List<CheckRound>();

            try
            {
                sqlCommand = "select * from RValueVerifikator where IDRS = '" + IDRS + "' and SUBSTRING(Verifikasi2Date, 1, 7) = '" + Periode + "' " +
                             "select distinct Status, Round from RABRS" + IDRS + " where Periode = '" + Periode.Replace("-", "") + "'  " +
                             "select * from  PenilaianRAB where IDRS = '" + IDRS + "' and Periode = '" + Periode.Replace("-", "") + "' " +
                             "select * from ApprovalRAB where IDRS = '" + IDRS + "' and Periode = '" + Periode.Replace("-", "") + "'  " +
                             "select * from RValueVerifikator where IDRS = '" + IDRS + "' and  Periode = '"+ Periode.Replace("-", "") + "'";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }
                if (result)
                {
                    if (ds != null)
                    {
                        List<RABCheck> dtTemp = new List<RABCheck>();
                        List<RABCheck> dkTemp = new List<RABCheck>();
                        List<CheckRound> dlTemp = new List<CheckRound>();
                        RABCheck data = new RABCheck();
                        CheckRound listdata = new CheckRound();
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            data = new RABCheck();
                            data.KodeAkses = KodeAkses;
                            data.Round = item["IDRound"].ToString();
                            data.StatusChecker = item["StatusData"].ToString();
                            data.Status1 = item["Status1"].ToString();
                            data.Status2 = item["Status2"].ToString();
                            data.Periode = item["Periode"].ToString();
                            data.Verifikasi2Date = item["Verifikasi2Date"].ToString();
                            foreach (DataRow dt in ds.Tables[1].Rows)
                            {
                                if (data.Round == dt["Round"].ToString())
                                {
                                    data.StatusUmum = dt["Status"].ToString();
                                }
                            }
                            foreach (DataRow dk in ds.Tables[2].Rows)
                            {
                                if (data.Round == dk["IDRound"].ToString())
                                {
                                    data.StatusPenilaian1 = dk["StatusData"].ToString();
                                    data.StatusPenilaian2 = dk["Status1"].ToString();
                                    data.StatusPenilaian3 = dk["Status2"].ToString();
                                }
                            }
                            foreach (DataRow dm in ds.Tables[3].Rows)
                            {
                                if (data.Round == dm["IDRound"].ToString()) {
                                    data.Approve = dm["Status3"].ToString();
                                    data.ApproveDate = dm["Approval3Date"].ToString();
                                }
                            }
                            dtTemp.Add(data);
                        }
                        model = dtTemp;
                        foreach (DataRow dl in ds.Tables[4].Rows)
                        {
                            listdata = new CheckRound();
                            listdata.Round = dl["IDRound"].ToString();
                            listdata.StatusChecker = dl["StatusData"].ToString();
                            listdata.Status1 = dl["Status1"].ToString();
                            listdata.Status2 = dl["Status2"].ToString();
                            listdata.Periode = dl["Periode"].ToString();
                            dlTemp.Add(listdata);
                        }
                        model2 = dlTemp;
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
                throw;
            }
            return result;
        }
        //========================== Cek Data RAB

        public static bool getDataRAB(string IDRS, string Round, string Periode, out RABVM model, out string strErr)
        {
            bool result = false;
            model = new RABVM();
            strErr = "";
            string sqlCommand = ""; DataSet dsOut = new DataSet();
            try
            {
                sqlCommand = "select * from RABRS" + IDRS + " where Round = '" + Round + "' and Periode = '" + Periode + "' and Status in ('Verify', 'SaveR') " +
                             " select distinct IDKategori, Kategori from DataHarga ";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }

                if (result)
                {
                    if (dsOut != null)
                    {
                        List<SubmitRABVM> tmpList = new List<SubmitRABVM>();
                        SubmitRABVM data = new SubmitRABVM();

                        #region FromTabelHarga
                        List<DtKategoriBahanVM> tmp = new List<DtKategoriBahanVM>();
                        DtKategoriBahanVM dtTmp = new DtKategoriBahanVM();
                        foreach (DataRow tempI in dsOut.Tables[1].Rows)
                        {
                            dtTmp = new DtKategoriBahanVM();
                            dtTmp.IDKategori = tempI["IDKategori"].ToString();
                            dtTmp.Kategori = tempI["Kategori"].ToString();
                            tmp.Add(dtTmp);
                        }
                        #endregion

                        foreach (DataRow item in dsOut.Tables[0].Rows)
                        {
                            data = new SubmitRABVM();
                            data.ID = int.Parse(item["ID"].ToString());
                            data.Round = item["Round"].ToString();
                            data.SubArea = item["SubArea"].ToString();
                            data.Item = item["Item"].ToString();
                            data.Penilaian = item["Komponen"].ToString();
                            data.Nilai = item["Nilai"].ToString();
                            data.Keterangan = item["Ket"].ToString();
                            data.Periode = item["Periode"].ToString();
                            data.LsKategoriBrg = tmp;
                            data.PictureBf = item["JPG"].ToString();
                            data.KomponenPerbaiki = item["KomponenPerbaikan"].ToString();
                            data.ItemPerbaiki = item["ItemPerbaikan"].ToString();
                            data.DeskripsiPerbaiki = item["DeskripsiPerbaikan"].ToString();
                            data.HargaDasar = item["HargaDasar"].ToString();
                            data.HargaInput = item["HargaInput"].ToString();
                            data.SelisihHarga = item["SelisihHarga"].ToString();
                            tmpList.Add(data);

                        }
                        model.listSubmitRAB = tmpList;
                    }
                    else
                    {
                        result = false;
                        strErr = "Data Tidak Ditemukan.!";
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }
            return result;
        }
        public static bool getDataSPK(string IDRS, string Round, string Periode, out DataSPK model, out string strErr) {
            strErr = "";
            bool result = true;
            string sqlCommand = ""; DataSet ds = new DataSet();
            model = new DataSPK();

            try
            {
                sqlCommand = "select * from RABRS" + IDRS + " where Round = '" + Round + "' and Periode = '" + Periode + "' and Status in ('Diketahui', 'Finish') " +
                             "select * from ApprovalRAB where IDRS ='"+ IDRS +"' and IDRound = '" + Round + "' and Periode = '" + Periode + "' and Status3 = 'Diketahui'";

                //sqlCommand = "select * from RABRS" + IDRS + " where Periode = '" + Periode.Replace("-", "") + "' and Status = 'Diketahui' " +
                //             "select * from ApprovalRAB where Periode = '" + Periode.Replace("-", "") + "' and Status3 = 'Diketahui'";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }
                if (result) {
                    if (ds != null) {
                        List<ListDataSPK> dspk = new List<ListDataSPK>();
                        ListDataSPK data = new ListDataSPK();
                        foreach (DataRow item in ds.Tables[0].Rows) {
                            data = new ListDataSPK();
                            data.ID = int.Parse(item["ID"].ToString());
                            data.Round = item["Round"].ToString();
                            data.SubArea = item["SubArea"].ToString();
                            data.Item = item["Item"].ToString();
                            data.Penilaian = item["Komponen"].ToString();
                            data.Nilai = item["Nilai"].ToString();
                            data.Keterangan = item["Ket"].ToString();
                            data.Periode = item["Periode"].ToString();
                            data.IDUnix = item["IDUnix"].ToString();

                            data.PictureBf = item["JPG"].ToString();
                            data.PictureAf = item["JPGAfter"].ToString();
                            data.KomponenPerbaiki = item["KomponenPerbaikan"].ToString();
                            data.ItemPerbaiki = item["ItemPerbaikan"].ToString();
                            data.DeskripsiPerbaiki = item["DeskripsiPerbaikan"].ToString();
                            data.HargaDasar = item["HargaDasar"].ToString();
                            data.HargaInput = item["HargaInput"].ToString();
                            data.SelisihHarga = item["SelisihHarga"].ToString();
                            dspk.Add(data);
                        }
                        model.ListSPK = dspk;
                        model.TimeStart = ds.Tables[1].Rows[0]["Approval3Date"].ToString();
                        model.Periode = ds.Tables[1].Rows[0]["Periode"].ToString();
                        model.Round = ds.Tables[1].Rows[0]["IDRound"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }

            return result;
        }
        public static bool getListSPK(string IDRS, out DataSPK model, out string strErr)
        {
            strErr = "";
            bool result = true;
            string sqlCommand = ""; DataSet ds = new DataSet();
            model = new DataSPK();
            try
            {
                sqlCommand = "select distinct Round, Status, Periode from RABRS" + IDRS + " where Status in ('Diketahui', 'Finish') " +
                             "select * from ApprovalRAB where IDRS = '"+ IDRS + "' and Status3 = 'Diketahui'";

                using (clsDBSQLConnection db = new clsDBSQLConnection()) {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }
                if (result) {
                    if (ds != null) {
                        List<ListDataSPK> spk = new List<ListDataSPK>();
                        ListDataSPK data = new ListDataSPK();
                        foreach (DataRow item in ds.Tables[0].Rows) {
                            data = new ListDataSPK();
                            data.Round = item["Round"].ToString();
                            data.Periode = item["Periode"].ToString();
                            data.Status = item["Status"].ToString();
                            spk.Add(data);
                        }
                        model.ListSPK = spk;
                        List<ListApproveSPK> appspk = new List<ListApproveSPK>();
                        ListApproveSPK app = new ListApproveSPK();
                        foreach (DataRow a in ds.Tables[1].Rows) {
                            app = new ListApproveSPK();
                            app.RoundApp = a["IDRound"].ToString();
                            app.PeriodeApp = a["Periode"].ToString();
                            app.TimeStartApp = a["Approval3Date"].ToString();
                            appspk.Add(app);
                        }
                        model.listAppSPK = appspk;
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }
            return result;

        }
        public static bool submitSPK(string IDRS, string Round, string Periode, out string strErr)
        {
            strErr = "";
            bool result = true;
            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = "update RABRS" + IDRS + " set Status = 'Finish' where Periode = '"+ Periode.Replace("-", "") + "' and Round = '"+ Round +"'  " +
                    @"BEGIN
                        IF NOT EXISTS(Select * from DataSPK where IDRS = '"+ IDRS +@"' and Round = '"+ Round +@"' and Periode = '"+ Periode +@"')
                        BEGIN
                            insert into DataSPK(IDRS, Periode, Round, Status, sumSPK, doneSPK) values
                           ('"+IDRS+@"', '"+Periode+@"', '"+Round+@"', 'Finish',
                           (select COUNT(*) from RABRS"+IDRS+@" where Round = '"+Round+@"' and Periode = '"+Periode+@"'), 
		                    (select count(JPGAfter) from RABRS"+IDRS+@" where Round = '"+Round+@"' and Periode = '"+Periode+@"' and JPGAfter is not null))
	                    END
                      END";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }
            return result;
        }

        public static bool uploadIMGSPK(string IDRS, string Periode, string Round, string IDUnix, string ImageAfter, out DataSPK model, out string strErr)
        {
            strErr = "";
            bool result = true;
            string sqlCommand = ""; DataSet ds = new DataSet();
            model = new DataSPK();
            try
            {
                sqlCommand = "update RABRS"+ IDRS + " set JPGAfter = '"+ ImageAfter + "' where Replace(IDunix, 'Round ', '') = '"+ IDUnix + "'";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }
            return result;
        }
        public static bool getItemHarga(string IDKategori, out string[,] data, out string strErr)
        {
            bool result = true;
            strErr = "";
            DataSet dsOut = new DataSet();
            data = new string[1, 1];
            string sqlCommand = "select distinct IDItem, Item from DataHarga where IDKategori = '" + IDKategori + "'";

            try
            {
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }


                if (result)
                {
                    data = new string[dsOut.Tables[0].Rows.Count, 2];

                    for (int i = 0; i < dsOut.Tables[0].Rows.Count; i++)
                    {
                        data[i, 0] = dsOut.Tables[0].Rows[i]["IDItem"].ToString();
                        data[i, 1] = dsOut.Tables[0].Rows[i]["Item"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }

            return result;
        }

        public static bool getTypeHarga(string IDItem, string IDRS, out string[,] data, out string strErr)
        {
            bool result = true;
            data = new string[1, 1];
            strErr = "";
            DataSet dsOut = new DataSet();

            try
            {
                string sqlCommand = @" declare @kota varchar(100)
		                                ,@prov varchar(100)

                                set @kota = (select b.Kota from DaftarRSIA a join Lokasi b on a.IDKota = b.ID where a.IDRS = '" + IDRS + @"')
                                set @prov = (select b.Provinsi from DaftarRSIA a join Lokasi b on a.IDKota = b.ID where a.IDRS = '" + IDRS + @"')
                                
                                if( @prov = 'DKI Jakarta' or @kota in ('Kota Tangerang','Tangerang Selatan','Bogor','Bekasi','Depok'))
	                                begin
		                                select TypeDes, HargaDKI as 'Harga' from DataHarga where IDItem = '" + IDItem + @"'
	                                end
                                else if @prov in ('Banten','Jawa Barat','Jawa Tengah','DI Yogyakarta','Jawa Timur')
	                                begin
		                                select TypeDes, HargaJawa as 'Harga' from DataHarga where IDItem = '" + IDItem + @"'
	                                end
                                else
	                                begin
		                                select TypeDes, HargaNJawa as 'Harga' from DataHarga where IDItem = '" + IDItem + @"'
	                                end

                                ";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }

                if (result)
                {
                    data = new string[dsOut.Tables[0].Rows.Count, 2];
                    for (int i = 0; i < dsOut.Tables[0].Rows.Count; i++)
                    {
                        data[i, 0] = dsOut.Tables[0].Rows[i]["TypeDes"].ToString();
                        data[i, 1] = dsOut.Tables[0].Rows[i]["Harga"].ToString() + "#";
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }
            return result;
        }

        public static bool saveDataRABRow(string IDRS, string ID, string Round, string Periode, string Kategori, string Item,
                                          string Deskripsi, string Budget, string Payout, string Quarel, out string strErr, out BackRABRowDataVM model)
        {
            bool result = true;
            strErr = ""; model = new BackRABRowDataVM();
            DataSet dsOut = new DataSet(); string sqlCommand = "";

            try
            {
                sqlCommand = @" declare @kategori varchar(50), @item varchar(225)
                                set @kategori = (select distinct Kategori from DataHarga where IDKategori = '" + Kategori + @"')
                                set @item = (select distinct Item from DataHarga where IDItem = '" + Item + @"')
                                update RABRS" + IDRS + @" set KomponenPerbaikan = @kategori, ItemPerbaikan = @item, DeskripsiPerbaikan = '" + Deskripsi + @"', 
                                HargaDasar = '" + Budget + @"', HargaInput = '" + Payout + @"', SelisihHarga = '" + Quarel + @"' where ID = '" + ID + @"'
                                update RABRS" + IDRS + @" set Status = 'SaveR' where ID = '" + ID + @"'
                                select * from RABRS" + IDRS + @" where ID = '" + ID + "'";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }

                if (result)
                {
                    if (dsOut != null & dsOut.Tables.Count > 0 & dsOut.Tables[0].Rows.Count > 0)
                    {
                        model.ID = int.Parse(dsOut.Tables[0].Rows[0]["ID"].ToString());
                        model.Round = dsOut.Tables[0].Rows[0]["Round"].ToString();
                        model.SubArea = dsOut.Tables[0].Rows[0]["SubArea"].ToString();
                        model.Item = dsOut.Tables[0].Rows[0]["Item"].ToString();
                        model.Penilaian = dsOut.Tables[0].Rows[0]["Komponen"].ToString();
                        model.Nilai = dsOut.Tables[0].Rows[0]["Nilai"].ToString();
                        model.Keterangan = dsOut.Tables[0].Rows[0]["Ket"].ToString();
                        model.Periode = dsOut.Tables[0].Rows[0]["Periode"].ToString();
                        model.PictureBf = dsOut.Tables[0].Rows[0]["JPG"].ToString();
                        model.KomponenPerbaiki = dsOut.Tables[0].Rows[0]["KomponenPerbaikan"].ToString();
                        model.ItemPerbaiki = dsOut.Tables[0].Rows[0]["ItemPerbaikan"].ToString();
                        model.DeskripsiPerbaiki = dsOut.Tables[0].Rows[0]["DeskripsiPerbaikan"].ToString();
                        model.HargaDasar = dsOut.Tables[0].Rows[0]["HargaDasar"].ToString();
                        model.HargaInput = dsOut.Tables[0].Rows[0]["HargaInput"].ToString();
                        model.SelisihHarga = dsOut.Tables[0].Rows[0]["SelisihHarga"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }

            return result;
        }

        public static bool saveDataSubmitRAB(string IDRS, string Round, string Periode, out string strErr)
        {
            bool result = true;
            strErr = "";
            string sqlCommand = "";
            try
            {
                sqlCommand = @"
                                update RABRS" + IDRS + @" set Status = 'Insert' where Status = 'SaveR' and Periode = '" + Periode + @"' and Round = '" + Round + @"'
                                declare @row int
                                set @row = (select Count(IDRS) from PenilaianRAB where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"')
                                if @row > 0
	                                begin
		                                update PenilaianRAB set DateSubmit = '" + DateTime.Now.ToString() + @"', StatusData = 'Insert',
		                                IDRegVerify1 = null, Verifikasi1Date = null, Status1 = null, KetVerifikator1 = null,
		                                IDRegVerify2 = null, Verifikasi2Date = null, Status2 = null, KetVerifikator2 = null 
		                                where IDRS = '" + IDRS + @"' and Periode = '" + Periode + @"' and IDRound = '" + Round + @"'
	                                end
                                else
	                                begin
		                                insert into PenilaianRAB(IDRS,IDRound,DateSubmit,StatusData,Periode)
		                                values('" + IDRS + @"','" + Round + @"','" + DateTime.Now.ToString() + @"','Insert','" + Periode + @"')
	                                end
                              ";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out strErr);
                }

            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }
            return result;
        }

        public static bool getExtReportRAB(string IDRS, string Round, string Periode, out string strErr, out ExtReportRABVM model)
        {
            bool result = true;
            model = new ExtReportRABVM(); strErr = "";
            string sqlCommand = ""; DataSet dsOut = new DataSet();
            try
            {
                sqlCommand = "select * from RABRS" + IDRS + " where Round = '" + Round + "' and Periode = '" + Periode + "'" +
                             @" declare @region varchar(50)
                                set @region = (select Regional from DaftarRSIA where IDRS = '" + IDRS + @"')
                                select Nama, KodeAkses from MUser
                                where IDRS = '" + IDRS + @"' or KodeAkses in ('Validator 1', 'Validator 3')
                                or KetReg = @region 
                                select NamaRS from DaftarRSIA where IDRS = '" + IDRS + "' " +
                                " select * from ApprovalRAB where IDRS = '" + IDRS + "' and IDRound = '" + Round + "' and Periode = '" + Periode + "' " +
                                " select * from PenilaianRAB where IDRS = '" + IDRS + "' and IDRound = '" + Round + "' and Periode = '" + Periode + "'  "+
                                "select * from RABRS" + IDRS + @" where Round = '" + Round + @"' and Periode = '" + Periode + "'";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }

                if (result)
                {
                    List<DataReportRABVM> ListData = new List<DataReportRABVM>();
                    List<DataImageRAB> ImageRAB = new List<DataImageRAB>();
                    DataReportRABVM data = new DataReportRABVM();
                    DataImageRAB Image = new DataImageRAB();
                    foreach (DataRow item in dsOut.Tables[0].Rows)
                    {
                        data = new DataReportRABVM();
                        data.ID = int.Parse(item["ID"].ToString());
                        data.Round = item["Round"].ToString();
                        data.SubArea = item["SubArea"].ToString();
                        data.Item = item["Item"].ToString();
                        data.Penilaian = item["Komponen"].ToString();
                        data.Nilai = item["Nilai"].ToString();
                        data.IDUnix = item["IDUnix"].ToString();
                        data.Keterangan = item["Ket"].ToString();
                        data.Periode = item["Periode"].ToString();
                        data.PictureBf = item["JPG"].ToString();
                        data.PictureAf = item["JPGAfter"].ToString();
                        data.KomponenPerbaiki = item["KomponenPerbaikan"].ToString();
                        data.ItemPerbaiki = item["ItemPerbaikan"].ToString();
                        data.DeskripsiPerbaiki = item["DeskripsiPerbaikan"].ToString();
                        data.HargaDasar = item["HargaDasar"].ToString();
                        data.HargaInput = item["HargaInput"].ToString();
                        data.SelisihHarga = item["SelisihHarga"].ToString();
                        ListData.Add(data);
                    }
                    model.listData = ListData;
                    foreach (DataRow dt in dsOut.Tables[1].Rows)
                    {
                        if (dt["KodeAkses"].ToString() == "Checker")
                        {
                            model.Checker = dt["Nama"].ToString();
                        }
                        else if (dt["KodeAkses"].ToString() == "Verifikator 1")
                        {
                            model.WakilDir = dt["Nama"].ToString();
                        }
                        else if (dt["KodeAkses"].ToString() == "Verifikator 2")
                        {
                            model.Direktur = dt["Nama"].ToString();
                        }
                        else if (dt["KodeAkses"].ToString() == "Validator 1")
                        {
                            model.Validator1 = dt["Nama"].ToString();
                        }
                        else if (dt["KodeAkses"].ToString() == "Validator 2")
                        {
                            model.Validator2 = dt["Nama"].ToString();
                        }
                        else if (dt["KodeAkses"].ToString() == "Validator 3")
                        {
                            model.Validator3 = dt["Nama"].ToString();
                        }
                    }
                    model.NamaRS = dsOut.Tables[2].Rows[0]["NamaRS"].ToString();
                    model.IDRS = IDRS;
                    if (dsOut.Tables[3].Rows.Count > 0)
                    {
                        model.Approve1 = dsOut.Tables[3].Rows[0]["Status1"].ToString();
                        model.Approve2 = dsOut.Tables[3].Rows[0]["Status2"].ToString();
                        model.Approve3 = dsOut.Tables[3].Rows[0]["Status3"].ToString();
                    }
                    model.StatusVer1 = dsOut.Tables[4].Rows[0]["Status1"].ToString();
                    model.StatusVer2 = dsOut.Tables[4].Rows[0]["Status2"].ToString();
                    foreach (DataRow img in dsOut.Tables[5].Rows)
                    {
                        Image = new DataImageRAB();
                        Image.RoundIMG = img["Round"].ToString();
                        Image.SubAreaIMG = img["SubArea"].ToString();
                        Image.ItemIMG = img["Item"].ToString();
                        Image.PenilaianIMG = img["Komponen"].ToString();
                        Image.KeteranganIMG = img["Ket"].ToString();
                        Image.BeforeIMG = img["JPG"].ToString();
                        Image.AfterIMG = img["JPGAfter"].ToString();
                        ImageRAB.Add(Image);
                    }
                    model.listDataImage = ImageRAB;
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }

            return result;
        }

        public static bool saveVerifyRAB(string IDRS, string Jabatan, string IDReg, string Comen, string Round, string Periode, string Acction, out string strErr)
        {
            bool result = true; strErr = "";
            string sqlCommand = "";
            DataSet ds = new DataSet();

            try
            {
                sqlCommand = @"if @Acction = 'Verify'
	                            begin
		                            if @Jabatan = 'Verifikator 1'
			                            begin
				                            update PenilaianRAB set IDRegVerify1 = @IDRegister, Verifikasi1Date = @DateAcct, Status1 = 'Disetujui', KetVerifikator1 = @Comen,
				                            IDRegVerify2 = '', Verifikasi2Date = '', Status2 = '', KetVerifikator2 = ''
				                            where IDRS = @IDRS and IDRound = @Round and Periode = @Periode
			                            end
		                            else
			                            begin
				                            update PenilaianRAB set IDRegVerify2 = @IDRegister, Verifikasi2Date = @DateAcct, Status2 = 'Disetujui', StatusData = 'Disetujui'
				                            where IDRS = @IDRS and IDRound = @Round and Periode = @Periode
				                            update RABRS" + IDRS + @" set Status = 'Disetujui' where Round = @Round and Periode = @Periode
				                            insert into ApprovalRAB(IDRS,IDRound,DateVerify,StatusData,Periode)
				                            values(@IDRS, @Round, @DateAcct, 'Disetujui', @Periode)
			                            end
	                            end
                            else
	                            begin
		                            if @Jabatan = 'Verifikator 1'
			                            begin
				                            update PenilaianRAB set IDRegVerify1 = 'IDRegister', Verifikasi1Date = 'DateTime.now', Status1 = 'Ditolak', StatusData = 'Ditolak',
				                            KetVerifikator1 = @Comen
				                            where IDRS = @IDRS and IDRound = @Round and Periode = @Periode
				                            update RABRS" + IDRS + @" set Status = 'Verify', KomponenPerbaikan = null, ItemPerbaikan = null, DeskripsiPerbaikan = null,
				                            HargaDasar = null, HargaInput = null, SelisihHarga = null 
				                            where Round = @Round and Periode = @Periode
			                            end
		                            else
			                            begin
				                            update PenilaianRAB set IDRegVerify2 = 'IDRegister', Verifikasi2Date = 'DateTime.now', Status2 = 'Ditolak', KetVerifikator2 = @Comen
				                            where IDRS = @IDRS and IDRound = @Round and Periode = @Periode
			                            end
	                            end

                                ";

                SqlParameter[] dbParam = new SqlParameter[8];
                dbParam[0] = new SqlParameter("@Acction", Acction);
                dbParam[1] = new SqlParameter("@Jabatan", Jabatan);
                dbParam[2] = new SqlParameter("@IDRegister", IDReg);
                dbParam[3] = new SqlParameter("@DateAcct", DateTime.Now.ToString());
                dbParam[4] = new SqlParameter("@Comen", Comen);
                dbParam[5] = new SqlParameter("@IDRS", IDRS);
                dbParam[6] = new SqlParameter("@Round", Round);
                dbParam[7] = new SqlParameter("@Periode", Periode);

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, ref dbParam, out ds, out strErr);
                }

            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }

            return result;
        }

        public static bool saveApprovalRAB(string Periode, string IDRS, string Round, string Jabatan, string IDReg, out string strErr)
        {
            bool result = true;
            strErr = "";
            string sqlCommand = "";

            try
            {
                if (Jabatan == "Validator 1")
                {
                    sqlCommand = @"update ApprovalRAB set IDRegApprov1 = '" + IDReg + @"', Approval1Date = '" + DateTime.Now.ToString() + @"', Status1 = 'Diketahui' 
                                   where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'";
                }
                else if (Jabatan == "Validator 2")
                {
                    sqlCommand = @"update ApprovalRAB set IDRegApprov2 = '" + IDReg + @"', Approval2Date = '" + DateTime.Now.ToString() + @"', Status2 = 'Diketahui' 
                                   where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'";
                }
                else
                {
                    sqlCommand = @"update ApprovalRAB set IDRegApprov3 = '" + IDReg + @"', Approval3Date = '" + DateTime.Now.ToString() + @"', Status3 = 'Diketahui', StatusData = 'Diketahui' 
                                   where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'  
                                   update RABRS" + IDRS + @" set Status = 'Diketahui' where Round = '" + Round + @"' and Periode = '" + Periode + @"'  ";
                }

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out strErr);
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }

            return result;
        }
        #endregion

        //Pemakaian di tangguhkan
        private static string getIDRoundArea(string ID)
        {
            string IDRoundArea = "";

            return IDRoundArea;
        }

    }
}