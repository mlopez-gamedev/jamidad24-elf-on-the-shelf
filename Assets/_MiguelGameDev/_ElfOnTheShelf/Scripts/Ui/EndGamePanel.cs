using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] Button _endButton;

        private void Start()
        {
            _endButton.onClick.AddListener(OnEndButtonClicked);
        }
        public void Show()
        {
            _endButton.transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0;
            _canvasGroup.gameObject.SetActive(true);

            DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1f, 1f))
                .AppendInterval(0.5f)
                .Append(_endButton.transform.DOScale(Vector3.one, 0.3f));
        }

        private void OnEndButtonClicked()
        {
            Audio2dService.Instance.PlayAudio(EAudioEvent.ClickSfx);
            SceneManager.LoadScene(0);
        }
    }
}