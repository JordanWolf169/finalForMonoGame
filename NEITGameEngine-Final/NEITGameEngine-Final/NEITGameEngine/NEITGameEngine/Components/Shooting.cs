using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.ObjectPooling;
using NEITGameEngine.Objects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace NEITGameEngine.Components
{
    public class Shooting
    {
        Texture2D projectileTexture;
        float projectileSpeed;
        Vector2 shootDirection;
        float cooldown;
        float timeSinceLastShot;
        //List<Projectile> projectiles;
        public ProjectilePool projectiles;

        public Shooting(Texture2D texture, float speed, Vector2 direction,int poolSize, float cooldown = 0.5f)
        {
            this.projectileTexture = texture;
            projectileSpeed = speed;
            shootDirection = direction;
            this.cooldown = cooldown;
            timeSinceLastShot = cooldown;
            //projectiles = new List<Projectile>();
            projectiles = new ProjectilePool(projectileTexture, poolSize);
        }
        public void FireProjectile(Vector2 playerPosition, Vector2 playerDirection, float rotation, Vector2 minBounds, Vector2 maxBounds)
        {
            
            Projectile newProjectile = new Projectile(projectileTexture, playerPosition, playerDirection, projectileSpeed, rotation, minBounds, maxBounds);
            //projectiles.Add(newProjectile);
            projectiles.GetProjectile(playerPosition, playerDirection, projectileSpeed, rotation, minBounds, maxBounds);

        }      
        
        public void Update(GameTime gameTime, Vector2 projectilePosition, Vector2 playerDir, float rotation, Vector2 minBounds, Vector2 maxBounds)
        {
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && timeSinceLastShot > cooldown)
            {
                
                FireProjectile(projectilePosition, playerDir, rotation, minBounds, maxBounds);
                timeSinceLastShot = 0;
            }

            //Update projectiles
            //for (int i = projectiles.Count -1; i>=0; i--)
            //{
            //    projectiles[i].Update(gameTime);

            //    if (!projectiles[i].IsActive)
            //    {
            //        projectiles.RemoveAt(i);
            //    }
            //}
            projectiles.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //Draws Projectiles without Object Pool
            //foreach(var projectile in projectiles)
            //{

            //    projectile.Draw(spriteBatch);
            //}

            projectiles.Draw(spriteBatch);
        }
        public List<Projectile> GetActiveProjectiles()
        {
            return projectiles.pool.Where(p => p.IsActive).ToList();

        }

    }
}
