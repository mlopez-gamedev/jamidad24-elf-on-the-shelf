using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] Button _startButton;
        [SerializeField] Button _tutorialButton;
        [SerializeField] Game _game;
        
        private void Awake()
        {
            _canvasGroup.gameObject.SetActive(true);
            _startButton.onClick.AddListener(OnStartButtonClick);
            _tutorialButton.onClick.AddListener(OnTutorialButtonClick);
        }

        private void Start()
        {
            Audio2dService.Instance.PlayAudio(EAudioEvent.TitleMusic);
            _startButton.interactable = PlayerPrefs.HasKey("TutorialPlayed");
        }

        private void OnStartButtonClick()
        {
            StartGame();
        }
        
        private void OnTutorialButtonClick()
        {
            Tutorial.Instance.Init();
            PlayerPrefs.SetInt("TutorialPlayed", 1);
            PlayerPrefs.Save();
            StartGame();
        }

        private void StartGame()
        {
            _canvasGroup.interactable = false;
            Audio2dService.Instance.PlayAudio(EAudioEvent.StartGameSfx);
            Audio2dService.Instance.StopAudio(EAudioEvent.TitleMusic);
            _canvasGroup.DOFade(0, 1f).OnComplete(HideAndInitGame);

            void HideAndInitGame()
            {
                _canvasGroup.gameObject.SetActive(false);
                // TODO: Show Elf Name
                _game.Init("Test", false);
                Audio2dService.Instance.PlayAudio(EAudioEvent.GameMusic);
            }
        }
    }
}