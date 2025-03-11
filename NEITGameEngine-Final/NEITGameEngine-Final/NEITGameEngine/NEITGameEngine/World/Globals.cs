using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NEITGameEngine.SaveDataSystem;

namespace NEITGameEngine.World
{
    public static class Globals
    {
        public static Point windowSize { get; set; }
        public static bool Paused { get; set; } = false;
        public static SaveSystem PlayerData { get; set; }
        public static GraphicsDeviceManager Graphics {  get; set; }
    }
}
