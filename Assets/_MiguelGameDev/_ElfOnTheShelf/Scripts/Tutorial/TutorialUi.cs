using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class TutorialUi : MonoBehaviour
    {
        [SerializeField] private DialoguePanel _dialoguePanel;
        
        [SerializeField] private RectTransform _handSlot;
        [SerializeField] private RectTransform _hand;
        
        [SerializeField] private RectTransform _deckSlot;
        [SerializeField] private RectTransform _deck;
        [SerializeField] private RectTransform _deckAmount;
        
        [SerializeField] private RectTransform _actionsSlot;
        [SerializeField] private RectTransform _actions;
        
        [SerializeField] private RectTransform _discardPileSlot;
        [SerializeField] private RectTransform _discardPile;
        
        [SerializeField] private RectTransform _goalAreaSlot;
        [SerializeField] private RectTransform _goalArea;
        
        public UniTask ShowDialogue(string textTerm, ETutorialPosition position = ETutorialPosition.Middle)
        {
            _dialoguePanel.SetPanelPosition(position);
            
            var text = I2.Loc.LocalizationManager.GetTranslation(textTerm);
            return _dialoguePanel.Show(text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries));
        }

        public void SetHandOnTop(bool onTop)
        {
            if (onTop)
            {
                _hand.SetParent(transform, true);
            }
            else
            {
                _hand.SetParent(_handSlot, true);
            }
        }
        
        public void SetDeckOnTop(bool onTop)
        {
            if (onTop)
            {
                _deck.SetParent(transform, true);
                _deckAmount.SetParent(transform, true);
            }
            else
            {
                _deck.SetParent(_deckSlot, true);
                _deckAmount.SetParent(_deckSlot, true);
            }
        }
        
        public void SetActionsRunOnTop(bool onTop)
        {
            if (onTop)
            {
                _actions.SetParent(transform, true);
            }
            else
            {
                _actions.SetParent(_actionsSlot, true);
            }
        }
        
        public void SetDiscardPileOnTop(bool onTop)
        {
            if (onTop)
            {
                _discardPile.SetParent(transform, true);
            }
            else
            {
                _discardPile.SetParent(_discardPileSlot, true);
            }
        }
        
        public void SetGoalArealOnTop(bool onTop)
        {
            if (onTop)
            {
                _goalArea.SetParent(transform, true);
            }
            else
            {
                _goalArea.SetParent(_goalAreaSlot, true);
            }
        }
    }
}