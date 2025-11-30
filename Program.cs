using QLCF.DAL;
using QLCF.BUS;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  BUS 
builder.Services.AddScoped<ToppingBUS>();
builder.Services.AddScoped<LuaChonToppingBUS>();
builder.Services.AddScoped<DonHangBUS>();
builder.Services.AddScoped<HoaDonBUS>();
builder.Services.AddScoped<ThanhToanBUS>();
builder.Services.AddScoped<NguoiDungBUS>();
builder.Services.AddScoped<VaiTroBUS>();
builder.Services.AddScoped<KhuVucBUS>();
builder.Services.AddScoped<BanBUS>();
builder.Services.AddScoped<CaLamBUS>();
builder.Services.AddScoped<ChiTietDonHangBUS>();
builder.Services.AddScoped<MonToppingBUS>();
builder.Services.AddScoped<MonBUS>();
builder.Services.AddScoped<SizeMonBUS>();

// DB
string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddSingleton(new DatabaseHelper(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  ENABLE CORS 
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
