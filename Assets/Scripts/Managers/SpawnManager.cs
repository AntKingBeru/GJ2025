using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private float _spawnRate;
    [SerializeField] private List<Vector2> _points;
    private GameObject _spawnedEnemy;
    
    private List<SpawnPoint> _spawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialize spawned enemy
        _spawnedEnemy = new GameObject("Circle");
     
        // The sprite is loaded from the circle image as textrue 2D. And the color of the circle is set.
        SpriteRenderer renderer = _spawnedEnemy.AddComponent<SpriteRenderer>();
        renderer.color = Color.blue;
        Texture2D tex = Resources.Load<Texture2D>("Circle");

        // This loading function load the pixels per unit to be equal to the texture width (which should be equal to it's height).
        // So that means that the loaded sprite will ocuppy a single unit of world space in height and width.
        Sprite sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;
        
        InitializeSpawnPoints();
        StartCoroutine(StartSpawning());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void InitializeSpawnPoints()
    {
        _spawnPoints = new List<SpawnPoint>();
        
        for (var i = 0; i < _points.Count; i++)
        {
            _spawnPoints.Add(new SpawnPoint(_points[i],_spawnedEnemy));
        }
    }

    private IEnumerator StartSpawning()
    {
        //TODO make better loop
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            var closedSp = _spawnPoints.FindAll((spawnPoint) => !spawnPoint.IsOpen);

            if (closedSp.Count > 0)
            {
                int randomIndex = Random.Range(0,closedSp.Count);
                closedSp[randomIndex].Open();
            }
        }

    }
}
