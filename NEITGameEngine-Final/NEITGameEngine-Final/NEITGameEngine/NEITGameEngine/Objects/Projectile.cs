using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects.Base;

namespace NEITGameEngine.Objects
{
    public class Projectile:BaseGameObject
    {
        //public new Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed {  get; set; }
        public float Rotation {  get; set; }
        public bool IsActive {  get; set; }

        public Vector2 MinBounds { get; set; }
        public Vector2 MaxBounds { get; set; }
        Texture2D texture;

        public Projectile(Texture2D texture, Vector2 startPos, Vector2 direction, float speed, float rotation, Vector2 minBounds, Vector2 maxBounds)
        {
            _texture = texture;
            Position = startPos;
            Direction = direction;
            Speed = speed;

            IsActive = true;
            Rotation = rotation;
            MinBounds = minBounds;
            MaxBounds = maxBounds;
        }

        public override void Update(GameTime gameTime)
        {
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Check Boundaries here
            if (Position.X < MinBounds.X || Position.X > MaxBounds.X || Position.Y < MinBounds.Y || Position.Y > MaxBounds.Y)
            {
                IsActive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive) {
                spriteBatch.Draw(_texture, Position, null, Color.White, MathHelper.ToRadians(Rotation), new Vector2(_texture.Width / 2, _texture.Height / 2), new Vector2(0.2f, 0.2f), SpriteEffects.None, 0);
            }

        }

        public void Deactivate()
        {
            IsActive = false;
        }

    }
}
