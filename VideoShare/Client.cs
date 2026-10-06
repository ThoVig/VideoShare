using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client : Connecter
{
	ushort port;
	string ip;
	TcpClient client;
	NetworkStream stream;
	public Client()
	{
		Console.Write("IP?: ");
		ip = Console.ReadLine() ?? "0";
		Console.WriteLine();

		Console.Write("Port? (0 - 65535): ");
		port = ushort.Parse(Console.ReadLine() ?? "0");

		client = new TcpClient(ip, port);
		stream = client.GetStream();
	}
	
	public string Update()
	{
		var message = Console.ReadLine();
		if (message == "exit")
		{
			client.Close();
			return message;
		}
		var buffer = Encoding.UTF8.GetBytes(message ?? "");

		stream.Write(buffer, 0, buffer.Length);
		return "";
	}
}