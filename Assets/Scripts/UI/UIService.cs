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
    [SerializeField] private Button _restartMenuButton;
    [SerializeField] private GameObject _shadowPanel;

    private int _levelIndex;
    // Start is called before the first frame update
    void Start()
    {
        _nextLevelButton.onClick.AddListener(OnNextLevel);
        _restartButton.onClick.AddListener(OnRestart);
        _restartMenuButton.onClick.AddListener(OnRestart);
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
       
    }
    public void SetProgress(float progress)
    {
        _progressImage.fillAmount = progress;
    }
    public void OnNextLevel() { _levelService.NextLevel(); }
    public void OnRestart()
    {
        _eventService.OnGameStart.InvokeEvent(_levelIndex);
    }
    public void OnGameStart(int levelIndex)
    {
        _levelIndex = levelIndex;
        _gameOverPanel.SetActive(false);
        _progressImage.fillAmount = 1;
        _progressLevelText.text = "Level " + levelIndex;
        _shadowPanel.SetActive(true);
        Debug.Log("Game Started");
    }
    public void ShowGameOverPanel(bool isWin)
    {
        _gameOverPanel.SetActive(true);
        _shadowPanel.SetActive(false);

        _nextLevelButton.gameObject.SetActive(isWin);
        _restartButton.gameObject.SetActive(!isWin);
        _levelText.text = "Level " + _levelIndex;
        _gameOverText.text = isWin ? "Victory!" : "Game Over!";
       
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}
