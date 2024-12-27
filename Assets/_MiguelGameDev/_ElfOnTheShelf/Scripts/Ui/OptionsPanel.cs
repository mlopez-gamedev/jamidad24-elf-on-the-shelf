using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class OptionsPanel : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] RectTransform _panel;
        [SerializeField] Button _endButton;
        [SerializeField] Button _continueButton;
        
        private void Start()
        {
            _endButton.onClick.AddListener(OnEndButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }
        public void Show()
        {
            _panel.localScale = Vector3.zero;
            _continueButton.transform.localScale = Vector3.zero;
            _endButton.transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0;
            _canvasGroup.gameObject.SetActive(true);

            DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1f, 0.2f))
                .Join(_panel.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack))
                .Append(_continueButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack))
                .Append(_endButton.transform.DOScale(Vector3.one, 0.2f)).SetEase(Ease.OutBack);
        }

        private void OnEndButtonClicked()
        {
            Audio2dService.Instance.PlayAudio(EAudioEvent.ClickSfx);
            SceneManager.LoadScene(0);
        }
        
        private void OnContinueButtonClicked()
        {
            Audio2dService.Instance.PlayAudio(EAudioEvent.ClickSfx);
            Hide();
        }
        
        private void Hide()
        {
            DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0, 0.2f))
                .Join(_panel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutBack))
                .Join(_continueButton.transform.DOScale(Vector3.zero, 0.2f))
                .Join(_endButton.transform.DOScale(Vector3.zero, 0.2f))
                .OnComplete(EndHide);

            void EndHide()
            {
                _canvasGroup.gameObject.SetActive(false);
            }
        }
    }
}