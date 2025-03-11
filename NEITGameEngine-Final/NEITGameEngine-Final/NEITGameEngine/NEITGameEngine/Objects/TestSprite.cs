using NEITGameEngine.Objects.Base;
using Microsoft.Xna.Framework.Graphics;

namespace NEITGameEngine.Objects
{
    public class TestSprite:BaseGameObject
    {
        public TestSprite(Texture2D texture)
        {
            _texture = texture;
        }
    }
}
