namespace HttpServer.Core.Http;

class Request
{
    public Request(HttpMethod httpMethod, string path, string httpVersion, Dictionary<string, string> headers)
    {
        HttpMethod = httpMethod;
        Path = path;
        HttpVersion = httpVersion;
        Headers = headers;
    }

    public HttpMethod HttpMethod { get; set; }
    public string Path { get; set; }
    public string HttpVersion { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public Dictionary<string, string> RouteParameters { get; set; } = new Dictionary<string, string>();
}