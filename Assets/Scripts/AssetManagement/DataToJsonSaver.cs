using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace AssetManagement
{
    public interface IJsonData
    {
    }

    public abstract class DataToJsonSaver<T> : IInitializable where T : IJsonData
    {
        public T Data { get; set; }

        private string _fileLocation;
        private volatile bool _persisting;
        private readonly string _folderLocation;
        private readonly string _fileName;

        protected DataToJsonSaver(string folderLocation, string fileName)
        {
            _folderLocation = folderLocation;
            _fileName = fileName;
        }

        public void Initialize()
        {
            var directory = Path.Combine(Application.persistentDataPath, _folderLocation);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _fileLocation = Path.Combine(directory, _fileName);
        }

        public void Save()
        {
            if (_persisting) return;
            _persisting = true;
            using (var streamWriter = new StreamWriter(_fileLocation, false))
            {
                try
                {
                    var json = JsonConvert.SerializeObject(Data);
                    streamWriter.Write(json);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            _persisting = false;
        }

        public bool TryLoadData()
        {
            if (!File.Exists(_fileLocation))
            {
                Debug.Log($"No such file in the location: {_fileLocation}, your script should create default data and call Save(); in this case");
                return false;
            }
            try
            {
                var json = File.ReadAllText(_fileLocation);
                if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(json)) return false;
                Data = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private async Task<string> ReadTextAsync(string filePath)
        {
            using var sr = File.OpenText(filePath);
            return await sr.ReadToEndAsync();
        }
    }
}
