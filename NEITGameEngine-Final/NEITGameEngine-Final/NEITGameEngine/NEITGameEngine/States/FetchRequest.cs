using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.States.Base;
using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Metadata;

namespace NEITGameEngine.States
{
    public class FetchRequest : BaseGameState
    {
        public static GraphicsDeviceManager _graphics;
        SpriteFont _font;
        SpriteBatch _spriteBatch;
        PlayerData _playerData;
        List<PlayerData> _players = new List<PlayerData>();

        int toolPaletteX;
        int toolPaletteWidth;
        int _paletteOffsetY;
        int tilePaletteY;
        int tilePaletteHeight;
        private Texture2D _editorBG;

        private int toolPalettePadding = 10;
        private bool _isViewingAll;
        private bool _isAddingNew;
        private string _viewAll;
        private string _addNew;
        private string _editPlayer = "Edit Player";
        private string _findPlayer = "Find Player";
        private string _deletePlayer = "Delete Player";

        private string _screenName = "Screen Name";
        private string _firstName = "First Name";
        private string _lastName = "Last Name";
        private string _dateStarted = "Date Started";
        private string _score;

        private bool _isEditingScreenName;
        private bool _isEditingFirstName;
        private bool _isEditingLastName;
        private bool _isEditingDateStarted;
        private bool _isEditingScore;
        private bool _isAddingPlayer;
        private bool _isEditingPlayer;
        private bool _EditPlayer;
        private bool _isFindingPlayer;
        private bool _FindPlayer;
        private bool _isDeeletingPlayer;
        private bool _DeletePlayer;

        private PlayerData _foundPlayer;
        private bool _isPlayerDataFetched = false;




        private string _addPlayer = "Add Player";

        private KeyboardState _previousKeyboardState;
        private MouseState _previousMouseState;

        string playerFinalScore;

        public FetchRequest(string theFinalScore)
        {
            playerFinalScore = theFinalScore;
        }


        public override async void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
            _viewAll = "View All Players";
            _addNew = "Add New Players";
            _previousKeyboardState = Keyboard.GetState();

            // Fetch player data
            await LoadPlayerData("http://localhost:3000/topTen");

        }

        private async Task FetchPlayerData(string screenName)
        {
            _foundPlayer = await GetPlayerData("http://localhost:3000/findPlayer", screenName);
            _isPlayerDataFetched = true; // Mark data as fetched
        }


        private async Task LoadPlayerData(string url)
        {
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();

                //_playerData = JsonSerializer.Deserialize<PlayerData>(jsonResponse);
                _players = JsonSerializer.Deserialize<List<PlayerData>>(jsonResponse);
                Debug.WriteLine(jsonResponse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching player data: {ex.Message}");
            }
        }

        private async Task<PlayerData> GetPlayerData(string url, string screenName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send GET request to find the player by screenName
                    HttpResponseMessage response = await client.GetAsync($"{url}?screenName={screenName}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content as JSON and deserialize it into a Player object
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        PlayerData player = JsonSerializer.Deserialize<PlayerData>(jsonResponse);
                        return player;
                    }
                    else
                    {
                        // If the player is not found or there is an error, handle it here
                        Debug.WriteLine("Player not found or error occurred.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error retrieving player data: {ex.Message}");
                    return null;
                }
            }
        }


        private async Task<bool> SendPlayerData(string url, string screenName, string firstName, string lastName, string dateStarted, string score)
        {
            score = playerFinalScore;

            var playerData = new { screenName, firstName, lastName, dateStarted, score };
            string json = JsonSerializer.Serialize(playerData);
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending player data {ex.Message}");
                return false;
            }

        }



        public override void UnloadContent(ContentManager contentManager)
        {

        }

        public override void HandleInput(GameTime gameTime)
        {

        }

        private bool isLeftButtonClickedLastFrame = false;
        private bool isRightButtonClickedLastFrame = false;

        private void HandleMouseClick(MouseState mouseState)
        {
            int inputX = toolPaletteX + toolPalettePadding;
            int inputY = toolPalettePadding + 75;

            // Check if Left Mouse Button was just pressed
            bool isLeftButtonJustPressed = mouseState.LeftButton == ButtonState.Pressed && !isLeftButtonClickedLastFrame;
            // Check if Right Mouse Button was just pressed
            bool isRightButtonJustPressed = mouseState.RightButton == ButtonState.Pressed && !isRightButtonClickedLastFrame;

            // Update the previous state for next frame
            isLeftButtonClickedLastFrame = mouseState.LeftButton == ButtonState.Pressed;
            isRightButtonClickedLastFrame = mouseState.RightButton == ButtonState.Pressed;

            // Hide all Players (Right-click)
            if (isRightButtonJustPressed &&
                mouseState.X >= inputX + 60 &&
                mouseState.X <= inputX + 150 &&
                mouseState.Y >= inputY + 30 &&
                mouseState.Y <= inputY + 50)
            {
                _isViewingAll = false;
            }

            // Handle showing all players (Left-click)
            if (isLeftButtonJustPressed &&
                mouseState.X >= inputX + 60 &&
                mouseState.X <= inputX + 150 &&
                mouseState.Y >= inputY + 30 &&
                mouseState.Y <= inputY + 50)
            {
                _isViewingAll = true;
                _isAddingNew = false;
                _isEditingPlayer = false;
                _isFindingPlayer = false;
                _isDeeletingPlayer = false;
            }

            // Hide adding player (Right-click)
            if (isRightButtonJustPressed &&
                mouseState.X >= inputX + 60 &&
                mouseState.X <= inputX + 150 &&
                mouseState.Y >= inputY + 60 &&
                mouseState.Y <= inputY + 80)
            {
                _isAddingNew = false;
            }

            // Handle adding player (Left-click)
            if (isLeftButtonJustPressed &&
                mouseState.X >= inputX + 60 &&
                mouseState.X <= inputX + 150 &&
                mouseState.Y >= inputY + 60 &&
                mouseState.Y <= inputY + 80)
            {
                _isFindingPlayer = false;
                _isViewingAll = false;
                _isEditingPlayer = false;
                _isDeeletingPlayer = false;
                _isAddingNew = true;
            }

            // Add screen name
            if (isLeftButtonJustPressed &&
                mouseState.X >= 120 &&
                mouseState.X <= 200 &&
                mouseState.Y >= 15 &&
                mouseState.Y <= 35 &&
                _isAddingNew)
            {
                Debug.WriteLine("left-click detected within the button area!");
                _isEditingScreenName = true;
                _isEditingFirstName = false;
                _isEditingLastName = false;
                _isEditingDateStarted = false;
                _isEditingScore = false;
                _screenName = "";
            }

            if (isLeftButtonJustPressed &&
                mouseState.X >= 210 &&
                mouseState.X <= 290 &&
                mouseState.Y >= 15 &&
                mouseState.Y <= 35 &&
                _isFindingPlayer)
            {
                Debug.WriteLine("left-click detected within the button area!");
                _isEditingScreenName = true;
                _isEditingFirstName = false;
                _isEditingLastName = false;
                _isEditingDateStarted = false;
                _isEditingScore = false;
                _screenName = "";
            }

            // Add first name
            if (isLeftButtonJustPressed &&
                mouseState.X >= 90 &&
                mouseState.X <= 170 &&
                mouseState.Y >= 40 &&
                mouseState.Y <= 60 &&
                _isAddingNew)
            {
                Debug.WriteLine("left-click detected within the button area!");
                _isEditingScreenName = false;
                _isEditingFirstName = true;
                _isEditingLastName = false;
                _isEditingDateStarted = false;
                _isEditingScore = false;
                _firstName = "";
            }

            // Add last name
            if (isLeftButtonJustPressed &&
                mouseState.X >= 90 &&
                mouseState.X <= 170 &&
                mouseState.Y >= 70 &&
                mouseState.Y <= 90 &&
                _isAddingNew)
            {
                Debug.WriteLine("left-click detected within the button area!");
                _isEditingScreenName = false;
                _isEditingFirstName = false;
                _isEditingLastName = true;
                _isEditingDateStarted = false;
                _isEditingScore = false;
                _lastName = "";
            }


            // Add date started
            if (isLeftButtonJustPressed &&
                mouseState.X >= 105 &&
                mouseState.X <= 185 &&
                mouseState.Y >= 100 &&
                mouseState.Y <= 120 &&
                _isAddingNew)
            {
                Debug.WriteLine("left-click detected within the button area!");
                _isEditingScreenName = false;
                _isEditingFirstName = false;
                _isEditingLastName = false;
                _isEditingDateStarted = true;
                _isEditingScore = false;
                _dateStarted = "";
            }

            // Add player button
            if (isLeftButtonJustPressed &&
                mouseState.X >= 20 &&
                mouseState.X <= 100 &&
                mouseState.Y >= 170 &&
                mouseState.Y <= 190 &&
                _isAddingNew)
            {
                Debug.WriteLine("left-click detected within the button area!");
                _isAddingPlayer = true;
                _isEditingScreenName = false;
                _isEditingFirstName = false;
                _isEditingLastName = false;
                _isEditingDateStarted = false;
                _isEditingScore = false;
                // You can now send the new player data to the server or store it locally
            }

        }


        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            HandleMouseClick(mouseState); // Handle any mouse click actions, such as button clicks

            // If we're in "finding player" mode and haven't fetched the data yet, fetch it
            if (_isFindingPlayer && !_isPlayerDataFetched)
            {
                // Fetch the player data asynchronously
                _ = GetPlayerData("http://localhost:3000/updatePlayer", _screenName);
                _isPlayerDataFetched = true; // Set the flag to true once data is fetched
            }

            // Handle adding new player
            if (_isAddingPlayer)
            {
                // Send player data to the server
                _ = SendPlayerData("http://localhost:3000/sentdatatodb", _screenName, _firstName, _lastName, _dateStarted, _score);

                // Reset the state
                _isAddingPlayer = false;
                _isAddingNew = false;
                _isEditingPlayer = false;
                _screenName = "";
                _firstName = "";
                _lastName = "";
                _dateStarted = "";
                _score = playerFinalScore;
            }

            // Handle finding player data
            if (_FindPlayer)
            {
                // Fetch player data asynchronously when the user is searching for a player
                _ = FetchPlayerData(_screenName);  // Don't await here to avoid blocking the Update loop
                _isAddingPlayer = false;
                _isAddingNew = false;
                _isEditingPlayer = false;
            }
        }

        public override void Render(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            toolPaletteX = spriteBatch.GraphicsDevice.Viewport.Width - 200;
            toolPaletteWidth = 200;
            _paletteOffsetY = 500;
            tilePaletteY = _paletteOffsetY;
            tilePaletteHeight = 128;

            // Begin sprite batch drawing
            //spriteBatch.Begin();
            _spriteBatch = spriteBatch;
            // Draw buttons and player data
            DrawButtons(contentManager);
            DrawPlayerData(new Vector2(20, 20), spriteBatch);
            DrawAddPlayerData(new Vector2(20, 20), spriteBatch);
            DrawFindPlayerData(new Vector2(20, 20), spriteBatch);

            // End sprite batch drawing
            //spriteBatch.End();

            base.Render(spriteBatch, contentManager);
        }


        private void DrawButtons(ContentManager contentManager)
        {
            int inputX = toolPaletteX + toolPalettePadding;
            int inputY = toolPalettePadding + 75;

            //label for show all players button
            _spriteBatch.DrawString(_font, "View All Players:", new Vector2(inputX - 60, inputY + 30), Color.White);
            //button for view all show all players button
            _spriteBatch.DrawString(_font, _viewAll, new Vector2(inputX + 60, inputY + 30), _isViewingAll ? Color.Yellow : Color.White);
            //label for add player button
            _spriteBatch.DrawString(_font, "Add New Players:", new Vector2(inputX - 60, inputY + 60), Color.White);
            //button for add new players button
            _spriteBatch.DrawString(_font, _addNew, new Vector2(inputX + 60, inputY + 60), _isAddingNew ? Color.Yellow : Color.White);


            if (_isAddingNew)
            {
                //label for screen name button
                _spriteBatch.DrawString(_font, "Screen Name:", new Vector2(20, 20), Color.White);
                //button for add new screen name for player button
                _spriteBatch.DrawString(_font, _screenName, new Vector2(120, 20), Color.White);

                //label for first name button
                _spriteBatch.DrawString(_font, "First Name:", new Vector2(20, 50), Color.White);
                //button for add new first name for player button
                _spriteBatch.DrawString(_font, _firstName, new Vector2(100, 50), Color.White);

                //label for last name button
                _spriteBatch.DrawString(_font, "Last Name:", new Vector2(20, 80), Color.White);
                //button for add new last name for player button
                _spriteBatch.DrawString(_font, _lastName, new Vector2(100, 80), Color.White);

                //label for date started button
                _spriteBatch.DrawString(_font, "Date Started:", new Vector2(20, 110), Color.White);
                //button for add new date started for player button
                _spriteBatch.DrawString(_font, _dateStarted, new Vector2(115, 110), Color.White);

                //label for score button
                _spriteBatch.DrawString(_font, "Score:", new Vector2(20, 140), Color.White);
                //button for add new score for player button
                _spriteBatch.DrawString(_font, playerFinalScore, new Vector2(70, 140), Color.White);

                _spriteBatch.DrawString(_font, _addPlayer, new Vector2(20, 180), Color.White);
            }

        }

        void DrawPlayerData(Vector2 startPosition, SpriteBatch spriteBatch)
        {
            Vector2 position = startPosition;
            if (_isViewingAll)
            {
                foreach (var player in _players)
                {
                    spriteBatch.DrawString(_font, $"Screen Name:{player.screenName}, First Name:{player.firstName}, Last Name:{player.lastName}, Date Started:{player.dateStarted}, Score:{player.score}", position, Color.White);

                    position.Y += 35;
                }
            }
        }


        void DrawFindPlayerData(Vector2 startPosition, SpriteBatch spriteBatch)
        {
            if (_isFindingPlayer)
            {

                KeyboardState currentKeyboardState = Keyboard.GetState();

                foreach (var key in Keyboard.GetState().GetPressedKeys())// new keybutton presses
                {
                    if (!_previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z && _isEditingScreenName)//handles letters and numbers
                        {
                            _screenName += key.ToString();
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9 && _isEditingScreenName)
                        {
                            _screenName += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && _screenName.Length > 0 && _isEditingScreenName)//handles backspacing
                        {
                            _screenName = _screenName.Substring(0, _screenName.Length - 1);
                        }
                    }
                }
                _previousKeyboardState = currentKeyboardState;

            }
        }

        void DrawAddPlayerData(Vector2 startPosition, SpriteBatch spriteBatch)
        {
            if (_isAddingNew)
            {

                KeyboardState currentKeyboardState = Keyboard.GetState();

                foreach (var key in Keyboard.GetState().GetPressedKeys())// new keybutton presses
                {
                    if (!_previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z && _isEditingScreenName)//handles letters and numbers
                        {
                            _screenName += key.ToString();
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9 && _isEditingScreenName)
                        {
                            _screenName += (key - Keys.D0).ToString();
                        }
                        else if (key >= Keys.A && key <= Keys.Z && _isEditingFirstName)//handles letters and numbers
                        {
                            _firstName += key.ToString();
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9 && _isEditingFirstName)
                        {
                            _firstName += (key - Keys.D0).ToString();
                        }
                        else if (key >= Keys.A && key <= Keys.Z && _isEditingLastName)//handles letters and numbers
                        {
                            _lastName += key.ToString();
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9 && _isEditingLastName)
                        {
                            _lastName += (key - Keys.D0).ToString();
                        }
                        else if (key >= Keys.A && key <= Keys.Z && _isEditingDateStarted)//handles letters and numbers
                        {
                            _dateStarted += key.ToString();
                        }
                        else if (key == Keys.Subtract && _isEditingDateStarted)//handles letters and numbers
                        {
                            _dateStarted += "-";
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9 && _isEditingDateStarted)
                        {
                            _dateStarted += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && _screenName.Length > 0 && _isEditingScreenName)//handles backspacing
                        {
                            _screenName = _screenName.Substring(0, _screenName.Length - 1);
                        }
                        else if (key == Keys.Back && _firstName.Length > 0 && _isEditingFirstName)//handles backspacing
                        {
                            _firstName = _firstName.Substring(0, _firstName.Length - 1);
                        }
                        else if (key == Keys.Back && _lastName.Length > 0 && _isEditingLastName)//handles backspacing
                        {
                            _lastName = _lastName.Substring(0, _lastName.Length - 1);
                        }
                        else if (key == Keys.Back && _dateStarted.Length > 0 && _isEditingDateStarted)//handles backspacing
                        {
                            _dateStarted = _dateStarted.Substring(0, _dateStarted.Length - 1);
                        }
                    }

                }

                _previousKeyboardState = currentKeyboardState;
            }

        }
    }
}

public class PlayerData
{
    public String screenName { get; set; }
    public String firstName { get; set; }
    public String lastName { get; set; }
    public String dateStarted { get; set; }
    public String score { get; set; }
}
