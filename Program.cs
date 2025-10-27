using QLCF.DAL;
using QLCF.BUS;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ToppingBUS>();
builder.Services.AddScoped<LuaChonToppingBUS>();
builder.Services.AddScoped<ToppingBUS>();
builder.Services.AddScoped<DonHangBUS>();
builder.Services.AddScoped<HoaDonBUS>();
builder.Services.AddSingleton<ThanhToanBUS>();
builder.Services.AddSingleton<NguoiDungBUS>();
builder.Services.AddSingleton<VaiTroBUS>();
builder.Services.AddSingleton<KhuVucBUS>();
builder.Services.AddSingleton<BanBUS>();
builder.Services.AddSingleton<CaLamBUS>();
builder.Services.AddSingleton<ChiTietDonHangBUS>();
builder.Services.AddSingleton<MonToppingBUS>();



string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddSingleton(new DatabaseHelper(connectionString));

builder.Services.AddScoped<MonBUS>();
builder.Services.AddScoped<SizeMonBUS>();

var app = builder.Build();

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
