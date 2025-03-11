using NEITGameEngine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEITGameEngine.States.Base;
using NEITGameEngine.Objects;
using NEITGameEngine.World;
using System.Diagnostics;
using NEITGameEngine.SaveDataSystem;
using Microsoft.Xna.Framework.Content;

namespace NEITGameEngine.Menus
{
    public class PauseMenu:BaseGameObject
    {
        public enum PauseMenuOptions
        {
            Resume,
            SaveGame,
            ExitGame
        }

        PauseMenuOptions _currentMenuOption = PauseMenuOptions.Resume;
        SpriteFont _font;
        Vector2 _menuPosition;
        bool _isKeyPressed = false;
        ContentManager ContentManager;
        BaseGameState _currentGameState;
        PlayerSprite _playerSprite;
        ScoreMenu _scoreMenu;

        public PauseMenu(SpriteFont font, Vector2 menuPosition, BaseGameState currentGameState, PlayerSprite playerSprite, ScoreMenu scoreMenu)
        {
            _font = font;
            _menuPosition = menuPosition;
            _currentGameState = currentGameState;
            _playerSprite = playerSprite;
            _scoreMenu = scoreMenu; 
        }

        public void HandleInput(GameTime gameTime)
        {
            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.Up) && !_isKeyPressed)
            {
                _currentMenuOption = _currentMenuOption == PauseMenuOptions.Resume
                    ? PauseMenuOptions.ExitGame
                    : _currentMenuOption - 1;
                _isKeyPressed = true;
            }

            if (keyBoardState.IsKeyDown(Keys.Down) && !_isKeyPressed)
            {
                _currentMenuOption = _currentMenuOption == PauseMenuOptions.ExitGame
                    ? PauseMenuOptions.Resume
                    : _currentMenuOption + 1;
                _isKeyPressed = true;
            }

            if(keyBoardState.IsKeyUp(Keys.Up) && keyBoardState.IsKeyUp(Keys.Down))
            {
                _isKeyPressed = false;
            }

            if (keyBoardState.IsKeyDown(Keys.Enter))
            {
                //Call menu option
                ExecuteMenuOption();
            }
        }

        void ExecuteMenuOption()
        {
            switch (_currentMenuOption) {

                case PauseMenuOptions.Resume:
                    ResumeGame();
                    break;

                case PauseMenuOptions.SaveGame:
                    SaveGame(); 
                    break;

                case PauseMenuOptions.ExitGame:
                    ExitGame();
                    break;

            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            //if not paused do not render
            if (!Globals.Paused) { return; }

            //Draws the pause menu
            string[] menuOptions = { "Resume", "Save Game", "Exit Game" };

            int selectedIndex = (int)_currentMenuOption;
            Color titleColor = Color.White;
            Vector2 titlePosition = _playerSprite.Position + new Vector2(_menuPosition.X, -40);
            spriteBatch.DrawString(_font, "Paused", titlePosition, titleColor);

            for (int i = 0; i<menuOptions.Length; i++)
            {
                Color color = i == selectedIndex ? Color.Yellow : Color.White;
                Vector2 position = _playerSprite.Position + new Vector2(_menuPosition.X, i * 40);
                spriteBatch.DrawString(_font, menuOptions[i], position, color);
            }
        }

        void ExitGame()
        {
            _currentGameState.NotifyEvent(Enum.Events.GAME_QUIT);
        }

        void ResumeGame()
        {
            Globals.Paused = false;
        }

        void SaveGame()
        {
            SaveData saveData = new SaveData { 
                PlayerScore = _scoreMenu.GetScore(),
                PlayerPosition = _playerSprite.Position,
            
            };

            Debug.WriteLine("Saving Game");
            Globals.PlayerData.SaveGame(saveData);
        }
    }
}
