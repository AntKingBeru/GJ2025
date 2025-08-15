using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private float _spawnRate;
    [SerializeField] private int _xCount;
    [SerializeField] private int _yCount;
    [SerializeField] private GameObject _spawnerPrefab;
    
    private List<GameObject> _spawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeSpawnPoints();
        StartCoroutine(StartSpawning());
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void InitializeSpawnPoints()
    {
        _spawnerPrefab.SetActive(false);
        _spawnPoints = new List<GameObject>();
        float baseX = 0;
        float x = 0;
        for (var i = 0; i < _xCount; i++)
        {
            float baseY = 0;
            float y = 0;
            
            for (var j = 0; j < _yCount; j++)
            {
                GameObject spawner = Instantiate(_spawnerPrefab,new Vector2(x,y),Quaternion.identity);
                SpawnPoint scriptReference = spawner.GetComponent<SpawnPoint>();
                // scriptReference.
                _spawnPoints.Add(spawner);
                
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
            yield return new WaitForSeconds(_spawnRate);
            var closedSp = _spawnPoints.FindAll((spawner) =>
            {
                SpawnPoint scriptReference = spawner.GetComponent<SpawnPoint>();
                return !scriptReference.IsOpen;
            });
            
            if (closedSp.Count > 0)
            {
                int randomIndex = Random.Range(0,closedSp.Count);
                SpawnPoint scriptReference = closedSp[randomIndex].GetComponent<SpawnPoint>();
                scriptReference.Open();
            }
        }
    }

    public int OpenPipesCount()
    {
        return _spawnPoints.FindAll((spawner) =>
        {
            SpawnPoint scriptReference = spawner.GetComponent<SpawnPoint>();
            return scriptReference.IsOpen;
        }).Count;
    }
}
