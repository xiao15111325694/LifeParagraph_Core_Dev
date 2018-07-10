using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Life_Web.Data;
using Life_Web.Services;
using Life_Web.Life_Server;
using Models;
using Life_Core_Repositories;
using AutoMapper;
using Hangfire;
using Life_Untiy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Life_Web.Auth;

namespace Life_Web
{
    public class Startup
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="option"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var ss = Configuration["JwtSecurityConfig:JwtSecurityKey"];
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //配置授权
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer("Bearer",
            (jwtBearerOptions) =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JwtSecurityConfig:JwtSecurityKey"])),//秘钥
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtSecurityConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtSecurityConfig:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };

                //jwtBearerOptions.Events = new JwtBearerEvents
                //{
                //    //重写OnMessageReceived
                //    OnMessageReceived = context => {
                //        var token = context.Request.Headers["token"];
                //        context.Token = token.FirstOrDefault();
                //        return Task.CompletedTask;
                //    }
                //};
            });


            services.AddMvc();

            //添加options
            services.AddOptions();
            services.Configure<JwtSecurityConfig>(Configuration.GetSection("JwtSecurityConfig"));

            // Add application services.
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Node>, Repository<Node>>();
            services.AddScoped<IRepository<Paragraph>, Repository<Paragraph>>();
            services.AddScoped<IRepository<ParagraphNode>, Repository<ParagraphNode>>();
            services.AddScoped<IRepository<UserCollection>, Repository<UserCollection>>();
            services.AddScoped<IRepository<UserMessage>, Repository<UserMessage>>();
            services.AddScoped<IRepository<HtmlPackUrlInfo>, Repository<HtmlPackUrlInfo>>();
            services.AddScoped<IRepository<GameStrategy>, Repository<GameStrategy>>();
            services.AddScoped<IRepository<GameNode>, Repository<GameNode>>(); 
            services.AddScoped<IRepository<GameHtmlInfo>, Repository<GameHtmlInfo>>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserServer, UserServer>();
            services.AddTransient<INodeServer, NodeServer>();
            services.AddTransient<IParagraphServer, ParagraphServer>();
            services.AddTransient<IParagrphNodeServer, ParagraphNodeServer>();
            services.AddTransient<IUserCollectionServer, UserCollectionServer>();
            services.AddTransient<IUserMessageServer, UserMessageServer>();
            services.AddTransient<IHtmlPack, HtmlPack>();
            services.AddTransient<IGameNodeServer, GameNodeServer>();
            services.AddTransient<IGameStrategyServer, GameStrategyServer>();
            services.AddTransient<IGameHtmlInfoServer, GameHtmlInfoServer>();

            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "LifeParagraph API"
            //    });

            //    //Determine base path for the application.  
            //    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            //    //Set the comments path for the swagger json and ui.  
            //    var xmlPath = Path.Combine(basePath, "Life_Web.xml");
            //    options.IncludeXmlComments(xmlPath);
            //});
            //添加认证Cookie信息
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                 {
                     options.LoginPath = new PathString("/login");
                     options.AccessDeniedPath = new PathString("/denied");
                 });

            //Token
            services.AddAuthorization(auth =>
            {
                    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });




            services.AddHangfire(r => r.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMemoryCache();

            services.AddAutoMapper();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();  
            app.UseAuthentication(); //注册认证中间件

            ////异常处理
            //app.UseStatusCodePages(new StatusCodePagesOptions()
            //{
            //    HandleAsync = (context) =>
            //    {
            //        if (context.HttpContext.Response.StatusCode == 401)
            //        {
            //            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(context.HttpContext.Response.Body))
            //            {
            //                sw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
            //                {
            //                    status = 401,
            //                    message = "access denied!",
            //                }));
            //            }
            //        }
            //        return System.Threading.Tasks.Task.Delay(0);
            //    }
            //});



            app.UseHangfireServer(); //启动HangFire服务
            app.UseHangfireDashboard();//启动HangFire可视化面板
                                       //RecurringJob.AddOrUpdate(() => Console.WriteLine("HangFire State Job"), Cron.Minutely());//定时（循环）任务代表可以重复性执行多次，支持CRON表达式：
                                       //BackgroundJob.Schedule(() => Console.WriteLine("HangFire State Job"), TimeSpan.FromSeconds(1))延迟（计划）任务跟队列任务相似，客户端调用时需要指定在一定时间间隔后调用：
                                       //BackgroundJob.Enqueue(() => Console.WriteLine("HangFire State Job"));//基于队列

            //BackgroundJob.Enqueue<IParagraphServer>(x => x.HtmlPackPostParagraph());

            //BackgroundJob.Enqueue<IParagraphServer>(x => x.HtmlPackByUrlToContnt());


            app.UseMiddleware<TokenProviderMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LifeParagraph API V1");
            //});
        }
    }
}
