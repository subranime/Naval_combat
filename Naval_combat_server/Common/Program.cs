﻿using Naval_combat_server.Common;
using Naval_combat_server.GameLogic;
using Naval_combat_server.Networking;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Naval_combat_server.Entities;

class Program
{
    private static GameEngine gameEngine;

    public static void Main(string[] args)
    {
        UDPManager udpManager = new UDPManager(49153);
        gameEngine = new GameEngine(udpManager);
        udpManager.DataReceived += OnDataReceived;

        // Основная логика сервера

        Console.ReadLine(); // Для ожидания ввода и предотвращения закрытия приложения
        udpManager.Close();
    }


    static void OnDataReceived(Game data, IPEndPoint clientEndPoint)
    {
        try
        {
            // Обработка полученных данных в GameEngine
            gameEngine.ProcessReceivedData(data, clientEndPoint);
        }
        catch (Exception ex)
        {
            // Обработка ошибок при обработке данных
            Console.WriteLine(ex.Message, "Error handling received data.");
        }
    }
}
