using UnityEngine;

// Class that helps with calculation Collider-Positions
public class CameraHelper : BaseHelperClass
{
    public CameraHelper(Camera camera)
    {
        InitValues(camera);
    }

    private void calculateSides(Camera camera)
    {
        height = 2 * camera.orthographicSize;
        width = height * camera.aspect;
        left = center.x - width / 2;
        right = center.x + width / 2;
        top = center.y + height / 2;
        bottom = center.y - height / 2;
    }

    // Sets the Values so they only need to be calculated once
    private void InitValues(Camera camera)
    {
        center = camera.transform.position;
        calculateSides(camera);
    }
}
