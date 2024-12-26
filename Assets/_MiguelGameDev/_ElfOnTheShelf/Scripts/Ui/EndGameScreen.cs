using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private EndGamePanel _winPanel;
        [SerializeField] private  EndGamePanel _losePanel;

        public async void Show(bool isWin)
        {
            await UniTask.Delay(500);
            if (isWin)
            {
                Audio2dService.Instance.PlayAudio(EAudioEvent.WinMusic);
                _winPanel.Show();
            }
            else
            {
                Audio2dService.Instance.PlayAudio(EAudioEvent.LoseMusic);
                _losePanel.Show();
            }
        }
    }
}