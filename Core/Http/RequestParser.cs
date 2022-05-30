namespace HttpServer.Core.Http;

class RequestParser
{
    public static Request? Parse(string input)
    {
        var lines = input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var statusLine = lines.First();

        var statusLineParts = statusLine.Split(" ");

        // Console.WriteLine(statusLineParts[0]);
        var method = ParseHttpMethod(statusLineParts[0]);
        var path = statusLineParts[1];
        var headerLines = lines.Skip(1);

        var headers = new Dictionary<string, string>();
        foreach (var header in headerLines)
        {
            if (String.IsNullOrWhiteSpace(header))
                continue;

            var headerParts = header.Split(":");
            var key = headerParts[0];
            var value = headerParts[1];
            headers.Add(key, value);
        }

        return new Request(method, path, "1.1", headers);
    }

    private static HttpMethod ParseHttpMethod(string method)
    {
        switch (method)
        {
            case "GET": return HttpMethod.Get;
            case "POST": return HttpMethod.Post;
            default:
                throw new ArgumentException($"{method} is not a valid HTTP method.", nameof(method));
        }
    }
}
