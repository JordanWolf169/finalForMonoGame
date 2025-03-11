using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NEITGameEngine.Objects;
using NEITGameEngine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.World
{
    public class GroundMap:BaseGameObject
    {
        Point _mapSize = new Point(13,13);
        public GroundSprite[,] _groundTiles;
        public Point TileSize { get; set; }
        public Point MapSize { get; set; }

        public GroundMap(ContentManager contentManager) {
            _groundTiles = new GroundSprite[_mapSize.X, _mapSize.Y];
            List<Texture2D> textures = new(1);

            for(int i = 0; i<2; i++)
            {
                textures.Add(contentManager.Load<Texture2D>("groundtile"));
            }

            TileSize = new(textures[0].Width, textures[0].Height);
            MapSize = new(TileSize.X * _mapSize.X, TileSize.Y * _mapSize.Y);

            for (int y = 0; y<_mapSize.Y; y++)
            {
                for (int x = 0; x<_mapSize.X; x++)
                {
                    //This is where we could randomize it
                    _groundTiles[x, y] = new(textures[0], new(x * TileSize.X, y * TileSize.Y));
                }
            }
        
        }

        public override void Render(SpriteBatch spriteBatch) {
            for (int y = 0; y<_mapSize.Y; y++)
            {
                for (int x = 0; x<_mapSize.X;x++)
                {
                    _groundTiles[x,y].Draw(spriteBatch);
                }
            }
        }


    }


}
