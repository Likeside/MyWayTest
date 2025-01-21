namespace AssetManagement
{
    public interface ISettingsDataLoader<T> where T : IJsonData
    {
        public T Data { get; set; }
        void Save();
        bool TryLoadData();
    }

    public class SettingsDataLoader: DataToJsonSaver<SettingsData>, ISettingsDataLoader<SettingsData>
    {
        private const string FolderLocation = "GameData";
        private const string FileName = "Settings.json";

        public SettingsDataLoader() : base(FolderLocation, FileName)
        {
        }
    }
}
