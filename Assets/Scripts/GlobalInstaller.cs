using System;
using AssetBundleBrowser.AssetManagement;
using AssetManagement;
using UnityEngine;
using Utilities;
using Zenject;

namespace AssetBundleBrowser
{
    public class GlobalInstaller: MonoInstaller
    {
        [SerializeField] private WaitingMonoBehaviour waitingMonoBehaviour;
        [SerializeField] private ConfigSo configSo;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WaitingMonoBehaviour>().FromInstance(waitingMonoBehaviour);
            Container.BindInterfacesAndSelfTo<ConfigSo>().FromInstance(configSo);
            Container.BindInterfacesAndSelfTo<AssetBundleManager>().AsSingle();
        }
    }
}
