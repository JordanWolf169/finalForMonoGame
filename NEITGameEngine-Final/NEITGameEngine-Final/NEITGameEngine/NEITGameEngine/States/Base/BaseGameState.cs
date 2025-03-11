using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Enum;
using NEITGameEngine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.States.Base
{
    public abstract class BaseGameState
    {
        public Matrix Translation { get; set; }
        private readonly List<BaseGameObject> _gameObject = new List<BaseGameObject>();
        public abstract void LoadContent(ContentManager contentManager);
        public abstract void UnloadContent(ContentManager contentManager);

        public abstract void HandleInput(GameTime gameTime);
        //public abstract void SetInputManager();

        public event EventHandler<BaseGameState> OnStateSwitched;
        public event EventHandler<Events> OnEventNotification;

        //public void Initialize()
        //{
        //    SetInputManager();
        //}

        public void NotifyEvent(Events eventType, object argument = null)
        {
            OnEventNotification?.Invoke(this, eventType);

            foreach(var gameObject in _gameObject)
            {
                gameObject.OnNotify(eventType);
            }

        }


        protected void SwitchState(BaseGameState gameState)
        {
            OnStateSwitched?.Invoke(this, gameState);
        }

        protected void AddGameObject(BaseGameObject gameObject)
        {
            _gameObject.Add(gameObject);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Render(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            //Render stuff here
            foreach (var gameObject in _gameObject.OrderBy(a => a.zIndex))
            {
                gameObject.Render(spriteBatch);
            }

        }
    }
}
