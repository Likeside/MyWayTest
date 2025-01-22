using System;
using System.Collections;
using System.Collections.Generic;
using AssetManagement;
using UnityEngine;
using Utilities;
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
        private readonly WaitingView _waitingView;
        private readonly IConfig _config;
        private readonly Dictionary<AssetBundleType, AssetBundle> _assetBundles = new Dictionary<AssetBundleType, AssetBundle>();
        
        public AssetBundleManager(WaitingView waitingView, IConfig config)
        {
            _waitingView = waitingView;
            _config = config;
        }
        public void Initialize()
        {
            
        }
        public void GetAsset<T>(AssetBundleType assetBundleType, string assetName, Action<bool, T> loadedCallback) where T : Object
        {
            if (!_assetBundles.ContainsKey(assetBundleType))
            {
                _waitingView.StartCoroutine(LoadRemoteBundle(assetBundleType, assetBundle =>
                {
                    if (assetBundle == null)
                    {
                        loadedCallback.Invoke(false, null);
                        return;
                    }
                    _assetBundles.Add(assetBundleType, assetBundle);
                    var asset = assetBundle.LoadAsset<T>(assetName);
                    loadedCallback.Invoke(asset != null, asset);
                }));
            }
            else
            {
                var asset = _assetBundles[assetBundleType].LoadAsset<T>(assetName);
                loadedCallback.Invoke(asset != null, asset); 
            }
        }
        public void UpdateBundle(AssetBundleType assetBundleType, Action<bool> updatedCallback)
        {
            _waitingView.StartCoroutine(LoadRemoteBundle(assetBundleType, assetBundle =>
            {
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
            }));
        }

        private IEnumerator LoadRemoteBundle(AssetBundleType assetBundleType,  Action<AssetBundle> loadedCallback)
        {
            using var web = new WWW($"{_config.RemoteBundleUrl}{assetBundleType.ToString().ToLower()}" + ".unity3d");
            yield return web;
            AssetBundle remoteAssetBundle = web.assetBundle;
            if (remoteAssetBundle == null) {
                Debug.LogError("Failed to download AssetBundle!");
                yield break;
            }
            loadedCallback?.Invoke(remoteAssetBundle);
        }
    }
}
