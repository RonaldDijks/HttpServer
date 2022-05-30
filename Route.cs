using HttpServer.Core.Routing;
using HttpServer.Core.Util;

class IndexController : Route
{
    public IndexController()
    {
    }

    [Get("/")]
    public Result Index()
    {
        return Result.Ok("<h1>Hello, world!</h1>");
    }

    [Get("/hello")]
    public Result Hello()
    {
        return Result.Ok("<h1>This is another page</h1>");
    }

    [Get("/hello/{name}")]
    public Result Hello2(Route.Context context)
    {
        string name = context.Parameters["name"];
        return Result.Ok($"<h1>Hello, {name.Capitalize()}!</h1>");
    }
}
