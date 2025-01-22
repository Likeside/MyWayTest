using System.Collections;
using System.Linq;
using AssetManagement;
using UnityEngine;
using Utils;
using Zenject;

namespace Game
{
    public interface ILoadingScreenController: IInitializable
    {
        
    }
    
    public class LoadingScreenController: ILoadingScreenController
    {
        private readonly ILoading[] _loadings;
        private readonly IWaitingView _waitingView;
        private readonly IConfig _config;
        private readonly IUiHolder _uiHolder;
        
        public LoadingScreenController(IUiHolder uiHolder, ILoading[] loadings, IWaitingView waitingView, IConfig config)
        {
            _uiHolder = uiHolder;
            _loadings = loadings;
            _waitingView = waitingView;
            _config = config;
        }

        public void Initialize()
        {
            _waitingView.StartCoroutine(Loading());
        }
        
        private IEnumerator Loading()
        {
             yield return new WaitUntil(() => _loadings.All(loading => loading.Loaded));
             yield return new WaitForSeconds(_config.LoadingScreenDelay);
             _uiHolder.LoadingScreen.SetActive(false);
        }
    }
}
