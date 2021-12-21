using System;
using Cinemachine;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Utils;
using Views;

    public class GameManager : SingletonBehaviour<GameManager>
    {
        public CinemachineVirtualCamera playerCamera;
        public PostProcessVolume postProcessVolume;
        private void Awake()
        {
            Application.targetFrameRate = 60;
                
            InitializeSingleton();
            
            EventHub.gameOvered += OnGameOver;

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.name == "Gameplay")
                {
                    InitGameScene(scene);
                }
            };

            SceneManager.sceneUnloaded += scene =>
            {
                if (scene.name == "Gameplay")
                {
                    LoadGameScene();
                }
            };
        }
        private void InitGameScene(Scene scene)
        {
            ViewManager.Show<MainMenuView>();
            SceneManager.SetActiveScene(scene);
            WorldBuilder.Instance.Init(0);
        }

        private void OnGameOver()
        {
            ViewManager.Show<GameOverView>();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }
  
        public void QuitGame()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            ViewManager.Show<LoadingScreenView>();
            SoundManager.Instance.PreRestartGame();
            SceneManager.UnloadSceneAsync("Gameplay");
        }

        public void StartGame()
        {
            EventHub.OnGameStarted();
            Time.timeScale = 1;
        }

        public void UnpauseGame()
        {
            
        }
        
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
        }
    }

