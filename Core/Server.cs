using System.Net.Sockets;
using System.Text;
using HttpServer.Core.Http;
using HttpServer.Core.Routing;

namespace HttpServer.Core;

class Server
{
    public Server(Routes router, TcpListener listener)
    {
        Router = router;
        Listener = listener;
    }

    public Routes Router { get; set; }
    public TcpListener Listener { get; set; }

    public void Accept()
    {
        TcpClient? client = null;

        try
        {
            client = Listener.AcceptTcpClient();
            var startTime = DateTime.Now;
            var buffer = new byte[1024];
            var stream = client.GetStream();
            var length = stream.Read(buffer, 0, buffer.Length);
            var message = Encoding.UTF8.GetString(buffer, 0, length);
            var result = new Response();
            var request = RequestParser.Parse(message);

            if (request == null)
            {
                result.StatusCode = 500;
            }
            else
            {
                Router.Route(request, result);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now.ToLongTimeString()}]");
                Console.ResetColor();
                Console.Write($" - {request.HttpMethod} {request.Path} - ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result.StatusCode);
                Console.ResetColor();
            }

            var response = ResponseSerializer.Serialize(result);
            stream.Write(Encoding.UTF8.GetBytes(response));
            var endTime = DateTime.Now;
            var took = endTime - startTime;

            Console.WriteLine($"Took {took.TotalMilliseconds}ms");
        }
        catch (System.Exception error)
        {
            Console.WriteLine(error);
        }
        finally
        {
            client?.Close();
        }
    }

    public void Start()
    {
        Listener.Start();

        Console.WriteLine($"Server started on http://{Listener.LocalEndpoint.ToString()}");

        while (true)
        {
            Accept();
        }
    }
}