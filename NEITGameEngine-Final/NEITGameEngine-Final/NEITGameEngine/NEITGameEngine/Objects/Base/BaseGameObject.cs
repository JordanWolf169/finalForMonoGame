using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Enum;
using System;

namespace NEITGameEngine.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        private Vector2 _position;
        public int zIndex;

        public Vector2 Position {
            get { return _position; }
            set { _position = value; }
        }

        public virtual void OnNotify(Events eventType) { }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual Rectangle BoxCollider 
        {
            get 
            {
                if (_texture != null)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
                }
                return Rectangle.Empty;
            }
        }
    }
}
