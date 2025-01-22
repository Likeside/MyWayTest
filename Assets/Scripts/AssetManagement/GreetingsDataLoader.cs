using Zenject;

namespace AssetManagement
{
    public interface IGreetingsDataLoader<T>: IInitializable where T : IJsonData 
    {
        public T Data { get; set; }
        void Save();
        bool TryLoadData();
    }
    
    public class GreetingsDataLoader: DataToJsonSaver<GreetingsData>, IGreetingsDataLoader<GreetingsData>
    {
        private const string FolderLocation = "GameData";
        private const string FileName = "Greetings.json";

        public GreetingsDataLoader() : base(FolderLocation, FileName)
        {
        }
    }
}
