using System.Web;
using System.Web.Mvc;

namespace OnlineSalesV5.eCommerce
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}