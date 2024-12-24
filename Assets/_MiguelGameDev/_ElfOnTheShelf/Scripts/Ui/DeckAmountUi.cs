using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class DeckAmountUi : MonoBehaviour
    {
        [SerializeField] TMP_Text _deckAmountText;

        public void PlaySetAmount(int amount)
        {
            DOTween.Sequence()
                .Append(_deckAmountText.transform.DOScale(1.2f, 0.1f).SetEase(Ease.Flash))
                .AppendCallback(SetAmountText)
                .Append(_deckAmountText.transform.DOScale(1f, 0.1f));

            void SetAmountText()
            {
                SetAmount(amount);
            }
        }

        public void SetAmount(int amount)
        {
            _deckAmountText.text = amount.ToString();
        }
    }
}