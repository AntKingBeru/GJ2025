using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var image = gameObject.GetComponent<Image>();
        var player = FindFirstObjectByType<Player>();

        image.fillAmount = player.currentHealthPercent;

    }
}
