using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    public int sceneToLoad;
    public List<GameObject> _cubeObjects;
    public List<GameObject> _inActiveCubeObjects;
    private List<CubeControl> _cubeController;

    private void switchActiveCube()
    {
		if(_cubeController[0].isRotating())
			return;

        if (_inActiveCubeObjects.Count > 0)
        {
            GameObject inactiveObject = _inActiveCubeObjects[0];
            GameObject activeObject = _cubeObjects[0];

            Color inactiveColor = inactiveObject.GetComponent<SpriteRenderer>().color;
            Color activeColor = activeObject.GetComponent<SpriteRenderer>().color;
            inactiveObject.GetComponent<SpriteRenderer>().color = activeColor;
            activeObject.GetComponent<SpriteRenderer>().color = inactiveColor;

            _inActiveCubeObjects.Remove(inactiveObject);
            _cubeObjects.Remove(activeObject);

            _inActiveCubeObjects.Add(activeObject);
            _cubeObjects.Add(inactiveObject);

			initCubeController();
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        initCubeController();
    }

	void initCubeController()
	{
		_cubeController = new List<CubeControl>();
        for (int i = 0; i < _cubeObjects.Count; i++)
            _cubeController.Add(new CubeControl(_cubeObjects[i]));
	}

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        for (int i = 0; i < _cubeObjects.Count; i++)
        {
            if (_cubeController[i] == null)
                continue;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                switchActiveCube();
                continue;
            }

			_cubeObjects[i] = _cubeController[i].getGameObject();
            _cubeController[i].Update();
            if (_cubeController[i].hasEnteredGoal())
                SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
            else if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
