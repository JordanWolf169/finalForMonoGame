using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.Animation
{
    public class Animation
    {
        public Texture2D _spriteSheet;
        public int _frameWidth;
        public int _frameHeight;
        int _frameCount;
        float _frameSpeed;
        int _currentFrame;
        float _timer;
        public bool isPlaying;

        public Animation(Texture2D spriteSheet, int frameWidth, int frameHeight, int frameCount, float frameSpeed)
        {
            _spriteSheet = spriteSheet;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _frameCount = frameCount;
            _frameSpeed = frameSpeed;
            _currentFrame = 0;
            _timer = 0f;
        }

        public void Play()
        {
            
                _currentFrame = 0;
                _timer = 0f;
                isPlaying = true;
           
        }

        public void Stop()
        {
            isPlaying = false;
        }

        public void Update(GameTime gameTime)
        {
            
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_timer >= _frameSpeed)
                {
                    _timer = 0f;
                    _currentFrame++;
                    if (_currentFrame >= _frameCount)
                    {
                        _currentFrame = 0;
                    }
                }
            
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects)
        {
            int row = _currentFrame / (_spriteSheet.Width / _frameWidth);
            int column = _currentFrame % (_spriteSheet.Width / _frameWidth);

            Rectangle sourceRectangle = new Rectangle(column * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);
            spriteBatch.Draw(_spriteSheet, position, sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }
}
