using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.World
{
    public class ScrollingBackground:BaseGameObject
    {
        float scrollingSpeed = 1.0f;
        float yPos;
        private ContentManager contentManager;

        public ScrollingBackground(Texture2D texture)
        {
            _texture = texture;
            Position = new Vector2(0, 0);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var viewport = spriteBatch.GraphicsDevice.Viewport;
            var sourceRectangle = new Rectangle(0,0,_texture.Width, _texture.Height);

            for (int bgVertical = -1; bgVertical < viewport.Height / _texture.Height + 1; bgVertical++)
            {
                var y = (int)Position.Y + bgVertical * _texture.Height;
                for (int bgHorizontal = -1; bgHorizontal < viewport.Width / _texture.Width + 1; bgHorizontal++)
                {
                    var x = (int)Position.X + bgHorizontal * _texture.Width;
                    var destinationRectangle = new Rectangle(x,y,_texture.Width,_texture.Height);
                    spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }

            yPos += scrollingSpeed;
            Position = new Vector2(0, (int)(yPos) % _texture.Height);
            
        }
    }
}
