using DG.Tweening;
using LeTai.TrueShadow;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    
    public class Highlight : MonoBehaviour
    {
        [SerializeField] private TrueShadow _glowEffect;

        private Tween _tween;

        [Button]
        public void Play()
        {
            _glowEffect.Color.SetAlpha(1f);
            _glowEffect.enabled = true;
            _tween = DOVirtual.Float(0.5f, 1.0f, 1f, SetAlpha)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            void SetAlpha(float alpha)
            {
                _glowEffect.Color = new Color(_glowEffect.Color.r, _glowEffect.Color.g, _glowEffect.Color.b,alpha);
            }
        }

        [Button]
        public void Stop()
        {
            _tween?.Kill();
            _glowEffect.enabled = false;
        }
    }
}