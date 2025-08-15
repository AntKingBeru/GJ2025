using UnityEngine;
using UnityEditor; // Required for EditorApplication

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _maxSpawnsAllowed = 20;
    [SerializeField] private GameObject _spawnManagerPrefab;
    private GameObject _spawnManager;
    
    void Start()
    {
        _spawnManager = Instantiate(_spawnManagerPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnManager scriptReference = _spawnManager.GetComponent<SpawnManager>();

        if (scriptReference.OpenPipesCount() >= _maxSpawnsAllowed)
        {
#if UNITY_EDITOR
            // Stop Play Mode in the Editor
            EditorApplication.isPlaying = false;
#else
        // Quit the built application
        Application.Quit();
#endif
        }
    }
}
