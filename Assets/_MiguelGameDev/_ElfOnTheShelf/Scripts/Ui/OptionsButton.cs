using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class OptionsButton : MonoBehaviour
    {
        [SerializeField] private Button _optionsButton;

        private void Start()
        {
            _optionsButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnExitButtonClick()
        {
            GameUi.Instance.ShowOptions();
        }
    }
}