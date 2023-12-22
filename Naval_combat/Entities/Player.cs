using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Naval_combat.Common;

namespace Naval_combat.Entities
{
    public class Player : IDataContainer
    {
        public string NickName { get; set; }
        public int Points { get; set; }
        public int RemainingTorpedoes { get; set; }

        public Player(string nickName)
        {
            NickName = nickName;
            Points = 0;
            RemainingTorpedoes = 10;
        }

        // Реализация методов интерфейса IDataContainer
        public string ToJson()
        {
            // Преобразование объекта в JSON
            return JsonConvert.SerializeObject(this);
        }

        public void FromJson(string json)
        {
            // Преобразование JSON в объект
            Player player = JsonConvert.DeserializeObject<Player>(json);

            // Копирование данных из преобразованного объекта в текущий объект
            NickName = player.NickName;
            Points = player.Points;
            RemainingTorpedoes = player.RemainingTorpedoes;
        }
    }

}
