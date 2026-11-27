using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LibVLCSharp.Shared;

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
		// var message = Console.ReadLine();
		// if (message == "exit")
		// {
		// 	client.Close();
		// 	return message;
		// }
		// var buffer = Encoding.UTF8.GetBytes(message ?? "");

		// client.Send(buffer, buffer.Length, serverEndPoint);
		

		var sliceAmountReceived = client.Receive(ref serverEndPoint);
		int sliceAmount = BitConverter.ToInt32(sliceAmountReceived);
		var sliceSizeReceived = client.Receive(ref serverEndPoint);
		int sliceSize = BitConverter.ToInt32(sliceSizeReceived);
		Console.WriteLine($"Slice amount: {sliceAmount}, slice size: {sliceSize}");

		byte[] totalReceivedBytes = new byte[sliceAmount*sliceSize];
		for (int i = 0; i < sliceAmount; i++)
		{
			var received = client.Receive(ref serverEndPoint);
			Console.WriteLine(received.Length);
			
			Console.WriteLine($"Received slice {i}");

			for (int j = 0; j < received.Length; j++)
			{
				totalReceivedBytes[j + i * received.Length] = received[j];
			}
		}

		string filePath;
		using (var fs = new FileStream("temp.mp4", FileMode.OpenOrCreate))
		{
			fs.Write(totalReceivedBytes, 0, totalReceivedBytes.Length);
			Console.WriteLine($"Wrote slices");
			filePath = fs.Name;
		}

		
		var received2 = client.Receive(ref serverEndPoint);
		Console.WriteLine(Encoding.UTF8.GetString(received2));


		using var libvlc = new LibVLC(enableDebugLogs: true);
		using var media = new Media(libvlc, filePath);
		using var mediaplayer = new MediaPlayer(media);
		mediaplayer.EndReached += (sender, e) => ThreadPool.QueueUserWorkItem(_ => mediaplayer.Dispose());

		mediaplayer.Play();

		Console.ReadLine();
		File.Delete(filePath);
		return "exit";
	}
}