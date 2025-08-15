using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool _open = false;
	[SerializeField] private GameObject _enemy;
    
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
		gameObject.SetActive(true);
    }

    public void Close()
    {
        _open = false;
		gameObject.SetActive(false);
    }
}
