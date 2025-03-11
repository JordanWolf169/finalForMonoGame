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
using System;

namespace NEITGameEngine.States
{
    public class MainMenu : BaseGameState
    {
        SpriteFont _font;
        InputManager _inputManager;
        int _selectionIndex;
        string[] _menuItems = { "Play Game", "Options", "Exit" };
        KeyboardState _previousKeyboardState;
        TimeSpan _keyPressDelay = TimeSpan.FromMilliseconds(150);
        TimeSpan _elapsedTime;
         

        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
           // _inputManager = new InputManager(new DevInputMapper());
            
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();
        }

        
        public override void HandleInput(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime >= _keyPressDelay)
            {
                if ( currentKeyboardState.IsKeyDown(Keys.Up) && 
                    !_previousKeyboardState.IsKeyDown(Keys.Up))
                {
                    _selectionIndex--;
                    if (_selectionIndex < 0)
                    {
                        _selectionIndex = _menuItems.Length - 1;
                    }
                    _elapsedTime = TimeSpan.Zero;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Down) &&
                    !_previousKeyboardState.IsKeyDown(Keys.Down))
                {
                    _selectionIndex++;
                    if (_selectionIndex >= _menuItems.Length)
                    {
                        _selectionIndex = 0;
                    }
                    _elapsedTime = TimeSpan.Zero;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Enter) &&
                    !_previousKeyboardState.IsKeyDown(Keys.Enter))
                {
                    switch (_selectionIndex)
                    {
                        case 0:
                            SwitchState(new EnemyMovementState());
                            break;
                        case 1:
                            //Go to the options menu;
                            SwitchState(new OptionsMenuState());
                            break;
                        case 2:
                            NotifyEvent(Events.GAME_QUIT);
                            break;
                    }
                    _elapsedTime = TimeSpan.Zero;
                }
            }
            _previousKeyboardState = currentKeyboardState;
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            string title = "Title of your Game";
            Vector2 titlePos = new Vector2(Globals.windowSize.X / 2 - 100, 100);
            spriteBatch.DrawString(_font, title, titlePos, Color.White);

            for(int i = 0; i < _menuItems.Length; i++)
            {
                Color color = i == _selectionIndex ? Color.Yellow : Color.White;
                Vector2 position = new Vector2(Globals.windowSize.X / 2 - 100, 200 + i * 50);
                spriteBatch.DrawString(_font, _menuItems[i], position, color);
            }
        }

       
    }

    public class OptionsMenuState:BaseGameState
    {
        SpriteFont _font;
        bool _isFullscreen;
        KeyboardState _previousKeyboardState;
        TimeSpan _keyPressDelay = TimeSpan.FromMilliseconds(150);
        TimeSpan _elapsedTime;

        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
            _isFullscreen = Globals.Graphics.IsFullScreen;

        }

        public override void UnloadContent(ContentManager contentManager)
        {
           
        }

        public override void HandleInput(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime >= _keyPressDelay)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Enter) &&
                    !_previousKeyboardState.IsKeyDown(Keys.Enter))
                {
                    _isFullscreen = !_isFullscreen;
                    Globals.Graphics.IsFullScreen = _isFullscreen;
                    Globals.Graphics.ApplyChanges();
                    _elapsedTime = TimeSpan.Zero;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Back) &&
                    !_previousKeyboardState.IsKeyDown(Keys.Back))
                {
                    SwitchState(new MainMenu());
                    _elapsedTime = TimeSpan.Zero;
                }
            }

            _previousKeyboardState = currentKeyboardState;
        }

        public override void Render(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            string optionsText = $"Options Menu\nFullscreen: {_isFullscreen}\nPress Enter to Toggle Fullscreen\nPress Backspace to return to Main Menu";
            Vector2 position = new Vector2(Globals.windowSize.X / 2 - 100, 150);
            spriteBatch.DrawString(_font, optionsText, position, Color.White);
        }
    }
}
