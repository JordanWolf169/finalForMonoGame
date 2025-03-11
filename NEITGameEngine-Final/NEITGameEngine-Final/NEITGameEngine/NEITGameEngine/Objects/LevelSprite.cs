using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.Objects
{
    public class LevelSprite
    {
        Texture2D _groundSprite;
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle SourceRect { get; set; }

        public LevelSprite(Texture2D texture, Vector2 position, Rectangle sourceRect)
        {
            _groundSprite = texture;
            Position = position;
            SourceRect = sourceRect;
            Origin = new(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_groundSprite, Position, SourceRect, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
