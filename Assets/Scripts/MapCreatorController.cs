using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapCreatorController : MonoBehaviour
{
    public GameControl gameController;
    public GameObject _gridLineHorizontalPrefab;
    public GameObject _gridLineVerticalPrefab;
    public List<GameObject> prefabList;
    public GameObject currentPrefab;
    private GameObject previewPrefab;
    private Vector2 lastMousePosition;

    public void setPrefabByIndex(int index)
    {
        currentPrefab = prefabList[index];
        destroyPrefabAtMousePosition();
    }

    private void destroyPrefabAtMousePosition()
    {
        Vector2 position = GridManager.mousePositionToGridPosition();
        Destroy(GridManager.getObjectAt(position.x, position.y));
    }

    private GameObject createPrefabAtMousePosition()
    {
        Vector2 position = GridManager.mousePositionToGridPosition();
        if (GridManager.getObjectAt(position.x, position.y) != null)
            return null;

        CameraHelper cameraHelper = new CameraHelper(Camera.main);
        if (position.x < cameraHelper.left || position.x > cameraHelper.right ||
           position.y < cameraHelper.bottom || position.y > cameraHelper.top)
            return null;

        GameObject newObject = Object.Instantiate(currentPrefab);
        Rigidbody2D rigidbody = newObject.GetComponent<Rigidbody2D>();
        if (rigidbody != null)
            Destroy(rigidbody);
        newObject.transform.position = position;
        GridManager.correctPositionToGrid(newObject.transform);
        GridManager.getGrid().setObjectAt((int)position.x, (int)position.y, newObject);
        return newObject;
    }

    public void saveGridToFile()
    {
        GridManager.saveGridToFile();
    }

    public void loadGridFromFile()
    {
        GridManager.copyPrefabList(prefabList);
        GridManager.loadGridFromFile(true);
    }

    public void play(int Scene)
    {
        GridManager.saveGridToFile();
        GridManager.copyPrefabList(prefabList);
        SceneManager.LoadScene(Scene);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GridManager.copyPrefabList(prefabList);
        GridManager.loadGridFromFile(true);
    }

    // Update is called once per frame
    void Update()
    {
        GridManager.drawGrid(_gridLineHorizontalPrefab, _gridLineVerticalPrefab);

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            Destroy(previewPrefab);
        else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Destroy(previewPrefab);
                if (currentPrefab != null)
                    createPrefabAtMousePosition();
                else
                    destroyPrefabAtMousePosition();

            }
            else
            {
                if (!lastMousePosition.Equals(GridManager.mousePositionToGridPosition()))
                {
                    lastMousePosition = GridManager.mousePositionToGridPosition();
                    Destroy(previewPrefab);
                    if (currentPrefab != null)
                        previewPrefab = createPrefabAtMousePosition();
                }
            }
        }
    }
}
