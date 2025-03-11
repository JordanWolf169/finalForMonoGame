using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.Animation
{
    public class SpriteManager
    {

        Dictionary<string, Animation> _animations;
        public Animation _currentAnimation;

        public SpriteManager()
        {
            _animations = new Dictionary<string, Animation>();
        }

        public void LoadAnimation(string name, Texture2D spriteSheet, int frameWidth, int frameHeight, int frameCount, float frameSpeed)
        {
            if (!_animations.ContainsKey(name))
            {
                _animations[name] = new Animation(spriteSheet,frameWidth,frameHeight,frameCount,frameSpeed);
            }
        }

        public void PlayAnimation(string name)
        {
            if (_animations.ContainsKey(name))
            {
                _currentAnimation = _animations[name];
                //_currentAnimation.Play();
            }
        }

        public void StopAnimation()
        {


            _currentAnimation = null;
            
        }



        public void Update(GameTime gameTime)
        {
            _currentAnimation?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
        {
            _currentAnimation?.Draw(spriteBatch, position, effects);
        }
    }
}
