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

        public UiController(IUiHolder uiHolder,
            IAssetBundleManager assetBundleManager,
            ISettingsDataLoader<SettingsData> settingsDataLoader,
            IGreetingsDataLoader<GreetingsData> greetingsDataLoader)
        {
            _uiHolder = uiHolder;
            _assetBundleManager = assetBundleManager;
            _settingsDataLoader = settingsDataLoader;
            _greetingsDataLoader = greetingsDataLoader;
        }

        public void Initialize()
        {
             _assetBundleManager.GetAsset<Sprite>(AssetBundleType.Images, "buttonBackground.png", SetSprite);
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
