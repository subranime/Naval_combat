using Naval_combat_server.Common;
using Naval_combat_server.GameLogic;
using Naval_combat_server.Networking;
using Newtonsoft.Json;
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
        GameEngine gameEngine = new GameEngine();
        UDPManager udpManager = new UDPManager(49153, gameEngine);
        udpManager.DataReceived += OnDataReceived;

        // Основная логика сервера

        Console.ReadLine(); // Для ожидания ввода и предотвращения закрытия приложения
        udpManager.Close();
    }


    static void OnDataReceived(string data, IPEndPoint clientEndPoint)
    {
        try
        {

        }
        catch (Exception ex)
        {
            // Обработка ошибок при десериализации
            Console.WriteLine(ex.Message, "Error handling received data.");
        }
    }
}
