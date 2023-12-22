using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naval_combat_server.Common;

namespace Naval_combat_server.Entities
{
    public class TorpedoLauncher : IDataContainer
    {
        public int Angle { get; set; }
        public int Speed { get; set; }

        public TorpedoLauncher(int angle, int speed)
        {
            Angle = angle;
            Speed = speed;
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
            TorpedoLauncher torpedoLauncher = JsonConvert.DeserializeObject<TorpedoLauncher>(json);

            // Копирование данных из преобразованного объекта в текущий объект
            Angle = torpedoLauncher.Angle;
            Speed = torpedoLauncher.Speed;
        }
    }

}
