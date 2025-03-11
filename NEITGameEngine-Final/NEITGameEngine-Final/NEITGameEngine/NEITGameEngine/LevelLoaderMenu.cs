using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine
{
    public class LevelLoaderMenu
    {
        public SpriteBatch _spriteBatch;
        public SpriteFont _font;
        private string[] _levelFiles;
        private bool _isVisible;
        private Action<string> _onLevelSelected;
        private GraphicsDeviceManager _graphicsDevice;
        

        public LevelLoaderMenu(SpriteBatch spriteBatch, SpriteFont font, Action<string> onLevelSelected)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _onLevelSelected = onLevelSelected;
            _isVisible = false;
            
        }

        public void Show()
        {
            _isVisible = true;
            LoadLevelFiles();
        }

        public void Hide()
        {
            _isVisible = false;
        }
        private void LoadLevelFiles()
        {
            string levelsDirectory = "Levels";

            if (Directory.Exists(levelsDirectory))
            {
                _levelFiles = Directory.GetFiles(levelsDirectory, "*.json");
            }
            else
            {
                _levelFiles = Array.Empty<string>();
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!_isVisible) return;

            //_spriteBatch.Begin();

            //Draw background for menu
            _spriteBatch.Draw(new Texture2D(Editor._graphics.GraphicsDevice,1,1), new Rectangle(0, 0, 800, 600), Color.Black);

            //Draw list of level files
            int yOffset = 50;
            foreach (var file in _levelFiles) 
            {
                _spriteBatch.DrawString(_font, Path.GetFileName(file), new Vector2(50, yOffset), Color.White);
                yOffset += 30;
            }

            //Draw Cancel button
            _spriteBatch.DrawString(_font, "Cancel", new Vector2(50, yOffset), Color.White);

            //_spriteBatch.End();
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            if (!_isVisible) return;

            int yOffset = 50;
            foreach(var file in _levelFiles)
            {
                Rectangle fileRect = new Rectangle(50,yOffset, 200, 30);

                if(fileRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _onLevelSelected(file);
                    Hide();
                    break;
                }
                yOffset += 30;
            }

            //Check to see if we click the cancel button
            Rectangle cancelButtonRect = new Rectangle(50, yOffset, 200, 30);

            if (cancelButtonRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                Hide();
                _onLevelSelected("cancel");
            }
        }

    }
}
