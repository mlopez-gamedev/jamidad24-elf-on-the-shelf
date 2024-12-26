using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class DeckPanel : MonoBehaviour
    {
        [SerializeField] private Highlight _highlight;
        [SerializeField] private RectTransform _cardReversed;
        [SerializeField] private RectTransform _cardReversedPrefab;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }

        public CardUi DrawCard(Card card)
        {
            var cardUi = CardUiFactory.Instance.CreateCardUi(card, transform);
            Audio2dService.Instance.PlayAudio(EAudioEvent.DrawSfx);
            return cardUi;
        }
        
        public async UniTask<CardUi> SearchCard(Card card)
        {
            var cardUi = CardUiFactory.Instance.CreateCardUi(card, transform);
            cardUi.transform.SetAsFirstSibling();
            Audio2dService.Instance.PlayAudio(EAudioEvent.DrawSfx);
            await cardUi.transform.DOLocalMoveY(200f, 0.1f).AsyncWaitForCompletion();
            return cardUi;
        }
        
        public async UniTask PlayShuffle()
        {
            await _rectTransform.DOAnchorPos(new Vector2(50f, 110f), 0.2f).AsyncWaitForCompletion();
            Audio2dService.Instance.PlayAudio(EAudioEvent.ShuffleSfx);
            for (int i = 0; i < 4; i++)
            {
                var shuffleCard = Instantiate(_cardReversedPrefab, transform);
                shuffleCard.SetAsFirstSibling();
                await DOTween.Sequence()
                    .Append(shuffleCard.DOAnchorPosX(-75f, 0.05f).SetEase(Ease.Flash))
                    .AppendCallback(shuffleCard.SetAsLastSibling)
                    .Append(shuffleCard.DOAnchorPosX(0, 0.08f))
                    .AsyncWaitForCompletion();
            }
            await _rectTransform.DOAnchorPos(Vector2.zero, 0.2f).AsyncWaitForCompletion();
        }

        public void SetHidden(bool hidden)
        {
            _cardReversed.gameObject.SetActive(!hidden);
        }
    }
}