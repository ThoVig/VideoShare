using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Host : Connecter
{
	int port;
	string filePath;
	UdpClient listender;
	IPEndPoint clientEndPoint;
	public Host()
	{
		Console.Write("Port? (0 - 65535): ");
		port = int.Parse(Console.ReadLine() ?? "0");
		Console.Write("File path: ");
		filePath = Console.ReadLine() ?? "";

		listender = new UdpClient(port);

		Console.WriteLine($"Starting UDP Server on port {port}...");
		clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
		
	}
	
	public string Update()
	{
		var received = listender.Receive(ref clientEndPoint);

		var message = Encoding.UTF8.GetString(received);
		Console.WriteLine($"Received: {message}");
		if (message == "Connected")
		{
			var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			var buffer = new byte[new FileInfo(filePath).Length];
			var bytesRead = fs.Read(buffer, 0, buffer.Length);
			byte[][] slices = ByteSlicer(buffer, 4096);
			Console.WriteLine(bytesRead);
			Console.WriteLine(buffer.Length);

			int sliceAmount = slices.Length;
			int sliceSize = 4096;
			byte[] sliceAmountToSend = BitConverter.GetBytes(sliceAmount);
			listender.Send(sliceAmountToSend, sliceAmountToSend.Length, clientEndPoint);
			byte[] sliceSizeToSend = BitConverter.GetBytes(sliceSize);
			listender.Send(sliceSizeToSend, sliceSizeToSend.Length, clientEndPoint);
			Console.WriteLine($"Slice amount: {sliceAmount}, slice size: {sliceSize}");
			int i = 0;
			foreach (byte[] slice in slices)
			{
				Console.WriteLine($"Sent slice {i++}");
				listender.Send(slice, slice.Length, clientEndPoint);
				Thread.Sleep(5);
			}
			var doneMessage = Encoding.UTF8.GetBytes("Done");
			listender.Send(doneMessage, doneMessage.Length, clientEndPoint);
			Console.WriteLine("Sent done");
		}

		Console.ReadLine();

		return "exit";
	}

	private byte[][] ByteSlicer(byte[] array, int size)
	{
		int sliceAmounts = array.Length / size + (array.Length % size == 0 ? 0 : 1);
		byte[][] slices = new byte[sliceAmounts][];
		for (int i = 0; i < sliceAmounts; i++)
		{
			int start = i * size;
			int length = Math.Min(size, array.Length - start);
			slices[i] = array[start..(start + length)];
		}

		return slices;
	}
}