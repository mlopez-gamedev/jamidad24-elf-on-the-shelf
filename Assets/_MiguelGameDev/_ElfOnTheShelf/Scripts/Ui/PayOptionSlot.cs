using UnityEngine;
using UnityEngine.EventSystems;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayOptionSlot : MonoBehaviour, IDropHandler
    {
        private PayGoalPanel _payGoalPanel;
        
        private System.Action _onDropAction;
        protected bool _isEnabled;
        
        public void Setup(System.Action onDropAction, bool enabled = true)
        {
            _onDropAction = onDropAction;
            _isEnabled = enabled;
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (!_isEnabled)
            {
                return;
            }
            _onDropAction.Invoke();
        }
        
        public virtual void SetEnable(bool enabled)
        {
            _isEnabled = enabled;
        }
    }
}