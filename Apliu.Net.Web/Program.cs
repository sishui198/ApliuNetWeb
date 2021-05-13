using ApliuCoreWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Apliu.Logger;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Apliu.Tools.Core;

namespace Apliu.Net.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Default.Info("����Apliu.Net.Web����");
            //��ʼ�������Ŀ¼
            ApliuCoreWeb.Models.Common.RootDirectory = Apliu.Tools.Core.Web.ServerInfo.SitePath + @"\";
            //���������ļ�
            ConfigurationJson.LoadConfig();
            CreateHostBuilder(args).Build().Run();
            Log.Default.Info("�ر�Apliu.Net.Web����");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(SetHostUrl);
                });

        /// <summary>
        /// ����Kestrel Https
        /// </summary>
        /// <param name="options"></param>
        private static void SetHostUrl(KestrelServerOptions options)
        {
            var hostUrls = ConfigurationJson.HostUrl;
            foreach (var endpointKvp in hostUrls.Endpoints)
            {
                var name = endpointKvp.Key;
                var url = endpointKvp.Value;
                if (url.IsEnabled)
                {
                    var address = IPAddress.Parse(url.Address);
                    options.Listen(address, url.Port, opt =>
                    {
                        //�������֤��������https
                        if (url.Certificate != null)
                        {
                            switch (url.Certificate.Source)
                            {
                                case "pfx":
                                    if (!File.Exists(url.Certificate.Path))
                                        throw new FileNotFoundException($"δ�ҵ�֤���ļ�{url.Certificate.Path}");

                                    opt.UseHttps(url.Certificate.Path, SecurityHelper.DESDecrypt(url.Certificate.Password, ConfigurationJson.AllEncodingAESKey));
                                    break;
                                default:
                                    throw new NotSupportedException($"�ݲ�֧��{url.Certificate.Source}֤������");
                            }
                            //opt.UseConnectionLogging();
                        }
                    });
                    //options.UseSystemd();
                }
            }
        }
    }
}
