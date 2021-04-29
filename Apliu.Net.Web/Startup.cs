using Apliu.Tools;
using ApliuCoreWeb.Models;
using ApliuCoreWeb.Models.WeChat;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Apliu.Net.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //ע��HttpContext
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //ע�Ỻ�����
            services.AddMemoryCache();
            //services.AddSingleton<Models.WeChat.WeChatHub>();//�Զ��建��

            //ע��SignalR
            services.AddSignalR();

            services.AddDistributedMemoryCache();
            //ע��Session
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                options.Cookie.HttpOnly = true;
            });

            Apliu.Tools.Logger.WriteLogWeb("Services Configure �������");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //����Https�ض���
            if (ConfigurationJson.IsUseHttps)
            {
                app.UseHttpsRedirection();
                Apliu.Tools.Logger.WriteLogWeb("���������ض���HTTPS�ķ���");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();

            //����Session
            app.UseSession();

            //https://docs.microsoft.com/zh-cn/aspnet/core/signalr/hubcontext?view=aspnetcore-2.1
            //app.Use(next => (context) =>
            //{
            //    var hubContext = (Microsoft.AspNetCore.SignalR.IHubContext<WeChatHub>)context
            //                        .RequestServices
            //                        .GetServices<Microsoft.AspNetCore.SignalR.IHubContext<WeChatHub>>();
            //    return null;
            //});

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<Controllers.WeChat.WeChatHub>("/weChatHub");
            //});

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            Apliu.Tools.Logger.WriteLogWeb("App Configure �������");

            //�����Զ����ʼ���¼�
            UserDefinedStartup();
        }

        /// <summary>
        /// �Զ����ʼ���¼�
        /// </summary>
        public void UserDefinedStartup()
        {
            try
            {
                Apliu.Tools.Logger.WriteLogWeb("��ʼִ���Զ����ʼ���¼�");

                //���������ļ�
                ConfigurationJson.LoadConfig();

                //��ʼ�������Ŀ¼
                ApliuCoreWeb.Models.Common.RootDirectory = Apliu.Tools.Web.ServerInfo.SitePath + @"\";

                //����access_token��������
                WxTokenManager.TokenTaskStart();

                //�����Զ���˵�
                //Models.WeChat.WxDefaultMenu.CreateMenus();

                //��ʼ����־����
                log4net.Repository.ILoggerRepository loggerRepository = LogManager.CreateRepository("NETCoreRepositoryObject");
                log4net.Config.XmlConfigurator.Configure(loggerRepository, new FileInfo("Config/log4net.config".ToLinuxOrWinPath()));
                log4net.ILog log4Net = LogManager.GetLogger(loggerRepository.Name, typeof(Object));
                ApliuCoreWeb.Models.Common.Log4Net = log4Net;

                //��ʼ�����ݿ�
                var sqls = File.ReadAllText("Config/MysqlDatabaseInitScript.txt").Split(";", StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in sqls)
                {
                    if (!DataAccess.Instance.PostData(s))
                    {
                        Console.WriteLine("-------------------------��ʼ�����ݿ�ʧ��-----------------------------");
                        Console.WriteLine(s);
                        Console.WriteLine("----------------------------------------------------------------------");
                    }
                }

                Apliu.Tools.Logger.WriteLogWeb("�Զ����ʼ���¼�ִ�����");
            }
            catch (System.Exception ex)
            {
                Apliu.Tools.Logger.WriteLogWeb("�Զ����ʼ���¼�ִ��ʧ�ܣ����飺" + ex.Message);
            }
        }
    }
}
