using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneFader.instance.LoadSceneWithFade("Level1");
    }

    public void Tutorial()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        SceneFader.instance.LoadSceneWithFade("MainMenu");
    }

    public void TryAgain()
    {
        SceneFader.instance.LoadSceneWithFade(SceneManager.GetActiveScene().name);
    }
}
