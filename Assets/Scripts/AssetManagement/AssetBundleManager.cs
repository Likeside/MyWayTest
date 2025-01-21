using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace AssetBundleBrowser.AssetManagement
{
    public interface IAssetBundleManager: IInitializable
    {
        bool GetAsset<T> (AssetBundleType assetBundleType, string assetName, out T asset) where T : Object;
    }
    
    public class AssetBundleManager: IAssetBundleManager
    {
        private readonly Dictionary<AssetBundleType, AssetBundle> _assetBundles = new Dictionary<AssetBundleType, AssetBundle>();

        public void Initialize()
        {
            foreach (var assetBundleType in Enum.GetValues(typeof(AssetBundleType)).Cast<AssetBundleType>())
            {
                var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleType.ToString().ToLower()));
                if(assetBundle == null) continue;
                _assetBundles.Add(assetBundleType, assetBundle);
            }


            if (GetAsset(AssetBundleType.Images, "buttonBackground.png", out Texture2D texture))
            {
                Debug.Log("loaded successfully");
            }
            else
            {
                Debug.Log("failed to load");
            }
        }
        public bool GetAsset<T>(AssetBundleType assetBundleType, string assetName, out T asset) where T : Object
        {
            if (!_assetBundles.ContainsKey(assetBundleType))
            {
                asset = null;
                return false;
            }
            asset = _assetBundles[assetBundleType].LoadAsset<T>(assetName);
            return asset != null;
        }
        
    }
}
