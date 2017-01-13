using UnityEngine;

// Class that helps with calculation Collider-Positions
public class LevelHelper : BaseHelperClass
{
    public LevelHelper(GameObject bottomLeft, GameObject topRight)
    {
        InitValues(bottomLeft, topRight);
    }

    private void calculateSides(GameObject bottomLeft, GameObject topRight)
    {
        left = bottomLeft.transform.position.x;
        right = topRight.transform.position.x;
        bottom = bottomLeft.transform.position.y;
        top = topRight.transform.position.y;
        height = top - bottom;
        width = right - left;
    }

    // Sets the Values so they only need to be calculated once
    private void InitValues(GameObject bottomLeft, GameObject topRight)
    {
        calculateSides(bottomLeft, topRight);
        center = new Vector3(right - width / 2, top - height / 2);
    }
}
