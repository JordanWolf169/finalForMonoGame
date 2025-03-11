using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEITGameEngine.Inputs.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel.DataAnnotations;

namespace NEITGameEngine.Inputs.DevState
{
    public class DevInputMapper:BaseInputMapper
    {
        public override IEnumerable<BaseInputCommands> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<DevInputCommand>();

            if (state.IsKeyDown(Keys.Enter))
            {
                commands.Add(new DevInputCommand.Confirm());
            }

            if (state.IsKeyDown(Keys.Enter))
            {
                commands.Add(new DevInputCommand.ToggleFullScreen());
            }

            if (state.IsKeyDown(Keys.Back))
            {
                commands.Add(new DevInputCommand.Back());
            }

            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new DevInputCommand.GameExit());
            }

            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new DevInputCommand.Shoot());
            }

            if (state.IsKeyDown(Keys.Up))
            {
                commands.Add(new DevInputCommand.Up());
            }

            if (state.IsKeyDown(Keys.Down))
            {
                commands.Add(new DevInputCommand.Down());
            }
            return commands;
        }
    }
}
