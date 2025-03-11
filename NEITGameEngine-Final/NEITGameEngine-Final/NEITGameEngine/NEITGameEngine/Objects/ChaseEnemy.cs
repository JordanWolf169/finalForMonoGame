using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects.Base;
namespace NEITGameEngine.Objects
{
    public class ChaseEnemy:BaseGameObject
    {
        Vector2 position;
        float speed;
        float detectionRadius;

        public ChaseEnemy(Vector2 startPos, float speed, float detectionRadius, Texture2D texture)
        {
            position = new(startPos.X - texture.Width / 2, startPos.Y - texture.Height / 2);
            this.speed = speed;
            this.detectionRadius = detectionRadius;
            _texture = texture;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            Vector2 direction = playerPos - position;
            float distance = direction.Length();
            if (distance < detectionRadius)
            {
                direction.Normalize();
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            Position = position;
        }
    }
}
