using System.Text.RegularExpressions;

namespace HttpServer.Core.Routing;

public class RoutePath
{
    public RoutePath(List<Part> parts)
    {
        Parts = parts;
    }

    public List<Part> Parts { get; set; }

    public class Part
    {
        public Part(string name, bool isParameter)
        {
            Name = name;
            IsParameter = isParameter;
        }

        public string Name { get; set; }
        public bool IsParameter { get; set; }
    }

    public static RoutePath Parse(string path)
    {
        List<Part> parts = new List<Part>();

        if (!path.StartsWith('/'))
        {
            throw new Exception("All routes should start with a '/'");
        }

        if (path == "/")
        {
            return new RoutePath(parts);
        }

        path = path.TrimStart('/');

        var pathStrings = path.Split("/");

        foreach (var pathString in pathStrings)
        {
            var routePartRegex = new Regex("^[a-zA-Z]+$", RegexOptions.Compiled);
            var routeArgumentRegex = new Regex("^{[a-zA-Z]+}$", RegexOptions.Compiled);

            if (routePartRegex.Match(pathString)?.Success ?? false)
            {
                parts.Add(new Part(pathString, false));
            }
            else if (routeArgumentRegex.Match(pathString)?.Success ?? false)
            {
                parts.Add(new Part(pathString.TrimStart('{').TrimEnd('}'), true));
            }
            else
            {
                throw new Exception($"Cannot parse path {path}");
            }
        }

        return new RoutePath(parts);
    }

    public class RouteMatch
    {
        public Dictionary<string, string> Parameters { get; set; }

        public RouteMatch(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }
    }

    public RouteMatch? Match(RoutePath actual)
    {
        var parameters = new Dictionary<string, string>();

        if (this.Parts.Count != actual.Parts.Count)
            return null;

        var match = true;

        for (int i = 0; i < this.Parts.Count; i++)
        {
            var expectedPart = this.Parts[i];
            var actualPart = actual.Parts[i];

            if (!expectedPart.IsParameter)
            {
                if (expectedPart.Name != actualPart.Name)
                {
                    match = false;
                }
            }
            else
            {
                parameters.Add(expectedPart.Name, actualPart.Name);
            }
        }

        if (!match)
            return null;

        return new RouteMatch(parameters);
    }
}
