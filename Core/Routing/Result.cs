namespace HttpServer.Core.Routing;

class Result
{
    public Result(int statusCode, string body)
    {
        StatusCode = statusCode;
        Body = body;
    }

    public int StatusCode { get; set; }
    public string Body { get; set; }

    public static Result NotFound(string body = "<h1>Not Found</h1>")
    {
        return new Result(404, body);
    }

    public static Result Ok(string body)
    {
        return new Result(200, body);
    }
}