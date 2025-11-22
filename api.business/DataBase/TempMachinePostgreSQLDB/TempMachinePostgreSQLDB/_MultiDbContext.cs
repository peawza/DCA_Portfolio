using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TempMachinePostgreSQLDB;

public class TempMachineDbContextEx : TempMachineDbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IConfiguration _config;
    public TempMachineDbContextEx(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        : base(new DbContextOptions<TempMachineDbContext>())
    {
        _httpContextAccessor = httpContextAccessor;
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connStr = ConnectionHelper.GetConnectionString(_httpContextAccessor);
            if (connStr != null)
            {
                optionsBuilder.UseNpgsql(connStr + "Database=MES-Temp-Machine");
            }
            else
            {
                var fallbackConnStr = _config.GetConnectionString("TempMachinePostgreSQLDBConnection");
                optionsBuilder.UseNpgsql(fallbackConnStr);
            }
        }
    }
}
