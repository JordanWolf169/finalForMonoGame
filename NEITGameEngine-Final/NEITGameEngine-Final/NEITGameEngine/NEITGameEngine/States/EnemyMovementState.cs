using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.States.Base;
using NEITGameEngine.States;
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
using NEITGameEngine.ObjectPooling;
using NEITGameEngine.Menus;

namespace NEITGameEngine.States
{
    public class EnemyMovementState:BaseGameState
    {
      
        SpriteManager _spriteManager;
        PlayerSprite _playerSprite;
        DynamicGroundMap _map;
        WanderingEnemy wanderingEnemy;
        WaypointEnemy waypointEnemy;
        List<Vector2> waypoints;
        SpawnFactory spawnFactory;
        bool _previouslyPressedKey = false;
        PauseMenu _pauseMenu;
        ScoreMenu _scoreMenu;
        private Vector2 cameraOffset;

        private Rectangle winTrigger;


        public override void LoadContent(ContentManager contentManager)
        {
            //Add in the groundmap here
            _map = new DynamicGroundMap(contentManager, "FINALLVL1.json", "newSpriteSheet");
            AddGameObject(_map);

            winTrigger = new Rectangle(new Point(2816, 256), new Point(128, 128));
            

            //Player Sprite
            _spriteManager = new SpriteManager();
            _playerSprite = new PlayerSprite(_spriteManager,new(/*Globals.windowSize.X/2*/32, /*Globals.windowSize.Y/2*/ 64), 1000f, new(1,0), contentManager.Load<Texture2D>("bulletSprite"), _map._groundTiles[0,0]);
            _playerSprite.SetBoundary(_map.MapSize, _map.TileSize);
            Debug.WriteLine($"{_map.MapSize} : {_map.TileSize}");
            Texture2D playerSpriteSheetRun = contentManager.Load<Texture2D>("run");
            Texture2D playerSpriteSheetIdle = contentManager.Load<Texture2D>("Idle");
            _spriteManager.LoadAnimation("run", playerSpriteSheetRun, 48, 48, 8, 0.1f);
            _spriteManager.LoadAnimation("idle", playerSpriteSheetIdle, 48, 48, 10, 0.1f);
            AddGameObject(_playerSprite);

            //Adds Wandering enemy
            //wanderingEnemy = new WanderingEnemy(new(200f, 200f), 100f, 1, contentManager.Load<Texture2D>("Circle"), _map, _playerSprite);
            //AddGameObject(wanderingEnemy);

            //Adds Waypoint Enemy
            waypoints = new List<Vector2>();
            waypoints.Add(new Vector2(0f, 0f));
            waypoints.Add(new Vector2(Globals.windowSize.X - 100f, 0f));
            waypoints.Add(new Vector2(Globals.windowSize.X - 100f, Globals.windowSize.Y - 100f));
            waypoints.Add(new Vector2(0f, Globals.windowSize.Y - 100f));
            waypointEnemy = new WaypointEnemy(waypoints, 100, contentManager.Load<Texture2D>("Diamond"), _map, _playerSprite);
            //AddGameObject(waypointEnemy);

            spawnFactory = new SpawnFactory(contentManager.Load<Texture2D>("Circle"), 10, 3, new(300,300), _map, _playerSprite);
            AddGameObject(spawnFactory);

            SpriteFont font = contentManager.Load<SpriteFont>("gameFont");

            _scoreMenu = new ScoreMenu(font, new(-Globals.windowSize.X/2 + 20, -Globals.windowSize.Y / 2 + 20),_playerSprite);
            AddGameObject(_scoreMenu);

            Vector2 menuPosition = new Vector2(-25, Globals.windowSize.Y/2);
            _pauseMenu = new PauseMenu(font, menuPosition, this, _playerSprite, _scoreMenu);
            AddGameObject(_pauseMenu);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            
        }

        private void SetTranslation()
        {
            var distX = (Globals.windowSize.X/2) - _playerSprite.Position.X;
            var distY = (Globals.windowSize.Y/2) - _playerSprite.Position.Y;

            cameraOffset = new Vector2(distX, distY);

            Translation = Matrix.CreateTranslation(distX, distY, 0f);
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

                //if (cmd is GamePlayCommand.Pause)
                if(Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    if (!_previouslyPressedKey)
                    {
                        NotifyEvent(Globals.Paused ?Events.RESUMED:Events.PAUSED);
                        _previouslyPressedKey = true;
                    }
                    
                }
                else
                {
                    _previouslyPressedKey = false;
                }

                if (Globals.Paused)
                {
                    return;
                }


                //All other inputs would go below
                //if (cmd is GamePlayCommand.moveLeft)
                //{
                //    _playerSprite.MoveLeft();

                //}
                //if (cmd is GamePlayCommand.moveRight)
                //{
                //    _playerSprite.MoveRight();

                //}



                //if (cmd is GamePlayCommand.moveUp)
                //{
                //    _playerSprite.MoveUp();
                //}

                //if(cmd is GamePlayCommand.moveDown)
                //{
                //    _playerSprite.MoveDown();
                //}

                if (cmd is GamePlayCommand.moveLeft)
                {

                    Vector2 newPosition = _playerSprite.Position + new Vector2(-_playerSprite.speed, 0);

                    Vector2 newPosition2 = _playerSprite.Position + new Vector2(0,_playerSprite.BoxCollider.Height/2) + new Vector2(-_playerSprite.speed, 0);

                    //grabs the new player position and camera offset
                    if (!_map.IsTileSolid(newPosition, cameraOffset) 
                    && !_map.IsTileSolid(newPosition2, cameraOffset))
                    {
                        _playerSprite.MoveLeft();
                    }
                   

                }
                if (cmd is GamePlayCommand.moveRight)
                {
                Vector2 newPosition = (_playerSprite.Position + new Vector2(_playerSprite.BoxCollider.Width/2,0)) + new Vector2(_playerSprite.speed, 0);
                Vector2 newPosition2 = (_playerSprite.Position + new Vector2(_playerSprite.BoxCollider.Width/2,_playerSprite.BoxCollider.Height/2)) + new Vector2(_playerSprite.speed, 0);
                
               
                    //Debug.WriteLine($"Upper Corner: {newPosition} Lower Corner{newPosition2}");
                    //grabs the new player position and camera offset
                    if (!_map.IsTileSolid(newPosition, cameraOffset) 
                    && !_map.IsTileSolid(newPosition2,cameraOffset))
                    {
                        _playerSprite.MoveRight();
                    }

                }

                if (cmd is GamePlayCommand.moveUp)
                {
                    Vector2 newPosition = (_playerSprite.Position)  + new Vector2(0, -_playerSprite.speed);
                    
                    Vector2 newPosition2 = (_playerSprite.Position + new Vector2(_playerSprite.BoxCollider.Width/2,0))  + new Vector2(0, -_playerSprite.speed);

                    //grabs the new player position and camera offset
                    if (!_map.IsTileSolid(newPosition, cameraOffset) && !_map.IsTileSolid(newPosition2, cameraOffset))
                    {
                        _playerSprite.MoveUp();
                    }
                }

                if (cmd is GamePlayCommand.moveDown)
                {
                    Vector2 newPosition = (_playerSprite.Position + new Vector2(0, _playerSprite.BoxCollider.Height/2)) + new Vector2(0, _playerSprite.speed);
                    Vector2 newPosition2 = (_playerSprite.Position + new Vector2(_playerSprite.BoxCollider.Width/2, _playerSprite.BoxCollider.Height/2)) + new Vector2(0, _playerSprite.speed);

                    //grabs the new player position and camera offset
                    if (!_map.IsTileSolid(newPosition, cameraOffset) && !_map.IsTileSolid(newPosition2, cameraOffset))
                    {
                        _playerSprite.MoveDown();
                    }
                }

                if (cmd is GamePlayCommand.moving)
                {
                    _playerSprite.Moving();
                }

                if (cmd is GamePlayCommand.idle)
                {
                    _playerSprite.Idle();
                }
               
            });


        }
        public override void Update(GameTime gameTime)
        {
            if (Globals.Paused == true)
            {
                _pauseMenu.HandleInput(gameTime);
                return;
            }
            _playerSprite.Update(gameTime);
            //wanderingEnemy.Update(gameTime);
            //waypointEnemy.Update(gameTime);
            spawnFactory.Update(gameTime);
            CheckCollisions(_playerSprite.shooting.GetActiveProjectiles(), spawnFactory.GetActiveEnemies());
            SetTranslation();

            if (_playerSprite.BoxCollider.Intersects(winTrigger))
            {
                _scoreMenu._score += 50;
                SwitchState(/*new WinState()*/ new Lvl2State(_scoreMenu, _spriteManager));
            }

        }

        private void CheckCollisions(List<Projectile> projectiles, List<WanderingEnemy> enemies)
        {
            foreach (var projectile in projectiles) { 
                if(!projectile.IsActive) continue;
                foreach(var enemy in enemies)
                {
                    if(!enemy.IsActive) continue;
                    if (projectile.BoxCollider.Intersects(enemy.BoxCollider))
                    {
                        projectile.Deactivate();
                        enemy.Deactivate();
                        _scoreMenu.AddScore(10);

                        //Debug.WriteLine("Collision Detected!");
                    }
                }
            }
           
        }
    }
}
