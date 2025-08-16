using System.Collections;
using UnityEngine;

public class DeadEnemy : MonoBehaviour
{
    public void DeathTimerFinished()
    {
        Destroy(gameObject);
    }
}
