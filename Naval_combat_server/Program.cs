using Naval_combat_server;
using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        var serverCertificate = new X509Certificate2(AppSettings.CertificatePath, AppSettings.CertificatePassword);
        var serverManager = new TcpServerManager(serverCertificate);
        serverManager.Start();
    }
}
