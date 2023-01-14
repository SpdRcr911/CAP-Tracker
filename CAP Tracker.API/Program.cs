using CAP_Tracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var data = DataService.LoadFile(@"C:\Users\rafae\Downloads\Cadet_Full_Track_Report-1_11_2023.xlsx");

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
