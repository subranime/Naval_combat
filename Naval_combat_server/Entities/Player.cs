using Naval_combat_server.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Naval_combat_server.Entities
{
    public class Player
    {
        public string NickName { get; set; }
        public int Points { get; set; }
        public int RemainingTorpedoes { get; set; }

        // Конструктор по умолчанию для десериализации JSON
        public Player()
        {
        }

        public Player(string nickName)
        {
            NickName = nickName;
            Points = 0;
            RemainingTorpedoes = 10;
        }

    }

}
