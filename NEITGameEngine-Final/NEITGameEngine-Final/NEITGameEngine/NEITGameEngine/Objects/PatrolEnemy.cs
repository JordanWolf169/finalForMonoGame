using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects.Base;
namespace NEITGameEngine.Objects
{
    public class PatrolEnemy:BaseGameObject
    {
        Vector2 position;
        float speed;
        float leftbound;
        float rightbound;
        bool movingRight;

        public PatrolEnemy(Vector2 startPos, float speed, float leftBound, float rightBound, Texture2D texture)
        {
            position = new(startPos.X - texture.Width / 2, startPos.Y - texture.Height / 2);
            this.speed = speed;
            this.leftbound = position.X - leftBound;
            this.rightbound = rightbound + position.X;
            this.movingRight = true;
            _texture = texture;
        }

        public override void Update(GameTime gameTime)
        {
            if (movingRight)
            {
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (position.X >= rightbound)
                {
                    movingRight = false;
                }
            }
            else
            {
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (position.X <= leftbound)
                {
                    movingRight = true;
                }
            }
            Position = position;
        }
    }
}
