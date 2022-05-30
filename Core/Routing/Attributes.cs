namespace HttpServer.Core.Routing;

public abstract class RoutePathAttribute : System.Attribute
{
    protected RoutePathAttribute(HttpMethod httpMethod, string path)
    {
        HttpMethod = httpMethod;
        Path = RoutePath.Parse(path);
    }

    public RoutePath Path { get; set; }
    public HttpMethod HttpMethod { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Method)]
class GetAttribute : RoutePathAttribute
{
    public GetAttribute(string path) : base(HttpMethod.Get, path) { }
}

[System.AttributeUsage(System.AttributeTargets.Method)]
class PostAttribute : RoutePathAttribute
{
    public PostAttribute(string path) : base(HttpMethod.Post, path) { }
}
