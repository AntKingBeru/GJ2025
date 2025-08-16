using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private float _spawnRate;
    [SerializeField] private int _xCount;
    [SerializeField] private int _yCount;
    [SerializeField] private float _spawnerXoffset;
    [SerializeField] private float _spawnerYoffset;
    private SpawnPoint _spawner;
    private Enemy _enemy;
    private GameManager _gameManager;
    
    private List<SpawnPoint> _spawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spawner = gameObject.GetComponentInChildren<SpawnPoint>();
        _spawner.gameObject.SetActive(false);
        _enemy = gameObject.GetComponentInChildren<Enemy>();
        _enemy.gameObject.SetActive(false);
        _gameManager = gameObject.GetComponentInParent<GameManager>();
        
        InitializeSpawnPoints();
    }

    void Awake()
    {
        StartCoroutine(StartSpawning());
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnDisable()
    {
        StopAllCoroutines();

        foreach (var spawnPoint in _spawnPoints)
        {
            spawnPoint.Close();
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void InitializeSpawnPoints()
    {
        _spawner.gameObject.SetActive(false);
        _spawnPoints = new List<SpawnPoint>();
        float baseX = 0;
        float x = 0;
        for (var i = 0; i < _xCount; i++)
        {
            float baseY = 0;
            float y = 0;
            
            for (var j = 0; j < _yCount; j++)
            {
                var location = new Vector2(x, y) + new Vector2(_spawnerXoffset, _spawnerYoffset);
                GameObject spawner = Instantiate(_spawner.gameObject,location,Quaternion.identity);
                SpawnPoint spawnPoint = spawner.GetComponent<SpawnPoint>();
                spawnPoint.SetParameters(_enemy.gameObject,_gameManager);
                // scriptReference.
                _spawnPoints.Add(spawnPoint);
                
                if (j % 2 == 0)
                {
                    baseY++;
                    y = baseY;
                }
                else
                    y = -baseY;
            }
            
            if (i % 2 == 0)
            {
                baseX++;
                x = baseX;
            }
            else
                x = -baseX;
        }
    }

    private IEnumerator StartSpawning()
    {
        //TODO make better loop
        while (true)
        {
            if(GameManager.pause) 
                continue;
            
            yield return new WaitForSeconds(_spawnRate);
            var closedSp = _spawnPoints.FindAll((spawner) => !spawner.IsOpen);
            
            if (closedSp.Count > 0)
            {
                int randomIndex = Random.Range(0,closedSp.Count);
                closedSp[randomIndex].Open();
            }
        }
    }

    public int OpenPipesCount()
    {
        return _spawnPoints.FindAll((spawner) => spawner.IsOpen).Count;
    }
}
