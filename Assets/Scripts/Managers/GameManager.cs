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
