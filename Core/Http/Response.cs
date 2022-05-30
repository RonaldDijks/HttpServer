namespace HttpServer.Core.Http;

class Response
{
    public int StatusCode { get; set; } = 404;
    public string ContentType { get; set; } = "text/html; charset=UTF-8";
    public string Body { get; set; } = "<h1>Not found</h1>";
}