using System.Web.Mvc;

namespace $rootnamespace$.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/blog");
        }
    }
}