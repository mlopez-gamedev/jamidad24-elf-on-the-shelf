using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

namespace MiguelGameDev.ElfOnTheShelf
{
    public abstract class CardUi : MonoBehaviour
    {
        [SerializeField] private Image _picture;
        [SerializeField] private Image _reversePicture;
        
        private bool _flippedUp = false;
        protected void Setup(Card card)
        {
            _picture.sprite = card.Picture;
        }

        [Button]
        public Tween FlipUp()
        {
            if (_flippedUp)
            {
                return default;
            }
            
            _flippedUp = true;

            return DOTween.Sequence()
                .Append(transform.DOScaleX(0f, 0.1f).SetEase(Ease.InQuad))
                .AppendCallback(HideReverse)
                .Append(transform.DOScaleX(1f, 0.1f).SetEase(Ease.OutQuad));

            void HideReverse()
            {
                _reversePicture.gameObject.SetActive(false);
            }
        }
        
        [Button]
        public Tween FlipDown()
        {
            if (!_flippedUp)
            {
                return default;
            }

            _flippedUp = false;
            
            return DOTween.Sequence()
                .Append(transform.DOScaleX(0f, 0.1f).SetEase(Ease.InQuad))
                .AppendCallback(ShowReverse)
                .Append(transform.DOScaleX(1f, 0.1f).SetEase(Ease.OutQuad));

            void ShowReverse()
            {
                _reversePicture.gameObject.SetActive(true);
            }
        }
    }
}