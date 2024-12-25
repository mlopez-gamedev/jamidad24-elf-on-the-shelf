using UnityEngine;
using UnityEngine.EventSystems;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayGoalWithTrickCardSlot : MonoBehaviour, IDropHandler
    {
        private PayGoalPanel _payGoalPanel;
        
        public void Setup(PayGoalPanel payGoalPanel)
        {
            _payGoalPanel = payGoalPanel;
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            _payGoalPanel.Pay();
        }
    }
}