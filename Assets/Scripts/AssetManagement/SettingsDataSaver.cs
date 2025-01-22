using Zenject;

namespace AssetManagement
{
    
    public interface ISettingsDataSaver<T>: IInitializable where T : IJsonData
    {
        public T Data { get; set; }
        void Save();
        bool TryLoadData();
    }
    public class SettingsDataSaver: DataToJsonSaver<SettingsData>, ISettingsDataSaver<SettingsData>
    {
        private const string FolderLocation = "GameData";
        private const string FileName = "SettingsSaveData.json";

        public SettingsDataSaver() : base(FolderLocation, FileName)
        {
        }
    }
}
