using UnityEngine;

namespace AssetManagement
{
    public interface IConfig
    {
        string RemoteBundleUrl { get; }
        string SpriteBackgroundName { get; }
    }
    
    [CreateAssetMenu(fileName = "ConfigSo", menuName = "ConfigSo")]
    public class ConfigSo: ScriptableObject, IConfig
    {
        [SerializeField] private string remoteBundleUrl;
        [SerializeField] private string spriteBackgroundName;
        public string RemoteBundleUrl => remoteBundleUrl;
        public string SpriteBackgroundName => spriteBackgroundName;
    }
}
