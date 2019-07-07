using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Ras.Plugin.Misc.Fortnox.Components
{
    [ViewComponent(Name = "Fortnox")]
    public class MiscFortnoxViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Misc.Fortnox/Views/FortnoxInfo.cshtml");
        }
    }
}
