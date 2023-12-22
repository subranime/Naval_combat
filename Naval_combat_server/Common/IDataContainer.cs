using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naval_combat_server.Common
{
    public interface IDataContainer
    {
        string ToJson();
        void FromJson(string json);
    }
}
