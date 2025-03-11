using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects;
using NEITGameEngine.Objects.Base;

namespace NEITGameEngine.Menus
{
    public class ScoreMenu:BaseGameObject
    {
        public int _score;
        Vector2 _position;
        SpriteFont _font;
        PlayerSprite _playerSprite;
        ContentManager contentManager;

        public ScoreMenu(SpriteFont font, Vector2 position, PlayerSprite playerSprite)
        {
            _score = 0;
            _font = font;
            _position = position;
            _playerSprite = playerSprite;
        }

        public void AddScore(int points)
        {
            _score += points;
        }

        public void ResetScore()
        {
            _score = 0;
        }

        public int GetScore()
        {
            return _score;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            string scoreText = $"Score: {_score}";
            spriteBatch.DrawString(_font, scoreText, _playerSprite.Position + _position, Color.White);
        }
    }
}
