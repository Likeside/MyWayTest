using AssetManagement;
using Game;
using UnityEngine;
using Utils;
using Zenject;

public class GameInstaller: MonoInstaller
{
    [SerializeField] private WaitingMonoBehaviour waitingMonoBehaviour;
    [SerializeField] private ConfigSo configSo;
    [SerializeField] private UiHolder uiHolder;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UiHolder>().FromInstance(uiHolder);
        Container.BindInterfacesAndSelfTo<LoadingScreenController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UiController>().AsSingle();
        Container.BindInterfacesAndSelfTo<SettingsDataLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<GreetingsDataLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<SettingsDataSaver>().AsSingle();
        Container.BindInterfacesAndSelfTo<WaitingMonoBehaviour>().FromInstance(waitingMonoBehaviour);
        Container.BindInterfacesAndSelfTo<ConfigSo>().FromInstance(configSo);
        Container.BindInterfacesAndSelfTo<AssetBundleManager>().AsSingle();
    }
}