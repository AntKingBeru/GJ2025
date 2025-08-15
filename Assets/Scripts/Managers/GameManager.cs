using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _maxWaterBeforeFlood = 20000;
    [SerializeField] private GameObject _spawnManagerPrefab;
    private GameObject _spawnManager;
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
            // Show the losing screen
            var losingScreen = GameObject.Find("YouLose");
            var image = losingScreen.GetComponent<Image>();
            image.enabled = true;
        }
    }

    private IEnumerator CalculateFlood()
    {
        SpawnManager scriptReference = _spawnManager.GetComponent<SpawnManager>();

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            var currentOpenPipes = scriptReference.OpenPipesCount();
            _currentFlood += currentOpenPipes;
        }
    }
}
