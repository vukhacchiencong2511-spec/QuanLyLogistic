using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.BUS;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// DAL (Data Access Layer)
builder.Services.AddScoped<KhachHangDAL>();
builder.Services.AddScoped<VaiTroDAL>();
builder.Services.AddScoped<NguoiDungDAL>();
builder.Services.AddScoped<DiaChiDAL>();
builder.Services.AddScoped<DonVanChuyenDAL>();
builder.Services.AddScoped<VanDonDAL>();
builder.Services.AddScoped<TuyenDuongDAL>();
builder.Services.AddScoped<DiemDungDAL>();
builder.Services.AddScoped<SuKienTrangThaiDAL>();
builder.Services.AddScoped<GiaoDichCODDAL>();
builder.Services.AddScoped<ChungTuDAL>();


// BUS (Business Layer)
builder.Services.AddScoped<KhachHangBUS>();
builder.Services.AddScoped<VaiTroBUS>();
builder.Services.AddScoped<NguoiDungBUS>();
builder.Services.AddScoped<DiaChiBUS>();
builder.Services.AddScoped<DonVanChuyenBUS>();
builder.Services.AddScoped<VanDonBUS>();
builder.Services.AddScoped<TuyenDuongBUS>();
builder.Services.AddScoped<DiemDungBUS>();
builder.Services.AddScoped<SuKienTrangThaiBUS>();
builder.Services.AddScoped<GiaoDichCODBUS>();
builder.Services.AddScoped<ChungTuBUS>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
