using System.Globalization;

namespace BusinessSQLDB.Models.StoredProcedure
{
    public class commonModels
    {
        public partial class MiscellaneousData
        {
            public string MiscTypeCode { get; set; }
            public string MiscCode { get; set; }
            public string MiscName { get; set; }
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            public int SeqNo { get; set; }
            public string Description { get; set; }
            public bool EditFlag { get; set; }
            public bool DeleteFlag { get; set; }
            public bool ActiveStatus { get; set; }
            public DateTime? InactiveDateTime { get; set; }
            public string DisplayComboText
            {
                get
                {
                    string textDisplayComboText = MiscName;
                    //var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                    //if (culture == "th" && Value1 != null)
                    //{
                    //    textDisplayComboText = Value1;
                    //}
                    return MiscCode + " : " + textDisplayComboText;
                }
            }
        }


        #region sp_Common_GetMiscCombo

        public partial class sp_Common_GetMiscCombo_Criteria
        {
            public string? MiscTypeCode { get; set; }
            public int? Status { get; set; }

        }
        public partial class sp_Common_GetMiscCombo_Result
        {
            public string? MiscCode { get; set; }
            public string? MiscName { get; set; }
            public string? Value1 { get; set; }
            public string? Value2 { get; set; }
            public string? DisplayName { get; set; }
            public string DisplayMiscName
            {
                get
                {
                    if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en" && !string.IsNullOrWhiteSpace(MiscName))
                    {

                        return MiscName;
                    }
                    else if (string.IsNullOrWhiteSpace(Value1))
                    {
                        return MiscName;
                    }
                    else
                    {

                        return Value1;
                    }

                }
            }


        }

        #endregion
    }
}
