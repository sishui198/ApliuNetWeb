﻿using Apliu.Tools.Core;
using Apliu.Tools.Core.Web;
using Apliu.Net.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Apliu.Net.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        /// <summary>
        /// GET api/tools/GetQRCode?Content=content
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task GetQRCode(string Content)
        {
            var qrcode = QRCode.CreateCodeSimpleBitmap(Content);
            await Response.WriteBodyAsync(qrcode);
        }

        [HttpGet]
        public async Task SearchTool(string Keyword)
        {
            String json = @"{""errorCode"":""0"",
                                  ""word"":[
                                        {""key"":""" + Keyword + @""",""count"":""3""}
                             ]}";

            String js = @"$('#sid').attr('type','text');
                          $('#sid').val('" + Keyword + @"');
                          //Suggest.prototype.updateList(JSON.parse('" + json.Replace(System.Environment.NewLine, "") + "'));";
            await Response.WriteBodyAsync(js);
        }

        [HttpGet]
        public async Task SearchSubmit(string sid, string keyword)
        {
            await Response.WriteBodyAsync(sid + keyword);
        }

        /// <summary>
        /// 链接服务器的测试数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpGet]
        public string ExecuteSql(string Sql, string Type)
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            Sql = Sql.Trim();
            if (string.IsNullOrEmpty(Sql)) return result.ToString();

            string type = ConfigurationJson.Appsettings.TestDatabaseType;
            string ip = ConfigurationJson.Appsettings.TestDatabaseIp;
            string dbname = ConfigurationJson.Appsettings.TestDatabaseName;
            string name = ConfigurationJson.Appsettings.TestDatabaseUserName;
            string psw = ConfigurationJson.Appsettings.TestDatabasePassword;
            DataAccess.LoadDataAccess("TestDatabase", type, SecurityHelper.DESDecrypt(ip, ConfigurationJson.AllEncodingAESKey), "", dbname, name, SecurityHelper.DESDecrypt(psw, ConfigurationJson.AllEncodingAESKey));
            switch (Type.ToUpper())
            {
                case "QUERY":
                    DataSet sqlds = DataAccess.InstanceKey["TestDatabase"].GetData(Sql);
                    if (sqlds != null && sqlds.Tables.Count > 0)
                    {
                        result.code = sqlds.Tables.Count;
                        result.msg = JsonConvert.SerializeObject(sqlds.Tables[0]);
                        result.result = "执行成功";
                    }
                    break;
                case "EXECUTE":
                    int rank = DataAccess.InstanceKey["TestDatabase"].PostDataExecute(Sql, 30);
                    if (rank >= 0)
                    {
                        result.code = rank;
                        result.msg = "受影响的数据条数：" + rank;
                        result.result = "执行成功";
                    }
                    break;
                default:
                    break;
            }
            return result.ToString();
        }

        /// <summary>
        /// 链接指定数据库进行操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        [HttpGet]
        public string ExecuteDatabseSql(string source, string userid, string password, string database, string sql)
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            sql = sql.Trim();
            if (string.IsNullOrEmpty(sql)) return result.ToString();

            string databaseType = "Mysql";
            #region 考虑到外部调用API接口会导致DataAccess对象越来越多，而占用过多内存，改成创建临时DataAccess对象进行服务
            //string ip = HYRequest.GetIP();//以客户端IP作为Key，避免重复加载数据库链接对象
            //string key = SecurityHelper.MD5Encrypt(ip, System.Text.Encoding.UTF8);
            //if (string.IsNullOrEmpty(ip)) key = Guid.NewGuid().ToString().Replace("-", "");
            //DataAccess.LoadDataAccess(key, databaseType, databaseConnection);
            #endregion
            DataAccess dataAccess = new DataAccess(databaseType, source, "", database, userid, password);
            if (sql.ToUpper().Contains("UPDATE") || sql.ToUpper().Contains("DELETE") || sql.ToUpper().Contains("INSERT"))
            {
                int rank = dataAccess.PostDataExecute(sql, 30);//DataAccess.InstanceKey[key].PostDataExecute(CommandType.Text, sql, 30, null);
                if (rank >= 0)
                {
                    result.code = rank;
                    result.msg = "受影响的数据条数：" + rank;
                    result.result = "执行成功";
                }
            }
            else
            {
                DataSet sqlds = dataAccess.GetData(sql);//DataAccess.InstanceKey[key].GetData(sql);
                if (sqlds != null && sqlds.Tables.Count > 0)
                {
                    result.code = sqlds.Tables.Count;
                    result.msg = JsonConvert.SerializeObject(sqlds.Tables[0]);
                    result.result = "执行成功";
                }
            }
            return result.ToString();
        }

        [HttpPost]
        public string SendSMS(string Mobile, string SMSContent, string TcSMSAppId, string TcSMSAppKey)
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            String sendLogSql = String.Empty;
            ISMSMessage sms = new TencentSMS();
            bool sendresult = sms.SendSMS(Mobile, SMSContent, out string SendMsg, out sendLogSql, TcSMSAppId, TcSMSAppKey);
            if (sendresult)
            {
                result.code = 0;
                result.result = "发送成功";
            }
            result.msg = SendMsg;
            bool logResult = DataAccess.Instance.PostData(sendLogSql);
            return result.ToString();
        }

        [HttpGet]
        public string SendSMSGet(string Mobile, string SMSContent, string TcSMSAppId, string TcSMSAppKey)
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            String sendLogSql = String.Empty;
            ISMSMessage sms = new TencentSMS();
            bool sendresult = sms.SendSMS(Mobile, SMSContent, out string SendMsg, out sendLogSql, TcSMSAppId, TcSMSAppKey);
            if (sendresult)
            {
                result.code = 0;
                result.result = "发送成功";
            }
            result.msg = SendMsg;
            bool logResult = DataAccess.Instance.PostData(sendLogSql);
            return result.ToString();
        }

        [HttpPost]
        public string SendSMSCode()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            string Mobile = HttpContext.Request.Form["Mobile"];
            CodeType codeType = (CodeType)HttpContext.Request.Form["codeType"].ToString().ToInt();

            if (codeType == CodeType.Register && !UserInfo.UserCheck(Mobile))
            {
                result.msg = "该用户已注册";
                return result.ToString();
            }

            if ((codeType == CodeType.Login || codeType == CodeType.ChangePassword) && UserInfo.UserCheck(Mobile))
            {
                result.msg = "该用户未注册";
                return result.ToString();
            }

            bool sendresult = VerificationCode.SendSMS(Mobile, codeType, out string SendMsg, out CodeCase codeCase);
            if (sendresult)
            {
                HttpContext.Session.SetValue(codeCase.Type.ToString(), codeCase);

                result.code = 0;
                result.result = "发送成功";
            }
            else
            {
                result.code = -1;
                result.result = "发送失败";
            }
            result.msg = SendMsg;
            return result.ToString();
        }

        [HttpPost]
        [HttpGet]
        public string ApliuAjax(object value)
        {
            return "{\"errorCode\":\"-1\",\"errorMsg\":\"默认方法\",\"value\":\"" + value + "\"}";
        }

        /// <summary>
        /// 获取临时文本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetTempContent()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            string Key = HttpContext.Request.Query["Key"];
            if (!string.IsNullOrEmpty(Key) && Key.Trim().Length >= 100)
            {
                result.msg = "标识符Key（text/key）长度必须小于100";
                result.result = "执行失败";
                return result.ToString();
            }
            string sqlWhere = string.IsNullOrEmpty(Key) ? " and TextKey is null " : " and TextKey= '" + SecurityHelper.UrlDecode(Key.Trim(), Encoding.UTF8) + "' ";
            string userid = HttpContext.Session.GetUserInfo()?.UserId;
            sqlWhere += string.IsNullOrEmpty(userid) ? " and UserId = 'Everyone' " : " and UserId= '" + userid + "' ";

            DataSet dsText = DataAccess.Instance.GetData("select TEXTCONTENT,UPDATETIME from TempText where 1=1 " + sqlWhere + "  limit 0,1");
            if (dsText != null && dsText.Tables.Count > 0)
            {
                result.code = 0;
                string content = string.Empty;
                string datetime = string.Empty;
                if (dsText.Tables[0].Rows.Count > 0)
                {
                    content = SecurityHelper.UrlDecode(dsText.Tables[0].Rows[0]["TEXTCONTENT"].ToString(), Encoding.UTF8);
                    datetime = dsText.Tables[0].Rows[0]["UPDATETIME"].ToString();
                }
                result.msg = content;
                result.result = "查询成功";
                result.remark = datetime;
            }
            return result.ToString();
        }

        /// <summary>
        /// 保存临时文本
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SetTempContent()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            string Content = HttpContext.Request.Form["Content"];
            string Key = HttpContext.Request.Form["Key"];//不存在key的时候，Key值为null
            if (!string.IsNullOrEmpty(Key) && Key.Trim().Length >= 100)
            {
                result.msg = "标识符Key（text/key）长度必须小于100";
                result.result = "执行失败";
                return result.ToString();
            }
            string sqlWhere = string.IsNullOrEmpty(Key) ? " and TextKey is null " : " and TextKey='" + SecurityHelper.UrlEncode(Key.Trim(), Encoding.UTF8) + "' ";
            string userid = string.IsNullOrEmpty(HttpContext.Session.GetUserInfo()?.UserId) ? "Everyone" : HttpContext.Session.GetUserInfo()?.UserId;
            sqlWhere += " and UserId = '" + userid + "'";

            string updatesql = string.Empty;
            DataSet dsText = DataAccess.Instance.GetData("select TEXTCONTENT from TempText where 1=1 " + sqlWhere + " limit 0,1");
            if (dsText != null && dsText.Tables.Count > 0 && dsText.Tables[0].Rows.Count > 0)
            {
                updatesql = string.Format(@"update TempText set UserId='{0}',TextContent='{1}',UpdateTime='{2}',IP='{3}' where 1=1 {4} ",
                    userid, SecurityHelper.UrlEncode(Content, Encoding.UTF8),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), HttpContext.GetClientIP(), sqlWhere);
            }
            else
            {
                string guid = Guid.NewGuid().ToString().ToUpper();
                updatesql = string.Format(@"insert into TempText(TempId,UserId,TextContent,UpdateTime,IP,TextKey) values('{0}','{1}','{2}','{3}','{4}',{5}) ",
                    guid, userid, SecurityHelper.UrlEncode(Content, Encoding.UTF8),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), HttpContext.GetClientIP(),
                    string.IsNullOrEmpty(Key) ? "null" : ("'" + Key + "'"));
            }

            bool setResult = DataAccess.Instance.PostData(updatesql);
            if (setResult)
            {
                result.code = 0;
                result.msg = "更新成功";
                result.result = "更新成功";
            }
            return result.ToString();
        }

        /// <summary>
        /// 字符串处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public string Security()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            string type = HttpContext.Request.Query["type"];
            string content = HttpContext.Request.Query["content"];
            if ("POST".Equals(HttpContext.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                type = HttpContext.Request.Form["type"];
                content = HttpContext.Request.Form["content"];
            }

            if (!type.Equals("GUID") && (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(content)))
            {
                result.msg = "处理类型或内容不能为空";
                result.result = "执行失败";
                return result.ToString();
            }

            String srcContent = String.Empty;
            switch (type)
            {
                case "MD5":
                    srcContent = SecurityHelper.MD5Encrypt(content, Encoding.UTF8);
                    break;
                case "GUID":
                    srcContent = Guid.NewGuid().ToString().ToUpper();
                    break;
                case "ToUpper":
                    srcContent = content.ToString().ToUpper();
                    break;
                case "ToLower":
                    srcContent = content.ToString().ToLower();
                    break;
                case "UrlDecode":
                    srcContent = SecurityHelper.UrlDecode(content, Encoding.UTF8);
                    break;
                case "UrlEncode":
                    srcContent = SecurityHelper.UrlEncode(content, Encoding.UTF8);
                    break;
                case "ASCIIEncode":
                    byte[] array = System.Text.Encoding.UTF8.GetBytes(content.Trim());
                    for (int i = 0; i < array.Length; i++)
                    {
                        int asciicode = (int)(array[i]);
                        if (!String.IsNullOrEmpty(srcContent)) srcContent += "-";
                        srcContent += Convert.ToString(asciicode);
                    }
                    break;
                case "ASCIIDecode":
                    if (content.ToByte() != 0) srcContent = System.Text.Encoding.UTF8.GetString(new Byte[] { content.ToByte() });
                    else srcContent = "ASCII码 格式有误，只能单个码值转单字符";
                    break;
                case "ToUnicode":
                    srcContent = SecurityHelper.ToUnicode(content.ToString().Trim());
                    break;
                case "UnicodeTo":
                    srcContent = SecurityHelper.UnicodeTo(content.ToString().Trim());
                    break;
                default: break;
            }

            result.code = 0;
            result.msg = srcContent;
            result.result = "处理成功";

            return result.ToString();
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string BaseConver()
        {
            ResponseMessage result = new ResponseMessage
            {
                code = -1,
                msg = "发生异常",
                result = "执行失败"
            };

            Int32 from = HttpContext.Request.Query["from"].ToString().ToInt();
            Int32 to = HttpContext.Request.Query["to"].ToString().ToInt();
            string content = HttpContext.Request.Query["content"];
            if (from <= 0 || to <= 0 || String.IsNullOrEmpty(content))
            {
                result.msg = "进制类型或内容不能为空";
                result.result = "执行失败";
                return result.ToString();
            }

            String srcContent = String.Empty;
            try
            {
                Int32 temp10 = Convert.ToInt32(content, from);
                srcContent = Convert.ToString(temp10, to).ToUpper();

                result.code = 0;
                result.msg = srcContent;
                result.result = "处理成功";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
                result.result = "处理失败";
            }

            return result.ToString();
        }
    }
}
