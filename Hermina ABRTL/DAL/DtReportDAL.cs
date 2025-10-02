using Hermina_ABRTL.clsLogict;
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
    public class DtReportDAL
    {
        public static string Connection = ConfigurationManager.ConnectionStrings["BuildingContext"].ToString();

        public static bool getDataExcRepVal(out ValidatorVM model, out string strErr)
        {
            bool result = false;
            model = new ValidatorVM(); strErr = "";

            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = @"select Regional, IDRS, NamaRS from DaftarRSIA " +
                              " select distinct Periode from RValueVerifikator where Status2 = 'Verify' " +
                              " select Periode, IDRound, IDRS from RValueVerifikator where Status2 = 'Verify' " +
                              " select distinct Periode from PenilaianRAB where Status2 = 'Disetujui' " +
                              " select Periode, IDRS, IDRound from PenilaianRAB where Status2 = 'Disetujui' " +
                              " select distinct Periode from ApprovalRAB where Status3 = 'Diketahui' " +
                              " select * from ApprovalRAB where Status3 = 'Diketahui'";

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
                            List<RegionVm> tmpListReg = new List<RegionVm>();
                            RegionVm tmpReg = new RegionVm();
                            for (int i = 1; i <= 5; i++)
                            {
                                tmpReg = new RegionVm();
                                tmpReg.IDRegion = "Regional_" + i;
                                tmpReg.Region = "Regional " + i;
                                tmpListReg.Add(tmpReg);
                            }
                            model.ListRegion = tmpListReg;

                            List<BiodataRSValVM> tmpListBioRS = new List<BiodataRSValVM>();
                            BiodataRSValVM tmpBioRS = new BiodataRSValVM();
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                tmpBioRS = new BiodataRSValVM();
                                tmpBioRS.IDRS = item["IDRS"].ToString();
                                tmpBioRS.Region = item["Regional"].ToString();
                                tmpBioRS.NamaRS = item["NamaRS"].ToString();
                                tmpListBioRS.Add(tmpBioRS);
                            }
                            model.ListRS = tmpListBioRS;
                        }
                        List<validatePeriodeEX> ValPeriode = new List<validatePeriodeEX>();
                        validatePeriodeEX VP = new validatePeriodeEX();
                        foreach (DataRow vv in ds.Tables[1].Rows) {
                            VP = new validatePeriodeEX();
                            VP.Periode = vv["Periode"].ToString();
                            ValPeriode.Add(VP);
                        }
                        model.ListPeriode = ValPeriode;

                        List<ValidasiRound> Pe = new List<ValidasiRound>();
                        ValidasiRound Perio = new ValidasiRound();
                        foreach (DataRow a in ds.Tables[2].Rows) {
                            Perio = new ValidasiRound();
                            Perio.Periode = a["Periode"].ToString();
                            Perio.Round = a["IDRound"].ToString();
                            Perio.IDRS = a["IDRS"].ToString();
                            Pe.Add(Perio);
                        }
                        model.ValidasiVal = Pe;

                        List<validatePeriodeRAB> PeriodeRAB = new List<validatePeriodeRAB>();
                        validatePeriodeRAB PR = new validatePeriodeRAB();
                        foreach (DataRow dd in ds.Tables[3].Rows)
                        {
                            PR = new validatePeriodeRAB();
                            PR.Periode = dd["Periode"].ToString();
                            PeriodeRAB.Add(PR);
                        }
                        model.ListPeriodeRAB = PeriodeRAB;

                        List<ValidateCondition> RABCon = new List<ValidateCondition>();
                        ValidateCondition RABList = new ValidateCondition();
                        foreach (DataRow b in ds.Tables[4].Rows) {
                            RABList = new ValidateCondition();
                            RABList.PeriodeRAB = b["Periode"].ToString();
                            RABList.IDRSRAB = b["IDRS"].ToString();
                            RABList.RoundRAB = b["IDRound"].ToString();
                            RABCon.Add(RABList);
                        }
                        model.ValidasiRAB = RABCon;

                        List<validatePeriodeSPK> PeriodeSPK = new List<validatePeriodeSPK>();
                        validatePeriodeSPK SPR = new validatePeriodeSPK();
                        foreach (DataRow dd in ds.Tables[5].Rows)
                        {
                            SPR = new validatePeriodeSPK();
                            SPR.Periode = dd["Periode"].ToString();
                            PeriodeSPK.Add(SPR);
                        }
                        model.ListPeriodeSPK = PeriodeSPK;

                        List<SPKCondition> SPKCon = new List<SPKCondition>();
                        SPKCondition data = new SPKCondition();
                        foreach (DataRow item in ds.Tables[6].Rows) {
                            data = new SPKCondition();
                            data.IDRS = item["IDRS"].ToString();
                            data.Round = item["IDRound"].ToString();
                            data.Periode = item["Periode"].ToString();
                            data.Status = item["Status3"].ToString();
                            SPKCon.Add(data);
                        }
                        model.listSPK = SPKCon;

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
        public static bool getDataSPK(string IDRS, string Round, string Periode, out ValidatorVM model, out string Time, out string Err)
        {
            bool result = false;
            model = new ValidatorVM(); Err = ""; Time = "";
            string sqlCommand = ""; DataSet ds = new DataSet();
            sqlCommand = "select * from RABRS"+IDRS+" where Round = '"+ Round +"' and Periode = '"+ Periode +"'  "+
                         "select * from ApprovalRAB where IDRS = '"+ IDRS + "' and IDRound = '"+ Round +"' and Periode = '"+ Periode + "' and Status3 = 'Diketahui'";

            using (clsDBSQLConnection db = new clsDBSQLConnection()) {
                result = db.QueryCommand(Connection, sqlCommand, out ds, out Err);
            }

            try
            {
                if (result) {
                    List<ReportSPK> dataSPK = new List<ReportSPK>();
                    ReportSPK data = new ReportSPK();
                    foreach (DataRow a in ds.Tables[0].Rows) {
                        data = new ReportSPK();
                        data.SubArea = a["SubArea"].ToString();
                        data.Item = a["Item"].ToString();
                        data.Penilaian = a["Komponen"].ToString();
                        data.Nilai = a["Nilai"].ToString();
                        data.Status = a["Status"].ToString();
                        data.Keterangan = a["Ket"].ToString();
                        data.KomponenPerbaiki = a["KomponenPerbaikan"].ToString();
                        data.ItemPerbaiki = a["ItemPerbaikan"].ToString();
                        data.DeskripsiPerbaiki = a["DeskripsiPerbaikan"].ToString();
                        data.PictureBf = a["JPG"].ToString();
                        data.PictureAf = a["JPGAfter"].ToString();
                        data.HargaDasar = a["HargaDasar"].ToString();
                        data.HargaInput = a["HargaInput"].ToString();
                        data.SelisihHarga = a["SelisihHarga"].ToString();
                        dataSPK.Add(data);
                    }
                    model.listReportSPK = dataSPK;
                    model.TimeStart = ds.Tables[1].Rows[0]["Approval3Date"].ToString();
                    Time = ds.Tables[1].Rows[0]["Approval3Date"].ToString();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Err = ex.Message;
            }
            return result;
        }
        public static bool getDataLogVal(string IDRS, string Round, string Periode, string Jabatan, string IDReg, out ExcReportVM model, out string strerr)
        {
            model = new ExcReportVM();
            strerr = "";
            bool result = false;

            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = @"
                                declare @regLogin varchar(50),
		                                @regRS varchar(50),
		                                @jabatan varchar(50),
		                                @strData varchar(max)

                                set @jabatan = '" + Jabatan + @"'
                                if @jabatan = 'Validator 2'
                                 begin
                                  set @regLogin = (select KetReg from Muser where IDRegister = '" + IDReg + @"')
                                  set @regRS = (select Regional from DaftarRSIA where IDRS = '" + IDRS + @"')

                                  if @regLogin != @regRS
                                   begin
                                    set @strData = 'Bukan region'
	                                goto ERROR
                                   end
                                 end

                                set @strData = (select Data from DataLog where IDRS = '" + IDRS + @"' and Round = '" + Round + @"' and Periode = '" + Periode + @"' )
                                if @strData is null
                                 begin
                                  set @strData = 'Data belum ada'
	                                goto ERROR
                                 end
                                select Status1, Status2, Status3 from RValueApproval where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'
                                select * from RABRS" + IDRS + @" where Round = '" + Round + @"' and Periode = '" + Periode + @"' 

                                ERROR:
                                select @strData as Data

                                ";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strerr);
                }
                if (result)
                {
                    if (ds != null & ds.Tables.Count > 0)
                    {
                        if (ds.Tables.Count == 1)
                        {
                            if (ds.Tables[0].Rows[0]["Data"].ToString() == "Bukan region")
                            {
                                result = false;
                                strerr = "Is Not Your Region";
                            }
                            else if (ds.Tables[0].Rows[0]["Data"].ToString() == "Data belum ada")
                            {
                                result = false;
                                strerr = "Belum ada Data Verify";
                            }
                        }
                        else if (ds.Tables.Count > 1)
                        {
                            DataSet data = new DataSet();
                            System.IO.StringReader XML = new System.IO.StringReader(ds.Tables[2].Rows[0]["Data"].ToString());
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
                            //string periode = data.Tables["Table5"].Rows[0]["Periode"].ToString().Trim().Substring(4, 2);
                            model.Bulan = RSDAL.convertPeriode(Periode.Substring(4, 2)) + " " + Periode.Substring(0, 4);
                            model.IDRS = IDRS;
                            model.Round = Round;
                            model.Periode = Periode;

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

                            //STATUS
                            foreach (DataRow dtStatus in ds.Tables[0].Rows)
                            {
                                model.Status1 = dtStatus["Status1"].ToString();
                                model.Status2 = dtStatus["Status2"].ToString();
                                model.Status3 = dtStatus["Status3"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strerr = ex.Message;
                result = false;
            }

            return result;
        }
        public static bool saveApprovVal(string Acses, string IDReg, string Nilai, string IDRS, string Round, string Periode, out string strErr)
        {
            bool result = false;
            strErr = "";
            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = @"
                                declare @strAksesLogn varchar(25),
		                                @strStatus1 varchar(25),
		                                @strStatus2 varchar(25),
		                                @strStatus3 varchar(25)

                                set @strAksesLogn = '" + Acses + @"'
                                if @strAksesLogn = 'Validator 1'
                                 begin
                                  update RValueApproval set IDRegApprov1 = '" + IDReg + @"', Approval1Date = '" + DateTime.Now.ToString() + @"', Status1 = 'Approve' 
                                  where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'
                                 end
                                else if @strAksesLogn = 'Validator 2'
                                 begin
                                  update RValueApproval set IDRegApprov2 = '" + IDReg + @"', Approval2Date = '" + DateTime.Now.ToString() + @"', Status2 = 'Approve' 
                                  where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'
                                 end
                                else if @strAksesLogn = 'Validator 3'
                                 begin
                                  update RValueApproval set IDRegApprov3 = '" + IDReg + @"', Approval3Date = '" + DateTime.Now.ToString() + @"', Status3 = 'Approve' 
                                  where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'
                                 end

                                set @strStatus1 = (select Status1 from RValueApproval where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"')
                                set @strStatus2 = (select Status2 from RValueApproval where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"')
                                set @strStatus3 = (select Status3 from RValueApproval where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"')

                                if @strStatus1 = 'Approve' and @strStatus2 = 'Approve' and @strStatus3 = 'Approve'
                                 begin
	                                Update RValueApproval set StatusData = 'Approve' where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'
                                 end
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
        public static bool getDataAuto(out PeerGroupVM model, out string strErr)
        {
            bool result = false;
            model = new PeerGroupVM(); strErr = "";
            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = @"
                                 select * from RValueVerifikator
                                 select IDRS, NamaRS from DaftarRSIA
                                ";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }

                if (result)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            List<LogVerifyVM> tmpList = new List<LogVerifyVM>();
                            LogVerifyVM tmpData = new LogVerifyVM();
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                tmpData = new LogVerifyVM();
                                tmpData.IDRS = item["IDRS"].ToString().Trim();
                                tmpData.Round = item["IDRound"].ToString().Trim();
                                tmpData.DateSubmit = item["DateSubmit"].ToString().Trim();
                                tmpData.StatusData = item["StatusData"].ToString().Trim();
                                tmpData.Periode = item["Periode"].ToString().Trim();
                                tmpData.IDVerify1 = item["IDRegVerify1"].ToString().Trim();
                                tmpData.DateVerify1 = item["Verifikasi1Date"].ToString().Trim();
                                tmpData.KetVerify1 = item["KetVerifikator1"].ToString().Trim();
                                tmpData.Status1 = item["Status1"].ToString().Trim();
                                tmpData.IDVerify2 = item["IDRegVerify2"].ToString().Trim();
                                tmpData.DateVerify2 = item["Verifikasi2Date"].ToString().Trim();
                                tmpData.KetVerify2 = item["KetVerifikator2"].ToString().Trim();
                                tmpData.Status2 = item["Status2"].ToString().Trim();
                                tmpList.Add(tmpData);
                            }
                            model.DataVerifikasi = tmpList;
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            List<ListDataRS> tmpList = new List<ListDataRS>();
                            ListDataRS tmpData = new ListDataRS();
                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                tmpData = new ListDataRS();
                                tmpData.IDRS = item["IDRS"].ToString().Trim();
                                tmpData.NamaRS = item["NamaRS"].ToString().Trim();
                                tmpList.Add(tmpData);
                            }
                            model.DataRS = tmpList;
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
        public static bool getDataLogFinal(string IDRS, string Round, string Periode, out ExcReportVM model, out string strErr)
        {
            bool result = false;
            model = new ExcReportVM(); strErr = "";
            string sqlCommand = ""; DataSet ds = new DataSet();
            try
            {
                sqlCommand = @" 
                            select Status1, Status2, Status3 from RValueApproval where IDRS = '" + IDRS + @"' and IDRound = '" + Round + @"' and Periode = '" + Periode + @"'
                            select Data from DataLog where IDRS = '" + IDRS + @"' and Round = '" + Round + @"' and Periode = '" + Periode + @"'  "+
                            "select * from RABRS" + IDRS + @" where Round = '" + Round + @"' and Periode = '" + Periode + "'";

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    result = db.QueryCommand(Connection, sqlCommand, out ds, out strErr);
                }

                if (result)
                {
                    DataSet data = new DataSet();
                    System.IO.StringReader XML = new System.IO.StringReader(ds.Tables[1].Rows[0]["Data"].ToString());
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

                    //STATUS
                    foreach (DataRow dtStatus in ds.Tables[0].Rows)
                    {
                        model.Status1 = dtStatus["Status1"].ToString();
                        model.Status2 = dtStatus["Status2"].ToString();
                        model.Status3 = dtStatus["Status3"].ToString();
                    }
                    //Image
                    if (ds.Tables[2].Rows.Count > 0) {
                        List<ViewImageVM> tmpListImage = new List<ViewImageVM>();
                        ViewImageVM tmpImage = new ViewImageVM();
                        foreach (DataRow gh in ds.Tables[2].Rows)
                        {
                            tmpImage = new ViewImageVM();
                            tmpImage.Item = gh["Item"].ToString();
                            tmpImage.JPG = gh["JPG"].ToString();
                            tmpImage.JPGAfter = gh["JPGAfter"].ToString();
                            tmpImage.Komponen = gh["Komponen"].ToString();
                            tmpImage.SubArea = gh["SubArea"].ToString();
                            tmpImage.Keterangan = gh["Ket"].ToString();
                            tmpListImage.Add(tmpImage);
                        }
                        model.ListImage = tmpListImage;
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
                string masa = tahun + RSDAL.rollBackBulan(bulan);
                string Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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
        public static bool getDataDashboaard(string Periode, string IDRS, out DashboardVM model, out string strErr)
        {
            strErr = "";
            model = new DashboardVM();
            bool result = false;
            string sqlCommand = ""; DataSet dsOut = new DataSet(); bool blnResult = false;
            
            try
            {
                #region SQLC
                sqlCommand = @"
                                create table #tmpDataUtama 
                                (
	                                ID				int	IDENTITY(1, 1) primary key,
	                                Round			varchar(10),
	                                Status			varchar(5),
	                                jmlDataRound	int,
	                                jmlDataChenck	int,
	                                jmlNilaiA		int,
	                                jmlNilaiB		int,
	                                jmlNilaiC		int,
	                                jmlAman			int,
	                                jmlBersih		int,
	                                jmlBaru			int,
	                                jmlRapih		int,
	                                jmlRL			int,
	                                dataLog			varchar(max)
                                )

                                insert into #tmpDataUtama(Round) select distinct Round from RSIA" + IDRS + @" 

                                declare @intA int, @intJmlData int, @strStat varchar(5), @strData varchar(max)

                                set @intA = 1
                                set @intJmlData = (select COUNT(ID) from #tmpDataUtama)

                                while @intA <= @intJmlData
                                 begin
	                                update #tmpDataUtama set jmlDataRound = (Select count(Komponen) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
											                                where b.ID = @intA),
							                                jmlDataChenck = (Select count(Komponen) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
											                                where b.ID = @intA and a.Status in ('Checked','Reject','Submt'))
	                                where ID = @intA
	                                set @strData = (select Data from DataLog a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.IDRS = '" + IDRS + @"' and a.Periode = '" + Periode + @"')
	                                if @strData is null
	                                 begin
		                                set @strStat = 'Not'
		                                Update #tmpDataUtama set Status = @strStat, 
		                                jmlNilaiA = (select Count(Nilai) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.Nilai = 'A'),
		                                jmlNilaiB = (select Count(Nilai) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.Nilai = 'B'),
		                                jmlNilaiC = (select Count(Nilai) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.Nilai = 'C'),
		                                jmlAman = (select Count(ParamValue) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.ParamValue = 'Aman'),
		                                jmlBaru = (select Count(ParamValue) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.ParamValue = 'Tampak Baru'),
		                                jmlBersih = (select Count(ParamValue) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.ParamValue = 'Bersih'),
		                                jmlRapih = (select Count(ParamValue) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.ParamValue = 'Rapih'),
		                                jmlRL = (select Count(ParamValue) from RSIA" + IDRS + @" a join #tmpDataUtama b on a.Round = b.Round 
					                                where b.ID = @intA and a.ParamValue = 'Ramah Lingkungan')
		                                where ID = @intA
	                                 end
	                                 else
	                                  begin
	                                   set @strStat = 'Done'
	                                   Update #tmpDataUtama set Status = @strStat, dataLog = @strData where ID = @intA
	                                  end	
	                                set @intA = @intA + 1
                                 end

                                select * from #tmpDataUtama
                                select * from DataAkumulasi where IDRS = '" + IDRS + @"'
                                select * from DataSPK where IDRS = '" + IDRS + @"' and Periode = '"+ Periode +@"' 
                            ";
                #endregion

                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    blnResult = db.QueryCommand(Connection, sqlCommand, out dsOut, out strErr);
                }

                if (blnResult)
                {
                    if (dsOut != null)
                    {
                        List<decimal> listPersenDataRound = new List<decimal>();
                        
                        for (int i = 1; i <= 4; i++)
                        {
                            List<int> listDataNilai = new List<int>();
                            List<int> listDataparameter = new List<int>();
                            foreach (DataRow item in dsOut.Tables[0].Rows)
                            {
                                if (item["Round"].ToString() == ("Round " + i).ToString())
                                {
                                    decimal jmlData = decimal.Parse(item["jmlDataRound"].ToString());
                                    if (item["Status"].ToString() == "Not")
                                    {
                                        //Data Belum di Verify                                        
                                        decimal jmlChek = decimal.Parse(item["jmlDataChenck"].ToString());
                                        decimal persen = (jmlChek / jmlData) * 100;
                                        listPersenDataRound.Add(persen);
                                        int nilaiA = int.Parse(item["jmlNilaiA"].ToString());
                                        int nilaiB = int.Parse(item["jmlNilaiB"].ToString());
                                        int nilaiC = int.Parse(item["jmlNilaiC"].ToString());
                                        listDataNilai.Add(nilaiA);
                                        listDataNilai.Add(nilaiB);
                                        listDataNilai.Add(nilaiC);
                                        int param1 = int.Parse(item["jmlAman"].ToString());
                                        int param2 = int.Parse(item["jmlBersih"].ToString());
                                        int param3 = int.Parse(item["jmlBaru"].ToString());
                                        int param4 = int.Parse(item["jmlRapih"].ToString());
                                        int param5 = int.Parse(item["jmlRL"].ToString());
                                        listDataparameter.Add(param1);
                                        listDataparameter.Add(param2);
                                        listDataparameter.Add(param3);
                                        listDataparameter.Add(param4);
                                        listDataparameter.Add(param5);
                                    }
                                    else
                                    {
                                        //Data Sudah di Verify
                                        DataSet data = new DataSet();
                                        System.IO.StringReader XML = new System.IO.StringReader(item["dataLog"].ToString());
                                        data.ReadXml(XML);
                                        int nilaiA = 0, nilaiB = 0, nilaiC = 0, totalCheck = 0;
                                        foreach (DataRow dtXML in data.Tables["Table"].Rows)
                                        {
                                            string IDArea = dtXML["IDRoundArea"].ToString();
                                            foreach (DataRow dataArea in data.Tables["Table1"].Rows)
                                            {
                                                if (dataArea["IDRoundArea"].ToString() == IDArea)
                                                {
                                                    nilaiA += int.Parse(dataArea["NialiA"].ToString());
                                                    nilaiB += int.Parse(dataArea["NilaiB"].ToString());
                                                    nilaiC += int.Parse(dataArea["NilaiC"].ToString());
                                                }   
                                            }
                                        }
                                        totalCheck = nilaiA + nilaiB + nilaiC;
                                        decimal persen = (totalCheck / jmlData) * 100;
                                        listPersenDataRound.Add(persen);
                                        listDataNilai.Add(nilaiA);
                                        listDataNilai.Add(nilaiB);
                                        listDataNilai.Add(nilaiC);
                                        int Aman = 0, Rapih = 0, Baru = 0, RL = 0, Bersih = 0;
                                        foreach (DataRow param in data.Tables["Table6"].Rows)
                                        {
                                            if (param["Parameter"].ToString().Trim() == "Aman")
                                            {
                                                Aman = int.Parse(param["Yes"].ToString().Trim());
                                            }
                                            else if (param["Parameter"].ToString().Trim() == "Rapih")
                                            {
                                                Rapih = int.Parse(param["Yes"].ToString().Trim());
                                            }
                                            else if (param["Parameter"].ToString().Trim() == "Tampak baru")
                                            {
                                                Baru = int.Parse(param["Yes"].ToString().Trim());
                                            }
                                            else if (param["Parameter"].ToString().Trim() == "Ramah Lingkungan")
                                            {
                                                RL = int.Parse(param["Yes"].ToString().Trim());
                                            }
                                            else if (param["Parameter"].ToString().Trim() == "Bersih")
                                            {
                                                Bersih = int.Parse(param["Yes"].ToString().Trim());
                                            }
                                        }
                                        listDataparameter.Add(Aman);
                                        listDataparameter.Add(Bersih);
                                        listDataparameter.Add(Baru);
                                        listDataparameter.Add(Rapih);
                                        listDataparameter.Add(RL);
                                    }
                                }
                            }

                            if (i == 1)
                            {
                                model.DataNilaiR1 = listDataNilai.ToArray();
                                model.DataParamR1 = listDataparameter.ToArray();
                            }
                            else if (i == 2)
                            {
                                model.DataNilaiR2 = listDataNilai.ToArray();
                                model.DataParamR2 = listDataparameter.ToArray();
                            }
                            else if (i == 3)
                            {
                                model.DataNilaiR3 = listDataNilai.ToArray();
                                model.DataParamR3 = listDataparameter.ToArray();
                            }
                            else if (i == 4)
                            {
                                model.DataNilaiR4 = listDataNilai.ToArray();
                                model.DataParamR4 = listDataparameter.ToArray();
                            }
                        }
                        model.PersenDataRound = listPersenDataRound.ToArray();
                        //if (dsOut.Tables[1].Rows.Count > 0)
                        //{
                        List<DataLineVM> tmpListline = new List<DataLineVM>();
                        DataLineVM tmpData = new DataLineVM();
                        if (dsOut.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dtLine in dsOut.Tables[1].Rows)
                            {
                                tmpData = new DataLineVM();
                                tmpData.Akumulasi = decimal.Parse(dtLine["Akumulasi"].ToString()/*.Replace(".", ",")*/);
                                string bulan = dtLine["Periode"].ToString().Substring(4, 2);
                                tmpData.Periode = RSDAL.convertPeriode(bulan) + " " + dtLine["Periode"].ToString().Substring(0, 4);
                                tmpListline.Add(tmpData);
                            }
                        }
                        else
                        {
                            tmpData = new DataLineVM();
                            tmpData.Akumulasi = 0;
                            tmpData.Periode = RSDAL.convertPeriode(Periode.Substring(4, 2)) + " " + Periode.Substring(0, 4);
                            tmpListline.Add(tmpData);
                        }
                            
                        model.dtLine = tmpListline;
                        //}
                        List<DataPieSPK> dSPK = new List<DataPieSPK>();
                        DataPieSPK pie = new DataPieSPK();
                        foreach (DataRow ada in dsOut.Tables[2].Rows) {
                            pie = new DataPieSPK();
                            pie.IDRS = ada["IDRS"].ToString();
                            pie.Periode = ada["Periode"].ToString();
                            pie.Round = ada["Round"].ToString();
                            pie.sumSPK = ada["sumSPK"].ToString();
                            pie.doneSPK = ada["doneSPK"].ToString();
                            pie.NTdoneSPK = (Decimal.Parse(pie.sumSPK) - Decimal.Parse(pie.doneSPK)).ToString();
                            dSPK.Add(pie);
                        }
                        model.dtPieSPK = dSPK;

                        result = true;
                    }
                    else
                    {
                        strErr = "Data tidak di temukan";
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                result = false;
            }
            return result;
        }

    }
}