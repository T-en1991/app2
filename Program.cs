using Microsoft.EntityFrameworkCore;
using app2; // 引入AppDbContext的命名空间

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 统一模型校验失败(400)返回为 ApiResponse<object>
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(kvp => kvp.Value!.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value!.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var msg = errors.Count > 0 ? string.Join("; ", errors) : "参数验证失败";
        var response = app2.Dtos.Common.ApiResponse<object>.Fail(msg, 400);
        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
    };
});

// 配置数据库连接
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)) // 根据你的MySQL版本调整
    ));

// 注册仓储与服务层
builder.Services.AddScoped<app2.Repositories.IUserRepository, app2.Repositories.UserRepository>();
builder.Services.AddScoped<app2.Services.IUserService, app2.Services.UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 全局异常处理中间件——尽量靠前放置
app.UseMiddleware<app2.Middlewares.ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();