using NEITGameEngine.States.Base;
using NEITGameEngine.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.Inputs;
using NEITGameEngine.Inputs.Base;
using NEITGameEngine.Inputs.DevState;
using NEITGameEngine.Enum;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using NEITGameEngine.Sound;
using NEITGameEngine.Animation;
using NEITGameEngine.World;
using NEITGameEngine.Inputs.GamePlayState;
using Microsoft.Xna.Framework;

namespace NEITGameEngine.States
{
    public class ScrollingBGExample : BaseGameState
    {

        public ScrollingBackground scrollingBackground;
        SpriteManager _spriteManager;
        PlayerSprite _playerSprite;

        public override void LoadContent(ContentManager contentManager)
        {
            scrollingBackground = new ScrollingBackground(contentManager.Load<Texture2D>("groundtile"));
            AddGameObject(scrollingBackground);

            _spriteManager = new SpriteManager();
            //_playerSprite = new PlayerSprite(_spriteManager);
            Texture2D playerSpriteSheetRun = contentManager.Load<Texture2D>("run");
            Texture2D playerSpriteSheetIdle = contentManager.Load<Texture2D>("Idle");
            _spriteManager.LoadAnimation("run", playerSpriteSheetRun, 48, 48, 8, 0.1f);
            _spriteManager.LoadAnimation("idle", playerSpriteSheetIdle, 48, 48, 10, 0.1f);
            AddGameObject(_playerSprite);

        }

        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();
        }

        
        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
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

                if (cmd is GamePlayCommand.moveDown)
                {
                    _playerSprite.MoveDown();
                }

                if (cmd is GamePlayCommand.moving)
                {
                    _playerSprite.Moving();
                }

                if (cmd is GamePlayCommand.idle)
                {
                    _playerSprite.Idle();
                }
            });
        }

        public override void Update(GameTime gameTime)
        {
            _playerSprite.Update(gameTime);


        }

        //protected override void SetInputManager()
        //{

        //}
    }
}
