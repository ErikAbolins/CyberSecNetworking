using System;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main(stirng[] args)
    {
        Console.Write("enter the ip addr to scan: ");
        string ipAddress = Console.ReadLine();

        Console.Write("enter the starting port: ");
        int startPort = int.Parse(Console.ReadLine());

        Console.Write("Ender the end port: ");
        int endPort = int.Parse(Console.ReadLine());

        Console.WriteLine($"Scanning {ipAddress} from port {startPort} to {endPort}...");

        for (int port = startPort; port <= endPort; port++)
        {
            await ScanPort(ipAddress, port);
        }
    }

    private static async Task ScanPort(string ipAddress, int port)
    {
        using (TcpClient client = new TcpClient())
        {
            try
            {
                var result = await client.ConnectAsync(ipAddress, port);
                Console.WriteLine($"port {port} is open.");
            }
            catch (SocketException)
            {
                Console.WriteLine($"no open ports");
            }
        }
    }
}
