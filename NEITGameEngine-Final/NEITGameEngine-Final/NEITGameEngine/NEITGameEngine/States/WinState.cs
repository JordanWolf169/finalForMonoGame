using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using NEITGameEngine.Enum;
using NEITGameEngine.Inputs.Base;
using NEITGameEngine.States.Base;
using NEITGameEngine.World;
using NEITGameEngine.SaveDataSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Diagnostics;
using NEITGameEngine.Menus;


namespace NEITGameEngine.States
{
    public class WinState : BaseGameState
    {
        SpriteFont _font;
        InputManager _inputManager;
        int _selectionIndex;
        string[] _menuItems = { "Menu", "Play Again", "Show Top Ten or Add Your Player", "Exit" };
        KeyboardState _previousKeyboardState;
        TimeSpan _keyPressDelay = TimeSpan.FromMilliseconds(150);
        TimeSpan _elapsedTime;
        public string _finalScore;
        string _finalTime;
        double _finalElapsedTime;

        public Texture2D background;

        public SaveSystem _saveSystem;


        ScrollingBackground _scrollingBackground;


        public WinState(string playerScore)
        {
            _finalScore = playerScore;

        }

        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
            // _inputManager = new InputManager(new DevInputMapper());

            //Scrolling bg
            _scrollingBackground = new ScrollingBackground(contentManager.Load<Texture2D>("Water 1"));
            AddGameObject(_scrollingBackground);

            background = contentManager.Load<Texture2D>("metal 3");
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
                if (currentKeyboardState.IsKeyDown(Keys.Up) &&
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
                            SwitchState(new MainMenu());
                            break;
                        case 1:
                            //Go to the options menu;
                            SwitchState(new EnemyMovementState());
                            break;
                        case 2:
                            //Go to the options menu;
                            SwitchState(new FetchRequest(_finalScore));
                            break;
                        case 3:
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

            spriteBatch.Draw(background, new Rectangle(0, 0, Globals.windowSize.X, Globals.windowSize.Y + 1000), Color.White);
            string title = "You Win!";
            Vector2 titlePos = new Vector2(Globals.windowSize.X / 2 - 200, 90);
            spriteBatch.DrawString(_font, title, titlePos, Color.White);


            for (int i = 0; i < _menuItems.Length; i++)
            {
                Color color = i == _selectionIndex ? Color.Yellow : Color.White;
                Vector2 position = new Vector2(Globals.windowSize.X / 2 - 200, 300 + i * 35);
                spriteBatch.DrawString(_font, _menuItems[i], position, color);
            }
        }
    }
}