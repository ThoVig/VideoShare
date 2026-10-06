// See https://aka.ms/new-console-template for more information

Console.WriteLine("new test().value");
Console.Write("Host? (y/n): ");
bool isHost = Console.ReadLine() == "y";
Console.WriteLine();

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