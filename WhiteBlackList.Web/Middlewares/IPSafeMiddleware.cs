using Microsoft.Extensions.Options;
using System.Net;

namespace WhiteBlackList.Web.Middlewares;

public class IPSafeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPList _ipList;

    public IPSafeMiddleware(IOptions<IPList> ipList, RequestDelegate next)
    {
        _next = next;
        _ipList = ipList.Value;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var reqIpAddress = httpContext.Connection.RemoteIpAddress;

        var isWhiteList = _ipList.WhiteList.Where(x=>IPAddress.Parse(x).Equals(reqIpAddress)).Any();

        if (!isWhiteList)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
        await _next(httpContext); //gelen isteği bir sonraki middleware'e aktarma
    }
}
