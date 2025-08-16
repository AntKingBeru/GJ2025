using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool pause = false;
    [SerializeField] private int _maxWaterBeforeFlood = 20000;
    [SerializeField] private int _maxWaterPerSpawn = 200;
    private SpawnManager _spawnManager;
    private GameObject _youLoseScreen;
    private Player _player;
    private int _currentFlood = 0;

    enum WaterLevel
    {
        Zero, // 0 - 0.1
        First, // 0.1 - 0.3
        Second, // 0.3 - 0.5
        Third, // 0.5 - 0.8
        Forth // 0.8 - 1
    }
    
    private WaterLevel _currentWaterLevel = WaterLevel.Zero;
    public float currentFloodPercent
    {
        get => (float)_currentFlood / (float)_maxWaterBeforeFlood;
    }

    public static void SetPause(bool pauseEntites = true)
    {
         pause = pauseEntites;
    }
    
    void Start()
    {
        _spawnManager = gameObject.GetComponentInChildren<SpawnManager>();
        StartCoroutine(CalculateFlood());
        _player = FindFirstObjectByType<Player>();
        GameManager.SetPause(false); // Reset pause
        
        var children = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.name == "YouLose")
            {
                _youLoseScreen = child.gameObject;
                _youLoseScreen.SetActive(false);
                break;
            }
        }
    }

    void Awake()
    {
        StartCoroutine(CalculateFlood());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame 1
    void Update()
    {
        if (_currentFlood >= _maxWaterBeforeFlood ||
            _player.isDead)
        {
            GameManager.SetPause();
            // Show the losing screen
            _youLoseScreen.SetActive(true);
            
            _spawnManager.gameObject.SetActive(false);
        }
        
        // Checking current flood to activate sound
        
        // Getting current water level
        WaterLevel waterLevel;
        if (_currentFlood >= 0f && _currentFlood < 0.1f)
            waterLevel = WaterLevel.Zero;
        else if (_currentFlood >= 0.1f && _currentFlood < 0.3f)
            waterLevel = WaterLevel.First;
        else if (_currentFlood >= 0.3f && _currentFlood < 0.5f)
            waterLevel = WaterLevel.Second;
        else if (_currentFlood >= 0.5f && _currentFlood < 0.8f)
            waterLevel = WaterLevel.Third;
        else
            waterLevel = WaterLevel.Forth;
        
        // Activating water sound on water level change
        if (this._currentWaterLevel != waterLevel)
        {
            this._currentWaterLevel = waterLevel;
            AudioManager.instance.PlaySound("Water", 1f);
        }
    }

    private IEnumerator CalculateFlood()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            var currentOpenPipes = _spawnManager.OpenPipesCount();

            var maxFloodAllowed = _maxWaterPerSpawn * currentOpenPipes;
            
            if (_currentFlood >= maxFloodAllowed)
            {
                if (_currentFlood - currentOpenPipes < maxFloodAllowed)
                    _currentFlood = maxFloodAllowed;
                else
                    _currentFlood -= currentOpenPipes;
            }
            else
                _currentFlood += currentOpenPipes;
        }
    }
}
