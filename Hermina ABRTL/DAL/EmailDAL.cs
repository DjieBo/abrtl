using Hermina_ABRTL.clsLogict;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Hermina_ABRTL.DAL
{
    public class EmailDAL
    {
        public static string Connection = ConfigurationManager.ConnectionStrings["BuildingContext"].ToString();
        public static bool SendingEmail(out string strErr, string from, string IDRS, string stat, string Round)
        {
            strErr = "";
            bool Result = false;
            try
            {
                string TimeEmail = DateTime.Now.ToString("MMMM");
                string body = "";
                string subject = "";
                string to = "";
                string NamaRS = "";
                string sqlCommand = "", errSQL; DataSet ds = new DataSet();
                if (from == "Checker")
                {
                    sqlCommand = "select Email from MUser where IDRS = '" + IDRS + "' and KodeAkses = 'Verifikator 1'";
                    subject = "Laporan " + Round + " " + " Bulan " + TimeEmail;
                    body = "<div style='width:80%;margin:30px auto;background:#f1f1f1;border:none;border-radius:10px;padding:15px;font-size:14pt'>" +
                           "<p>Kepada Wakil Direktur,</p>" +
                           "<p>Laporan Form Checker " + Round + " Telah <b>Selesai</b>.</p>" +
                           "<p><a href='http://202.157.184.198:6060'>Mohon untuk diperiksa</a></p>" +
                           "</div>" +
                           "<div style='color:red;line-height:10px'>" +
                           "<p>Ini adalah Email Otomatis System Hermina ABRTL-RL. </p>" +
                           "<p>Disarankan untuk tidak melakukan replay atau balas untuk email ini</p>" +
                           "</div>" +
                           "<p><u>Best Regards</u></p>" +
                           "<p style='line-height:0px'>System Admin Hermina ABRTL-RL</p>";
                }
                else if (from == "Verifikator 1")
                {
                    if (stat == "Verify")
                    {
                        sqlCommand = "select Email from MUser where IDRS = '" + IDRS + "' and KodeAkses = 'Verifikator 2'";
                        subject = "Laporan " + Round + " " + " Bulan " + TimeEmail;
                        body = "<div style='width:80%;margin:30px auto;background:#f1f1f1;border:none;border-radius:10px;padding:15px;font-size:14pt'>" +
                               "<p>Direktur " + NamaRS + " ,</p>" + //Tambahakan NamaRS di  (............)
                                                      "<p>Wakil Direktur " + NamaRS + " telah <b>menyetujui</b>.</p>" + //Tambahakan NamaRS di  (............)
                                                      "<p><a href='http://202.157.184.198:6060'>Mohon untuk diperiksa</a></p>" +
                                                      "</div>" +
                                                      "<div style='color:red;line-height:10px'>" +
                                                      "<p>Ini adalah Email Otomatis System Hermina ABRTL-RL. </p>" +
                                                      "<p>Disarankan untuk tidak melakukan replay atau balas untuk email ini</p>" +
                                                      "</div>" +
                                                      "<p><u>Best Regards</u></p>" +
                                                      "<p style='line-height:0px'>System Admin Hermina ABRTL-RL</p>";
                    }
                    else
                    {
                        sqlCommand = "select Email from MUser where IDRS = '" + IDRS + "' and KodeAkses = 'Checker'";
                        subject = "Laporan " + Round + " " + " Bulan " + TimeEmail + " Ditolak";
                        body = "<div style='width:80%;margin:30px auto;background:#f1f1f1;border:none;border-radius:10px;padding:15px;font-size:14pt'>" +
                                                      "<p>Kepada Checker " + NamaRS + ",</p>" + //Tambahakan NamaRS di  (............)
                                                                                                //"<p>" + KetNilai1 + "</p>" + //Tambahakan Keterangan Reject di  (............)
                                                      "<p><a href='http://202.157.184.198:6060'>Mohon untuk diperiksa.</a></p>" +
                                                      "</div>" +
                                                      "<div style='color:red;line-height:10px'>" +
                                                      "<p>Ini adalah Email Otomatis System Hermina ABRTL-RL. </p>" +
                                                      "<p>Disarankan untuk tidak melakukan replay atau balas untuk email ini</p>" +
                                                      "</div>" +
                                                      "<p><u>Best Regards</u></p>" +
                                                      "<p style='line-height:0px'>System Admin Hermina ABRTL-RL</p>";
                    }
                }
                else if (from == "Verifikator 2")
                {
                    if (stat == "Verify")
                    {
                        sqlCommand = "select Email from MUser where KodeAkses = 'Validator 1'";
                        subject = "Laporan " + Round + " " + " Bulan " + TimeEmail;
                        body = "<div style='width:80%;margin:30px auto;background:#f1f1f1;border:none;border-radius:10px;padding:15px;font-size:14pt'>" +
                               "<p>Kepala Departemen Penunjang Umum,</p>" +
                               "<p>Data penilaian " + Round + " telah di <b>Verifikasi</b>.</p>" +
                               "<p><a href='http://202.157.184.198:6060'>Mohon untuk disetujui.</a></p>" +
                               "</div>" +
                               "<div style='color:red;line-height:10px'>" +
                               "<p>Ini adalah Email Otomasis Sistem Hermina ABRTL-RL. </p>" +
                               "<p>Disarankan untuk tidak melakukan replay atau balas untuk email ini</p>" +
                               "</div>" +
                               "<p><u>Best Regards</u></p>" +
                               "<p style='line-height:0px'>System Admin Hermina ABRTL-RL</p>";
                    }
                    else
                    {
                        sqlCommand = "select Email from MUser where IDRS = '" + IDRS + "' and KodeAkses = 'Verifikator 1'";
                        subject = "Laporan " + Round + " " + " Bulan " + TimeEmail + " " + " Ditolak";
                        body = "<div style='width:80%;margin:30px auto;background:#f1f1f1;border:none;border-radius:10px;padding:15px;font-size:14pt'>" +
                                                      "<p> Kepada Wakil Direktur " + NamaRS + ",</p>" + //Tambahakan NamaRS di  (............)
                                                      "<p> Direktur " + NamaRS + " : Meriject data terakhir yg anda verify " + "</p>" + //Tambahakan Keterangan Reject di  (............)
                                                      "<p><a href='http://202.157.184.198:6060'>Mohon untuk diperiksa.</a></p>" +
                                                      "</div>" +
                                                      "<div style='color:red;line-height:10px'>" +
                                                      "<p>Ini adalah Email Otomasis Sistem Hermina ABRTL-RL. </p>" +
                                                      "<p>Disarankan untuk tidak melakukan replay atau balas untuk email ini</p>" +
                                                      "</div>" +
                                                      "<p><u>Best Regards</u></p>" +
                                                      "<p style='line-height:0px'>System Admin Hermina ABRTL-RL</p>";
                    }
                }

                //get alamat email tujuan
                sqlCommand += Environment.NewLine + "select NamaRS from DaftarRSIA where IDRS = '"+ IDRS +"' ";
                bool res = false;
                using (clsDBSQLConnection db = new clsDBSQLConnection())
                {
                    res = db.QueryCommand(Connection, sqlCommand, out ds, out errSQL);
                }
                if (res)
                {
                    if (ds != null & ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            to = ds.Tables[0].Rows[0]["Email"].ToString().Trim();
                            NamaRS = ds.Tables[1].Rows[0]["NamaRS"].ToString().Trim();
                        }
                        else
                        {
                            strErr = "alamat email tujuan tidak ditemukan";
                            Result = false;
                            return Result;
                        }
                    }
                }
                else
                {
                    strErr = errSQL;
                    Result = false;
                    return Result;
                }


                MailMessage mm = new MailMessage("adm.abrtl@gmail.com", to);
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                
                smtp.UseDefaultCredentials = false;
                NetworkCredential nc = new NetworkCredential("adm.abrtl@gmail.com", "mynameis(kribo)");
                smtp.EnableSsl = true;
                smtp.Credentials = nc;
                smtp.Send(mm);

                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                strErr = ex.Message;
            }
            return Result;
        }


    }
}