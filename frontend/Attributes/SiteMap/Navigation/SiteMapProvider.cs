using Microsoft.Extensions.Caching.Memory;
using WEB.APP.ApiClients;
using static WEB.APP.ApiClients.ApiClientsModels.authApiClientsModel;

namespace WEB.APP.MvcWebApp.Navigation
{
    public interface ISiteMapProvider
    {
        IList<SiteMapNode> GetSiteMaps();
        IList<SiteMapNode> BuildGroupedChildren(IList<SiteMapNode> nodes);
        // IList<SiteMapNode> GetSiteMapsAll();
        SiteMapNode FindSiteMap(string key);
    }

    public class DbSiteMapProvider : ISiteMapProvider
    {
        //private readonly SiteMapDbContext _dbContext;



        private readonly IMemoryCache _cache;
        private readonly IauthApiClients _api;


        public DbSiteMapProvider(IMemoryCache cache, IauthApiClients api)
        {
            //_dbContext = dbContext;
            _cache = cache;
            _api = api;


        }

        private IList<SiteMapNode> EnsureLoadDataFromStore()
        {
            var siteMapNodes = _cache.GetOrCreate("$SiteMap", (entry) =>
            {
                entry.Priority = CacheItemPriority.NeverRemove;


                var dataScreenAPI = _api.GetSiteMaps().Result;

                List<ModuleModel> modules_List = dataScreenAPI.Module;
                List<GetSiteScreenModel> screens_List = dataScreenAPI.Screen;
                var screens = modules_List.Join(
                            screens_List.Where(s => "WEB" == "WEB"),
                            module => module.ModuleCode,
                            screen => screen.ModuleCode,
                            (module, screen) => new ScreenDataView
                            {
                                ModuleCode = module.ModuleCode,
                                ModuleSeq = screen.ModuleSeq,
                                ModuleName_EN = module.Module_EN,
                                ModuleName_TH = module.Module_TH,
                                SubModuleCode = screen.SubModuleCode,
                                SubModuleSeq = screen.SubModuleSeq,
                                SubModuleName_EN = screen.SubModuleName_EN,
                                SubModuleName_TH = screen.SubModuleName_TH,
                                ModuleIconClass = module.IconClass,
                                Seq = module.Seq,
                                ScreenId = screen.ScreenId,
                                Name = screen.Name_EN,
                                IconClass = screen.Screen_IconClass,
                                ScreenSeq = screen.Screen_Seq,
                                MainMenuFlag = screen.Screen_MainMenuFlag,
                                PermissionFlag = screen.Screen_PermissionFlag,
                                PageTitleName_EN = screen.PageTitleName_EN,
                                PageTitleName_TH = screen.PageTitleName_TH,
                                BreadcrumbNavigation_EN = string.IsNullOrEmpty(screen.SubModuleName_EN) ? $"{module.Module_EN} /" : $"{module.Module_EN} / {screen.SubModuleName_EN} /",
                                BreadcrumbNavigation_TH = string.IsNullOrEmpty(screen.SubModuleName_TH) ? $"{module.Module_TH} /" : $"{module.Module_TH} / {screen.SubModuleName_TH} /"

                            })
                            .OrderBy(t => t.Seq).ThenBy(t => t.ScreenSeq).ThenBy(t => t.ScreenId).ToList();

                var modules = screens.Select(t => t.ModuleCode).Distinct();
                var nodes = new List<SiteMapNode>();
                foreach (var module in modules)
                {
                    var childScreens = screens.Where(t => t.ModuleCode == module);
                    var m = childScreens.First();
                    var parent = new SiteMapNode()
                    {
                        ObjectId = module,
                        ModuleCode = module,
                        //Title = m.ModuleName_EN,
                        ModuleSeq = m.ModuleSeq,
                        ModuleName_EN = m.ModuleName_EN,
                        ModuleName_TH = m.ModuleName_TH,
                        SubModuleCode = m.SubModuleCode,
                        SubModuleSeq = m.SubModuleSeq,
                        SubModuleName_EN = m.SubModuleName_EN,
                        SubModuleName_TH = m.SubModuleName_TH,
                        PageTitleName_EN = m.PageTitleName_EN,
                        PageTitleName_TH = m.PageTitleName_TH,
                        RouteName = module,
                        IconClass = m.ModuleIconClass,
                        BreadcrumbNavigation_EN = m.BreadcrumbNavigation_EN,
                        BreadcrumbNavigation_TH = m.BreadcrumbNavigation_TH,
                        Attributes = new Dictionary<string, object> { { "main-menu", m.MainMenuFlag }, { "permission-enabled", m.PermissionFlag }, { "child-screens", childScreens.Select(t => t.ScreenId).ToArray() } }
                    };
                    CreateSitMapNode(parent, screens);
                    nodes.Add(parent);
                }
                return nodes;
            });
            return siteMapNodes;
        }

        public IList<SiteMapNode> GetSiteMaps()
        {
            var data = EnsureLoadDataFromStore();
            return data;
        }

        private void CreateSitMapNode(SiteMapNode parent, IEnumerable<ScreenDataView> screens)
        {
            var ms = screens.Where(t => t.ModuleCode == parent.ModuleCode).OrderBy(t => t.ScreenSeq).ThenBy(t => t.ScreenId);
            foreach (var s in ms)
            {
                parent.ChildNodes.Add(s.ToSiteMapNode());
            }
        }

        public SiteMapNode FindSiteMap(string key)
        {
            var siteMapNodes = EnsureLoadDataFromStore();
            var findSiteMapNode = siteMapNodes.FirstOrDefault(t => t.ObjectId == key);
            if (findSiteMapNode == null)
            {
                foreach (var siteMapNode in siteMapNodes)
                {
                    FindSiteMapRecursive(siteMapNode, key, out findSiteMapNode);
                    if (findSiteMapNode != null) { break; }
                }
            }
            return findSiteMapNode;
        }

        private void FindSiteMapRecursive(SiteMapNode parent, string key, out SiteMapNode result)
        {
            result = parent.ChildNodes.FirstOrDefault(t => t.ObjectId == key);
            if (result == null)
            {
                foreach (var child in parent.ChildNodes)
                {
                    FindSiteMapRecursive(child, key, out result);
                    if (result != null) { break; }
                }
            }
        }

        public IList<SiteMapNode> BuildGroupedChildren(IList<SiteMapNode> nodes)
        {
            if (nodes == null || nodes.Count == 0) return nodes ?? new List<SiteMapNode>();

            var noKey = nodes.Where(n => string.IsNullOrWhiteSpace(n.SubModuleCode)).OrderBy(n => n.SubModuleSeq).ToList();
            var withKey = nodes.Where(n => !string.IsNullOrWhiteSpace(n.SubModuleCode)).OrderBy(n => n.SubModuleSeq).ToList();

            var groups = withKey
                .GroupBy(n => n.SubModuleCode!) // safe: filtered above

                .Select(g =>
                {
                    var first = g.First();
                    // ชื่อกรุ๊ป fallback: EN > TH > SubModuleCode > "Ungrouped"
                    var title =
                        !string.IsNullOrWhiteSpace(first.SubModuleName_EN) ? first.SubModuleName_EN :
                        !string.IsNullOrWhiteSpace(first.SubModuleName_TH) ? first.SubModuleName_TH :
                        (!string.IsNullOrWhiteSpace(g.Key) ? g.Key : "Ungrouped");

                    return new SiteMapNode
                    {
                        ObjectId = $"GROUP_{first.ModuleCode}_{g.Key}",
                        //Title = title,
                        RouteName = null,                 // group ไม่มี route
                        IconClass = first.IconClass,             // ปรับตามธีมได้
                        ModuleCode = first.ModuleCode,
                        ModuleName_EN = first.ModuleName_EN,
                        ModuleName_TH = first.ModuleName_TH,
                        SubModuleCode = g.Key,
                        SubModuleName_EN = first.SubModuleName_EN,
                        SubModuleName_TH = first.SubModuleName_TH,
                        Attributes = first.Attributes,
                        ChildNodes = g.ToList()
                    };
                })
                .OrderBy(n => n.SubModuleSeq)
                .ThenBy(n => n.ScreenSeq)
                .ToList();

            // เอาเมนูเดี่ยว (ไม่มี SubModuleCode) ไว้ก่อน แล้วตามด้วยกรุ๊ป
            return noKey.Concat(groups).ToList();
        }
    }

    public class ScreenDataView
    {
        public int ModuleSeq { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName_EN { get; set; }
        public string ModuleName_TH { get; set; }


        public int SubModuleSeq { get; set; }
        public string SubModuleCode { get; set; }
        public string SubModuleName_EN { get; set; }
        public string SubModuleName_TH { get; set; }

        public string ModuleIconClass { get; set; }
        public int Seq { get; set; }
        public string ScreenId { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        public int ScreenSeq { get; set; }
        //public string ChildScreen { get; set; }
        public bool MainMenuFlag { get; set; }
        public bool PermissionFlag { get; set; }

        public string PageTitleName_EN { get; set; }
        public string PageTitleName_TH { get; set; }

        public string BreadcrumbNavigation_EN { get; set; }
        public string BreadcrumbNavigation_TH { get; set; }






        public SiteMapNode ToSiteMapNode()
        {
            var node = new SiteMapNode()
            {
                IconClass = this.IconClass,
                ObjectId = this.ScreenId,
                RouteName = this.ScreenId,
                ModuleCode = this.ModuleCode,
                ModuleSeq = this.ModuleSeq,
                ModuleName_EN = this.ModuleName_EN,
                ModuleName_TH = this.ModuleName_TH,
                SubModuleCode = this.SubModuleCode,
                SubModuleSeq = this.SubModuleSeq,
                SubModuleName_EN = this.SubModuleName_EN,
                SubModuleName_TH = this.SubModuleName_TH,
                //Title = this.Name,
                PageTitleName_EN = this.PageTitleName_EN,
                PageTitleName_TH = this.PageTitleName_TH,
                BreadcrumbNavigation_EN = this.BreadcrumbNavigation_EN,
                BreadcrumbNavigation_TH = this.BreadcrumbNavigation_TH,
                ScreenSeq = this.Seq
            };
            if (this.MainMenuFlag) { node.Attributes.Add("main-menu", true); }
            if (this.PermissionFlag) { node.Attributes.Add("permission-enabled", true); }
            return node;
        }
    }

    [Flags]
    public enum ScreenAttributes : int
    {
        None = 0,
        MainMenu = 1,
        Permission = 2,
        All = MainMenu | Permission
    }
}
