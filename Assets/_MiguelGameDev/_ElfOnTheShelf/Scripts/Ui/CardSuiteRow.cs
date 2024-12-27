using TMPro;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class CardSuiteRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _pranksText;
        [SerializeField] private TMP_Text _goodiesText;
        [SerializeField] private TMP_Text _tricksText;

        public void Setup(CardsAmount cardsAmount)
        {
            _pranksText.text = cardsAmount.Pranks.ToString();
            _goodiesText.text = cardsAmount.Goodies.ToString();
            _tricksText.text = cardsAmount.Tricks.ToString();
        }
    }
}