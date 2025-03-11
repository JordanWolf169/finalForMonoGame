using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.States.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEITGameEngine.Enum;
using Microsoft.Xna.Framework;
using NEITGameEngine.Inputs.Base;
using NEITGameEngine.Inputs.GamePlayState;
using NEITGameEngine.Objects;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Animation;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace NEITGameEngine.States
{
    public class GamePlayState:BaseGameState
    {
      
        SpriteManager _spriteManager;
        PlayerSprite _playerSprite;

        public override void LoadContent(ContentManager contentManager)
        {
            _spriteManager = new SpriteManager();
            //_playerSprite = new PlayerSprite(_spriteManager);
            Texture2D playerSpriteSheetRun = contentManager.Load<Texture2D>("run");
            Texture2D playerSpriteSheetIdle = contentManager.Load<Texture2D>("Idle");
            _spriteManager.LoadAnimation("run", playerSpriteSheetRun, 48, 48, 8, 0.1f);
            _spriteManager.LoadAnimation("idle", playerSpriteSheetIdle, 48, 48, 10, 0.1f);
            //_spriteManager.PlayAnimation("run");
            AddGameObject(_playerSprite);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            
        }

        //Old GamePlayState Input Code
        //public override void HandleInput()
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        //    {
        //        //Notify Event System
        //        NotifyEvent(Events.GAME_QUIT);
        //    }

        //}

        public override void HandleInput(GameTime gameTime)
        {
            var _inputManager = new InputManager(new GamePlayInputMapper());
            
            _inputManager.GetCommands(cmd =>
            {
               
                if (cmd is GamePlayCommand.GameExit)
                {
                    NotifyEvent(Events.GAME_QUIT);
                }

                //All other inputs would go below
                if (cmd is GamePlayCommand.moveLeft)
                {
                    _playerSprite.MoveLeft();
                   
                }
                if (cmd is GamePlayCommand.moveRight)
                {
                    _playerSprite.MoveRight();
                   
                }

                

                if (cmd is GamePlayCommand.moveUp)
                {
                    _playerSprite.MoveUp();
                }

                if(cmd is GamePlayCommand.moveDown)
                {
                    _playerSprite.MoveDown();
                }

                if(cmd is GamePlayCommand.moving)
                {
                    _playerSprite.Moving();
                }

                if (cmd is GamePlayCommand.idle)
                {
                    _playerSprite.Idle();
                }
               
            });

            
        }
        public override void Update(GameTime gameTime) {
            _playerSprite.Update(gameTime);

        
        }
    }
}
