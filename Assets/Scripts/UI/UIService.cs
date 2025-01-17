using Blaster.Events;
using Blaster.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{
    #region Services
    private EventService _eventService;
    private LevelService _levelService;
    #endregion
    [Header("GameOver")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMPro.TMP_Text _gameOverText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private TMPro.TMP_Text _levelText;

    [Header("Gameplay")]
    [SerializeField] private TMPro.TMP_Text _progressLevelText;
    [SerializeField] private Image _progressImage;

    // Start is called before the first frame update
    void Start()
    {
        _nextLevelButton.onClick.AddListener(OnNextLevel);
    }

    public void Init(EventService eventService, LevelService levelService)
    {
        _eventService = eventService;
        _levelService = levelService;
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        _eventService.OnUpdateProgress.AddListener(SetProgress);
        _eventService.OnGameStart.AddListener(OnGameStart);
        _eventService.OnGameEnd.AddListener(ShowGameOverPanel);
    }
    private void UnsubscribeEvents()
    {
        _eventService.OnUpdateProgress.RemoveListener(SetProgress);
        _eventService.OnGameStart.RemoveListener(OnGameStart);
        _eventService.OnGameEnd.RemoveListener(ShowGameOverPanel);
        _restartButton.onClick.AddListener(OnRestart);
    }
    public void SetProgress(float progress)
    {
        _progressImage.fillAmount = progress;
    }
    public void OnNextLevel() { _levelService.NextLevel(); }
    public void OnRestart()
    {

    }
    public void OnGameStart(int levelIndex)
    {
        _gameOverPanel.SetActive(false);
        _progressImage.fillAmount = 1;
        _progressLevelText.text = "Level " + levelIndex;
        Debug.Log("Game Started");
    }
    public void ShowGameOverPanel(bool isWin)
    {
        _gameOverPanel.SetActive(true);

        _nextLevelButton.gameObject.SetActive(isWin);
        _restartButton.gameObject.SetActive(!isWin);

        _gameOverText.text = isWin ? "Victory!" : "Game Over!";
       
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}
