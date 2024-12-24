using DG.Tweening;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class MagicalPortalSlot : MonoBehaviour
    {
        [SerializeField] private Transform _cardSocket;

        private CardUi _card;
        
        public void Setup(CardUi card)
        {
            _card = card;
            card.transform.SetParent(_cardSocket);
            card.transform.rotation = Quaternion.identity;
            card.transform.DOLocalMove(Vector3.zero, 0.2f);
        }

        public CardUi PopCard(Transform newParent)
        {
            _card.transform.SetParent(newParent);
            Destroy(gameObject);
            return _card;
        }
    }
}