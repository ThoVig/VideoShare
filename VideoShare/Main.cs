// See https://aka.ms/new-console-template for more information
// https://code.videolan.org/videolan/LibVLCSharp
using LibVLCSharp.Shared;


Console.WriteLine("new test().value");
Console.Write("Host? (y/n): ");
bool isHost = Console.ReadLine() == "y";
Console.WriteLine();

using var libvlc = new LibVLC(enableDebugLogs: true);
using var media = new Media(libvlc, new Uri(@"D:\Video (D)\;-discord vids\boowomp.mp4"));
using var mediaplayer = new MediaPlayer(media);
mediaplayer.EndReached += (sender, e) => ThreadPool.QueueUserWorkItem(_ => mediaplayer.Dispose());

mediaplayer.Play();



Connecter connecter;

if (isHost)
{
	connecter = new Host();
}
else
{
	connecter = new Client();
}

while (true)
{
	var r = connecter.Update();
	if (r == "exit")
	{
		break;
	}
}