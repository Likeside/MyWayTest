using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using Zenject;
using Object = UnityEngine.Object;

namespace AssetManagement
{
    public interface IAssetBundleManager: IInitializable, ILoading
    {
        void GetAsset<T> (AssetBundleType assetBundleType, string assetName, Action<bool,T> loadedCallback) where T : Object;
        void UpdateBundle(AssetBundleType assetBundleType, Action<bool> updatedCallback);
    }
    
    public class AssetBundleManager: IAssetBundleManager
    {
        private readonly IWaitingView _waitingView;
        private readonly IConfig _config;
        private readonly Dictionary<AssetBundleType, AssetBundle> _assetBundles = new Dictionary<AssetBundleType, AssetBundle>();

        private int _assetBundleTypesCount;
        private int _loadedCount;

        public bool Loaded { get; private set; }
        public AssetBundleManager(IWaitingView waitingView, IConfig config)
        {
            _waitingView = waitingView;
            _config = config;
        }
        public void Initialize()
        {
          //  AssetBundle.UnloadAllAssetBundles(true);
          //  return;
            var assetBundleTypes = Enum.GetValues(typeof(AssetBundleType)).Cast<AssetBundleType>().ToList();
            _assetBundleTypesCount = assetBundleTypes.Count();
            foreach (var assetBundleType in assetBundleTypes)
            {
                UpdateBundle(assetBundleType, _ =>
                {
                    _loadedCount++;
                    if (_loadedCount == _assetBundleTypesCount)
                    {
                        Loaded = true;
                    }
                });
            }
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
                _assetBundles.Add(assetBundleType, assetBundle);
                updatedCallback.Invoke(true);
            }, true));
        }

        private IEnumerator LoadRemoteBundle(AssetBundleType assetBundleType,  Action<AssetBundle> loadedCallback, bool forceUpdate = false)
        {
            AssetBundle existingBundle = AssetBundle.GetAllLoadedAssetBundles()
                .FirstOrDefault(bundle => bundle.name == assetBundleType.ToString().ToLower());
            if (existingBundle != null)
            {
                if (forceUpdate)
                {
                    if (_assetBundles.ContainsKey(assetBundleType))
                    {
                        _assetBundles.Remove(assetBundleType);
                    }
                    existingBundle.Unload(true);
                }
                else
                {
                    loadedCallback?.Invoke(existingBundle);
                    yield break;
                }
            }
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle($"{_config.RemoteBundleUrl}{assetBundleType.ToString().ToLower()}" + ".unity3d");
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading AssetBundle: " + www.error);
                yield break;
            }
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(www);
            loadedCallback?.Invoke(assetBundle);
        }
    }
}
