using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private List<List<GameObject>> _horizontalList;
    public List<GameObject> _prefabList;

    public int getWidth()
    {
        return _horizontalList.Count;
    }

    public int getHeight()
    {
        int height = -1;
        for (int i = 0; i < _horizontalList.Count; i++)
            if (_horizontalList[i].Count > height)
                height = _horizontalList[i].Count;
        return height;
    }

    public Grid()
    {
        _horizontalList = new List<List<GameObject>>();
    }

    public GameObject getObjectAt(int x, int y)
    {
        if (_horizontalList.Count <= x)
            return null;
        List<GameObject> verticalLine = _horizontalList[x];
        if (verticalLine.Count <= y)
            return null;

        return verticalLine[y];
    }

    private bool objectsSharePosition(GameObject object1, GameObject object2)
    {

        return false;
    }

    public void setObjectAt(int x, int y, GameObject gameObject)
    {
        while (_horizontalList.Count <= x)
            _horizontalList.Add(new List<GameObject>());

        List<GameObject> verticalLine = _horizontalList[x];
        while (verticalLine.Count <= y)
            verticalLine.Add(null);

        if (verticalLine[y] != null && gameObject != null && verticalLine[y] != gameObject)
            throw new System.Exception(string.Format("There is already an Object at X:{0} Y:{1}", x, y));

        verticalLine[y] = gameObject;
    }

}