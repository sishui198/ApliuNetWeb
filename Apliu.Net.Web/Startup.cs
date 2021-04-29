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

            //注册HttpContext
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //注册缓存服务
            services.AddMemoryCache();
            //services.AddSingleton<Models.WeChat.WeChatHub>();//自定义缓存

            //注册SignalR
            services.AddSignalR();

            services.AddDistributedMemoryCache();
            //注册Session
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                options.Cookie.HttpOnly = true;
            });

            Apliu.Tools.Logger.WriteLogWeb("Services Configure 配置完成");
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

            //开启Https重定向
            if (ConfigurationJson.IsUseHttps)
            {
                app.UseHttpsRedirection();
                Apliu.Tools.Logger.WriteLogWeb("开启请求重定向到HTTPS的服务");
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

            //开启Session
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

            Apliu.Tools.Logger.WriteLogWeb("App Configure 配置完成");

            //启动自定义初始化事件
            UserDefinedStartup();
        }

        /// <summary>
        /// 自定义初始化事件
        /// </summary>
        public void UserDefinedStartup()
        {
            try
            {
                Apliu.Tools.Logger.WriteLogWeb("开始执行自定义初始化事件");

                //加载配置文件
                ConfigurationJson.LoadConfig();

                //初始化程序跟目录
                ApliuCoreWeb.Models.Common.RootDirectory = Apliu.Tools.Web.ServerInfo.SitePath + @"\";

                //启动access_token管理任务
                WxTokenManager.TokenTaskStart();

                //创建自定义菜单
                //Models.WeChat.WxDefaultMenu.CreateMenus();

                //初始化日志配置
                log4net.Repository.ILoggerRepository loggerRepository = LogManager.CreateRepository("NETCoreRepositoryObject");
                log4net.Config.XmlConfigurator.Configure(loggerRepository, new FileInfo("Config/log4net.config".ToLinuxOrWinPath()));
                log4net.ILog log4Net = LogManager.GetLogger(loggerRepository.Name, typeof(Object));
                ApliuCoreWeb.Models.Common.Log4Net = log4Net;

                //初始化数据库
                var sqls = File.ReadAllText("Config/MysqlDatabaseInitScript.txt").Split(";", StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in sqls)
                {
                    if (!DataAccess.Instance.PostData(s))
                    {
                        Console.WriteLine("-------------------------初始化数据库失败-----------------------------");
                        Console.WriteLine(s);
                        Console.WriteLine("----------------------------------------------------------------------");
                    }
                }

                Apliu.Tools.Logger.WriteLogWeb("自定义初始化事件执行完成");
            }
            catch (System.Exception ex)
            {
                Apliu.Tools.Logger.WriteLogWeb("自定义初始化事件执行失败，详情：" + ex.Message);
            }
        }
    }
}
