using Microsoft.EntityFrameworkCore;
using static BusinessSQLDB.Models.StoredProcedure.commonModels;

namespace BusinessSQLDB;


public partial class MSDBContext
{
    void OnModelCreatingForCommon(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MiscellaneousData>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("MiscellaneousData");
        });

        modelBuilder.Entity<sp_Common_GetMiscCombo_Result>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("sp_Common_GetMiscCombo_Result");
        });
    }




}

