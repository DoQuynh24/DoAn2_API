using BLL;
using DAL;
using DAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// Add services to the container.
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();
builder.Services.AddTransient<IUserBusiness, UserBusiness>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDanhMucSanPhamRepository, DanhMucSanPhamRepository>();
builder.Services.AddTransient<IDanhMucSanPhamBusiness, DanhMucSanPhamBusiness>();
builder.Services.AddTransient<ISizeRepository, SizeRepository>();
builder.Services.AddTransient<ISizeBusiness, SizeBusiness>();
builder.Services.AddTransient<INhaCungCapRepository, NhaCungCapRepository>();
builder.Services.AddTransient<INhaCungCapBusiness, NhaCungCapBusiness>();
builder.Services.AddTransient<ITuiXachRepository, TuiXachRepository>();
builder.Services.AddTransient<ITuiXachBusiness, TuiXachBusiness>();
builder.Services.AddTransient<IMauSacRepository, MauSacRepository>();
builder.Services.AddTransient<IMauSacBusiness, MauSacBusiness>();
builder.Services.AddTransient<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddTransient<IHoaDonBusiness, HoaDonBusiness>();
builder.Services.AddTransient<IDonHangNhapRepository, DonHangNhapRepository>();
builder.Services.AddTransient<IDonHangNhapBusiness, DonHangNhapBusiness>();

//builder.Services.AddTransient<IChiTietHoaDonRepository, ChiTietHoaDonRepository>();
//builder.Services.AddTransient<IChiTietHoaDonBusiness, ChiTietHoaDonBusiness>();
//builder.Services.AddTransient<IBinhLuanBusiness, BinhLuanBusiness>();
//builder.Services.AddTransient<IBinhLuanRepository, BinhLuanRepository>();
//builder.Services.AddTransient<ITinTucBusiness, TinTucBusiness>();
//builder.Services.AddTransient<ITinTucRepository, TinTucRepository>();

// configure strongly typed settings objects
IConfiguration configuration = builder.Configuration;
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
builder.Logging.AddConsole();  // Thêm log vào console

// configure jwt authentication
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
// Add services to the container.

builder.Services.AddControllers();
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
app.UseCors(options =>
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();