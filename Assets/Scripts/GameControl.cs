using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    public int sceneToLoad;
    public int sceneToLoadOnEscape = -1;
    public List<GameObject> _cubeObjects;
    public List<GameObject> _inActiveCubeObjects;
    public List<CubeControl> _cubeController;
    public List<GameObject> _goals;

    private void switchActiveCube()
    {
        if (_cubeController.Count == 0 || _inActiveCubeObjects.Count == 0)
            return;

        if (_cubeController[0] == null || _inActiveCubeObjects[0] == null)
            return;

        if (_cubeController[0].isRotating() || !_cubeController[0].isGrounded())
            return;

        GameObject inactiveObject = _inActiveCubeObjects[0];
        GameObject activeObject = _cubeController[0].getGameObject();

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

    private void initCubeController()
    {
        _cubeController = new List<CubeControl>();
        for (int i = 0; i < _cubeObjects.Count; i++)
            _cubeController.Add(new CubeControl(_cubeObjects[i]));
    }

    private bool shareGoalPositionAndScale(GameObject cubeObject, GameObject goal)
    {
        return goal.transform.position.Equals(cubeObject.transform.position) &&
                goal.transform.transform.localScale.Equals(cubeObject.transform.localScale);
    }

    private bool goalHasBeenReached(GameObject goal)
    {
        for (int i = 0; i < _cubeObjects.Count; i++)
            if (shareGoalPositionAndScale(_cubeObjects[i], goal))
                return true;
        return false;
    }

    private bool allGolasHaveBeenReached()
    {
        for (int i = 0; i < _goals.Count; i++)
            if (!goalHasBeenReached(_goals[i]))
                return false;

        return true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        for (int i = 0; i < _cubeObjects.Count; i++)
        {
            if (_cubeObjects[i].transform.position.y < 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (_cubeController == null || _cubeController[i] == null)
                continue;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                switchActiveCube();
                break;
            }

            _cubeController[i].Update();
            _cubeObjects[i] = _cubeController[i].getGameObject();

        }
        for (int i = 0; i < _inActiveCubeObjects.Count; i++)
        {
            GridManager.correctPositionToGrid(_inActiveCubeObjects[i].transform);
        }

        if (allGolasHaveBeenReached())
            SceneManager.LoadScene(sceneToLoad);
        else if (Input.GetKeyDown(KeyCode.Escape))
            if (sceneToLoadOnEscape > -1)
                SceneManager.LoadScene(sceneToLoadOnEscape);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        initCubeController();
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        DebugHelper.drawRayFromPoint(new Vector2(), Color.yellow, 0);
    }
}
