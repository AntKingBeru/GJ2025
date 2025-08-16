using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void StartGame()
    {
        SceneFader.instance.LoadSceneWithFade(1);
    }

    public void Tutorial()
    {
        SceneFader.instance.LoadSceneWithFade(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        SceneFader.instance.LoadSceneWithFade(0);
    }

    public void TryAgain()
    {
        SceneFader.instance.LoadSceneWithFade(SceneManager.GetActiveScene().buildIndex);
    }
}
