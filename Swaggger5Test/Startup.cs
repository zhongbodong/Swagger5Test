using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swaggger5Test
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region 添加Swagger5.0服务
            //注册Swagger生成器
            services.AddSwaggerGen(c=> {
                //添加Swagger文档
                c.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
                //
                c.CustomOperationIds(apiDesc =>
                {
                    return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
                });
                //接口添加注释
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "Swaggger5Test.xml");
                c.IncludeXmlComments(filePath);

                //Model添加注释
                var modelPath = Path.Combine(System.AppContext.BaseDirectory, "Swagger5Test.Models.xml");
                c.IncludeXmlComments(modelPath);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //过滤器
                app.UseSwagger(c => {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => {
                        OpenApiPaths paths = new OpenApiPaths();
                        foreach (var path in swaggerDoc.Paths)
                        {
                            if (path.Key.StartsWith("/api/Product"))//过滤Path
                                paths.Add(path.Key, path.Value);
                        }
                        swaggerDoc.Paths = paths;
                      
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                    });

                });
                app.UseSwaggerUI(c =>
                {

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");//Swagger文档路径
                    c.RoutePrefix = "";//设置为首页访问






                });
            }

            app.UseMvc();
        }
    }
}
