using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects.Base;
using System.Collections.Generic;
using System.Diagnostics;
namespace NEITGameEngine.Objects
{
    public class WaypointEnemy:BaseGameObject
    {
        Vector2 position;
        float speed;
        List<Vector2> waypoints;
        int currentWaypointIndex;
        BaseGameObject parent;
        PlayerSprite player;

        public WaypointEnemy(List<Vector2> waypoints, float speed,  Texture2D texture,BaseGameObject parent = null, PlayerSprite player = null)
        {
            position = waypoints[0];
            this.speed = speed;
            this.waypoints = waypoints;
            currentWaypointIndex = 1;
            _texture = texture;
            this.parent = parent;
            this.player = player;
        }

        public override void Update(GameTime gameTime)
        {
            if (waypoints.Count == 0) return;

            Vector2 targetWaypoint = waypoints[currentWaypointIndex];
            Vector2 direction = targetWaypoint - position;
            float distance = direction.Length();

            if (distance < 1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
            else
            {
                direction.Normalize();
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (parent != null)
            {
                position += parent.Position;
            }

            //Check Collision with player
            //if (player != null)
            //{
            //    if (BoxCollider.Intersects(player.BoxCollider))
            //    {
            //        Debug.WriteLine("Hit Player");
            //    }

                
            //}

            Position = position;
        }
    }
}
