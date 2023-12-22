using Naval_combat_server;
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        UDPManager udpManager = new UDPManager(49153);
        udpManager.DataReceived += OnDataReceived;

        // Основная логика сервера

        Console.ReadLine(); // Для ожидания ввода и предотвращения закрытия приложения
        udpManager.Close();
    }

    static void OnDataReceived(string data, IPEndPoint clientEndPoint)
    {
        Console.WriteLine($"Received data from {clientEndPoint}: {data}");
    }
}
