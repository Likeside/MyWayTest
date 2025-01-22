using UnityEngine;

namespace AssetManagement
{
    public interface IConfig
    {
        string RemoteBundleUrl { get; }
    }
    
    [CreateAssetMenu(fileName = "ConfigSo", menuName = "ConfigSo")]
    public class ConfigSo: ScriptableObject, IConfig
    {
        [SerializeField] private string remoteBundleUrl;
        public string RemoteBundleUrl => remoteBundleUrl;
    }
}
