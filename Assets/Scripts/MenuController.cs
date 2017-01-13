using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public void CloseApplication()
    {
        Application.Quit();
    }

    public void StartLevel(int level)
    {
		SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

}
