using System;
using Cinemachine;
using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views;


public class RoomController : SingletonBehaviour<RoomController>
{
    private double _startGameDelay = 3;
    private Text _timerText;
    
    [HideInInspector] public Player localPlayer;
    [SerializeField] private CinemachineVirtualCamera playerCamera;

    public Timer startTimer = new Timer();
    public bool isGameStarted = false;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        localPlayer = Instantiate(Resources.Load<Player>("Player"), new Vector3(0f, 0f, 10f), Quaternion.identity);
        localPlayer.Init();
        localPlayer.transform.SetParent(transform);
        playerCamera.Follow = localPlayer.transform;
        playerCamera.LookAt = localPlayer.transform;
        _timerText = ViewManager.GetView<InGameView>().timerText;
    }

    private void Update()
    {
        if(!startTimer.IsStarted)
            return;
        
        if (startTimer.IsStarted)
            UpdateTimerView();
        
        if (startTimer.Time >= _startGameDelay)
            StartGame();
    }

    private void UpdateTimerView()
    {
        double countdown = _startGameDelay - startTimer.Time;
        _timerText.text = $"Game starts in {countdown:n0} seconds";
    }

    private void StartGame()
    {
        isGameStarted = true;
        startTimer.Stop();
        GameManager.Instance.StartGame();
        _timerText.text = string.Empty;

        Debug.LogError("Start Game RPC");
    }
}
