using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class MagicalPortalCards : MonoBehaviour
    {
        [SerializeField] private Transform _slotsContainer;
        //[SerializeField] private Transform _portalEffect;
        [SerializeField] private MagicalPortalSlot _slotPrefab;
        [SerializeField] private Highlight _highlight;

        private readonly List<MagicalPortalSlot> _slots = new List<MagicalPortalSlot>();

        private RectTransform _rectTransform;
        
        public RectTransform RectTransform => _rectTransform ??= (RectTransform)transform;
        
        public void AddCard(CardUi card)
        {
            var cardSlot = Instantiate(_slotPrefab, _slotsContainer);
            cardSlot.Setup(card);
            _slots.Add(cardSlot);
            // if (_slots.Count < 2)
            // {
            //     _portalEffect.gameObject.SetActive(true);
            //     return;
            // }

            var rotation = 360f / _slots.Count;
            for (int i = 0; i < _slots.Count; ++i)
            {
                var slot = _slots[i];
                if (slot == cardSlot)
                {
                    slot.transform.localRotation = Quaternion.Euler(0, 0, rotation * i);
                    continue;
                }
                
                slot.transform.DOLocalRotate(new Vector3(0, 0, rotation * i), 0.1f);
            }
        }

        public bool TryPopCard(Transform newParent, out CardUi cardUi)
        {
            if (_slots.Count == 0)
            {
                cardUi = null;
                return false;
            }
            
            cardUi = _slots[0].PopCard(newParent);

            _slots.RemoveAt(0);
            // if (_slots.Count == 0)
            // {
            //     _portalEffect.gameObject.SetActive(false);    
            // }
            
            return cardUi;
        }

        private void Update()
        {
            _slotsContainer.Rotate(0, 0, -180f * Time.deltaTime);
        }

        public void PlayHighlight()
        {
            _highlight.Play();
        }
        
        public void StopHighlight()
        {
            _highlight.Stop();
        }
    }
}