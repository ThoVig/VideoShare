using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Host : Connecter
{
	int port;
	TcpListener listender;
	public Host()
	{
		Console.Write("Port? (0 - 65535): ");
		port = int.Parse(Console.ReadLine() ?? "0");

		listender = new TcpListener(IPAddress.Any, port);

		Console.WriteLine($"Starting TCP Server on port {port}...");
		listender.Start();

	}
	
	public string Update()
	{
		Console.WriteLine("Waiting for connections");
		var client = listender.AcceptTcpClient();
		Console.WriteLine("Client connected");

		var stream = client.GetStream();
		var buffer = new byte[1024];
		int bytesRead;

		while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
		{
			var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
			Console.WriteLine($"Received: {message}");
			Console.WriteLine($"BytesRead: {bytesRead}");
			Console.WriteLine($"Buffer length: {buffer.Length}");

		}

		client.Close();
		Console.WriteLine("Client disconnected");
		return "";
	}
}