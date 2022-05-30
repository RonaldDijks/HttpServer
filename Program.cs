using System.Net;
using System.Net.Sockets;
using HttpServer.Core;
using HttpServer.Core.Routing;

var router = new Routes();
router.AddControllers();

var port = 3000;
var address = IPAddress.Parse("127.0.0.1");
var listener = new TcpListener(address, port);
var server = new Server(router, listener);
server.Start();
