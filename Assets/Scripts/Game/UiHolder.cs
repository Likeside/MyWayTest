using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AssetBundleBrowser.Game
{
    public interface IUiHolder
    {
        Button MainButton { get; }
        Image ButtonImage { get; }
        TextMeshProUGUI CounterText { get; }
        Button UpdateContent { get; }
        TextMeshProUGUI GreetingText { get; }
    }
    
    public class UiHolder: MonoBehaviour, IUiHolder
    {
       [SerializeField] private Button mainButton;
       [SerializeField] private Image buttonImage;
         [SerializeField] private TextMeshProUGUI counterText;
            [SerializeField] private Button updateContent;
            [SerializeField] private TextMeshProUGUI greetingText;
        public Button MainButton => mainButton;
        public Image ButtonImage => buttonImage;
        public TextMeshProUGUI CounterText => counterText;
        public Button UpdateContent => updateContent;
        public TextMeshProUGUI GreetingText => greetingText;
    }
}
