using AssetBundleBrowser.AssetManagement;
using Zenject;

namespace AssetBundleBrowser
{
    public class GlobalInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AssetBundleManager>().AsSingle();
        }
    }
}
