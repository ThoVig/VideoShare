using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Host : Connecter
{
	int port;
	UdpClient listender;
	IPEndPoint clientEndPoint;
	public Host()
	{
		Console.Write("Port? (0 - 65535): ");
		port = int.Parse(Console.ReadLine() ?? "0");

		listender = new UdpClient(port);

		Console.WriteLine($"Starting UDP Server on port {port}...");
		clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
		
		var received = listender.Receive(ref clientEndPoint);

		var message = Encoding.UTF8.GetString(received);
	}
	
	public string Update()
	{
		Console.WriteLine("111asdwas");
		var received = listender.Receive(ref clientEndPoint);

		var message = Encoding.UTF8.GetString(received);
		Console.WriteLine($"Received: {message}");

		Console.WriteLine("asdwas");

		return "";
	}
}