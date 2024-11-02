using System;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Enter the IP address to scan: ");
        string ipAddress = Console.ReadLine();

        Console.Write("Enter the starting port: ");
        int startPort;
        while (!int.TryParse(Console.ReadLine(), out startPort) || startPort < 1 || startPort > 65535)
        {
            Console.Write("Invalid input. Please enter a valid starting port (1-65535): ");
        }

        Console.Write("Enter the ending port: ");
        int endPort;
        while (!int.TryParse(Console.ReadLine(), out endPort) || endPort < startPort || endPort > 65535)
        {
            Console.Write("Invalid input. Please enter a valid ending port (greater than or equal to starting port and <= 65535): ");
        }

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
                // Attempt to connect to the port asynchronously
                await client.ConnectAsync(ipAddress, port);
                Console.WriteLine($"Port {port} is open.");
            }
            catch (SocketException)
            {
                // Handle the case for closed ports
                Console.WriteLine($"Port {port} is closed.");
            }
            catch (Exception ex)
            {
                // General exception handling for other errors
                Console.WriteLine($"An error occurred on port {port}: {ex.Message}");
            }
        }
    }
}

