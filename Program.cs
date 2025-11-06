using QLCF.DAL;
using QLCF.BUS;

var builder = WebApplication.CreateBuilder(args);

// Add CORS - Cho phép Frontend kết nối
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                // Live Server (VS Code)
                "http://localhost:5500",
                "http://127.0.0.1:5500",
                // Một số máy sẽ dùng port 5501
                "http://localhost:5501",
                "http://127.0.0.1:5501",
                // Dev servers phổ biến
                "http://localhost:3000",
                "http://localhost:8080"
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Cho phép gửi credentials (cookie, token)
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Cho phép nhận cả camelCase từ frontend và tự động map sang PascalCase của C# model
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
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
