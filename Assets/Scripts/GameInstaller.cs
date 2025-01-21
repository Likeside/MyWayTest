using AssetBundleBrowser.Game;
using UnityEngine;
using Zenject;

namespace AssetBundleBrowser
{
    public class GameInstaller: MonoInstaller
    {
        [SerializeField] private UiHolder uiHolder;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UiHolder>().FromInstance(uiHolder);
            Container.BindInterfacesAndSelfTo<UiController>().AsSingle();
        }
    }
}
