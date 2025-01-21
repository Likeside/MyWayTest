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
        void GetAsset<T> (AssetBundleType assetBundleType, string assetName, Action<bool,T> loadedCallback) where T : Object;
        void UpdateBundle(AssetBundleType assetBundleType, Action<bool> updatedCallback);
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
        }
        public void GetAsset<T>(AssetBundleType assetBundleType, string assetName, Action<bool, T> loadedCallback) where T : Object
        {
            if (!_assetBundles.ContainsKey(assetBundleType))
            {
                var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleType.ToString().ToLower()));
                if (assetBundle == null)
                {
                    loadedCallback.Invoke(false, null);
                    return;
                }
                _assetBundles.Add(assetBundleType, assetBundle);
            }
            var asset = _assetBundles[assetBundleType].LoadAsset<T>(assetName);
            loadedCallback.Invoke(asset != null, asset);
        }
        public void UpdateBundle(AssetBundleType assetBundleType, Action<bool> updatedCallback)
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleType.ToString().ToLower()));
            if (assetBundle == null)
            {
                updatedCallback.Invoke(false);
                return;
            } 
            if (_assetBundles.ContainsKey(assetBundleType))
            {
                _assetBundles[assetBundleType].Unload(true);
                _assetBundles.Remove(assetBundleType);
            }
            _assetBundles.Add(assetBundleType, assetBundle);
            updatedCallback.Invoke(true);
        }
    }
}
