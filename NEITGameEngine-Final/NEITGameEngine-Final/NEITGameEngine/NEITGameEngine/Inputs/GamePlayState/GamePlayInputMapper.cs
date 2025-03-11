using Microsoft.Xna.Framework.Input;
using NEITGameEngine.Inputs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.Inputs.GamePlayState
{
    public class GamePlayInputMapper:BaseInputMapper
    {
        public override IEnumerable<BaseInputCommands> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<GamePlayCommand>();
            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new GamePlayCommand.GameExit());
            }

            if (state.IsKeyDown(Keys.P))
            {
                commands.Add(new GamePlayCommand.Pause());
            }

            if (state.IsKeyDown(Keys.Left))
            {
                commands.Add(new GamePlayCommand.moveLeft());
            }


            if (state.IsKeyDown(Keys.Right))
            {
                commands.Add(new GamePlayCommand.moveRight());
            }
            
            if (state.IsKeyDown(Keys.Up))
            {
                commands.Add(new GamePlayCommand.moveUp());
            }

            if (state.IsKeyDown(Keys.Down))
            {
                commands.Add(new GamePlayCommand.moveDown());
            }

            

            // Moving and Idle Code
            bool leftPressed = state.IsKeyDown(Keys.Left);
            bool rightPressed = state.IsKeyDown(Keys.Right);
            bool upPressed = state.IsKeyDown(Keys.Up);
            bool downPressed = state.IsKeyDown(Keys.Down);

            if (leftPressed ^ rightPressed || upPressed ^ downPressed) // XOR to check if only one of them is pressed
            {
                commands.Add(new GamePlayCommand.moving());
            }
            else if (!leftPressed && !rightPressed)
            {
                commands.Add(new GamePlayCommand.idle());
            }
            return commands;
        }
    }
}
