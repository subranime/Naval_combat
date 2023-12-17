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

    public Server(X509Certificate certificate)
    {
        this.tcpListener = new TcpListener(System.Net.IPAddress.Any, 7777);
        this.serverCertificate = certificate;
    }

    public void Start()
    {
        this.tcpListener.Start();
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

                Console.WriteLine("Клиент подключен. Ожидание данных...");

                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = sslStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Получено от клиента: {receivedData}");

                    //TODO код для обработки полученных данных
                }

                Console.WriteLine("Соединение с клиентом разорвано.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка обработки клиента: {ex.Message}");
        }
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
