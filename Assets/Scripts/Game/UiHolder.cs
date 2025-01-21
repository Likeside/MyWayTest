using UnityEngine;
using UnityEngine.UI;

namespace AssetBundleBrowser.Game
{
    public interface IUiHolder
    {
        Button MainButton { get; }
        Image ButtonImage { get; }
    }
    
    public class UiHolder: MonoBehaviour, IUiHolder
    {
       [SerializeField] private Button mainButton;
       [SerializeField] private Image buttonImage;
        public Button MainButton => mainButton;
        public Image ButtonImage => buttonImage;
    }
}
