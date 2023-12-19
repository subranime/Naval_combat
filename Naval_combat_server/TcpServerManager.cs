using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Naval_combat_server
{
    public class TcpServerManager
    {
        private ConnectionHandler connectionHandler;

        public TcpServerManager(X509Certificate serverCertificate)
        {
            this.connectionHandler = new ConnectionHandler(serverCertificate);
        }

        public void Start()
        {
            connectionHandler.Start();
        }
    }
}
