using AssetBundleBrowser.AssetManagement;
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
        
        public UiController(IUiHolder uiHolder, IAssetBundleManager assetBundleManager)
        {
            _uiHolder = uiHolder;
            _assetBundleManager = assetBundleManager;
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
