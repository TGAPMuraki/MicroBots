using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GridManager
{
    private static Grid _grid;
    private static GameObject _gridParent;
    private static List<GameObject> _horizontalLines;
    private static List<GameObject> _verticalLines;
    private static List<GameObject> _prefabList;

    private static float roundToGrid(float number, float scale)
    {
        float absNumber = Mathf.Abs(number) + scale / 5;
        float truncated = (int)absNumber;
        float decimalNumber = (int)((absNumber - truncated) * 10);
        float roundedNumber = truncated + (scale - (int)scale);

        return number >= 0 ? roundedNumber : roundedNumber * -1;
    }

    public static void correctVerticalPositionToGrid(Transform transform)
    {
        float posX = transform.position.x;
        float posY = GridManager.roundToGrid(transform.position.y, transform.localScale.y);

        transform.position = new Vector3(posX, posY, transform.position.z);
        transform.rotation = new Quaternion();
    }

    public static void correctHorizontalPositionToGrid(Transform transform)
    {
        float posX = GridManager.roundToGrid(transform.position.x, transform.localScale.x);
        float posY = transform.position.y;

        transform.position = new Vector3(posX, posY, transform.position.z);
        transform.rotation = new Quaternion();
    }

    public static void correctPositionToGrid(Transform transform)
    {
        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        float posX = GridManager.roundToGrid(transform.position.x, scaleX);
        float posY = GridManager.roundToGrid(transform.position.y, scaleY);

        transform.position = new Vector3(posX, posY, transform.position.z);
        transform.rotation = new Quaternion();
    }

    private static void emptyList(ref List<GameObject> list)
    {
        while (list.Count > 0)
        {
            GameObject.Destroy(list[0]);
            list.RemoveAt(0);
        }
    }

    private static void initiateList(ref List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();
        else
            emptyList(ref list);
    }

    private static void addGridLine(ref List<GameObject> list, GameObject prefab, Vector3 position)
    {
        if (_gridParent == null)
            _gridParent = new GameObject("Grid");
        GameObject newLine = GameObject.Instantiate(prefab);
        newLine.transform.position = position;
        newLine.transform.parent = _gridParent.transform;
        list.Add(newLine);
    }

    public static void drawGrid(GameObject lineHorizontalPrefab, GameObject lineVerticalPrefab)
    {
        initiateList(ref _horizontalLines);
        initiateList(ref _verticalLines);

        CameraHelper cameraHelper = new CameraHelper(Camera.main);
        for (float y = roundToGrid(cameraHelper.bottom, 0.5F) - 0.5F; y < cameraHelper.top + 1; y++)
        {
            for (float x = roundToGrid(cameraHelper.left, 0.5F) - 0.5F; x < cameraHelper.right + 1; x++)
            {
                addGridLine(ref _horizontalLines, lineHorizontalPrefab, new Vector3(x, y));
                addGridLine(ref _verticalLines, lineVerticalPrefab, new Vector3(x, y));
            }
        }
    }

    public static Vector2 mousePositionToGridPosition()
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.x = (int)GridManager.roundToGrid(position.x, 0.5F);
        position.y = (int)GridManager.roundToGrid(position.y, 0.5F);

        return position;
    }

    public static Grid getGrid()
    {
        if (_grid == null)
            _grid = new Grid();
        return _grid;
    }

    public static GameObject getObjectAt(float x, float y)
    {


        if (x < 0 || y < 0)
            return null;

        return getGrid().getObjectAt((int)x, (int)y);
    }

    public static void saveGridToFile()
    {
        string filePath = Application.dataPath + "\\EditorLevel.txt";
        using (StreamWriter file = new StreamWriter(filePath))
        {
            Grid grid = GridManager.getGrid();
            int width = grid.getWidth();
            int height = grid.getHeight();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject gameObject = grid.getObjectAt(x, y);
                    if (gameObject != null)
                    {
                        file.WriteLine(x);
                        file.WriteLine(y);
                        file.WriteLine(gameObject.tag);
                    }
                }
            }
        }
    }

    private static void destroyOldGrid()
    {
        Grid oldGrid = getGrid();
        for (int x = 0; x < oldGrid.getWidth(); x++)
        {
            for (int y = 0; y < oldGrid.getHeight(); y++)
            {
                GameObject gameObject = oldGrid.getObjectAt(x, y);
                if (gameObject != null)
                {
                    if (gameObject.tag != "GameController")
                    {
                        GameObject.Destroy(gameObject);
                        oldGrid.setObjectAt(x, y, null);
                    }
                }
            }
        }
    }

    public static void loadGridFromFile(bool destroyRigidBodys)
    {
        destroyOldGrid();

        _grid = new Grid();
        string filePath = Application.dataPath + "\\EditorLevel.txt";
        using (StreamReader file = new StreamReader(filePath))
        {
            while (!file.EndOfStream)
            {
                int x = Convert.ToInt32(file.ReadLine());
                int y = Convert.ToInt32(file.ReadLine());
                string tag = file.ReadLine();

                for (int i = 0; i < _prefabList.Count; i++)
                {
                    if (_prefabList[i].tag == tag)
                    {
                        GameObject newObject = GameObject.Instantiate(_prefabList[i]);
                        newObject.SetActive(true);
                        Rigidbody2D rigidbody = newObject.GetComponent<Rigidbody2D>();
                        if (rigidbody != null && destroyRigidBodys)
                            GameObject.Destroy(rigidbody);
                        newObject.transform.position = new Vector3(x, y);
                        GridManager.correctPositionToGrid(newObject.transform);
                        _grid.setObjectAt(x, y, newObject);
                        break;
                    }
                }
            }
        }
    }

    public static void copyPrefabList(List<GameObject> prefabList)
    {
        if (_prefabList == null)
        {
            _prefabList = new List<GameObject>();
            for (int i = 0; i < prefabList.Count - 1; i++)
            {
                GameObject gameObject = GameObject.Instantiate(prefabList[i]);
                gameObject.SetActive(false);
                GameObject.DontDestroyOnLoad(gameObject);
                _prefabList.Add(gameObject);
            }
        }
    }
}