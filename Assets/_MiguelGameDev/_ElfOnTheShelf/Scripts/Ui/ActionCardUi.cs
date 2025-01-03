using System;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class ActionCardUi : CardUi
    {
        [SerializeField] private BetterImage _background;
        [SerializeField] private Image _strokeImage;
        [SerializeField] private Image _actionTypeIconPanel;
        [SerializeField] private Image _actionTypeIcon;
        [SerializeField] private TMP_Text _cardName;
        
        private ActionCard _actionCard;

        public ActionCard ActionCard => _actionCard;
            
        public void Setup(ActionCard card)
        {
            base.Setup(card);
            _actionCard = card;
            _background.CurrentSpriteSettings.PrimaryColor = card.Suit.BackgroundTopColor;
            _background.CurrentSpriteSettings.SecondaryColor = card.Suit.BackgroundBottomColor;
            
            _strokeImage.color = card.Suit.StrokeColor;
            _actionTypeIconPanel.color = card.Suit.StrokeColor;
            _actionTypeIcon.color = card.Suit.BackgroundBottomColor;
            _actionTypeIcon.sprite = card.ActionType.Icon;

            var cardSuitName = I2.Loc.LocalizationManager.GetTranslation(card.Suit.NameTerm);
            var actionSuitName = I2.Loc.LocalizationManager.GetTranslation(card.ActionType.NameTerm);

            _cardName.color = card.Suit.StrokeColor;
            _cardName.text = $"{cardSuitName}\n{actionSuitName}";
        }
    }
}