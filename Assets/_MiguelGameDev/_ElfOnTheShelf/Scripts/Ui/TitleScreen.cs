using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] Button _startButton;
        [SerializeField] Game _game;

        private void Awake()
        {
            _canvasGroup.gameObject.SetActive(true);
            _startButton.onClick.AddListener(OnStartButtonClick);
        }

        private void OnStartButtonClick()
        {
            _canvasGroup.DOFade(0, 1f).OnComplete(HideAndInitGame);

            void HideAndInitGame()
            {
                _canvasGroup.gameObject.SetActive(false);
                // TODO: Show Elf Name
                _game.Init("Test", false);
            }
        }
    }
}