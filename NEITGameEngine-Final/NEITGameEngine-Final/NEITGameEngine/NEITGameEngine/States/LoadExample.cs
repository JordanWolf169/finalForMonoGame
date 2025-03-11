using NEITGameEngine.States.Base;
using NEITGameEngine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.Inputs.Base;
using NEITGameEngine.Inputs.DevState;
using NEITGameEngine.Enum;
using NEITGameEngine.SaveDataSystem;
using Microsoft.Xna.Framework;
using NEITGameEngine.World;
using System.Diagnostics;

namespace NEITGameEngine.States
{
    public class LoadExample : BaseGameState
    {
        SaveSystem _saveSystem;
        int _playerScore;
        SaveData _playerSave;
        SpriteFont _font;
         

        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _saveSystem = Globals.PlayerData;
            _playerSave = _saveSystem.LoadGame();
            _playerScore = _playerSave.PlayerScore;
            _font = contentManager.Load<SpriteFont>("gameFont");
        
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();
        }

        
        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var _inputManager = new InputManager(new DevInputMapper());

            _inputManager.GetCommands(cmd => { 
            if (cmd is DevInputCommand.GameStart) 
                {
                SwitchState(new EnemyMovementState());
                }
            if (cmd is DevInputCommand.GameExit) 
                {
                    NotifyEvent(Events.GAME_QUIT);
                }
            if(cmd is DevInputCommand.Shoot)

                {
                    
                }
            });
        }

        public override void Update(GameTime gameTime)
        {
            Debug.WriteLine(_playerScore);
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            string scoreText = $"Player Loaded Score: {_playerScore}\n     Press Enter to Start";
            spriteBatch.DrawString(_font, scoreText, new Vector2(Globals.windowSize.X/2 - 100, Globals.windowSize.Y/2), Color.White);
        }

        //protected override void SetInputManager()
        //{

        //}
    }
}
