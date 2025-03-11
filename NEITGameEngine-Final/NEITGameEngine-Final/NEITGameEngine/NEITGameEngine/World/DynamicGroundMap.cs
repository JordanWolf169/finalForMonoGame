using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NEITGameEngine.Objects;
using NEITGameEngine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.Threading;


namespace NEITGameEngine.World
{
    public class DynamicGroundMap : BaseGameObject
    {
        Point _mapSize = new Point(13, 13);
        public LevelSprite[,] _groundTiles;
        public Point TileSize { get; set; }
        public Point MapSize { get; set; }

        private int[,] _collisionGrid;

        private ContentManager contentManager;

        private Texture2D collsionSprite;

        public class LevelData 
        {
            public string name { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int[][] BackgroundLayer { get; set; }
            public int[][] ForegroundLayer { get; set; }
            public int[][] ColisionLayer { get; set; }
        }

        public DynamicGroundMap(ContentManager contentManager, string levelFile, string tileSetFile)
        {
            //Load the level data
            LevelData levelData = LoadLevelData(levelFile);

            _collisionGrid = ConvertTo2DArray(levelData.ColisionLayer);
            collsionSprite = contentManager.Load<Texture2D>("rocks 1");

            Debug.WriteLine($"Name: {levelData.name}");
            Debug.WriteLine($"Width: {levelData.Width}");
            Debug.WriteLine($"Height: {levelData.Height}");

            //Convert the jagged array to 2D array
            int[,] backgroundTiles = ConvertTo2DArray(levelData.BackgroundLayer);

            //Load the tile set texture
            Texture2D tileSet = contentManager.Load<Texture2D>(tileSetFile);
            _mapSize = new Point(levelData.Width, levelData.Height);
            //Gets the uniform tile size
            int tileWidth = 64;
            int tileHeight = 64;
            TileSize = new(tileWidth, tileHeight);
            MapSize = new(TileSize.X * _mapSize.X, TileSize.Y * _mapSize.Y);

            //Initialize the array of tiles for the layer in the json
            _groundTiles = new LevelSprite[levelData.Width, levelData.Height];

            for(int y = 0; y < levelData.Height; y++)
            {
                for (int x = 0; x < levelData.Width; x++)
                {
                    if (x < backgroundTiles.GetLength(0) && y < backgroundTiles.GetLength(1)) {
                        int tileID = backgroundTiles[x, y];

                        if (tileID >= 0)
                        {
                            int tilesPerRow = tileSet.Width / tileWidth;
                            int tileX = (tileID % tilesPerRow) * tileWidth;
                            int tileY = (tileID / tilesPerRow) * tileHeight;

                            Rectangle sourceRect = new Rectangle(tileX, tileY, tileWidth, tileHeight);
                            //Come back here to add the masking
                            _groundTiles[x, y] = new LevelSprite(tileSet, new Vector2((x * tileWidth)+tileWidth, (y * tileHeight)+tileHeight), sourceRect);
                        }

                    }
                }
            }
            PrintCollisionGrid();

        }

        public void PrintCollisionGrid()
        {
            for (int y = 0; y < _collisionGrid.GetLength(1); y++) {
                string row = "";
                for (int x = 0; x < _collisionGrid.GetLength(0); x++)
                {
                    row += _collisionGrid[x, y] + " ";
                }
                Debug.WriteLine(row);

            }
        }
        private LevelData LoadLevelData(string filename)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Levels", filename);

            //Error handling
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Level file not found: {path}");
            }

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<LevelData>(json);
        }

        private int[,] ConvertTo2DArray(int[][] jaggedArray)
        {
            if (jaggedArray == null || jaggedArray.Length == 0) {
                return new int[0, 0];
            }

            int rows = jaggedArray.Length;
            int cols = jaggedArray.Max(row => row?.Length ?? 0);

            int[,] twoDimensionalArray = new int[rows, cols];

            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < (jaggedArray[i]?.Length ?? 0); j++)
                {
                    twoDimensionalArray[i, j] = jaggedArray[i][j];
                }
            }

            return twoDimensionalArray;
        }

        public bool IsTileSolid(Vector2 worldPosition, Vector2 cameraOffset)
        {
            //Adjust position for camera movement
            //Vector2 adjustedPosition = worldPosition - cameraOffset;

            int tileX = (int)(worldPosition.X / TileSize.X);
            int tileY = (int)(worldPosition.Y / TileSize.Y);

            //Anything out of bounds will be treated as solid
            if (tileX < 0 || tileX  >= _collisionGrid.GetLength(0) || 
                tileY < 0 || tileY >= _collisionGrid.GetLength(1)||
                _collisionGrid[tileX,tileY] > 0)
            {
                return true;
            }

            //Debug.WriteLine($"Checking tile at X:{tileX}, Y:{tileY}, Solid?:{_collisionGrid[tileX, tileY]}");

            return _collisionGrid[tileX, tileY] > 0;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < _mapSize.Y; y++)
            {
                for (int x = 0; x < _mapSize.X; x++)
                {
                    _groundTiles[x, y].Draw(spriteBatch);
                }
            }

            //Draw the collsion tiles
            for (int y = 0; y < _collisionGrid.GetLength(1); y++)
            {
                for (int x = 0; x < _collisionGrid.GetLength(0); x++)
                {
                    if (_collisionGrid[x,y] > 0)
                    {
                        Vector2 tilePosition = new Vector2((x * TileSize.X)+ TileSize.X/2, (y * TileSize.Y)+TileSize.Y/2);

                        spriteBatch.Draw(collsionSprite, new Rectangle((int)tilePosition.X, (int)tilePosition.Y, TileSize.X, TileSize.Y), Color.White);

                    }
                }
            }
        }


    }


}
