using NEITGameEngine.Inputs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.Inputs.GamePlayState
{
    public class GamePlayCommand:BaseInputCommands
    {
        public class GameExit : GamePlayCommand { }
        public class moveLeft : GamePlayCommand { }
        public class moveRight : GamePlayCommand { }
        public class moveUp : GamePlayCommand { }
        public class moveDown : GamePlayCommand { }
        public class moving : GamePlayCommand { }
        public class idle : GamePlayCommand { }
        public class Pause : GamePlayCommand { }

    }
}
