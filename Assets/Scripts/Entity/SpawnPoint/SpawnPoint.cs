using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    private bool _open = false;
	[SerializeField] private GameObject _enemy;
	[SerializeField] private float _spawnRate;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

    public bool IsOpen
    {
        get => _open;
    }

    public void Open()
    {
        _open = true;
        // Instantiate your object at spawnPosition
		gameObject.SetActive(true);
        Instantiate(_enemy,gameObject.transform.position,gameObject.transform.rotation);
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
            Instantiate(_enemy,gameObject.transform.position,gameObject.transform.rotation);
        }

    }
}
