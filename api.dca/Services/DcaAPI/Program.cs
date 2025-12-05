using BusinessAPI;
using DCAPostgreSQLDB;
using Microsoft.EntityFrameworkCore;
using Utils;

var builder = WebApplication.CreateBuilder(args);

StartupAPIMicoService.StartupCreateBuilder(builder);

/* --- DB Conneted Config--- */

var connectionString =
    builder.Configuration.GetConnectionString("DBConnection");


builder.Services.AddDbContext<DcaDbContext>(options =>
                    options.UseNpgsql(connectionString));



/* --- Add Repository & Service ---*/
StartupService.InitialService(builder.Services);


//Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


/* --- Add SetupHttpClient ---*/
SetupHttpClient.InitialService(builder);


var app = builder.Build();
StartupAPIMicoService.StartupCreateApplication(builder, app);



app.Run();
