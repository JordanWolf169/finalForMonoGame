using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.World;
using NEITGameEngine.Objects.Base;
using System;
using System.Diagnostics;
namespace NEITGameEngine.Objects
{
    public class WanderingEnemy:BaseGameObject
    {
        Vector2 position;
        Vector2 direction;
        float speed;
        float changeDirectionInterval;
        float timeSinceLastChange;
        Random random;
        BaseGameObject parent;
        PlayerSprite player;
        public bool IsActive { get; set; } = false;
        public WanderingEnemy(Vector2 startPos, float speed, float changeDirectionInterval, Texture2D texture, BaseGameObject parent = null, PlayerSprite player = null)
        {
            position = startPos;
            this.speed = speed;
            this.changeDirectionInterval = changeDirectionInterval;
            timeSinceLastChange = 0;
            _texture = texture;
            random = new Random();
            this.parent = parent;
            this.player = player;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastChange > changeDirectionInterval) 
            {
                SetRandomDirection();
                timeSinceLastChange = 0;
            }
            position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(parent != null)
            {
                position += parent.Position;
            }

            //Check to see of wandering enemy hits boundary
            //Clamp the enemy position on screen

            if (position.X <= 0 || position.X >= Globals.windowSize.X - _texture.Width)
            {
                SetRandomDirection();
                position.X = MathHelper.Clamp(position.X, 0, Globals.windowSize.X - _texture.Width);
            }

            if (position.Y <= 0 || position.Y >= Globals.windowSize.Y - _texture.Height)
            {
                SetRandomDirection();
                position.Y = MathHelper.Clamp(position.Y, 0, Globals.windowSize.Y - _texture.Height);
            }

            //Check Collision with player
            //if (player != null)
            //{
            //    if (BoxCollider.Intersects(player.BoxCollider))
            //    {
            //        Debug.WriteLine("Hit Player");
            //    }

            //    for (int i = 0; i < player.shooting.projectiles.pool.Count; i++)
            //    {
                   
            //        if (BoxCollider.Intersects(player.shooting.projectiles.pool[i].BoxCollider))
            //        {
            //            IsActive = false;
            //        }
            //    }
            //}
            
            Position = position;
        }

        private void SetRandomDirection()
        {
            direction = new Vector2((float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1);
            if (direction.Length() > 0)
            {
                direction.Normalize();
            }
        }

        public void Activate(Vector2 spawnPosition, float speed)
        {
            Position = spawnPosition;
            this.speed = speed;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

       
    }
}
