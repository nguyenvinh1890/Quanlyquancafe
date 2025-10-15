var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DatabaseHelper as a service using connection string from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddScoped<QLCF.DAL.DatabaseHelper>(provider => new QLCF.DAL.DatabaseHelper(connectionString));

// Add MonBUS as a service
builder.Services.AddScoped<QLCF.BUS.MonBUS>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
