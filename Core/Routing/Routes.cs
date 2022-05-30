using System.Reflection;
using HttpServer.Core.Http;

namespace HttpServer.Core.Routing;

class Routes
{
    public class Entry
    {
        public Entry(RoutePath path, Action<Request, Response> handle)
        {
            Path = path;
            Handle = handle;
        }

        public RoutePath Path { get; set; }
        public Action<Request, Response> Handle { get; set; }
    }

    public List<Entry> Entries { get; set; } = new List<Entry>();

    public void Add(RoutePath path, Action<Request, Response> handle)
    {
        this.Entries.Add(new Entry(path, handle));
    }

    public bool Route(Request request, Response response)
    {
        try
        {
            foreach (var route in Entries)
            {
                var expected = route.Path;
                var actual = RoutePath.Parse(request.Path);
                var match = expected.Match(actual);

                if (match != null)
                {
                    request.RouteParameters = match.Parameters;
                    route.Handle(request, response);
                    return true;
                }
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    public void AddControllers()
    {
        AddControllers(Assembly.GetExecutingAssembly());
    }

    public void AddControllers(Assembly assembly)
    {

        var controllers = assembly
          .GetTypes()
          .Where(t => t.IsSubclassOf(typeof(Route)));

        foreach (var controller in controllers)
        {
            foreach (var method in controller.GetMethods())
            {
                var attributes = method.GetCustomAttributes(false);
                var getAttribute = attributes.SingleOrDefault(obj => obj.GetType().Equals(typeof(GetAttribute))) as GetAttribute;

                if (getAttribute == null)
                {
                    continue;
                }

                Console.WriteLine($"Added route: GET {getAttribute.Path}");

                Entries.Add(new Entry(getAttribute.Path, (req, res) =>
                {
                    var constructors = controller.GetConstructors();
                    var instance = constructors[0].Invoke(new object[] { });
            // TODO: Improve this shitty constructing
            var methodParameters = method.GetParameters().Length == 0
              ? new object[] { }
              : new object[] { new Route.Context(req.RouteParameters) };
                    var result = method.Invoke(instance, methodParameters) as Result;

                    if (result != null)
                    {
                        res.StatusCode = result.StatusCode;
                        res.ContentType = "text/html; charset=UTF-8";
                        res.Body = result.Body;
                    }
                }));
            }
        }

        Console.WriteLine($"Added {Entries.Count} routes");
    }
}