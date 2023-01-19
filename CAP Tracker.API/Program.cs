using CAP_Tracker.Library;
using CAP_Tracker.Services;

var builder = WebApplication.CreateBuilder(args);

var cadetTrackerOptions = builder.Configuration.GetSection(CadetTrackerOptions.Options);

// Add services to the container.
var data = DataService.LoadFile(cadetTrackerOptions.GetValue<string>("Path") ?? 
    throw new ArgumentNullException("Missing Cadet Tracker Path value."));

builder.Services.AddSingleton(new TrackerService(data));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
