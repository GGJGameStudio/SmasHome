using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    static class Global
    {
        static Global()
        {
            NbPlayers = 2;
        }

        public static int NbPlayers { get; set; }

    }
}
