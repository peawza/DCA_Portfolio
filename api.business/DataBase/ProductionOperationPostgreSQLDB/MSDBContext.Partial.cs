using Microsoft.EntityFrameworkCore;

namespace BusinessSQLDB;

public partial class MSDBContext : DbContext
{
    void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // OnModelCreatingForAPI001(modelBuilder);

        OnModelCreatingForCommon(modelBuilder);
    }


}

