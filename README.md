# Swagger5接口文档使用
## 1.添加Nuget包：Swashbuckle.AspNetCore
## 2.注册Swagger生成器
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
### 2.1接口/Model添加注释
                //接口添加注释
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "Swaggger5Test.xml");
                c.IncludeXmlComments(filePath);

                //Model添加注释
                var modelPath = Path.Combine(System.AppContext.BaseDirectory, "Swagger5Test.Models.xml");
                c.IncludeXmlComments(modelPath);
    
    
 ## 3. 注册Swagger服务
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
### 3.1 过滤Path 
                       OpenApiPaths paths = new OpenApiPaths();
                        foreach (var path in swaggerDoc.Paths)
                        {
                            if (path.Key.StartsWith("/api/Product"))//过滤Path
                                paths.Add(path.Key, path.Value);
                        }
                        swaggerDoc.Paths = paths;
  ### 4 注册UseSwaggerUI 
              //  app.UseSwaggerUI(c =>
                 {

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");//Swagger文档路径
                    c.RoutePrefix = "";//设置为首页访问
                });
       

