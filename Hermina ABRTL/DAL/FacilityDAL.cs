using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hermina_ABRTL.clsLogict;
using Hermina_ABRTL.Model;
using Hermina_ABRTL.ViewModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Hermina_ABRTL.DAL
{
    public class FacilityDAL
    {
        public static string Connection = ConfigurationManager.ConnectionStrings["BuildingContext"].ToString();
        public static FacilityVM GetDataMaster(out string strErr) {
            FacilityVM result = new FacilityVM();
            DataTableVM data = new DataTableVM();
            List<DataTableVM> roundarealist = new List<DataTableVM>();
            List<DataTableVM> arealist = new List<DataTableVM>();
            List<DataTableVM> subarealisttypennull = new List<DataTableVM>();
            List<DataTableVM> subarealisttypenull = new List<DataTableVM>();
            List<DataTableVM> typelistnnull = new List<DataTableVM>();
            List<DataTableVM> optionlistnull = new List<DataTableVM>();
            List<DataTableVM> optionlistnnull = new List<DataTableVM>();
            strErr = "";
            string strCommand = ""; bool res = false; DataSet dsOut = new DataSet();
            try
            {
                strCommand = @"select distinct RoundArea, IDRoundArea from DataDinamis order by IDRoundArea asc
                             select distinct Area, IDArea, IDRoundArea from DataDinamis order by IDArea asc
                             select distinct SubArea, IDSubArea, IDArea from DataDinamis where IDType != '' order by IDArea asc
                             select distinct SubArea, IDSubArea, IDArea from DataDinamis where IDType = '' and IDArea != 'A037' order by IDArea asc
                             select distinct IDSubArea, Type, IDType from DataDinamis where IDType != '' order by IDType asc
                             select distinct IDType, IDOption, OptionArea from DataDinamis where IDOption = '' order by IDOption asc
                             select distinct IDType, IDOption, OptionArea from DataDinamis where IDOption != '' order by IDOption asc";
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    res = db.QueryCommand(Connection, strCommand, out dsOut, out strErr);
                }
                if (res & dsOut != null)
                {
                    foreach (DataRow item in dsOut.Tables[0].Rows)
                    {
                        data = new DataTableVM();
                        data.IDRoundArea = item["IDRoundArea"].ToString();
                        data.RoundArea = item["RoundArea"].ToString();
                        roundarealist.Add(data);
                    }
                    foreach (DataRow item in dsOut.Tables[1].Rows)
                    {
                        data = new DataTableVM();
                        data.IDRoundArea = item["IDRoundArea"].ToString();
                        data.IDArea = item["IDArea"].ToString();
                        data.Area = item["Area"].ToString();
                        arealist.Add(data);
                    }
                    foreach (DataRow item in dsOut.Tables[2].Rows)
                    {
                        data = new DataTableVM();
                        data.IDArea = item["IDArea"].ToString();
                        data.IDSubArea = item["IDSubArea"].ToString();
                        data.SubArea = item["SubArea"].ToString();
                        subarealisttypennull.Add(data);
                    }
                    foreach (DataRow item in dsOut.Tables[3].Rows)
                    {
                        data = new DataTableVM();
                        data.IDArea = item["IDArea"].ToString();
                        data.IDSubArea = item["IDSubArea"].ToString();
                        data.SubArea = item["SubArea"].ToString();
                        subarealisttypenull.Add(data);
                    }
                    foreach (DataRow item in dsOut.Tables[4].Rows)
                    {
                        data = new DataTableVM();
                        data.IDSubArea = item["IDSubArea"].ToString();
                        data.IDType = item["IDType"].ToString();
                        data.Type = item["Type"].ToString();
                        typelistnnull.Add(data);
                    }
                    foreach (DataRow item in dsOut.Tables[5].Rows)
                    {
                        data = new DataTableVM();                        
                        data.IDType = item["IDType"].ToString();
                        data.IDOption = item["IDOption"].ToString();
                        data.OptionArea = item["OptionArea"].ToString();
                        optionlistnull.Add(data);
                    }
                    foreach (DataRow item in dsOut.Tables[6].Rows)
                    {
                        data = new DataTableVM();
                        data.IDType = item["IDType"].ToString();
                        data.IDOption = item["IDOption"].ToString();
                        data.OptionArea = item["OptionArea"].ToString();
                        optionlistnnull.Add(data);
                    }
                    result.DataRoundArea = roundarealist;
                    result.DataArea = arealist;
                    result.DataSubAreaTypeNNull = subarealisttypennull;
                    result.DataSubAreaTypeNull = subarealisttypenull;
                    result.DataTypeNNull = typelistnnull;
                    result.DataOptionNull = optionlistnull;
                    result.DataOptionNNull = optionlistnnull;
                }
                else {
                    strErr = "Error #101: " + strErr;
                }
            }
            catch (Exception ex)
            {
                strErr = "Error #100: " + ex.Message;
            }
            return result;
        }
    }
}