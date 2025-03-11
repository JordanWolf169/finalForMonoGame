using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEITGameEngine.Inputs.Base;

namespace NEITGameEngine.Inputs.DevState
{
    public class DevInputCommand:BaseInputCommands
    {
        public class GameStart : DevInputCommand { }
        public class GameExit : DevInputCommand { }
        public class Shoot : DevInputCommand { }
        public class Up : DevInputCommand { }
        public class Down : DevInputCommand { }
        public class Confirm : DevInputCommand { }
        public class ToggleFullScreen : DevInputCommand { }
        public class Back : DevInputCommand { }
    }
}
