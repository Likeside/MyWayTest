using AssetBundleBrowser.AssetManagement;
using AssetManagement;
using UnityEngine;
using Zenject;

namespace AssetBundleBrowser.Game
{
    public interface IUiController: IInitializable
    {
        
    }
    
    public class UiController: IUiController
    {
        private readonly IUiHolder _uiHolder;
        private readonly IAssetBundleManager _assetBundleManager;
        private readonly ISettingsDataLoader<SettingsData> _settingsDataLoader;
        private readonly IGreetingsDataLoader<GreetingsData> _greetingsDataLoader;
        private readonly ISettingsDataSaver<SettingsData> _settingsDataSaver;
        private readonly IConfig _config;

        private int _counter;

        public UiController(IUiHolder uiHolder,
            IAssetBundleManager assetBundleManager,
            IGreetingsDataLoader<GreetingsData> greetingsDataLoader,
            ISettingsDataLoader<SettingsData> settingsDataLoader,
            ISettingsDataSaver<SettingsData> settingsDataSaver,
            IConfig config)
        {
            _uiHolder = uiHolder;
            _assetBundleManager = assetBundleManager;
            _settingsDataLoader = settingsDataLoader;
            _greetingsDataLoader = greetingsDataLoader;
            _settingsDataSaver = settingsDataSaver;
            _config = config;
        }

        public void Initialize()
        {
            _uiHolder.MainButton.onClick.AddListener(Increment);
            _uiHolder.UpdateContent.onClick.AddListener(UpdateContent);
            if (_greetingsDataLoader.TryLoadData())
            {
                _uiHolder.GreetingText.text = _greetingsDataLoader.Data.Greetings;
            }
            else
            {
                _greetingsDataLoader.Data = new GreetingsData() {Greetings = "Greetings!"};
                _greetingsDataLoader.Save();
            }
            if (_settingsDataSaver.TryLoadData())
            {
                _counter = _settingsDataSaver.Data.StartingNumber;
            }
            else
            {
                if (_settingsDataLoader.TryLoadData())
                {
                    _counter = _settingsDataLoader.Data.StartingNumber;
                }
                else
                {
                    _settingsDataLoader.Data = new SettingsData() {StartingNumber = 5};
                    _settingsDataLoader.Save();
                    _counter = _settingsDataLoader.Data.StartingNumber;
                }
                _settingsDataSaver.Data = _settingsDataLoader.Data;
                _settingsDataSaver.Save();
            }
            UpdateCounterText();
            _assetBundleManager.GetAsset<Sprite>(AssetBundleType.Images, _config.SpriteBackgroundName, SetSprite);
        }
        private void UpdateContent()
        {
            _assetBundleManager.UpdateBundle(AssetBundleType.Images, _ =>
            {
                _assetBundleManager.GetAsset<Sprite>(AssetBundleType.Images, _config.SpriteBackgroundName, SetSprite);
            });
            if (_settingsDataLoader.TryLoadData())
            {
                _counter = _settingsDataLoader.Data.StartingNumber;
                _uiHolder.CounterText.text = _settingsDataLoader.Data.StartingNumber.ToString();
            }
            
        }
        private void Increment()
        {       
            _counter++;
            UpdateCounterText();
            _settingsDataSaver.Data.StartingNumber = _counter;
            _settingsDataSaver.Save();
        }

        private void UpdateCounterText()
        {
            _uiHolder.CounterText.text = _counter.ToString();
        }

        private void SetSprite(bool loaded, Sprite sprite)
        {
            if (loaded)
            {
                _uiHolder.ButtonImage.sprite = sprite;
            }
        }
    }
}
