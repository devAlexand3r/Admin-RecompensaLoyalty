﻿using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new OutputCacheAttribute { VaryByParam="*", Duration=0, NoStore = true });
        }
    }
}
