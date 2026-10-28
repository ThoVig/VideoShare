using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client : Connecter
{
	ushort port;
	string ip;
	UdpClient client;
	IPEndPoint serverEndPoint;
	
	public Client()
	{
		Console.Write("IP?: ");
		ip = Console.ReadLine() ?? "0";
		Console.WriteLine();

		Console.Write("Port? (0 - 65535): ");
		port = ushort.Parse(Console.ReadLine() ?? "0");

		client = new UdpClient();

		serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
		var connectedMessage = Encoding.UTF8.GetBytes("Connected");
		client.Send(connectedMessage, connectedMessage.Length, serverEndPoint);
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

		client.Send(buffer, buffer.Length, serverEndPoint);

		return "";
	}
}