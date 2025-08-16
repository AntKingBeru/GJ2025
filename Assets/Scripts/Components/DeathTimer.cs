using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeathTimer : MonoBehaviour
{
    [SerializeField] private int _seconds;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Timer());
    }
    
    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator Timer()
    {
        var countdownTime = _seconds;
        //TODO make better loop
        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownTime--; // Decrement the timer
            
            var image = gameObject.GetComponent<Image>();
            image.fillAmount = (float) countdownTime / (float)_seconds;
        }

        var deadEnemy = gameObject.GetComponentInParent<DeadEnemy>();
        deadEnemy.DeathTimerFinished();
    }
}
