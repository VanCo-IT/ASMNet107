using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewData["Layout"] = "_LayoutAdmin";

        if (HttpContext.Session.GetString("MaNhanVien") == null)
        {
            context.Result = RedirectToAction("Login", "DangNhap");
        }

        base.OnActionExecuting(context);
    }
}