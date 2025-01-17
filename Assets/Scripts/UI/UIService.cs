using Blaster.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{
    #region Services
    private EventService _eventService;
    #endregion
    [Header("GameOver")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMPro.TMP_Text _gameOverText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private TMPro.TMP_Text _levelText;

    [Header("Gameplay")]
    [SerializeField] private TMPro.TMP_Text _scoreText;
    [SerializeField] private Image _progressImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    public void Init(EventService eventService)
    {
        _eventService = eventService;
       SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        _eventService.OnUpdateProgress.AddListener(SetProgress);
    }
    public void SetProgress(float progress)
    {
        _progressImage.fillAmount = progress;
    }
}
