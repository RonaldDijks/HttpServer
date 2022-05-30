using System.Text;

namespace HttpServer.Core.Http;

class ResponseSerializer
{
    public static string Serialize(Response response)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"HTTP/1.1 {response.StatusCode} OK");
        sb.AppendLine($"Content-Length: {response.Body.Length}");
        sb.AppendLine($"Content-Type: {response.ContentType}\n");
        sb.AppendLine($"{response.Body}\n");

        return sb.ToString();
    }
}