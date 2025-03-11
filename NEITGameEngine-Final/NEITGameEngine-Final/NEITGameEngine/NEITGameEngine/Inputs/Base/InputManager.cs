using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.Inputs.Base
{

    public class InputManager
    {
        private readonly BaseInputMapper _inputMapper;

        public InputManager(BaseInputMapper inputMapper)
        {

            _inputMapper = inputMapper;
        }

        public void GetCommands(Action<BaseInputCommands> actOnState)
        {
            var keyboardState = Keyboard.GetState();

            foreach (var state in _inputMapper.GetKeyboardState(keyboardState))
            {
                actOnState(state);
            }
        }

    }
}
