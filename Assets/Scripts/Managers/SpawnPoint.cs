using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    private Vector2 _location;
    private bool _open;
    private GameObject _spawned;

    public SpawnPoint(Vector2 location,GameObject spawned)
    {
        _open = false;
        _location = location;
        _spawned = spawned;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsOpen
    {
        get => _open;
    }

    public void Open()
    {
        _open = true;
        // Instantiate your object at spawnPosition
        Instantiate(_spawned, _location, Quaternion.identity);
    }

    public void Close()
    {
        _open = false;
    }
}
