using NEITGameEngine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Animation;
using System.Diagnostics;
using NEITGameEngine.Components;
using Microsoft.Xna.Framework.Content;

namespace NEITGameEngine.Objects
{
    public class PlayerSprite:BaseGameObject
    {
        public float speed = 10.0f;
        private SpriteManager _spriteManager;
        private ContentManager contentManager;
        public bool IsMoving { get; set; }

        private bool _facingLeft; //Tracks player direction left or right

        private LevelSprite _bgOrigin;
        Vector2 _minBoundary, _maxBoundary;

        public Shooting shooting { get; }
        Vector2 movementDirection = new(1,0);
        float projectileRotation;

        public void SetBoundary(Point mapSize, Point tileSize)
        {
            _minBoundary = new((-tileSize.X) + _bgOrigin.Origin.X, (-tileSize.Y) + _bgOrigin.Origin.Y);
            _maxBoundary = new(mapSize.X - (tileSize.X/2) - _bgOrigin.Origin.X, mapSize.Y - (tileSize.Y/2)- _bgOrigin.Origin.Y);
        }

        public PlayerSprite(SpriteManager spriteManager, Vector2 playerPos, float? projectileSpeed = null, Vector2? projectileDir = null, Texture2D projectileSprite = null, LevelSprite groundSprite = null )
        {
            _spriteManager = spriteManager;
            _bgOrigin = groundSprite;
            Position = playerPos;

            if (projectileSpeed.HasValue && projectileDir.HasValue)
            {
                Debug.WriteLine(projectileSprite);
                //We know there is shooting
                shooting = new Shooting(projectileSprite, projectileSpeed.Value, projectileDir.Value, 10);
            }
        }

        public override Rectangle BoxCollider 
        {
            get
            {
                if (_spriteManager != null)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, _spriteManager._currentAnimation._frameWidth, _spriteManager._currentAnimation._frameHeight);
                }
                return Rectangle.Empty;
            }
        }

        public void MoveLeft()
        {
            Position = new Vector2(Position.X - speed, Position.Y);
            Position = Vector2.Clamp(Position, _minBoundary, _maxBoundary);
            movementDirection = new Vector2(-1,0);
            projectileRotation = 180;
            _facingLeft = true;
            
        }

        

        public void MoveRight() {
            Position = new Vector2(Position.X + speed, Position.Y);
            Position = Vector2.Clamp(Position, _minBoundary, _maxBoundary);
            movementDirection = new Vector2(1, 0);
            projectileRotation = 0;
            _facingLeft = false;
           
        }
        public void MoveUp()
        {
            Position = new Vector2(Position.X, Position.Y - speed);
            Position = Vector2.Clamp(Position, _minBoundary, _maxBoundary);
            projectileRotation = 270;

            movementDirection = new Vector2(0, -1);
        }

        public void MoveDown()
        {
            Position = new Vector2(Position.X, Position.Y + speed);
            Position = Vector2.Clamp(Position, _minBoundary, _maxBoundary);
            movementDirection = new Vector2(0,1);
            projectileRotation = 90;

        }

        public void Moving()
        {
            IsMoving = true;
        }

        public void Idle()
        {
            IsMoving = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsMoving)
            {
               _spriteManager.PlayAnimation("run");
            }
            else
            {
                _spriteManager.PlayAnimation("idle");
            }
            _spriteManager.Update(gameTime);
            shooting.Update(gameTime, new Vector2(Position.X + _spriteManager._currentAnimation._frameWidth / 2, Position.Y + _spriteManager._currentAnimation._frameHeight/2), movementDirection, projectileRotation,_minBoundary, _maxBoundary);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            SpriteEffects flipEffect = _facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _spriteManager.Draw(spriteBatch, Position);
           
        }

        public override void Render(SpriteBatch spriteBatch)
        {
           
            SpriteEffects flipEffect = _facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _spriteManager.Draw(spriteBatch, Position, flipEffect);
            shooting.Draw(spriteBatch);
        }
    }
}
