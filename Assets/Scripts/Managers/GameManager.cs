using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _maxWaterBeforeFlood = 20000;
    [SerializeField] private int _maxWaterPerSpawn = 200;
    [SerializeField] private GameObject _spawnManagerPrefab;
    private GameObject _spawnManager;
    private GameObject _youLoseScreen;
    private Player _player;
    private int _currentFlood = 0;
    
    public float currentFloodPercent
    {
        get => (float)_currentFlood / (float)_maxWaterBeforeFlood;
    }
    
    void Start()
    {
        _spawnManager = Instantiate(_spawnManagerPrefab);
        StartCoroutine(CalculateFlood());
        _player = FindFirstObjectByType<Player>();
        
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
            Entity.pause = true;
            // Show the losing screen
            _youLoseScreen.SetActive(true);
        }
    }

    private IEnumerator CalculateFlood()
    {
        SpawnManager scriptReference = _spawnManager.GetComponent<SpawnManager>();

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            var currentOpenPipes = scriptReference.OpenPipesCount();

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
