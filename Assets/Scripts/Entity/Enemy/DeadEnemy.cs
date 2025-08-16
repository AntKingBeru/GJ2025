using System.Collections;
using UnityEngine;

public class DeadEnemy : MonoBehaviour
{
    public bool isCarried = false;
    public void DeathTimerFinished()
    {
        Destroy(gameObject);
    }
}
