using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.World;
using NEITGameEngine.States;
using NEITGameEngine.States.Base;
using NEITGameEngine.SaveDataSystem;
using Microsoft.Xna.Framework.Content;

namespace NEITGameEngine
{
    public class MainGame : Game
    {
        private BaseGameState _currentGameState;
        ContentManager contentManager;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SaveSystem _saveSystem;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Globals.windowSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _saveSystem = new SaveSystem("gamesave.json");
            Globals.PlayerData = _saveSystem;
            Globals.Graphics = _graphics;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here 
            //SwitchGameState(new EnemyMovementState());
            SwitchGameState(new MainMenu());
            //SwitchGameState(new LoadExample());
            //SwitchGameState(new TestEnemyMovementState());
            //SwitchGameState(new FetchRequest());
        }

        private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e) {
            SwitchGameState(e);
        }

        private void SwitchGameState(BaseGameState gameState)
        {
            if (_currentGameState != null)
            {
                // Unload/Switch state
                _currentGameState.OnStateSwitched -= CurrentGameState_OnStateSwitched;
                _currentGameState.OnEventNotification -= _currentGameState_OnEventNotification;
                _currentGameState.UnloadContent(Content);
            }

            _currentGameState = gameState;
            _currentGameState.LoadContent(Content);
            _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;

        }

        private void _currentGameState_OnEventNotification(object sender, Enum.Events e)
        {
            switch (e)
            {
                case Enum.Events.GAME_QUIT:
                    Exit();
                    break;

                case Enum.Events.PAUSED:
                    Globals.Paused = true;
                    break;
                case Enum.Events.RESUMED:
                    Globals.Paused = false;
                    break;
            }
        }

        protected override void UnloadContent()
        {
            _currentGameState?.UnloadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            
            // TODO: Add your update logic here
            _currentGameState.HandleInput(gameTime);
            _currentGameState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: _currentGameState.Translation);
            _currentGameState.Render(_spriteBatch, contentManager);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
