using Apliu.Standard.ORM;
using Apliu.Standard.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using DbType = Apliu.Database.DbType;
using Newtonsoft.Json;

namespace ApliuCoreConsole
{
    public class Test
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Msg { get; set; }
        public String CreateTime { get; set; }

        public Test() { }
        public Test(String id, String name) { this.Id = id; this.Name = name; }
    }

    public static class RunFuction
    {
        public static void testFun(object obj)
        {
            // AutoResetEvent
            Console.WriteLine(string.Format("{0}:第{1}个线程", DateTime.Now.ToString(), obj.ToString()));
            Thread.Sleep(10000);
        }

        /// <summary>
        /// 小Z专属知识产权 @copyright zz
        /// </summary>
        /// <param name="n">a个数</param>
        /// <param name="m">z个数</param>
        /// <param name="x">第几个</param>
        /// <returns></returns>
        private static String RunAsResult(Int32 n, Int32 m, Int32 x)
        {
            StringBuilder mmm = new StringBuilder();
            for (int i = 0; i < m; i++) mmm.Append("1");
            StringBuilder nnn = new StringBuilder();
            for (int i = 0; i < n; i++) nnn.Append("0");
            Int32 min = Convert.ToInt32(mmm.ToString(), 2);
            Int32 max = Convert.ToInt32(mmm.ToString() + nnn.ToString(), 2);
            Int32 result = min, forX = 0;
            for (int i = min; i < max; i++)
            {
                if (Regex.Matches(Convert.ToString(i, 2), @"1").Count == m)
                {
                    result = i;
                    forX++;
                    //Console.WriteLine(Convert.ToString(result, 2).Replace("0", "a").Replace("1", "z").PadLeft(n + m, 'a'));
                }
                if (forX >= x) break;
            }
            return Convert.ToString(result, 2).Replace("0", "a").Replace("1", "z").PadLeft(n + m, 'a');
        }

        public static void Run()
        {
            var q1 = SecurityHelper.DESEncrypt("", "");
            var q2 = SecurityHelper.DESEncrypt("", "");

            var d11 = Apliu.Database.DbContext.CreateDbContext(DbType.Mysql, "", "", "apliunetweb", "apliu", "");
            var a11 = d11.Query("select * from test");
            Console.WriteLine(JsonConvert.SerializeObject(a11));
            var b11 = d11.Execute("update test set name='666'");
            Console.WriteLine(JsonConvert.SerializeObject(b11));
            var c11 = d11.Query("select * from test");
            Console.WriteLine(JsonConvert.SerializeObject(c11));
            var d22 = Apliu.Database.DbContext.CreateDbContext(DbType.Oracle, "1", "1521", "orcl", "1", "1");
            var a22 = d22.Query("select * from tf_fundinfo where fund_id=1");
            Console.WriteLine(JsonConvert.SerializeObject(a22));
            var b22 = d22.Execute("update tf_fundinfo set fund_enname='666' where fund_id=1");
            Console.WriteLine(JsonConvert.SerializeObject(b22));
            var c22 = d22.Query("select * from tf_fundinfo where fund_id=1");
            Console.WriteLine(JsonConvert.SerializeObject(c22));
            //var dbContext = Apliu.Database.DbContext.CreateDbContext(DbType.OleDb, "", "", "C:\\Users\\hspcadmin\\Desktop\\test\\apliu.accdb", "", "");
            //var a11 = dbContext.Query("select * from test");
            //var b11 = dbContext.Execute("update test set name='444'");
            //var c11 = dbContext.Query("select * from test");
            return;


            SecurityHelper.GenerateRSAKeys(out string publicKey, out String privateKey);
            String enc = SecurityHelper.RSAEncrypt(publicKey, "1111111111111");
            String dec = SecurityHelper.RSADecrypt(privateKey, enc);
            return;
            Console.WriteLine(RunAsResult(7, 7, 20));
            return;
            //ThreadPool.SetMinThreads(1, 1);
            //ThreadPool.SetMaxThreads(5, 5);
            for (int i = 1; i <= 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(testFun), i.ToString());
            }
            return;
            Console.WriteLine("开始获取");
            HttpClient httpClient = new HttpClient();
            try
            {
                Stream stream = httpClient.GetStreamAsync("https://t11.baidu.com/it/u=1895047213,2045297736&amp;fm=173&amp;app=25&amp;f=JPEG?w=218&amp;h=146&amp;s=20145D93D4AB10AF710181F10300E032").Result;
                Console.WriteLine("获取完成：" + stream.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine("获取完成：" + ex.Message);
            }

            return;

            ModelClass modelClass = new ModelClass()
            {
                Id = Guid.NewGuid().ToString().ToUpper(),
                Name = "modelname",
                Count = 8
            };
            String insertSql = new WeChatMsg().GetInsertSql();
            String updateSql = modelClass.GetUpdateSql();
            String deleteSql = modelClass.GetDeleteSql();

            Console.WriteLine(insertSql);
            Console.WriteLine(updateSql);
            Console.WriteLine(deleteSql);
            return;

            List<Test> list = new List<Test>() { new Test("3", "w"), new Test("1", "q"), new Test("1", null), new Test("2", "w") };


            var query = from a in list
                        let b = a.Name
                        group a by b into c
                        where c.Key is null
                        orderby c ascending
                        select c;

            var temp02 = from a in list
                         where a.Name is null
                         orderby a.Id ascending
                         select a;


            return;

            //DatabaseType databaseType = DatabaseType.SqlServer;
            //String databaseConnection = @"Data Source=APLIUDELL\SQLEXPRESS;Database=ApliuWeb;User ID=sa;Password=sa";
            //DatabaseHelper databaseHelper = new DatabaseHelper(databaseType, databaseConnection);
            //string sql01 = "insert into test (ID,NAME) values('" + Guid.NewGuid().ToString().ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            //databaseHelper.BeginTransaction(60);
            //int p1 = databaseHelper.PostData(sql01);
            //databaseHelper.Complete();

            //DataSet ds = databaseHelper.GetData("select * from test where name like '%2018-08-13%'");
            return;
            DataTable dataTable = new DataTable
            {
                TableName = "Test"
            };
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("NAME");
            DataRow dataRow01 = dataTable.NewRow();
            dataRow01["ID"] = Guid.NewGuid().ToString().ToUpper();
            dataRow01["NAME"] = DateTimeHelper.DataTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
            dataTable.Rows.Add(dataRow01);

            DataRow dataRow02 = dataTable.NewRow();
            dataRow02["ID"] = Guid.NewGuid().ToString().ToUpper();
            dataRow02["NAME"] = DateTimeHelper.DataTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
            dataTable.Rows.Add(dataRow02);

            //databaseHelper.BeginTransaction(60);
            //int affected = databaseHelper.InsertTableAsync(dataTable, 60).Result;
            //databaseHelper.Dispose();

            //DataSet temp = databaseHelper.GetData("select * from test where name like '%2018-08-13%'");

            return;

            String ddd = HttpGet("http://apliu.xyz/api/wx");
            String dddA = HttpGetA("http://apliu.xyz/api/wx");
            Console.WriteLine("HttpWebRequest:" + ddd);
            Console.WriteLine("HttpClient:" + dddA);
        }

        public static string HttpGetA(string getUrl)
        {
            string result = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();

                var ddd = httpClient.GetAsync(getUrl).Result;
                result = ddd.Content.ReadAsStringAsync().Result;

            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="getUrl"></param>
        /// <returns></returns>
        public static string HttpGet(string getUrl)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(getUrl);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
