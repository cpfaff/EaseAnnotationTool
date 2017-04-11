using System.Web.Mvc;
using CAFE.Web.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using CAFE.Core.Integration;

namespace CAFE.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var annModT = typeof (AnnotationObject);
            //var str = GetMprDef(annModT);
            //Debug.Write(str);

            return View();
        }

        //**** Don't ask me what is it pleas ****
        //it's temporaly need
        private string GetMprDef(System.Type ant)
        {
            var nm1 = ant.Name;
            var nm2 = "Db" + ant.Name;

            StringBuilder b = new StringBuilder();
            b.AppendFormat("c.CreateMap<{0}, {1}>();{2}", nm1, nm2, Environment.NewLine);
            b.AppendFormat("c.CreateMap<{0}, {1}>();{2}", nm2, nm1, Environment.NewLine);

            var publProps = ant.GetProperties();
            foreach (var propertyInfo in publProps)
            {
                var pt = propertyInfo.PropertyType;
                if (propertyInfo.PropertyType.IsGenericType)
                {
                    pt = propertyInfo.PropertyType.GetGenericArguments()[0];
                }
                if(pt.FullName.StartsWith("System.")) continue;

                b.Append(GetMprDef(pt));
            }

            return b.ToString();
        }
    }
}