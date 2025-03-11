using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine
{
    public class Editor:Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _tileset;
        private Texture2D _editorBG;
        private List<Rectangle> _tileRectangles;
        private int _tileSize = 64;

        private int[,] _grid;
        private int _gridWidth = 50;
        private int _gridHeight = 50;

        private int _paletteOffsetY = 955;

        private Point _selectedTile = new Point(0, 0);

        private int _gridOffsetX = 0;
        private int _gridOffsetY = 0;

        private Point _lastMousePosition = Point.Zero;
        private bool _isPanning = false;

        private int toolPaletteX;
        private int toolPaletteWidth;
        private int tilePaletteY;
        private int tilePaletteHeight;

        private int toolPalettePadding = 10;

        private string _inputWidth = "50";
        private string _inputHeight = "50";
        private bool _isEditingWidth = false; 
        private bool _isEditingHeight = false;

        private KeyboardState _previousKKeyboarState;

        private int[,]  _backgroundLayer; 
        private int[,]  _foregroundLayer; 
        private int[,]  _collisionLayer;

        private string _activeLayer = "Background";

        private string _levelName = "NewLevel";
        private bool _isEditingLevel = false;


        public class LevelData { 
            public string name { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int[][] BackgroundLayer { get; set; }
            public int[][] ForegroundLayer { get; set; }
            public int[][] ColisionLayer { get; set; }
        }

        private LevelLoaderMenu _levelLoaderMenu;
        private bool _isLoadingLevelOpen;


        public Editor()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
            };

            

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _grid = new int[_gridWidth, _gridHeight];
            _backgroundLayer = new int[_gridWidth, _gridHeight];
            _foregroundLayer = new int[_gridWidth, _gridHeight];
            _collisionLayer = new int[_gridWidth, _gridHeight];
            _previousKKeyboarState = Keyboard.GetState();


            //Code for a test grid
            //for (int y = 0; y < _gridHeight; y++)
            //{
            //    for (int x = 0; x < _gridWidth; x++)
            //    {
            //        _grid[x, y] = 1;
            //    }

            //}
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            try {
                _tileset = Content.Load<Texture2D>("newSpriteSheet");
                _editorBG = Content.Load<Texture2D>("EditorBG");
                Debug.WriteLine("Tilesets loaded successfully!");
            }
            catch {
                Debug.WriteLine("Failed to load tile set");
            }

            GenerateTileRectangles();
            _levelLoaderMenu = new LevelLoaderMenu(_spriteBatch, Content.Load<SpriteFont>("gameFont"), LoadLevelFromJson);
        }

        private void GenerateTileRectangles()
        {
            _tileRectangles = new List<Rectangle>();

            for (int y = 0; y < _tileset.Height / _tileSize; y++)
            {
                for (int x = 0; x < _tileset.Width / _tileSize; x++)
                {
                    //Add in tile rectangles
                    _tileRectangles.Add(new Rectangle(x * _tileSize, y * _tileSize,_tileSize, _tileSize));

                }
            }

            toolPaletteX = GraphicsDevice.Viewport.Width - 200;
            toolPaletteWidth = 200;
            tilePaletteY = _paletteOffsetY;
            tilePaletteHeight = 128;// (_tileRectangles.Count / (GraphicsDevice.Viewport.Width / _tileSize) + 1) * _tileSize;
        }

        private void HandleMouseClick(MouseState mouseState)
        {
            int inputX = toolPaletteX + toolPalettePadding;
            int inputY = toolPalettePadding + 75;

            //Handle Clicking on the width field
            if (mouseState.LeftButton == ButtonState.Pressed && 
                mouseState.X >= inputX + 60 &&
                mouseState.X <= inputX + 150 &&
                mouseState.Y >= inputY &&
                mouseState.Y <= inputY + 20)
            {
                _isEditingWidth = true;
                _isEditingHeight = false;
                _inputWidth = "";
            }
            //Handle clicking on the height field
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.X >= inputX + 60 &&
                mouseState.X <= inputX + 150 &&
                mouseState.Y >= inputY + 30 &&
                mouseState.Y <= inputY + 50)
            {
                _isEditingHeight = true;
                _isEditingWidth = false;
                _inputHeight = "";
            }

        }

        private void HandleKeyboardInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            //Check to see if we are editing width
            if (_isEditingWidth)
            {
                foreach(var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!_previousKKeyboarState.IsKeyDown(key))
                    {
                        if (key >= Keys.D0 && key <= Keys.D9)//Detects Numeric Keys 0-9
                        {
                            _inputWidth += (key - Keys.D0).ToString();
                        }
                        else if(key == Keys.Back && _inputWidth.Length > 0)//Handles backspace and deleting values
                        {
                            _inputWidth = _inputWidth.Substring(0, _inputWidth.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            _isEditingWidth = false;
                            //Change the grid size
                            ApplyTileMapSizeChange();
                        }
                    }
                }
            }

            //Check to see if we are editing height
            if (_isEditingHeight)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!_previousKKeyboarState.IsKeyDown(key))
                    {
                        if (key >= Keys.D0 && key <= Keys.D9)//Detects Numeric Keys 0-9
                        {
                            _inputHeight += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && _inputHeight.Length > 0)//Handles backspace and deleting values
                        {
                            _inputHeight = _inputHeight.Substring(0, _inputHeight.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            _isEditingHeight = false;
                            //Change the grid size
                            ApplyTileMapSizeChange();
                        }
                    }
                }
            }

            if (_isEditingLevel)
            {
                foreach(var key in Keyboard.GetState().GetPressedKeys())// new keybutton presses
                {
                    if (!_previousKKeyboarState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z )//handles letters and numbers
                        {
                            _levelName += key.ToString();
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            _levelName += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && _levelName.Length > 0)//handls backspace
                        {
                            _levelName = _levelName.Substring(0, _levelName.Length - 1);
                        }
                        else if (key == Keys.Enter)// Done naming level
                        {
                            _isEditingLevel = false;
                        }
                    }
                   
                }
            }

            _previousKKeyboarState = currentKeyboardState;
        }

        private void ApplyTileMapSizeChange()
        {
            if (int.TryParse(_inputWidth, out int newWidth) && int.TryParse(_inputHeight, out int newHeight))
            {
                if (newWidth > 0 && newHeight > 0)
                {
                    //Apply changes to tilemap grid
                    ResizeTileMap(newWidth, newHeight);
                }
            }
        }

        private void SaveLevelToJson()
        {
            var levelData = new LevelData { 
               name = _levelName,
               Width = _gridWidth,
               Height = _gridHeight,
               BackgroundLayer = ConvertToJaggedArray(_backgroundLayer),
               ForegroundLayer = ConvertToJaggedArray(_foregroundLayer),
               ColisionLayer = ConvertToJaggedArray(_collisionLayer)
            };

            string levelDirectory = "Levels";
            if (!Directory.Exists(levelDirectory))
            {
                Directory.CreateDirectory(levelDirectory);
            }

            string filePath = Path.Combine(levelDirectory, $"{_levelName}.json");

            string json = System.Text.Json.JsonSerializer.Serialize(levelData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true});
            
            try 
            {
                System.IO.File.WriteAllText(filePath,json);
                Debug.WriteLine($"Level saved as {_levelName}.json");
            }
            catch(Exception ex) 
            {
                Debug.WriteLine($"Failed to save level: {ex.Message}");
            }
        }

        private int[][] ConvertToJaggedArray(int[,] multidimesnionalArray)
        {
            int rows = multidimesnionalArray.GetLength(0);
            int cols = multidimesnionalArray.GetLength(1);

            int[][] jaggedArray = new int[rows][];

            for(int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for(int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = multidimesnionalArray[i, j];
                }
            }

            return jaggedArray;
        }

        private void LoadLevelFromJson(string filePath)
        {
            try {

                if (filePath == "cancel")
                {
                    _isLoadingLevelOpen = false;
                    return;
                }
                //Read the jsn content
                string json = System.IO.File.ReadAllText(filePath);
                
                //Deserialize the Json into our LevelData object
                LevelData levelData = System.Text.Json.JsonSerializer.Deserialize<LevelData>(json);

                //Apply the loaded data to the Editor Variables
                _levelName = levelData.name;
                _gridWidth = levelData.Width;
                _gridHeight = levelData.Height;

                _backgroundLayer = ConvertTo2DArray(levelData.BackgroundLayer);
                _foregroundLayer = ConvertTo2DArray(levelData.ForegroundLayer);
                _collisionLayer = ConvertTo2DArray(levelData.ColisionLayer);
                _isLoadingLevelOpen = false;
                Debug.WriteLine($"Level {_levelName} loaded.");
            } 
            catch (Exception ex) {
                Debug.WriteLine($"Failed to load level: {ex.Message}");
            }
        }

        private int[,] ConvertTo2DArray(int[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            int[,] twoDimensionalArray = new int[rows, cols];

            for (int i = 0; i < rows; i ++) {
                for (int j = 0; j < cols; j++)
                {
                    twoDimensionalArray[i, j] = jaggedArray[i][j];
                }
            }

            return twoDimensionalArray;
        }

        private void ResizeTileMap(int newWidth, int newHeight)
        {
            int[,] newGrid = new int[newWidth, newHeight];
            int[,] bgGrid = new int[newWidth, newHeight];
            int[,] fgGrid = new int[newWidth, newHeight];
            int[,] colGrid = new int[newWidth, newHeight];

            //Copy the old grid data into the new grid
            for(int y = 0; y < Math.Min(newHeight, _grid.GetLength(1)); y++)
            {
                for (int x = 0; x < Math.Min(newWidth, _grid.GetLength(0)); x++)
                {
                    newGrid[x,y] = _grid[x,y];
                    bgGrid[x,y] = _backgroundLayer[x,y];
                    fgGrid[x,y] = _foregroundLayer[x,y];
                    colGrid[x,y] = _collisionLayer[x,y];
                }

            }

            _grid = newGrid;
            _gridWidth = newWidth;
            _gridHeight = newHeight;
            _backgroundLayer = bgGrid;
            _foregroundLayer = fgGrid;
            _collisionLayer = colGrid;
        }

        private void HandleTilemapPanning(MouseState mouseState)
        {
            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                if (!_isPanning)
                {
                    _isPanning = true;
                    _lastMousePosition = mouseState.Position;
                }
                else
                {
                    //Calculates delta
                    Point delta = mouseState.Position - _lastMousePosition;
                    //Update the last position
                    _lastMousePosition = mouseState.Position;
                    //Update the grid offset
                    _gridOffsetX -= (int)Math.Round((float)delta.X / _tileSize);
                    _gridOffsetY -= (int)Math.Round((float)delta.Y / _tileSize);

                    //Values to keep grid in viewport
                    int maxOffsetX = Math.Max(0, _gridWidth - GraphicsDevice.Viewport.Width/_tileSize);
                    int maxOffsetY = Math.Max(0, _gridHeight - GraphicsDevice.Viewport.Height/_tileSize);

                    _gridOffsetX = Math.Clamp(_gridOffsetX, 0, maxOffsetX);
                    _gridOffsetY = Math.Clamp(_gridOffsetY, 0, maxOffsetY);

                    Debug.WriteLine($"Offset X: {_gridOffsetX}, Offset Y: {_gridOffsetY}");
                }

            }
            else
            {
                _isPanning = false;
            }
        }

        private void HandleLayerButtonClick(MouseState mouseState)
        {
            int buttonX = toolPaletteX + toolPalettePadding;
            int buttonY = toolPalettePadding + 140;

            string[] layers = { "Background", "Foreground", "Collision" };

            for(int i = 0; i < layers.Length; i++)
            {
                if (mouseState.LeftButton == ButtonState.Pressed &&
                    mouseState.X >=  buttonX &&
                    mouseState.X <= buttonX + 160 &&
                    mouseState.Y >= buttonY&&
                    mouseState.Y <= buttonY +30)
                {
                    _activeLayer = layers[i];
                    Debug.WriteLine(_activeLayer);
                    break;
                }
                buttonY += 40;
            }

            int inputX = toolPaletteX + toolPalettePadding;
            int inputY = toolPalettePadding + 250;

            //Input for naming level
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.X >= inputX + 90 && mouseState.X <= inputX + 250 &&
                mouseState.Y >= inputY && mouseState.Y <= inputY + 20)
            {
                _isEditingLevel = true;
                _levelName = "";
            }

            //Save button for saving level
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.X >= inputX && mouseState.X <= inputX + 160 &&
                mouseState.Y >= inputY +30 && mouseState.Y <= inputY + 60)
            {
                SaveLevelToJson();
            }

            //Load button for loading level
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.X >= inputX && mouseState.X <= inputX + 160 &&
                mouseState.Y >= inputY + 80 && mouseState.Y <= inputY + 120)
            {
                //LoadLevelFromJson("NewLevel.json");
                _isLoadingLevelOpen = true;
                _levelLoaderMenu.Show();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            HandleMouseClick(mouseState);
            HandleKeyboardInput();
            HandleTilemapPanning(mouseState);
            HandleLayerButtonClick(mouseState);

            TilePaletteSelection(mouseState, mousePosition);

            if (_isLoadingLevelOpen)
            {
                _levelLoaderMenu.Update(gameTime, mouseState);
            }

            //Placing selected tiles on level grid
            if (mouseState.LeftButton == ButtonState.Pressed && mousePosition.Y < _gridHeight * _tileSize && !_isLoadingLevelOpen)
            {
                PlaceTile(mouseState);
            }


            EraseTiles(mouseState, mousePosition);

            base.Update(gameTime);
        }


        private void EraseTiles(MouseState mouseState, Point mousePosition)
        {
            //Handles Right Click to erase tiles
            if (mouseState.RightButton == ButtonState.Pressed && mousePosition.Y < _paletteOffsetY)
            {
                int mouseX = mouseState.X;
                int mouseY = mouseState.Y;
                //Checks for Tool Palette area
                if (mouseX >= toolPaletteX && mouseX < toolPaletteX + toolPaletteWidth) return;
                //Checks for Tile Palette area
                if (mouseY >= tilePaletteY && mouseY < tilePaletteY + tilePaletteHeight) return;

                int x = (mouseState.X / _tileSize) + _gridOffsetX;
                int y = (mouseState.Y / _tileSize) + _gridOffsetY;

                if (x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight)
                {

                    switch (_activeLayer)
                    {
                        case "Background":
                            _backgroundLayer[x, y] = 0;
                            break;
                        case "Foreground":
                            _foregroundLayer[x, y] = 0;
                            break;
                        case "Collision":
                            _collisionLayer[x, y] = 0;
                            break;
                    }
                }
            }
        }

        private void TilePaletteSelection(MouseState mouseState, Point mousePosition)
        {
            int maxColumns = GraphicsDevice.Viewport.Width / _tileSize;
            //Tile palette selection
            if (mouseState.LeftButton == ButtonState.Pressed && mousePosition.Y >= _paletteOffsetY && mousePosition.Y < _paletteOffsetY + (_tileRectangles.Count / maxColumns + 1) * _tileSize)
            {
                int column = mousePosition.X / _tileSize;
                int row = (mousePosition.Y - _paletteOffsetY) / _tileSize;

                int tileIndex = row * maxColumns + column;

                if (tileIndex >= 0 && tileIndex < _tileRectangles.Count)
                {
                    _selectedTile = new Point(tileIndex % (_tileset.Width / _tileSize), tileIndex / (_tileset.Width / _tileSize));
                    Debug.WriteLine($"Selected Tile: {_selectedTile}");
                }
            }

        }

        private void PlaceTile(MouseState mouseState)
        {
            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;
            //Checks for Tool Palette area
            if (mouseX >= toolPaletteX && mouseX < toolPaletteX + toolPaletteWidth) return;
            //Checks for Tile Palette area
            if (mouseY >= tilePaletteY && mouseY < tilePaletteY + tilePaletteHeight) return;

            int x = (mouseState.X / _tileSize) + _gridOffsetX;
            int y = (mouseState.Y / _tileSize) + _gridOffsetY;


            if (x >= 0 && x<_gridWidth && y >=0 && y < _gridHeight)
            {
                int tileIndex = _selectedTile.Y * (_tileset.Width / _tileSize) + _selectedTile.X;

                switch (_activeLayer)
                {
                    case "Background":
                        _backgroundLayer[x, y] = tileIndex;
                        break;
                    case "Foreground":
                        _foregroundLayer[x, y] = tileIndex;
                        break;
                    case "Collision":
                        _collisionLayer[x, y] = tileIndex;
                        break;
                }
            }

            //if (x>=0 && x < _grid.GetLength(0) && y >= 0 && y < _grid.GetLength(1) && _selectedTile.X != -1)
            //{
            //    _grid[x,y] = _selectedTile.Y * (_tileset.Width/ _tileSize) + _selectedTile.X;
            //}
        }

        private void DrawResizeFields()
        {
            int inputX = toolPaletteX + toolPalettePadding;
            int inputY = toolPalettePadding + 75;

            //Draw labels on the inputs
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), "Width: ", new Vector2(inputX,inputY), Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), "Height: ", new Vector2(inputX,inputY +30), Color.White);

            //Draw the input fields
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), _inputWidth, new Vector2(inputX + 60, inputY), _isEditingWidth ? Color.Yellow: Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), _inputHeight, new Vector2(inputX + 60, inputY + 30), _isEditingHeight ? Color.Yellow : Color.White);

        }


        private void DrawToolsPanel()
        {
            int panelX = GraphicsDevice.Viewport.Width - 200;
            int panelWidth = 200;

            //Set background for panel
            _spriteBatch.Draw(_editorBG, new Rectangle(panelX, 0, panelWidth, GraphicsDevice.Viewport.Height),Color.Gray);

            // Add tools below

            // Display Current tile
            if (_selectedTile.X >= 0 && _selectedTile.Y >= 0)
            {
                int selectedTileIndex = _selectedTile.Y * (_tileset.Width / _tileSize) + _selectedTile.X;
                var selectedTileRectangle = new Rectangle(panelX + 10, 10, _tileSize, _tileSize);

                //Draw the selected sprite
                _spriteBatch.Draw(_tileset, selectedTileRectangle, _tileRectangles[selectedTileIndex], Color.White);

                //Show the tile name
                _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), $" Tile: {selectedTileIndex}", new Vector2(panelX + 10, 50), Color.White);
            }

            //Start calling out tool components here
            DrawLayerButtons();
            DrawSaveButtonAndLevelName();
            DrawLoadButton();
        }

        private void DrawSaveButtonAndLevelName()
        {
            int intputX = toolPaletteX + toolPalettePadding;
            int inputY = toolPalettePadding + 250;

            //Input Label for level name
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"),"Level Name:", new Vector2(intputX, inputY),Color.White);
            //Input for Level Name
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), _levelName, new Vector2(intputX + 90, inputY), _isEditingLevel ? Color.Yellow : Color.White);

            //Draw Save Level Button
            int buttonY = inputY + 30;
            _spriteBatch.Draw(_editorBG, new Rectangle(intputX,buttonY, 160,30), Color.Blue);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), "Save Level", new Vector2(intputX + 10, buttonY + 5), Color.White);

        }

        private void DrawLayerButtons()
        {
            int buttonX = toolPaletteX + toolPalettePadding;
            int buttonY = toolPalettePadding + 140;

            string[] layers = { "Background", "Foreground", "Collision" };

            foreach (var layer in layers)
            {
                //Highlight the active layer
                Color buttonColor = (_activeLayer == layer) ? Color.Yellow: Color.Gray;

                //Draw the button  background
                _spriteBatch.Draw(_editorBG, new Rectangle(buttonX, buttonY, 160, 30), buttonColor);

                //Draw the button text
                _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), layer, new Vector2(buttonX + 10, buttonY + 5), Color.White);
                buttonY += 40;
            }

        }

        private void DrawLoadButton() { 
            int inputX = toolPaletteX + toolPalettePadding;
            int inputY = toolPalettePadding + 320;

            _spriteBatch.Draw(_editorBG, new Rectangle(inputX, inputY, 160, 30), Color.Blue);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("gameFont"), "Load Level", new Vector2(inputX + 10, inputY + 5), Color.White);

        } 

        private void DrawLayer(int[,] layer, Color color)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    int gridX = x + _gridOffsetX;
                    int gridY = y + _gridOffsetY;

                    //Skips tiles outside grid boundary
                    if (gridX < 0 || gridY < 0 || gridX >= layer.GetLength(0) || gridY >= layer.GetLength(1))
                    {
                        continue;
                    }

                    int tileIndex = layer[gridX, gridY];
                    if(tileIndex != 0)
                        {
                            var sourceRectangle = _tileRectangles[tileIndex];

                        _spriteBatch.Draw(_tileset, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), sourceRectangle, color);
                        } 
                }
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            //Draws Level Editor Grid
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    int gridX = x + _gridOffsetX;
                    int gridY = y + _gridOffsetY;

                    if (gridX < 0 || gridY < 0 || gridX >= _grid.GetLength(0) || gridY >= _grid.GetLength(1)) continue;

                    int tileIndex = _grid[gridX, gridY];
                    if(tileIndex != 0)
                    {
                        var sourceRectangle = _tileRectangles[tileIndex];
                        _spriteBatch.Draw(_tileset, new Rectangle(x * _tileSize, y * _tileSize, _tileSize,_tileSize), sourceRectangle, Color.White);
                    }
                }
            }

            DrawLayer(_backgroundLayer, _activeLayer == "Background" ? Color.White : Color.White * 0.5f);
            DrawLayer(_foregroundLayer, _activeLayer == "Foreground" ? Color.White : Color.White * 0.5f);
            DrawLayer(_collisionLayer, _activeLayer == "Collision" ? Color.White : Color.White * 0.5f);

            

            //Draw the tools panel
            DrawToolsPanel();

            DrawResizeFields();

            //Draws Tile Palette Background
            _spriteBatch.Draw(_editorBG,new Rectangle(0, _paletteOffsetY, GraphicsDevice.Viewport.Width, 128), Color.Gray);

            //draw tile sprites for palette
            int maxColumns = GraphicsDevice.Viewport.Width / _tileSize;

            for (int i = 0; i< _tileRectangles.Count; i++)
            {
                //Calculate Row and Columns
                int row = i / maxColumns;
                int column = i % maxColumns;

                //Set position of each tile
                int x = column * _tileSize;
                int y = _paletteOffsetY + row * _tileSize;

                var sourceRectangle = _tileRectangles[i];
                _spriteBatch.Draw(_tileset, 
                    new Rectangle(x, y, _tileSize, _tileSize),
                    sourceRectangle, 
                    Color.White);
            }

            //Draw the tile selection
            if (_selectedTile.X >= 0 && _selectedTile.Y >= 0) 
            {
                int selectedIndex = _selectedTile.Y * (_tileset.Width / _tileSize) + _selectedTile.X;

                int selectedRow = selectedIndex / maxColumns;
                int selectedColumn = selectedIndex % maxColumns;

                var selectedTileRectangle = new Rectangle(
                    selectedColumn * _tileSize,
                    _paletteOffsetY + selectedRow * _tileSize,
                    _tileSize,
                    _tileSize
                    );

                _spriteBatch.Draw(
                    _tileset,
                    selectedTileRectangle,
                    _tileRectangles[selectedIndex],
                    Color.Red * 0.5f
                    );
            }

            if (_isLoadingLevelOpen)
            {
                _levelLoaderMenu.Draw(gameTime, _spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
