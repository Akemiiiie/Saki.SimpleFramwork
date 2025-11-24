using Autofac;
using Autofac.Extensions.DependencyInjection;
using Examination.Filter;
using Examination.Middleware;
using Examination.Startups;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Panda.DynamicWebApi;
using Saki.BaseTemplate.ConfigerOptions;
using Saki.Framework.AppBase;
using Saki.Framework.AppBase.ConfigerOptions;
using Saki.Framework.AppBase.Enums;
using Saki.Framework.Mapster;
using SqlSugar.Extensions;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMapster();
// 注册额外的映射配置
MapsterConfig.Register();
// 获取数据库连接配置
builder.Configuration.GetSection("BaseDBConfig").Get<BaseDbConfig>();
builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
//注册SqlSugar
builder.Services.AddSqlSugar();
// Autofac自动注册
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => { builder.RegisterModule<AutofacRegisterModule>(); });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var endpoint = context.HttpContext.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
            // 只有非匿名接口才从 Cookie 读取 Token
            if (!allowAnonymous)
            {
                var accessToken = context.Request.Cookies[builder.Configuration["JwtSettings:CookieName"]];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // 默认会自动写 WWW-Authenticate 头，还会终止响应，这里把它关掉
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var res = new ApiResponse<object> { Code = ErrorEnum.UnauthorizedAccess.ObjToInt(), Message = "未授权访问，请重新登录" };
            // 关键：拿到全局 MVC 的 JSON 配置
            var jsonOptions = context.HttpContext.RequestServices
                .GetRequiredService<IOptions<JsonOptions>>().Value;
            var json = JsonSerializer.Serialize(res, jsonOptions.JsonSerializerOptions);
            return context.Response.WriteAsync(json);
        }
    };
});

// 添加MVC中间件,防止接口无法被动态访问
builder.Services.AddRazorPages();
builder.Services.AddMvc();
// 动态api
builder.Services.AddDynamicWebApi();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName.Replace("+", "."));
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "lt_Project",
        Version = "v1",
        Description = "这是一个简单的.netCore mvc项目模板 POWER BY LiTuo",
        Contact = new OpenApiContact { Name = "lt'CoreTemplate", Email = "Null" }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
    options.DocInclusionPredicate((name, api) => api.HttpMethod != null);
    // 选择为XML注释提供支持
    var basePath = AppContext.BaseDirectory;
    var xmlFiles = Directory.GetFiles(basePath, "*.xml", SearchOption.TopDirectoryOnly);
    foreach (var xml in xmlFiles)
    {
        options.IncludeXmlComments(xml, includeControllerXmlComments: true);
    }
});

//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
// 添加性能分析中间件
builder.Services.AddMiniProfiler(options => options.RouteBasePath = "/profiler")
    // AddEntityFramework需要配置EntityFrameworkCore的SQL
    .AddEntityFramework();

/// 注册全局API响应过滤器
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ApiResponseFilter>(); // 注册过滤器
});

var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
// 添加跨域策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", cors =>
    {
        cors.WithOrigins(allowedOrigins)      // 配置文件里读取的多个域名
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()             // 必须加
        .WithExposedHeaders("Content-Disposition"); // 添加一个描述
    });
});

var app = builder.Build();

app.UseRouting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.IndexStream = () => Assembly.GetExecutingAssembly().GetManifestResourceStream("Examination.index.html");
    });
}
app.UseCors("AllowAll");
app.UseMiddleware<GlobalExceptionMiddleware>(); // 注册全局异常
app.UseMiddleware<JwtSlidingRefreshMiddleware>(); // 注册JWT滑动刷新中间件
app.UseMiddleware<RequestLogMiddleware>(); // 注册请求日志
app.UseHttpsRedirection();
app.UseAuthorization();
// 使用性能分析中间件
app.UseMiniProfiler();
app.MapControllers();
app.InitDefaultDb();
app.Run();
