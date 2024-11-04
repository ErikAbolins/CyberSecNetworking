using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {

        TcpListener listener = new TcpListener(IPAddress.Any, 5555);
        listener.Start();
        Console.WriteLine("Server started at port 5555");
        Console.WriteLine(@"
     _____                      _          _____ _          _ _
    |  __ \                    | |        / ____| |        | | |
    | |__) |___ _ __ ___   ___ | |_ ___  | (___ | |__   ___| | |
    |  _  // _ \ '_ ` _ \ / _ \| __/ _ \  \___ \| '_ \ / _ \ | |
    | | \ \  __/ | | | | | (_) | ||  __/  ____) | | | |  __/ | |
    |_|  \_\___|_| |_| |_|\___/ \__\___| |_____/|_| |_|\___|_|_|
    ");

        TcpClient client = listener.AcceptTcpClient();
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Client Connected.");


        StreamWriter writer = new StreamWriter(stream);
        StreamReader reader = new StreamReader(stream);

        while (client.Connected)
        {
            try
            {
                Console.Write("Enter command: ");
                string command = Console.ReadLine();

                // send the command to the client
                writer.WriteLine(command);
                writer.Flush();

                // receive the response from the client
                string response = reader.ReadLine();
                Console.WriteLine("Response: " + response);

                if (command == "exit")
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                break;
            }




        }

        //Cleanup
        client.Close();
        listener.Stop();
        Console.WriteLine("Server stopped.");
    }
}
