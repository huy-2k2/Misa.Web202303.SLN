

using Misa.Web202303.SLN.BL.Service.FixedAsset;
using AutoMapper;
using Misa.Web202303.SLN.MiddleWares;
using Misa.Web202303.SLN.DL.Repository.FixedAsset;
using Misa.Web202303.SLN.BL.Service.Department;
using Misa.Web202303.SLN.BL.Service.FixedAssetCategory;
using Misa.Web202303.SLN.DL.Repository.Department;
using Misa.Web202303.SLN.DL.Repository.FixedAssetCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader();
}
));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFixedAssetService, FixedAssetService>();
builder.Services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<IFixedAssetCategoryService, FixedAssetCategoryService>();
builder.Services.AddScoped<IFixedAssetCategoryRepository, FixedAssetCategoryRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.UseCors("corspolicy");


app.Run();
