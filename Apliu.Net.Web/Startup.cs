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
            //services.AddSingleton<Models.WeChat.WeChatHub>();//�Զ��建��
            services.AddSignalR();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                //options.Cookie.HttpOnly = true;
            });

            Log.Default.Info("Services Configure �������");
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
                Log.Default.Info("���������ض���HTTPS�ķ���");
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

            Log.Default.Info("App Configure �������");
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
                Log.Default.Info("��ʼִ���Զ����ʼ���¼�");
                //����access_token��������
                WxTokenManager.TokenTaskStart();
                //�����Զ���˵�
                //Models.WeChat.WxDefaultMenu.CreateMenus();
                //��ʼ�����ݿ�
                var sqls = File.ReadAllText("config/mysqlscript.sql").Split(";", StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine("��ʼ��ʼ�����ݿ�ṹ");
                foreach (var s in sqls)
                {
                    Console.Write("*");
                    if (!DataAccess.Instance.PostData(s))
                    {
                        Console.WriteLine("-------------------------��ʼ�����ݿ�ʧ��-----------------------------");
                        Console.WriteLine(s);
                        Console.WriteLine("----------------------------------------------------------------------");
                    }
                }
                Console.WriteLine("");
                Log.Default.Info("�Զ����ʼ���¼�ִ�����");
            }
            catch (System.Exception ex)
            {
                Log.Default.Error("�Զ����ʼ���¼�ִ��ʧ�ܣ����飺" + ex.Message, ex);
            }
        }
    }
}
