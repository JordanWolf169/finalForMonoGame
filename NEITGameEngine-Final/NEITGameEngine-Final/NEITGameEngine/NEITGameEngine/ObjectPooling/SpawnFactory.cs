using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects;
using NEITGameEngine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NEITGameEngine.ObjectPooling
{
    public class SpawnFactory:BaseGameObject
    {
        List<WanderingEnemy> enemyPool;
        private ContentManager contentManager;
        Texture2D enemyTexture;
        int poolSize;
        float spawnInterval;
        float timeSinceLastSpawn;
        Vector2 spawnPosition;

        public SpawnFactory(Texture2D enemyTexture, int poolSize, float spawnInterval, Vector2 spawnPosition, BaseGameObject map = null, PlayerSprite playerSprite = null)
        {
            _texture = enemyTexture;
            this.enemyTexture = enemyTexture;
            this.poolSize = poolSize;
            this.spawnInterval = spawnInterval;
            this.spawnPosition = spawnPosition;
            timeSinceLastSpawn = 0f;

            enemyPool = new List<WanderingEnemy>();
            for(int i = 0; i<poolSize; i++)
            {
                enemyPool.Add(new WanderingEnemy(spawnPosition, 100f, 1f, enemyTexture, map, playerSprite));
            }
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastSpawn >= spawnInterval)
            {
                //Spawn our enemy
                SpawnEnemy();
                timeSinceLastSpawn = 0;
            }

            foreach(var enemy in enemyPool)
            {
                if (enemy.IsActive)
                {
                    enemy.Update(gameTime);
                }
            }
        }

        private void SpawnEnemy()
        {
            foreach(var enemy in enemyPool)
            {
                if (!enemy.IsActive)
                {
                    Vector2 position = spawnPosition;
                    float speed = Random.Shared.Next(100, 200);
                    enemy.Activate(spawnPosition,speed);
                    break;

                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            foreach (var enemy in enemyPool)
            {
                if (enemy.IsActive)
                {
                    enemy.Render(spriteBatch);
                }
            }
        }

        public List<WanderingEnemy> GetActiveEnemies()
        {
            return enemyPool.Where(enemy => enemy.IsActive).ToList();
        }
    }
}
