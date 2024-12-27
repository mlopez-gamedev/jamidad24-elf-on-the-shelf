using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GoalCardUi : CardUi
    {
        [SerializeField] private Image _strokeImage;
        [SerializeField] private BetterImage _background;

        public void Setup(GoalCard card)
        {
            base.Setup(card);
            _strokeImage.color = card.Suit.StrokeColor;
            _background.CurrentSpriteSettings.PrimaryColor = card.Suit.BackgroundTopColor;
            _background.CurrentSpriteSettings.SecondaryColor = card.Suit.BackgroundBottomColor;
        }
    }
}