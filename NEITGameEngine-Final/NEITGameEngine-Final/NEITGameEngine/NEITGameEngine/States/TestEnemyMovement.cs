using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.States.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEITGameEngine.Enum;
using Microsoft.Xna.Framework;
using NEITGameEngine.Inputs.Base;
using NEITGameEngine.Inputs.GamePlayState;
using NEITGameEngine.Objects;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Animation;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using NEITGameEngine.World;

namespace NEITGameEngine.States
{
    public class TestEnemyMovementState:BaseGameState
    {
        
        SpriteManager _spriteManager;
        PlayerSprite _playerSprite;
        PatrolEnemy patrolEnemy;
        ChaseEnemy chaseEnemy;
        WaypointEnemy waypointEnemy;
        List<Vector2> waypoints;
        WanderingEnemy wanderingEnemy;


        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            //Adds Patrol Enemy type
            patrolEnemy = new PatrolEnemy(new(Globals.windowSize.X / 2, Globals.windowSize.Y / 2), 100f, 100f, 100f, contentManager.Load<Texture2D>("Circle"));
            AddGameObject(patrolEnemy);

            //Adds ChaseEnemy
            chaseEnemy = new ChaseEnemy(new(Globals.windowSize.X - 100, 100), 100, 250, contentManager.Load<Texture2D>("Circle"));
            AddGameObject(chaseEnemy);

            //Adds Waypoint Enemy
            waypoints = new List<Vector2>();
            waypoints.Add(new Vector2(0f,0f));
            waypoints.Add(new Vector2(Globals.windowSize.X - 100f, 0f));
            waypoints.Add(new Vector2(Globals.windowSize.X - 100f, Globals.windowSize.Y - 100f));
            waypoints.Add(new Vector2(0f, Globals.windowSize.Y - 100f));
            waypointEnemy = new WaypointEnemy(waypoints, 100, contentManager.Load<Texture2D>("Diamond"));
            AddGameObject(waypointEnemy);

            //Adds Wandering Enemy;
            wanderingEnemy = new WanderingEnemy(new(600f, 100f), 100, 1, contentManager.Load<Texture2D>("Diamond"));
            AddGameObject(wanderingEnemy);

            //Adds Player
            _spriteManager = new SpriteManager();
            _playerSprite = new PlayerSprite(_spriteManager, new(Globals.windowSize.X / 2, Globals.windowSize.Y / 2));
            Texture2D playerSpriteSheetRun = contentManager.Load<Texture2D>("run");
            Texture2D playerSpriteSheetIdle = contentManager.Load<Texture2D>("Idle");
            _spriteManager.LoadAnimation("run", playerSpriteSheetRun, 48, 48, 8, 0.1f);
            _spriteManager.LoadAnimation("idle", playerSpriteSheetIdle, 48, 48, 10, 0.1f);
            AddGameObject(_playerSprite);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            
        }

        //Old GamePlayState Input Code
        //public override void HandleInput()
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        //    {
        //        //Notify Event System
        //        NotifyEvent(Events.GAME_QUIT);
        //    }

        //}

        public override void HandleInput(GameTime gameTime)
        {
            var _inputManager = new InputManager(new GamePlayInputMapper());
            
            _inputManager.GetCommands(cmd =>
            {
               
                if (cmd is GamePlayCommand.GameExit)
                {
                    NotifyEvent(Events.GAME_QUIT);
                }

                //All other inputs would go below
                if (cmd is GamePlayCommand.moveLeft)
                {
                    _playerSprite.MoveLeft();
                   
                }
                if (cmd is GamePlayCommand.moveRight)
                {
                    _playerSprite.MoveRight();
                   
                }

                

                if (cmd is GamePlayCommand.moveUp)
                {
                    _playerSprite.MoveUp();
                }

                if(cmd is GamePlayCommand.moveDown)
                {
                    _playerSprite.MoveDown();
                }

                if(cmd is GamePlayCommand.moving)
                {
                    _playerSprite.Moving();
                }

                if (cmd is GamePlayCommand.idle)
                {
                    _playerSprite.Idle();
                }
               
            });

            
        }
        public override void Update(GameTime gameTime) {
            _playerSprite.Update(gameTime);
            patrolEnemy.Update(gameTime);
            chaseEnemy.Update(gameTime, _playerSprite.Position);
            waypointEnemy.Update(gameTime);
            wanderingEnemy.Update(gameTime);
        }
    }
}
