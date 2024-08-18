using Demo.DotnetCore.FeatureManagement.Filters;
using Demo.DotnetCore.FeatureManagement.Repositories;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IWhitelistRepository, WhitelistRepository>();

// 環境準備：Microsoft.FeatureManagement
// 加入 Feature Flag 功能，預設會抓 appsettings.json 中的 "FeatureManagement" 節點
builder.Services
    .AddFeatureManagement()
    .AddFeatureFilter<RandomFilter>() // 加入自訂的 Feature Filter. see: Filters/RandomFilter.cs
    .AddFeatureFilter<WhitelistFilter>();

// 如果想要自訂 Feature Flag 的設定檔位置，可以這樣寫
//builder.Services.AddFeatureManagement(builder.Configuration.GetSection("CustomFeatureManagement"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();