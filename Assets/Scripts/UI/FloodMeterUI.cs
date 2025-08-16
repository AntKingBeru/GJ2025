using UnityEngine;
using UnityEngine.UI;

public class FloodMeterUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var image = gameObject.GetComponent<Image>();
        var manager = FindFirstObjectByType<GameManager>();

        image.fillAmount = manager.currentFloodPercent;

    }
}
