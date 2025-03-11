using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects;
using System.Collections.Generic;

namespace NEITGameEngine.ObjectPooling
{
    public class ProjectilePool
    {
        public List<Projectile> pool;
        private Texture2D texture;
        private int poolSize;

        public ProjectilePool(Texture2D texture, int poolSize)
        {
            this.texture = texture;
            this.poolSize = poolSize;
            pool = new List<Projectile>();

            for (int i = 0; i < poolSize; i++)
            {
                pool.Add(new Projectile(texture, Vector2.Zero, Vector2.Zero, 0f, 0f, Vector2.Zero, Vector2.Zero));
                pool[i].IsActive = false;
            }
        }

        public Projectile GetProjectile(Vector2 startPosition, Vector2 direction, float speed, float rotation, Vector2 minBoundary, Vector2 maxBoundary)
        {
            foreach (var projectile in pool)
            {
                if (!projectile.IsActive)
                {
                    projectile.Position = startPosition;
                    projectile.Direction = direction;
                    projectile.Speed = speed;
                    projectile.Rotation = rotation;
                    projectile.IsActive = true;
                    projectile.MinBounds = minBoundary;
                    projectile.MaxBounds = maxBoundary;
                    return projectile;
                }
            }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var projectile in pool)
            {
                if (projectile.IsActive)
                {
                    projectile.Update(gameTime);
                    //Check to see if it is out of the level
                    if (projectile.Position.X < projectile.MinBounds.X || projectile.Position.X > projectile.MaxBounds.X || projectile.Position.Y < projectile.MinBounds.Y || projectile.Position.Y > projectile.MaxBounds.Y)
                    {
                        projectile.IsActive = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var projectile in pool)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }
}
