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

namespace Apliu.Net.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Apliu.Tools.Logger.WriteLogWeb("����Apliu Core Web����");
            CreateHostBuilder(args).Build().Run();
            Apliu.Tools.Logger.WriteLogWeb("�ر�Apliu Core Web����");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// ����Kestrel
        /// </summary>
        /// <param name="options"></param>
        private static void SetHostUrl(Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions options)
        {
            //����Host�෴���л�appsettings.json��ָ���ڵ�
            var hostUrl = ConfigurationJson.HostUrl;
            foreach (var endpointKvp in hostUrl.Endpoints)
            {
                var endpointName = endpointKvp.Key;
                var endpoint = endpointKvp.Value;//��ȡappsettings.json�����������Ϣ
                if (!endpoint.IsEnabled)
                {
                    continue;
                }

                var address = System.Net.IPAddress.Parse(endpoint.Address);
                options.Listen(address, endpoint.Port, opt =>
                {
                    if (endpoint.Certificate != null)//֤�鲻Ϊ��ʹ��UserHttps
                    {
                        if (endpoint.Certificate.Source != "File" || File.Exists(endpoint.Certificate.Path))
                        {
                            switch (endpoint.Certificate.Source)
                            {
                                case "File":
                                    opt.UseHttps(endpoint.Certificate.Path, endpoint.Certificate.Password);
                                    break;
                                default:
                                    throw new NotImplementedException($"�ļ� {endpoint.Certificate.Source}��û��ʵ��");
                            }
                            //opt.UseConnectionLogging();
                        }
                    }
                });

                options.UseSystemd();
            }
        }

    }
}
