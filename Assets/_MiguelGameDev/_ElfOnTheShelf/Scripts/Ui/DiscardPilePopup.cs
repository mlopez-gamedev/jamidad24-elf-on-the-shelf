using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class DiscardPilePopup : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] RectTransform _panel;
        [SerializeField] Button _backButton;

        [SerializeField] private CardSuiteRow _bedroomCards;
        [SerializeField] private CardSuiteRow _livingRoomCards;
        [SerializeField] private CardSuiteRow _kitchenCards;
        [SerializeField] private CardSuiteRow _bathroomCards;
        
        [SerializeField] private TMP_Text _bustCardsText;
        
        private void Start()
        {
            _backButton.onClick.AddListener(OnContinueButtonClicked);
        }
        public void Show()
        {
            _bedroomCards.Setup(
                Game.Instance.Player.CountDiscardedCards(ECardSuit.Bedroom));
            _livingRoomCards.Setup(
                Game.Instance.Player.CountDiscardedCards(ECardSuit.LivingRoom));
            _kitchenCards.Setup(
                Game.Instance.Player.CountDiscardedCards(ECardSuit.Kitchen));
            _bathroomCards.Setup(
                Game.Instance.Player.CountDiscardedCards(ECardSuit.Bathroom));

            _bustCardsText.text = Game.Instance.Player.CountBustCards().ToString();
            
            _panel.localScale = Vector3.zero;
            _backButton.transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0;
            _canvasGroup.gameObject.SetActive(true);

            DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1f, 0.2f))
                .Join(_panel.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack))
                .Append(_backButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
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
                .Join(_backButton.transform.DOScale(Vector3.zero, 0.2f))
                .OnComplete(EndHide);

            void EndHide()
            {
                _canvasGroup.gameObject.SetActive(false);
            }
        }
    }
}