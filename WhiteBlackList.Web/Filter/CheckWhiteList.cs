using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;
using WhiteBlackList.Web.Middlewares;

namespace WhiteBlackList.Web.Filter;

public class CheckWhiteList : ActionFilterAttribute
{
    private readonly IPList _ipList;

    public CheckWhiteList(IOptions<IPList> ipList) 
    {
        _ipList = ipList.Value;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var reqIpAddress = context.HttpContext.Connection.RemoteIpAddress;

        var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(reqIpAddress)).Any();

        if (!isWhiteList)
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            return;
        }

        base.OnActionExecuting(context);
    }
}
