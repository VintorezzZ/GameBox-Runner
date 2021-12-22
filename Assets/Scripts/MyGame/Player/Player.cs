using System;
using System.Collections;
using MyGame.Managers;
using MyGame.Other;
using MyGame.Player;
using UnityEngine;
using Utils;
using Views;

[RequireComponent(typeof(PickUpHandler), typeof(MoveController), typeof(Animator))]
public class Player : MonoBehaviour
{
    #region Public Fields

    public bool god;
    public bool canMove = true;
    public float score;
    public MoveController moveController;
    public WeaponManager weaponManager;
    public GameObject snowParticles;

    #endregion

    #region Private Variables

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform rayCastPoint;
    [SerializeField] private int startHealth = 3;
    [SerializeField] private int startBullets = 3;
    [SerializeField] private PickUpHandler pickUpHandler;
    private int _health;
    private int _bullets;
    private int _coins;
    private Transform _generatedBullets;
    private bool _canShoot = true;
   
    #endregion

    private int Health
    {
        get => _health;
        set => _health = value;
    }

    public int Ammo
    {
        get => _bullets;

        set
        {
            _bullets = value;
            EventHub.OnBulletsChanged(_bullets);
        }
    }

    public int Coins
    {
        get => _coins;

        set
        {
            _coins = value;
            EventHub.OnCoinsChanged(_coins);
        }
    }
    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    public void Init()
    {
        CreateBulletsContainer();

        canMove = false;
        Health = GameSettings.Config.startHealth;
        Ammo = GameSettings.Config.startAmmo;
        Coins = 0;
        
        AddHealth(Health);
        
        weaponManager = new WeaponManager(gunHolder, rayCastPoint);
        pickUpHandler.Init(this);
        moveController.Init(this, _animator);
        EventHub.gameStarted += OnGameStarted;
    }
    
    private void OnGameStarted()
    {
        _animator.SetTrigger("run");
        canMove = true;
    }

    private void Update()
    {
        if (!canMove)
            return;
        
        if (_health <= 0f)
            StartDeathRoutine();
        
        weaponManager.Tick();
        moveController.Tick();
        
        UpdateScore();
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_canShoot && _bullets > 0)
            {
                //StartCoroutine(Shoot());
            }
        }
    }
    
    private void UpdateScore()
    {
        score += moveController.Speed * Time.deltaTime;
    }

    public void CheckForBestScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore_distance"))
        {
            PlayerPrefs.SetInt("HighScore_distance", Mathf.RoundToInt(score));
        }
    }
    
    private void CreateBulletsContainer()
    {
        _generatedBullets = new GameObject("generatedBullets").transform;
        _generatedBullets.SetParent(FindObjectOfType<WorldBuilder>().transform);
    }
    
    private IEnumerator Shoot()
    {
        weaponManager.Shoot();

        Ammo--;

        _canShoot = false;
        yield return new WaitForSeconds(0.3f);
        _canShoot = true;
    }

    private void AddHealth(int health)
    {
        for (int i = 0; i < health; i++)
        {
            ViewManager.GetView<InGameView>().AddHealth(i);
        }
    }

    private void StartDeathRoutine()
    {
        canMove = false;
        _animator.SetTrigger("dead");
        score = Mathf.RoundToInt(score);
        CheckForBestScore();
        ApplyCoins();
        
        EventHub.OnGameOvered();
    }

    private void ApplyCoins()
    {
        var totalCoins = PlayerPrefs.GetInt("Total_coins") + _coins;
        PlayerPrefs.SetInt("Total_coins", totalCoins);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!canMove || moveController.isRocketMovement)
            return;

        CheckForObstacle(other);
        
        if (other.gameObject.CompareTag("Lose"))
            StartDeathRoutine();
    }
    
    private void CheckForObstacle(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            SoundManager.Instance.PlayHit();
            if(!god)
                Health--;
            ViewManager.GetView<InGameView>().RemoveHealth(Health); //Event OnHit
            if (Health >= 1)
            {
                PoolManager.Return(other.gameObject.GetComponentInParent<PoolItem>());
            }
            else
            {
                StartDeathRoutine();
            }
        }
    }

    private void OnDestroy()
    {
        EventHub.gameStarted -= OnGameStarted;
    }

    public GameObject GetSnowEffect()
    {
        return snowParticles;
    }
}
