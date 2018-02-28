using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using System.Net.Mime;
using NPOI.XSSF.UserModel;
using System.Reflection;
using System.Globalization;
using NPOI.HSSF.UserModel;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Collections;
using System.Reflection;

namespace Coach.Services{
    public class CheckService
    {
        //判断用户名
        public bool checkUser(string userName) {
            if (userName == "Admin")
            {
                return true;
            }
            return false;
        }
        //校验密码
        public bool checkPwd(string pwd) {
            if (pwd == "111")
            {
                return true;
            }
            return false;
        }
        //校验用户名和密码
        public bool checkAll(string userName, string pwd) {
            if (checkUser(userName) && checkPwd(pwd))
            {
                return true;
            }
            return false;
        }
        //密码加密
        public string encryptPwd(string pwd) {
            try
            {
                pwd = pwd + "2017";
                byte[] pwds = Encoding.UTF8.GetBytes(pwd);
                //byte[] pwds = Encoding.Default.GetBytes(pwd);
                pwd = Convert.ToBase64String(pwds);
                return pwd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        //密码解密
        public string decryptPwd(string pwd) {
            string pwds = "";
            try
            {
                byte[] bytes = Convert.FromBase64String(pwd);
                pwds = Encoding.UTF8.GetString(bytes);
                if (pwds.Contains("2017"))
                {
                    pwd = pwds.Substring(0, pwds.Length - 4);
                    return pwd;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        //Index
        public IDictionary<string, string> indexMain(string userName, string pwd) {
            try
            {
                string flag = "";
                IDictionary<string, string> rsdic = new Dictionary<string, string>();
                if (checkAll(userName, pwd))
                {
                    flag = "A";
                }
                else
                {
                    flag = "B";
                }
                pwd = encryptPwd(pwd);
                rsdic.Add("flag", flag);
                rsdic.Add("userName", userName);
                rsdic.Add("pwd", pwd);
                return rsdic;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        //Form
        public IDictionary<string, string> FormInfo(string userName, string pwd) {
            try
            {
                string flag = "";
                IDictionary<string, string> rsdic = new Dictionary<string, string>();
                if (checkUser(userName))
                {
                    flag = "A";
                }
                pwd = decryptPwd(pwd);
                rsdic.Add("flag", flag);
                rsdic.Add("userName", userName);
                rsdic.Add("pwd", pwd);
                return rsdic;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        localdbEntities db = new localdbEntities();
        //personnel_info数据
        public List<personnel_info> getPersonnelData()
        {
            try
            {
                var model = new List<personnel_info>();
                model = db.personnel_info.Distinct().Select(m => m).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //allowance_summary数据
        public List<allowance_summary> getAllowanceData()
        {
            try
            {
                var model = new List<allowance_summary>();
                model = db.allowance_summary.Distinct().Select(m => m).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //salary_change数据
        public List<salary_change> getSalaryData()
        {
            try
            {
                var model = new List<salary_change>();
                model = db.salary_change.Distinct().Select(m => m).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //personnel_info邮件部分
        public List<personnel_info> personnelInfoMail(string dateStartString, string dateEndString)
        {
            try
            {
                DateTime dateStart, dateEnd;
                var model = new List<personnel_info>();
                if (dateStartString == null || dateEndString == null)
                {
                    dateStart = dateEnd = DateTime.Now;
                }
                else
                {
                    dateStart = DateTime.ParseExact(dateStartString, "yyyy/M/d", CultureInfo.CurrentCulture);
                    dateEnd = DateTime.ParseExact(dateEndString, "yyyy/M/d", CultureInfo.CurrentCulture);
                }
                model = getPersonnelData().Where(m => (m.SYS_CREATE_DATE >= dateStart && m.SYS_CREATE_DATE <= dateEnd)).OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //allowance_summary邮件部分
        public List<allowance_summary> allowaneMail(string dateStartString, string dateEndString)
        {
            try
            {
                DateTime dateStart, dateEnd;
                var model = new List<allowance_summary>();
                if (dateStartString == null || dateEndString == null)
                {
                    dateStart = dateEnd = DateTime.Now;
                }
                else
                {
                    dateStart = DateTime.ParseExact(dateStartString, "yyyy/M/d", CultureInfo.CurrentCulture);
                    dateEnd = DateTime.ParseExact(dateEndString, "yyyy/M/d", CultureInfo.CurrentCulture);
                }
                model = getAllowanceData().Where(m => (m.SYS_CREATE_DATE >= dateStart && m.SYS_CREATE_DATE <= dateEnd)).OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //新入职人员信息
        public List<personnel_info> getPersonnelInfo()
        {
            try
            {
                var model = new List<personnel_info>();
                model = getPersonnelData().Where(m => m.STATUS == "1").OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //离职人员信息
        public List<personnel_info> getResignationInfo()
        {
            try
            {
                var model = new List<personnel_info>();
                model = getPersonnelData().Where(m => m.STATUS == "2").OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //一次性津贴文件
        public List<allowance_summary> getOnetimeAllowance()
        {
            try
            {
                var model = new List<allowance_summary>();
                model = getAllowanceData().Where(m => m.IS_ONCE == "1").OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //固定津贴信息文件
        public List<allowance_summary> getFixedAllowance()
        {
            try
            {
                var model = new List<allowance_summary>();
                model = getAllowanceData().Where(m => m.IS_ONCE == "2").OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //薪资变动文件
        public List<salary_change> getSalaryChange()
        {
            try
            {
                var model = new List<salary_change>();
                model = getSalaryData().OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //新入职人员邮件部分
        public List<personnel_info> personnelMail(string dateStartString, string dateEndString)
        {
            try
            {
                var model = new List<personnel_info>();
                model = personnelInfoMail(dateStartString, dateEndString).Where(m => m.STATUS == "1").ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //离职人员邮件部分
        public List<personnel_info> resignationMail(string dateStartString, string dateEndString)
        {
            try
            {
                var model = new List<personnel_info>();
                model = personnelInfoMail(dateStartString, dateEndString).Where(m => m.STATUS == "2").ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //一次性津贴邮件部分
        public List<allowance_summary> OnetimeMail(string dateStartString, string dateEndString)
        {
            try
            {
                var model = new List<allowance_summary>();
                model = allowaneMail(dateStartString, dateEndString).Where(m => m.IS_ONCE == "1").ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //固定津贴邮件部分
        public List<allowance_summary> fixedMail(string dateStartString, string dateEndString)
        {
            try
            {
                var model = new List<allowance_summary>();
                model = allowaneMail(dateStartString, dateEndString).Where(m => m.IS_ONCE == "2").ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //薪资变动邮件部分
        public List<salary_change> salaryMail(string dateStartString, string dateEndString)
        {
            try
            {
                DateTime dateStart, dateEnd;
                var model = new List<salary_change>();
                if (dateStartString == null || dateEndString == null)
                {
                    dateStart = dateEnd = DateTime.Now;
                }
                else
                {
                    dateStart = DateTime.ParseExact(dateStartString, "yyyy/M/d", CultureInfo.CurrentCulture);
                    dateEnd = DateTime.ParseExact(dateEndString, "yyyy/M/d", CultureInfo.CurrentCulture);
                }
                model = getSalaryChange().Where(m => (m.SYS_CREATE_DATE >= dateStart && m.SYS_CREATE_DATE <= dateEnd)).OrderByDescending(m => m.SYS_CREATE_DATE).ToList();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //邮件重发
        string mail_receivers, mail_ccs, mail_mobiles, mail_folder, mail_sign, mail_connectionname, mail_securitytype, mail_host, mail_account, mail_accountpass;
        SmtpClient smtp = new SmtpClient();
        int mail_port;
        DataTable dt, dat;
        public string SentMail(String fileName, String configpathmail, String dateStartString, String dateEndString)
        {
            var configMail = XElement.Load(configpathmail);
            var connmail = configMail.Element("SMTP").Element("Groups");
            var mainmailpro = configMail.Element("SMTP");
            MailMessage mailMessage = new MailMessage();
            try
            {
                int count = 0;
                foreach (XElement xe in connmail.Elements())
                {
                    if (xe.Attribute("Name").Value.Equals("TestUsers"))
                    {
                        mail_receivers = xe.Element("MailTo").Value.Trim();
                        mail_ccs = xe.Element("CCTo").Value.Trim();
                        mail_mobiles = xe.Element("MSGTo").Value.Trim();
                        count++;
                    }
                }
                mail_folder = mainmailpro.Element("AttachmentsFolder").Value.Trim();
                mail_sign = mainmailpro.Element("Signature").Value.Trim();
                mail_connectionname = mainmailpro.Element("Connection").Value.Trim();
                mail_securitytype = mainmailpro.Element("SecurityType").Value.Trim();
                mail_host = mainmailpro.Element("SmtpHost").Value.Trim();
                mail_port = int.Parse(mainmailpro.Element("SmtpPort").Value.Trim());
                mail_account = mainmailpro.Element("EmailAddress").Value.Trim();
                mail_accountpass = decryptPwd(mainmailpro.Element("EmailPass").Value.Trim());
                //if (count > 1)  WriteMsg("配置文件中配置了多次邮件组" + _sr.SMTP_GROUP);
                switch (fileName)//此处文件表头和页面上的一样
                {
                    case "PersonnelInfo":
                        dat = ToDataTable<personnel_info>(personnelMail(dateStartString, dateEndString));//ToDataTable
                        if (dat != null && dat.Rows.Count == 0)
                        {
                            dat.Columns.Add("COMPANY", Type.GetType("System.String"));
                            dat.Columns.Add("WORKER_TYPE", Type.GetType("System.String"));
                            dat.Columns.Add("CHINESE_NAME", Type.GetType("System.String"));
                            dat.Columns.Add("PERSON_ID", Type.GetType("System.String"));
                            dat.Columns.Add("ALIAS_NAME", Type.GetType("System.String"));
                            dat.Columns.Add("NATIONAL_ID", Type.GetType("System.String"));
                            dat.Columns.Add("PERS_MOBILE", Type.GetType("System.String"));
                            dat.Columns.Add("PERS_EMAIL", Type.GetType("System.String"));
                            dat.Columns.Add("LASTEST_HIRE_DATE", Type.GetType("System.String"));
                            dat.Columns.Add("POSITION_TITLE", Type.GetType("System.String"));
                            dat.Columns.Add("SAL_AMT", Type.GetType("System.String"));
                            dat.Columns.Add("WORK_LOCATION", Type.GetType("System.String"));
                            dat.Rows.Add("");
                        }
                        dt = dat.DefaultView.ToTable(false, new string[] { "COMPANY", "WORKER_TYPE", "CHINESE_NAME", "PERSON_ID", "ALIAS_NAME", "NATIONAL_ID", "PERS_MOBILE", "PERS_EMAIL", "LASTEST_HIRE_DATE", "POSITION_TITLE", "SAL_AMT", "WORK_LOCATION" });    //筛选
                        break;
                    case "ResignationInfo":
                        dat = ToDataTable<personnel_info>(resignationMail(dateStartString, dateEndString));
                        if (dat != null && dat.Rows.Count == 0)
                        {
                            dat.Columns.Add("PERSON_ID", Type.GetType("System.String"));
                            dat.Columns.Add("TERM_REASON", Type.GetType("System.String"));
                            dat.Columns.Add("CHINESE_NAME", Type.GetType("System.String"));
                            dat.Columns.Add("NATIONAL_ID", Type.GetType("System.String"));
                            dat.Columns.Add("LAST_EMP_DATE", Type.GetType("System.String"));
                            dat.Columns.Add("DEPARTMENT", Type.GetType("System.String"));
                            dat.Rows.Add("");
                        }
                        dt = dat.DefaultView.ToTable(false, new string[] { "PERSON_ID", "TERM_REASON", "CHINESE_NAME", "NATIONAL_ID", "LAST_EMP_DATE", "DEPARTMENT" });
                        break;
                    case "OnetimeAllowance":
                        dat = ToDataTable<allowance_summary>(OnetimeMail(dateStartString, dateEndString));
                        if (dat != null && dat.Rows.Count == 0)
                        {
                            dat.Columns.Add("COMPANY", Type.GetType("System.String"));
                            dat.Columns.Add("WORKER_TYPE", Type.GetType("System.String"));
                            dat.Columns.Add("WORK_LOCATION", Type.GetType("System.String"));
                            dat.Columns.Add("PERSON_ID", Type.GetType("System.String"));
                            dat.Columns.Add("CHINESE_NAME", Type.GetType("System.String"));
                            dat.Columns.Add("NATIONAL_ID", Type.GetType("System.String"));
                            dat.Columns.Add("PAY_CODE", Type.GetType("System.String"));
                            dat.Columns.Add("PAY_CODE_DESC", Type.GetType("System.String"));
                            dat.Columns.Add("AMT", Type.GetType("System.String"));
                            dat.Columns.Add("START_DATE", Type.GetType("System.String"));
                            dat.Columns.Add("CURRENCY_CODE", Type.GetType("System.String"));
                            dat.Rows.Add("");
                        }
                        dt = dat.DefaultView.ToTable(false, new string[] { "COMPANY", "WORKER_TYPE", "WORK_LOCATION", "PERSON_ID", "CHINESE_NAME", "NATIONAL_ID", "PAY_CODE", "PAY_CODE_DESC", "AMT", "START_DATE", "CURRENCY_CODE" });
                        break;
                    case "FixedAllowance":
                        dat = ToDataTable<allowance_summary>(fixedMail(dateStartString, dateEndString));
                        if (dat != null && dat.Rows.Count == 0)
                        {
                            dat.Columns.Add("COMPANY", Type.GetType("System.String"));
                            dat.Columns.Add("WORKER_TYPE", Type.GetType("System.String"));
                            dat.Columns.Add("WORK_LOCATION", Type.GetType("System.String"));
                            dat.Columns.Add("PERSON_ID", Type.GetType("System.String"));
                            dat.Columns.Add("CHINESE_NAME", Type.GetType("System.String"));
                            dat.Columns.Add("NATIONAL_ID", Type.GetType("System.String"));
                            dat.Columns.Add("PAY_CODE", Type.GetType("System.String"));
                            dat.Columns.Add("PAY_CODE_DESC", Type.GetType("System.String"));
                            dat.Columns.Add("AMT", Type.GetType("System.String"));
                            dat.Columns.Add("START_DATE", Type.GetType("System.String"));
                            dat.Columns.Add("END_DATE", Type.GetType("System.String"));
                            dat.Columns.Add("FREQUENCY", Type.GetType("System.String"));
                            dat.Columns.Add("CURRENCY_CODE", Type.GetType("System.String"));
                            dat.Rows.Add("");
                        }
                        dt = dat.DefaultView.ToTable(false, new string[] { "COMPANY", "WORKER_TYPE", "WORK_LOCATION", "PERSON_ID", "CHINESE_NAME", "NATIONAL_ID", "PAY_CODE", "PAY_CODE_DESC", "AMT", "START_DATE", "END_DATE", "FREQUENCY", "CURRENCY_CODE" });
                        break;
                    case "SalaryChange":
                        dat = ToDataTable<salary_change>(salaryMail(dateStartString, dateEndString));
                        if (dat != null && dat.Rows.Count == 0)
                        {
                            dat.Columns.Add("COMPANY", Type.GetType("System.String"));
                            dat.Columns.Add("WORKER_TYPE", Type.GetType("System.String"));
                            dat.Columns.Add("WORK_LOCATION", Type.GetType("System.String"));
                            dat.Columns.Add("PERSON_ID", Type.GetType("System.String"));
                            dat.Columns.Add("CHINESE_NAME", Type.GetType("System.String"));
                            dat.Columns.Add("NATIONAL_ID", Type.GetType("System.String"));
                            dat.Columns.Add("SAL_START_DATE", Type.GetType("System.String"));
                            dat.Columns.Add("SAL_AMT", Type.GetType("System.String"));
                            dat.Columns.Add("CURRENCY_CODE", Type.GetType("System.String"));
                            dat.Rows.Add("");
                        }
                        dt = dat.DefaultView.ToTable(false, new string[] { "COMPANY", "WORKER_TYPE", "WORK_LOCATION", "PERSON_ID", "CHINESE_NAME", "NATIONAL_ID", "SAL_START_DATE", "SAL_AMT", "CURRENCY_CODE" });
                        break;
                    default:
                        return ("发送失败!");
                }
                bool flag = DataTableToExcel(dt, fileName);//DataTableToExcel
                smtp = GetMailClient();//GetMailClient()
                                       //收件人
                mailMessage.To.Add(mail_receivers);
                if (!mail_ccs.Equals(""))
                {
                    mailMessage.CC.Add(mail_ccs);
                }
                mailMessage.IsBodyHtml = false;
                mailMessage.From = new MailAddress(mail_account, mail_sign);
                if (flag)
                {
                    DirectoryInfo dir = new DirectoryInfo("C://ExcelPath//");
                    Attachment data = new Attachment("C://ExcelPath//" + fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx", MediaTypeNames.Application.Octet); //定义附件，前一个参数为文件实际路径，后一个参数表示附件媒体信息类型
                    mailMessage.Attachments.Add(data);
                    mailMessage.Subject = "测试邮件";
                    smtp.Send(mailMessage);
                    mailMessage.Dispose(); //释放文件资源
                    return ("正在发送中!");
                }
                else if (!flag)
                {
                    return ("发送失败!");
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message.ToString();
                return ("发送失败!");
            }
            return ("发送失败!");
        }
        public SmtpClient GetMailClient()
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = mail_host;
            smtpClient.Port = mail_port;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(mail_account, mail_accountpass);
            smtpClient.Timeout = 60000;
            if (mail_securitytype.ToUpper() == "SSL")
            {
                smtpClient.EnableSsl = true;
            }
            else
            {
                smtpClient.EnableSsl = false;
            }
            return smtpClient;
        }
        //List转成DataTable
        public static DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            PropertyInfo[] oProps = null;
            try
            {

                if (varlist == null) return dtReturn;
                foreach (T rec in varlist)
                {
                    if (oProps == null)
                    {
                        oProps = ((Type)rec.GetType()).GetProperties();
                        foreach (PropertyInfo pi in oProps)
                        {
                            Type colType = pi.PropertyType;
                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                    }
                    DataRow dr = dtReturn.NewRow();
                    foreach (PropertyInfo pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                    }
                    dtReturn.Rows.Add(dr);
                }
                return dtReturn;
            }
           catch(Exception e)
           {
               return null;
           }
        }
        //导出Excel
        public static bool DataTableToExcel(DataTable dt, String fileName)
        {
            bool result = false;
            IWorkbook workbook = null;
            FileStream fs = null;
            IRow row = null;//行
            ISheet sheet = null;//列
            ICell cell = null;
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    workbook = new XSSFWorkbook();//xlsx    操作Excel的
                    sheet = workbook.CreateSheet("Sheet0");//创建一个名称为Sheet0的表  
                    int rowCount = dt.Rows.Count;//行数  
                    int columnCount = dt.Columns.Count;//列数  
                    //设置列头  
                    row = sheet.CreateRow(0);//excel第一行设为列头  
                    for (int c = 0; c < columnCount; c++)
                    {
                        cell = row.CreateCell(c);
                        cell.SetCellValue(dt.Columns[c].ColumnName);
                    }
                    //设置每行每列的单元格  
                    for (int i = 0; i < rowCount; i++)
                    {
                        row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < columnCount; j++)
                        {
                            cell = row.CreateCell(j);//excel第二行开始写入数据  
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }
                    string directoryPath = "C://ExcelPath";
                    string filePath = "C://ExcelPath//" + fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                    if (!Directory.Exists(directoryPath))//判断
                    {
                        Directory.CreateDirectory(directoryPath);//创建目录
                    }
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    using (fs = File.OpenWrite(filePath))
                    {
                        workbook.Write(fs);//向打开的这个xlsx文件中写入数据  
                        result = true;
                    }
                }
                fs.Close();
                return result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                if (fs != null)
                {
                    fs.Close();
                }
                return false;
            }
        }
        //上传文件 
        public string UploadFile(string fileName, string filePath,string logPath,DataTable dtExcel)
        {
            try
            {
                DataTable dtBase = DatabaseToDataTable(fileName);
                if (dtBase == null)
                {
                    string msg = dtBase.Rows[0]["exMessage"].ToString();
                    return dtBase.Rows[0]["exMessage"].ToString();
                }else if (dtBase.Columns.Count == 1)
                {
                    return "无新数据更新！";
                }
                DataTable dt = compareFields(dtExcel, dtBase, fileName, logPath);
                if (dt == null)
                {
                    return "无新数据更新！";
                }else if (dt.Columns.Count == 1) {
                    string msg = dt.Rows[0]["exMessage"].ToString();
                    return dt.Rows[0]["exMessage"].ToString();
                }
                string flagmsg = SqlBulkCopyInsert(fileName, dt);
                if (flagmsg != "1")
                {
                    return flagmsg;
                }
                return "文件上传成功！";
            }
            catch (Exception ex) {
                return ex.ToString();
            }
        }
        //读取Excel
        List<string> listHead = new List<string>();
        List<string> personnelHead = new List<string>();
        List<string> allowanceHead = new List<string>();
        List<string> salaryHead = new List<string>();
        public DataTable ExcelToDataTable(string fileName, string filePath)
        {
            #region 20171211 陆宁
            if (!File.Exists(filePath))
            {
                throw new Exception("指定的Excel文件不存在!");
            }
            //注意：把一个excel文件看做一个数据库，一个sheet看做一张表。语法 "SELECT * FROM [sheet1$]"，表单要使用"[]"和"$"
            string excelPath = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended properties=\"Excel 8.0;Imex=1;HDR=Yes;\"";
            string strConn = "";
            DataTable dtGBPatient = new DataTable();
            // 2、通过IMEX=1来把混合型作为文本型读取，避免null值。
            if (filePath.Contains("xls") && !filePath.Contains("xlsx"))
            {
                // Excel2003 .xls文件
                strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
            }
            else if (filePath.Contains("xlsx"))
            {
                // Excel2007 .xlsx文件
                strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1';";
            }

            string strConnection = string.Format(strConn, excelPath);
            OleDbConnection conn = new OleDbConnection(strConnection);
            conn.Open();
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", strConnection);

            //dtGBPatient.TableName = "gbPatientInfo";
            oada.Fill(dtGBPatient);
            #endregion

            DataTable dt = new DataTable();
            try
            {
                switch (fileName)
                {
                    case "PersonnelInfo.xlsx":
                    case "PersonnelInfo.xls":
                    case "ResignationInfo.xlsx":
                    case "ResignationInfo.xls":
                        listHead.Add("PERSON_ID");
                        listHead.Add("FIRST_NAME");
                        listHead.Add("LAST_NAME");
                        listHead.Add("MIDDLE_NAME");
                        listHead.Add("CHINESE_NAME");
                        listHead.Add("ALIAS_NAME");
                        listHead.Add("PERS_MOBILE");
                        listHead.Add("COMPANY");
                        listHead.Add("PERS_EMAIL");
                        listHead.Add("NATIONAL_ID");
                        listHead.Add("LASTEST_HIRE_DATE");
                        listHead.Add("SAL_AMT");
                        listHead.Add("POSITION_TITLE");
                        listHead.Add("WORK_LOCATION");
                        listHead.Add("COST_CENTER");
                        listHead.Add("ACTION");
                        listHead.Add("LAST_EMP_DATE");
                        listHead.Add("TERM_REASON");
                        listHead.Add("STATUS");
                        listHead.Add("WORKER_TYPE");
                        listHead.Add("SYS_CREATE_DATE");
                        listHead.Add("COST_CENTER_DESC");
                        personnelHead = listHead;
                        break;
                    case "OnetimeAllowance.xlsx":
                    case "OnetimeAllowance.xls":
                    case "FixedAllowance.xlsx":
                    case "FixedAllowance.xls":
                        listHead.Add("PERSON_ID");
                        listHead.Add("PAY_CODE");
                        listHead.Add("PAY_CODE_DESC");
                        listHead.Add("AMT");
                        listHead.Add("START_DATE");
                        listHead.Add("CURRENCY_CODE");
                        listHead.Add("END_DATE");
                        listHead.Add("FREQUENCY");
                        listHead.Add("IS_ONCE");
                        listHead.Add("SYS_CREATE_DATE");
                        listHead.Add("WORK_LOCATION");
                        listHead.Add("WORKER_TYPE");
                        listHead.Add("COMPANY");
                        listHead.Add("CHINESE_NAME");
                        listHead.Add("NATIONAL_ID");
                        allowanceHead = listHead;
                        break;
                    case "SalaryChange.xlsx":
                    case "SalaryChange.xls":
                        listHead.Add("PERSON_ID");
                        listHead.Add("FIRST_NAME");
                        listHead.Add("LAST_NAME");
                        listHead.Add("MIDDLE_NAME");
                        listHead.Add("CHINESE_NAME");
                        listHead.Add("ALIAS_NAME");
                        listHead.Add("NATIONAL_ID");
                        listHead.Add("COMPANY");
                        listHead.Add("SAL_START_DATE");
                        listHead.Add("SAL_AMT");
                        listHead.Add("SYS_CREATE_DATE");
                        listHead.Add("WORK_LOCATION");
                        listHead.Add("WORKER_TYPE");
                        salaryHead = listHead;
                        break;
                    default:
                        return null;
                }
                //获得表头
                for (int i = 0; i < listHead.Count(); i++)
                {
                    var str = listHead[i].ToString();
                    dt.Columns.Add(str, Type.GetType("System.String"));
                }
                //给DataTable赋值
                foreach (DataRow dl in dtGBPatient.Rows)
                {
                    //int num = 0;
                    DataRow dr = dt.NewRow();
                    if (dl != null)
                    {
                        foreach (String temphead in listHead)
                        {
                            dr[temphead] = dl[temphead];
                        }
                    }
                    dt.Rows.Add(dr);
                }
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                conn.Close();
                return null;

            }
        }
        //Excel查重
        public ArrayList compareExcel(string fileName, string filePath)
        {
            string msg = "";
            ArrayList arrMsg = new ArrayList();
            try
            {
                DataTable dtExcel = ExcelToDataTable(fileName, filePath);
                int[] arr = new int[dtExcel.Rows.Count];
                int num = 0;
                for (var i=0; i<dtExcel.Rows.Count; i++)
                {
                    DataRow drf = dtExcel.Rows[i];
                    var flag = true;
                    for (var j=i+1; j<dtExcel.Rows.Count; j++)
                    {
                        if(arr[j] == 0)
                        {
                            DataRow drs = dtExcel.Rows[j];
                            switch (fileName)
                            {
                                case "PersonnelInfo.xlsx":
                                case "PersonnelInfo.xls":
                                case "ResignationInfo.xlsx":
                                case "ResignationInfo.xls":
                                    if ((drf["PERSON_ID"].ToString().Trim()).Equals(drs["PERSON_ID"].ToString().Trim()) && (drf["LASTEST_HIRE_DATE"].ToString().Trim()).Equals(drs["LASTEST_HIRE_DATE"].ToString().Trim()))
                                    {
                                        num = i + 1;
                                        arr[j] = 1;
                                        flag = false;
                                    }
                                    break;
                                case "OnetimeAllowance.xlsx":
                                case "OnetimeAllowance.xls":
                                case "FixedAllowance.xlsx":
                                case "FixedAllowance.xls":
                                    if ((drf["PERSON_ID"].ToString().Trim()).Equals(drs["PERSON_ID"].ToString().Trim()) && (drf["PAY_CODE"].ToString().Trim()).Equals(drs["PAY_CODE"].ToString().Trim()) && (drf["START_DATE"].ToString().Trim()).Equals(drs["START_DATE"].ToString().Trim()) && (drf["END_DATE"].ToString().Trim()).Equals(drs["END_DATE"].ToString().Trim()))
                                    {
                                        num = i + 1;
                                        arr[j] = 1;
                                        flag = false;
                                    }
                                    break;
                                case "SalaryChange.xlsx":
                                case "SalaryChange.xls":
                                    if ((drf["PERSON_ID"].ToString().Trim()).Equals(drs["PERSON_ID"].ToString().Trim()) && (drf["SAL_START_DATE"].ToString().Trim()).Equals(drs["SAL_START_DATE"].ToString().Trim()))
                                    {
                                        num = i + 1;
                                        arr[j] = 1;
                                        flag = false;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        msg += "该 Excel 中第" + num + "条数据主键有重复" + "\n";
                    }
                }
                arrMsg.Add(msg);
                arrMsg.Add(dtExcel);
                return arrMsg;
            }
            catch(Exception ex)
            {
                msg += ex.Message;
                arrMsg.Add(msg);
                return arrMsg;
            }
        }
        //数据库数据转成datatable
        public DataTable DatabaseToDataTable(String fileName)
        {
            try
            {
                switch (fileName)
                {
                    case "PersonnelInfo.xlsx":
                    case "PersonnelInfo.xls":
                    case "ResignationInfo.xlsx":
                    case "ResignationInfo.xls":
                        var modelf = getPersonnelData();
                        dt = ToDataTable<personnel_info>(modelf);
                        break;
                    case "OnetimeAllowance.xlsx":
                    case "OnetimeAllowance.xls":
                    case "FixedAllowance.xlsx":
                    case "FixedAllowance.xls":
                        var models = getAllowanceData();
                        dt = ToDataTable<allowance_summary>(models);
                        break;
                    case "SalaryChange.xlsx":
                    case "SalaryChange.xls":
                        var modelt = getSalaryData();
                        dt = ToDataTable<salary_change>(modelt);
                        break;
                    default:
                        return null;
                }
                return dt;
            }
            catch(Exception ex)
            {
                DataTable dtex = exDeal(ex);
                return dtex;
            }
        }
        //筛选数据
        public DataTable compareFields(DataTable dtFile, DataTable dtDatabase, string fileName, string logPath)
        {
            if(dtDatabase.Rows.Count == 0)
            {
                return dtFile;
            }
            bool flag = true;
            DataTable dtTemp = dtDatabase.Clone();
            StreamWriter sw = null;
            string timeNow;
            string str = null;
            string directoryPath = logPath;
            bool flags = false;
            try
            {
                if (!Directory.Exists(directoryPath))//判断
                {
                    Directory.CreateDirectory(directoryPath);//创建目录
                }
                string filePath = logPath + "log" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                sw = new StreamWriter(filePath, true, Encoding.Default);//创建写入文件  true为追加
                switch (fileName)
                {
                    case "PersonnelInfo.xlsx":
                    case "PersonnelInfo.xls":
                    case "ResignationInfo.xlsx":
                    case "ResignationInfo.xls":
                        foreach (DataRow dr in dtFile.Rows)
                        {
                            string valdr = dr["PERSON_ID"].ToString().Trim();
                            string valdrs = dr["NATIONAL_ID"].ToString().Trim();
                            if (valdr == "" || valdrs == "")
                            {
                                flag = false;
                                timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                str = "[" + timeNow + "]" + "    " + fileName + "    " + valdr + "    " + valdrs + "    " + "主键存在空值";
                                sw.WriteLine(str);
                                continue;
                            }
                            //忘记为什么这么写了--------------------------------------------
                            dr["LASTEST_HIRE_DATE"] = Convert.ToDateTime(dr["LASTEST_HIRE_DATE"].ToString().Trim());//DateTime
                            dr["LAST_EMP_DATE"] = Convert.ToDateTime(dr["LAST_EMP_DATE"].ToString().Trim());
                            dr["SYS_CREATE_DATE"] = Convert.ToDateTime(dr["SYS_CREATE_DATE"].ToString().Trim());
                            flag = true;
                            foreach (DataRow dro in dtDatabase.Rows)
                            {
                                string valdro = dro["PERSON_ID"].ToString().Trim();
                                string valdros = dro["NATIONAL_ID"].ToString().Trim();
                                if (valdr.Equals(valdro) && valdrs.Equals(valdros))
                                {
                                    timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    str = "[" + timeNow + "]" + "    " + fileName + "    " + valdr + "    " + valdrs + "    " + "主键重复";
                                    sw.WriteLine(str);
                                    foreach(string listString in personnelHead)
                                    {
                                        //这里有问题------------------------------------------------------------------
                                        var valdrlist = dr[listString].ToString().Trim();
                                        var valdrolist = dro[listString].ToString().Trim();
                                        if (!(valdrlist.Equals(valdrolist)) && valdrlist != "")
                                        {
                                            personnel_info valChange = db.personnel_info.First(m => m.PERSON_ID == valdro && m.NATIONAL_ID == valdros);
                                            Type type = valChange.GetType(); //获取类型
                                            PropertyInfo propertyInfo = type.GetProperty(listString); //获取指定名称的属性
                                           
                                            //Type typeSomeClass = propertyInfo.GetType();
                                            //var name = typeSomeClass.FullName;
                                            //Convert.ChangeType(dr[listString], typeSomeClass);
                                            switch (listString)
                                            {
                                                case "LASTEST_HIRE_DATE":
                                                case "LAST_EMP_DATE":
                                                case "SYS_CREATE_DATE":
                                                    DateTime dtdr = new DateTime();
                                                    dtdr = DateTime.Parse(valdrlist);
                                                    propertyInfo.SetValue(valChange, dtdr, null);
                                                    break;
                                                case "SAL_AMT":
                                                    Decimal dtdrs;
                                                    dtdrs = Decimal.Parse(valdrlist);
                                                    propertyInfo.SetValue(valChange, dtdrs, null);
                                                    break;
                                                default:
                                                    propertyInfo.SetValue(valChange, valdrlist, null); //给对应属性赋值
                                                    break;
                                            }
                                            int valf = db.SaveChanges();
                                            if(valf > 0)
                                            {
                                                flags = true;
                                            }
                                        }
                                    } 
                                    flag = false; 
                                }
                            }
                            if (flag)
                            {
                                DataRow drtemp = dtTemp.NewRow();
                                drtemp.ItemArray = dr.ItemArray;
                                dtTemp.Rows.Add(drtemp);
                            }
                        }
                        break;
                    case "OnetimeAllowance.xlsx":
                    case "OnetimeAllowance.xls":
                    case "FixedAllowance.xlsx":
                    case "FixedAllowance.xls":
                        foreach (DataRow dr in dtFile.Rows)
                        {
                            string valdr = dr["PERSON_ID"].ToString().Trim();
                            string valsdr = dr["PAY_CODE"].ToString().Trim();
                            string valtdr = dr["START_DATE"].ToString().Trim();
                            string valfdr = dr["END_DATE"].ToString().Trim();
                            if (valdr == "" || valsdr == "" || valtdr == "" || valfdr == "")
                            {

                                flag = false;
                                timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                str = "[" + timeNow + "]" + "    " + fileName + "    " + valdr + "    " + valsdr + "    " + valtdr + "    " + valfdr + "    " + "主键存在空值";
                                sw.WriteLine(str);
                                continue;
                            }
                            dr["START_DATE"] = Convert.ToDateTime(dr["START_DATE"].ToString().Trim());
                            dr["END_DATE"] = Convert.ToDateTime(dr["END_DATE"].ToString().Trim());
                            dr["SYS_CREATE_DATE"] = Convert.ToDateTime(dr["SYS_CREATE_DATE"].ToString().Trim());
                            flag = true;
                            foreach (DataRow dro in dtDatabase.Rows)
                            {
                                string valdro = dro["PERSON_ID"].ToString().Trim();
                                string valsdro = dro["PAY_CODE"].ToString().Trim();
                                string valtdro = dro["START_DATE"].ToString().Trim();
                                string valfdro = dro["END_DATE"].ToString().Trim();
                                if (valdr.Equals(valdro) && valsdr.Equals(valsdro) && valtdr.Equals(valtdro) && valfdr.Equals(valfdro))
                                {
                                    flag = false;
                                    timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    str = "[" + timeNow + "]" + "    " + fileName + "    " + valdr + "    " + valsdr + "    " + valtdr + "    " + valfdr + "    " + "主键重复";
                                    foreach (string listString in allowanceHead)
                                    {
                                        var valdrlist = dr[listString].ToString().Trim();
                                        var valdrolist = dro[listString].ToString().Trim();
                                        if (!(valdrlist.Equals(valdrolist)) && valdrlist != "")
                                        {
                                            var datefirst = DateTime.Parse(dr["START_DATE"].ToString().Trim());
                                            var datesecond = DateTime.Parse(dr["END_DATE"].ToString().Trim());
                                            allowance_summary valChange = db.allowance_summary.First(m => m.PERSON_ID == valdr && m.PAY_CODE == valsdr && m.START_DATE == datefirst && m.END_DATE == datesecond);
                                            //allowance_summary valChange = db.allowance_summary.First(m => m.PERSON_ID == valdr && m.PAY_CODE == valsdr && m.START_DATE.ToString() == valtdro);
                                            Type type = valChange.GetType(); //获取类型
                                            PropertyInfo propertyInfo = type.GetProperty(listString); //获取指定名称的属性
                                            switch (listString)
                                            {
                                                case "SYS_CREATE_DATE":
                                                    DateTime dtdr = new DateTime();
                                                    dtdr = DateTime.Parse(valdrlist);
                                                    propertyInfo.SetValue(valChange, dtdr, null);
                                                    break;
                                                case "AMT":
                                                    Decimal dtdrs;
                                                    dtdrs = Decimal.Parse(valdrlist);
                                                    propertyInfo.SetValue(valChange, dtdrs, null);
                                                    break;
                                                default:
                                                    propertyInfo.SetValue(valChange, valdrlist, null); //给对应属性赋值  属性所在的对象 属性 索引
                                                    break;
                                            }
                                            int valf = db.SaveChanges();
                                            if (valf > 0)
                                            {
                                                flags = true;
                                            }
                                        }
                                    }
                                    flag = false;
                                }
                            }
                            if (flag)
                            {
                                DataRow drtemp = dtTemp.NewRow();
                                drtemp.ItemArray = dr.ItemArray;
                                dtTemp.Rows.Add(drtemp);
                            }
                        }
                        break;
                    case "SalaryChange.xlsx":
                    case "SalaryChange.xls":
                        foreach (DataRow dr in dtFile.Rows)
                        {
                            string valdr = dr["PERSON_ID"].ToString().Trim();
                            string valsdr = dr["SAL_START_DATE"].ToString().Trim();
                            if (dr["PERSON_ID"].ToString().Trim() == "" || dr["SAL_START_DATE"].ToString().Trim() == "")
                            {
                                flag = false;
                                timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                                str = "[" + timeNow + "]" + "    " + fileName + "    " + valdr + "    " + valsdr + "    " + "主键存在空值";
                                sw.WriteLine(str);
                                continue;
                            }
                            dr["SYS_CREATE_DATE"] = Convert.ToDateTime(dr["SYS_CREATE_DATE"].ToString());
                            flag = true;
                            foreach (DataRow dro in dtDatabase.Rows)
                            {
                                string valdro = dro["PERSON_ID"].ToString().Trim();
                                string valsdro = dro["SAL_START_DATE"].ToString().Trim();
                                if (valdr.Equals(valdro) && valsdr.Equals(valsdro))
                                {
                                    flag = false;
                                    timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
                                    str = "[" + timeNow + "]" + "    " + fileName + "    " + valdr + "    " + valsdr + "    " + "主键重复";
                                    sw.WriteLine(str);
                                    foreach (string listString in salaryHead)
                                    {
                                        var valdrlist = dr[listString].ToString().Trim();
                                        var valdrolist = dro[listString].ToString().Trim();
                                        if (!(valdrlist.Equals(valdrolist)) && valdrlist != "")
                                        {
                                            var datefirst = DateTime.Parse(dr["SAL_START_DATE"].ToString().Trim());
                                            salary_change valChange = db.salary_change.First(m => m.PERSON_ID == valdro && m.SAL_START_DATE == datefirst);
                                            Type type = valChange.GetType(); //获取类型
                                            var name = type.FullName;
                                            PropertyInfo propertyInfo = type.GetProperty(listString); //获取指定名称的属性
                                            switch (listString)
                                            {
                                                case "SYS_CREATE_DATE":
                                                    DateTime dtdr = new DateTime();
                                                    dtdr = DateTime.Parse(valdrlist);
                                                    propertyInfo.SetValue(valChange, dtdr, null);
                                                    break;
                                                case "SAL_AMT":
                                                    Double dtdrs;
                                                    dtdrs = Double.Parse(valdrlist);
                                                    propertyInfo.SetValue(valChange, dtdrs, null);
                                                    break;
                                                default:
                                                    propertyInfo.SetValue(valChange, valdrlist, null); //给对应属性赋值
                                                    break;
                                            }
                                            int valf = db.SaveChanges();
                                            if (valf > 0)
                                            {
                                                flags = true;
                                            }
                                        }
                                    }
                                    flag = false;
                                }
                            }
                            if (flag)
                            {
                                DataRow drtemp = dtTemp.NewRow();
                                drtemp.ItemArray = dr.ItemArray;
                                dtTemp.Rows.Add(drtemp);
                            }
                        }
                        break;
                    default:
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                        return null;
                }
                sw.Flush();
                sw.Close();
                sw.Dispose();
                if(dtTemp.Rows.Count == 0 && !flags)
                {
                    return null;
                }else{
                    return dtTemp;
                }
                
            }
            catch (Exception ex)
            {
                DataTable dtex = exDeal(ex);
                sw.Flush();
                sw.Close();
                sw.Dispose();
                return dtex;
            }
           
        }
        //datatable数据插入到数据库中
        public string SqlBulkCopyInsert(string fileName, DataTable dtTemp)
        {
            string strTableName;
            //DataTable dtTemp = null;
            try
            {
                switch (fileName)
                {
                    case "PersonnelInfo.xlsx":
                    case "PersonnelInfo.xls":
                    case "ResignationInfo.xlsx":
                    case "ResignationInfo.xls":
                        strTableName = "personnel_info";
                        break;
                    case "OnetimeAllowance.xlsx":
                    case "OnetimeAllowance.xls":
                    case "FixedAllowance.xlsx":
                    case "FixedAllowance.xls":
                        strTableName = "allowance_summary";
                        break;
                    case "SalaryChange.xlsx":
                    case "SalaryChange.xls":
                        strTableName = "salary_change";
                        break;
                    default:
                        return "1";
                }
                //dtTemp = DatabaseToDataTable(fileName);
                using (SqlBulkCopy sqlRevdBulkCopy = new SqlBulkCopy(getConnection()))//引用SqlBulkCopy  
                {
                    sqlRevdBulkCopy.DestinationTableName = strTableName;//数据库中对应的表名  
                    sqlRevdBulkCopy.NotifyAfter = dtTemp.Rows.Count;//有几行数据  
                    sqlRevdBulkCopy.WriteToServer(dtTemp);//数据导入数据库  
                    sqlRevdBulkCopy.Close();//关闭连接  
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        //数据库连接
        private string getConnection()
        {
            try
            {
                return "server=127.0.0.1;uid=sa;pwd=123456;database=localdb";
            }
            catch(Exception ex)
            {
                string msg = ex.Message.ToString();
                return null;
            }
        }
        //操作数据库
        public void executeDb(string sqlText)
        {
            try
            {
                SqlConnection con = new SqlConnection(getConnection());
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlText;//设置SQL语句 
                cmd.Connection = con;//调用打开数据库连接方法 
                con.Open();
                cmd.ExecuteNonQuery();//执行 
                con.Close();
            }
           catch(Exception ex)
           {
                string msg = ex.Message.ToString();
           }
        }
        //处理异常
        public DataTable exDeal(Exception ex) {
            DataTable dtex = new DataTable();
            DataRow drex = dtex.NewRow();
            dtex.Columns.Add("exMessage");
            drex["exMessage"] = ex.Message;
            dtex.Rows.Add(drex);
            return dtex;
        }
    }
}
