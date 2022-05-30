namespace HttpServer.Core.Routing;

abstract class Route
{
    public Route() { }

    public class Context
    {
        public Context(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }

        public Dictionary<string, string> Parameters { get; set; }
    }
}