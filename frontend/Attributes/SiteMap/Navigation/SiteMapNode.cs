using System.Globalization;

namespace WEB.APP.MvcWebApp.Navigation
{
    public class SiteMapNode
    {
        public string ObjectId { get; set; }
        public string Title =>
            GetLocalized(PageTitleName_TH, PageTitleName_EN);
        public string RouteName { get; set; }
        public string IconClass { get; set; }
        public string ModuleCode { get; set; }
        public int ModuleSeq { get; set; }
        public string ModuleName
        {
            get
            {
                if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en" && !string.IsNullOrWhiteSpace(ModuleName_EN))
                {
                    return ModuleName_EN;
                }
                else if (string.IsNullOrWhiteSpace(ModuleName_TH))
                {
                    return ModuleName_TH;   // <- มาตรงนี้เฉพาะตอน TH ว่าง แล้วเราก็ return ค่า "ว่าง"
                }
                else
                {
                    return ModuleName_EN;   // <- กรณี TH มีค่า กลับไปใช้ EN อีก
                }

            }
        }
        public string ModuleName_EN { get; set; }
        public string ModuleName_TH { get; set; }
        public string SubModuleCode { get; set; }
        public int SubModuleSeq { get; set; }
        public string SubModuleName_EN { get; set; }
        public string SubModuleName_TH { get; set; }
        public string PageTitleName_EN { get; set; }
        public string PageTitleName_TH { get; set; }
        public string BreadcrumbNavigation_EN { get; set; }
        public string BreadcrumbNavigation_TH { get; set; }
        public int ScreenSeq { get; set; }
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        public IList<SiteMapNode> ChildNodes { get; set; } = new List<SiteMapNode>();
        public string SubModuleName =>
        GetLocalized(SubModuleName_TH, SubModuleName_EN);

        public string PageTitleName =>
            GetLocalized(PageTitleName_TH, PageTitleName_EN);

        public string BreadcrumbNavigation =>
            GetLocalized(BreadcrumbNavigation_TH, BreadcrumbNavigation_EN);

        // ----- Common helper -----
        private static string GetLocalized(string th, string en)
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;


            if (lang == "en" && !string.IsNullOrWhiteSpace(en))
                return en;


            if (lang != "en" && !string.IsNullOrWhiteSpace(th))
                return th;


            if (!string.IsNullOrWhiteSpace(th))
                return th;

            if (!string.IsNullOrWhiteSpace(en))
                return en;

            return string.Empty;
        }

    }
}
