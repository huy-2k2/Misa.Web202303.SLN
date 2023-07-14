

using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using AutoMapper;
using Misa.Web202303.QLTS.API.MiddleWares;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using Misa.Web202303.QLTS.BL.Service.Department;
using Misa.Web202303.QLTS.BL.Service.FixedAssetCategory;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using Misa.Web202303.QLTS.BL.ImportService.FixedAsset;
using Misa.Web202303.QLTS.BL.ImportService.Department;
using Misa.Web202303.QLTS.BL.ImportService.FixedAssetCategory;
using Misa.Web202303.QLTS.BL.AuthService;
using Misa.Web202303.QLTS.DL.AuthRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Misa.Web202303.QLTS.BL.JwtService;
using Misa.Web202303.QLTS.BL.Service.Budget;
using Misa.Web202303.QLTS.DL.Repository.Budget;
using Misa.Web202303.QLTS.BL.Service.License;
using Misa.Web202303.QLTS.DL.Repository.License;
using Misa.Web202303.QLTS.BL.RecommendCode;
using Misa.Web202303.QLTS.DL.unitOfWork;
using Misa.Web202303.QLTS.BL.DomainService.FixedAsset;
using Misa.Web202303.QLTS.BL.DomainService.FixedAssetCategory;
using Misa.Web202303.QLTS.BL.DomainService.Department;
using Misa.Web202303.QLTS.BL.DomainService.License;
using Misa.Web202303.QLTS.BL.DomainService.BudgetDetail;
using Misa.Web202303.QLTS.BL.DomainService.LicenseDetail;
using Misa.Web202303.QLTS.DL.Repository.LicenseDetail;
using Misa.Web202303.QLTS.DL.Repository.BudgetDetail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}
));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IFixedAssetService, FixedAssetService>();
builder.Services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<IFixedAssetCategoryService, FixedAssetCategoryService>();
builder.Services.AddScoped<IFixedAssetCategoryRepository, FixedAssetCategoryRepository>();

builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

builder.Services.AddScoped<ILicenseService, LicenseService>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();

builder.Services.AddScoped<ILicenseDetailRepository, LicenseDetailRepository>();
builder.Services.AddScoped<IBudgetDetailRepository, BudgetDetailRepository>();
builder.Services.AddScoped<IFixedAssetImportService, FixedAssetImportService>();

builder.Services.AddScoped<IDepartmentImportService, DepartmentImportService>();

builder.Services.AddScoped<IFixedAssetCategoryImportService, FixedAssetCategoryImportService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IRecommendCodeService, RecommendCodeService>();

builder.Services.AddScoped<IFixedAssetDomainService, FixedAssetDomainService>();
builder.Services.AddScoped<IFixedAssetCategoryDomainService, FixedAssetCategoryDomainService>();
builder.Services.AddScoped<IDepartmentDomainService, DepartmentDomainService>();
builder.Services.AddScoped<ILicenseDomainService, LicenseDomainService>();
builder.Services.AddScoped<IBudgetDetailDomainService, BudgetDetailDomainService>();
builder.Services.AddScoped<ILicenseDetailDomainService, LicenseDetailDomainService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseCors("corspolicy");

app.Run();
