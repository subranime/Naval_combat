using Naval_combat_server;
using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Server
{
    private TcpListener tcpListener;
    private X509Certificate serverCertificate;
    private Logger logger;

    public Server(X509Certificate certificate)
    {
        this.tcpListener = new TcpListener(System.Net.IPAddress.Any, 7777);
        this.serverCertificate = certificate;
        this.logger = new Logger(LogLevel.Info, $"{AppSettings.LogPath}server_log.txt");
    }

    public void Start()
    {
        this.tcpListener.Start();
        logger.Log(LogLevel.Info, "Сервер запущен. Ожидание подключений...");
        Console.WriteLine("Сервер запущен. Ожидание подключений...");

        while (true)
        {
            TcpClient client = this.tcpListener.AcceptTcpClient();
            Task.Run(() => HandleClient(client));
        }
    }

    private void HandleClient(TcpClient tcpClient)
    {
        try
        {
            using (SslStream sslStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateClientCertificate),
                null
            ))
            {
                sslStream.AuthenticateAsServer(
                    this.serverCertificate,
                    false,
                    System.Security.Authentication.SslProtocols.Tls,
                    true
                );

                logger.Log(LogLevel.Info, "Клиент подключен. Ожидание данных...");
                Console.WriteLine("Клиент подключен. Ожидание данных...");

                // Чтение никнейма от клиента
                string nickname = ReadNickname(sslStream);

                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = sslStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    logger.Log(LogLevel.Info, $"Получено от клиента {nickname}: {receivedData}");
                    Console.WriteLine($"Получено от клиента {nickname}: {receivedData}");

                    //TODO код для обработки полученных данных
                }
                logger.Log(LogLevel.Info, $"Соединение с клиентом {nickname} разорвано.");
                Console.WriteLine($"Соединение с клиентом {nickname} разорвано.");
            }
        }
        catch (Exception ex)
        {
            logger.LogException(ex, "Ошибка обработки клиента");
            Console.WriteLine($"Ошибка обработки клиента: {ex.Message}");
        }
    }

    private string ReadNickname(SslStream sslStream)
    {
        // Ваш код для чтения никнейма от клиента (например, считывание строки из sslStream)
        // Замените этот код на фактическую реализацию ввода никнейма.

        // Пример:
        byte[] nicknameBuffer = new byte[256];
        int bytesRead = sslStream.Read(nicknameBuffer, 0, nicknameBuffer.Length);
        string nickname = Encoding.UTF8.GetString(nicknameBuffer, 0, bytesRead);

        return nickname;
    }

    private bool ValidateClientCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        System.Net.Security.SslPolicyErrors sslPolicyErrors
    )
    {
        //TODO код для проверки сертификата клиента
        return true;
    }

    public static void Main(string[] args)
    {

        // Создаем экземпляр класса Server и запускаем сервер
        var serverCertificate = new X509Certificate2(AppSettings.CertificatePath, AppSettings.CertificatePassword);
        var server = new Server(serverCertificate);
        server.Start();
    }
}
