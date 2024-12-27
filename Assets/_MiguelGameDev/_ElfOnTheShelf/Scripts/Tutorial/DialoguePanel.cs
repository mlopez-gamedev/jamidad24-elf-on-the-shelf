using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class DialoguePanel : MonoBehaviour
    {
        private const float BottomPosition = -180f;
        private const float MiddlePosition = 0f;
        private const float TopPosition = 180f;
        
        [SerializeField] private TextVisibilityAnimation _textAnimation;
        [SerializeField] private Button _nextButton;
        
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private RectTransform _panel;
        
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ??= (RectTransform)transform;

        private string[] _textLines;
        private int _lineIndex;
        private UniTaskCompletionSource _dialogueCompletionSource;

        private void Start()
        {
            _nextButton.onClick.AddListener(NextButtonClicked);
        }

        public UniTask Show(string[] textLines)
        {
            _dialogueCompletionSource = new UniTaskCompletionSource();
            _textLines = textLines;
            _lineIndex = 0;
            _textAnimation.Text.text = _textLines[_lineIndex];
            _textAnimation.Text.maxVisibleCharacters = 0;
         
            _overlay.alpha = 0;
            _panel.localScale = Vector3.zero;
            
            _panel.gameObject.SetActive(true);
            _overlay.gameObject.SetActive(true);
            
            DOTween.Sequence()
                .Join(_overlay.DOFade(1f, 0.2f))
                .Join(_panel.DOScale(1f, 0.2f))
                .OnComplete(_textAnimation.Play);
            
            return _dialogueCompletionSource.Task;
        }
        
        private void NextButtonClicked()
        {
            if (!_textAnimation.IsComplete)
            {
                _textAnimation.Complete();
                return;
            }
            
            ++_lineIndex;
            if (_lineIndex < _textLines.Length)
            {
                _textAnimation.Play(_textLines[_lineIndex]);
                return;
            }
            
            Hide();
        }

        private void Hide()
        {
            _panel.DOScale(0, 0.2f).OnComplete(
                EndDialogue);
            
            void EndDialogue()
            {
                _textAnimation.Text.text = string.Empty;
                _panel.gameObject.SetActive(false);
                _overlay.gameObject.SetActive(false);
                _dialogueCompletionSource.TrySetResult();
            }
        }

        public void SetPanelPosition(ETutorialPosition position)
        {
            _panel.SetScale(0);
            switch (position)
            {
                case ETutorialPosition.Bottom:
                    _panel.SetAnchoredPositionY(BottomPosition);
                    break;
                case ETutorialPosition.Middle:
                    _panel.SetAnchoredPositionY(MiddlePosition);
                    break;
                case ETutorialPosition.Top:
                    _panel.SetAnchoredPositionY(TopPosition);
                    break;
            }
        }
    }
}