using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace NEITGameEngine.Inputs.Base
{
    public class BaseInputMapper
    {
        public virtual IEnumerable<BaseInputCommands> GetKeyboardState(KeyboardState state)
        {
            return new List<BaseInputCommands>();
        }
    }
}
