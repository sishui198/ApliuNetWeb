using Apliu.Logger;
using Apliu.Tools.Core;
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
            services.AddDistributedMemoryCache();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();
            //services.AddSingleton<Models.WeChat.WeChatHub>();//自定义缓存
            services.AddSignalR();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                //options.Cookie.HttpOnly = true;
            });

            Log.Default.Info("Services Configure 配置完成");
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
                Log.Default.Info("开启请求重定向到HTTPS的服务");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ApliuCoreWeb.Controllers.WeChat.WeChatHub>("/weChatHub");
            });

            Log.Default.Info("App Configure 配置完成");
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
                Log.Default.Info("开始执行自定义初始化事件");
                //启动access_token管理任务
                WxTokenManager.TokenTaskStart();
                //创建自定义菜单
                //Models.WeChat.WxDefaultMenu.CreateMenus();
                //初始化数据库
                var sqls = File.ReadAllText("config/mysqlscript.sql").Split(";", StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine("开始初始化数据库结构");
                foreach (var s in sqls)
                {
                    Console.Write("*");
                    if (!DataAccess.Instance.PostData(s))
                    {
                        Console.WriteLine("-------------------------初始化数据库失败-----------------------------");
                        Console.WriteLine(s);
                        Console.WriteLine("----------------------------------------------------------------------");
                    }
                }
                Console.WriteLine("");
                Log.Default.Info("自定义初始化事件执行完成");
            }
            catch (System.Exception ex)
            {
                Log.Default.Error("自定义初始化事件执行失败，详情：" + ex.Message, ex);
            }
        }
    }
}
