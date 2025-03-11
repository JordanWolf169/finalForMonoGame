using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace NEITGameEngine.SaveDataSystem
{
    public class SaveSystem
    {
        private string _saveFilePath;

        public SaveSystem(string fileName)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _saveFilePath = Path.Combine(folderPath, fileName);
        }

        public void SaveGame(SaveData data)
        {
            try {
                //Serialize the class data to json
                //string json = JsonSerializer.Serialize(data, new JsonSerializerOptions 
                //{
                //    WriteIndented = true,//Makes the json easier to read
                //});

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };

                options.Converters.Add(new Vector2Converter());

                string json = JsonSerializer.Serialize(data, options);

                File.WriteAllText(_saveFilePath, json);
                Debug.WriteLine("Game Saved");

            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error saving game: {ex.Message}");
            }
        }

        public SaveData LoadGame()
        {
            try {
                if (File.Exists(_saveFilePath))
                {
                    //Read the json
                    string json = File.ReadAllText(_saveFilePath);

                    //Deserialize json to a SaveData Object
                    SaveData data = JsonSerializer.Deserialize<SaveData>(json);
                    return data;

                }
                else
                {
                    Debug.WriteLine("File not found");
                    return null;    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading game: {ex.Message}");
                return null;
            }
        }

        public void DeleteData()
        {
            try {
                if (File.Exists(_saveFilePath))
                {
                    File.Delete(_saveFilePath);
                    Debug.WriteLine($"Save file Deleted");
                }
                else
                {
                    Debug.WriteLine($"No file to delete");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting game data: {ex.Message}");
            }

        }
    }
}
