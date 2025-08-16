using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    private bool _open = false;
	private GameObject _enemy;
	[SerializeField] private float _spawnRate;
	[SerializeField] private float _spawnChance = 30f;
    private GameManager _gameManager;
        
    // For all animations
    protected Animator _animator;

    public void SetParameters(GameObject enemy,GameManager gameManager)
    {
        _enemy = enemy;
        _gameManager = gameManager;
    }

    void Awake()
    {
        // Getting animator
        this._animator = GetComponent<Animator>();
    }

    void Update()
    {
        this._animator.SetFloat("Percentage", _gameManager.currentFloodPercent);
    }
    
    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public bool IsOpen
    {
        get => _open;
    }

    public void Open()
    {
        _open = true;
        // Instantiate your object at spawnPosition
		gameObject.SetActive(true);
        Instantiate(_enemy,gameObject.transform.position,gameObject.transform.rotation).SetActive(true);
        StartCoroutine(StartSpawning());
    }

    public void Close()
    {
        _open = false;
		gameObject.SetActive(false);
        StopCoroutine(StartSpawning());
    }
    private IEnumerator StartSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);

            if(Random.Range(0f, 100f) <= _spawnChance)
                Instantiate(_enemy,gameObject.transform.position,gameObject.transform.rotation).SetActive(true);
        }
    }
}
