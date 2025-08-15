using System.Collections;
using UnityEngine;

public class DeadEnemy : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DeathTimer());
    }
    
    
    // Timer until object is destroyed on its own
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}
