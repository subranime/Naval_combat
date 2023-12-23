using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naval_combat_server.Common;

namespace Naval_combat_server.Entities
{
    public class TorpedoLauncher
    {
        public int Angle { get; set; }
        public int Speed { get; set; }

        public TorpedoLauncher(int angle, int speed)
        {
            Angle = angle;
            Speed = speed;
        }
    }
}
