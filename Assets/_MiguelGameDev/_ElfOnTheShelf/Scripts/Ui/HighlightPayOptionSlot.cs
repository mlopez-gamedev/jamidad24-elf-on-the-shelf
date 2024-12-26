using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class HighlightPayOptionSlot : PayOptionSlot, IDropHandler
    {
        [SerializeField] Highlight _highlight;
        [SerializeField] Image _disablePanel;
        
        public void PlayHighlight()
        {
            if (!_isEnabled)
            {
                return;
            }
            _highlight.Play();
        }
        
        public void StopHighlight()
        {
            if (!_isEnabled)
            {
                return;
            }
            _highlight.Stop();
        }

        public override void SetEnable(bool enabled)
        {
            base.SetEnable(enabled);
            _disablePanel.gameObject.SetActive(!enabled);
        }
    }
}