using UnityEngine;

public class BaseHelperClass
{
    public Vector3 center;
    public float top;
    public float bottom;
    public float left;
    public float right;
    public float width;
    public float height;

    public void DrawHelperInfos(Color color)
    {
        // Top-Left corner
        Debug.DrawRay(new Vector3(left, top), Vector2.right, color);
        Debug.DrawRay(new Vector3(left, top), Vector2.down, color);
        // Top-Right corner
        Debug.DrawRay(new Vector3(right, top), Vector2.left, color);
        Debug.DrawRay(new Vector3(right, top), Vector2.down, color);
        // Bottom-Left corner
        Debug.DrawRay(new Vector3(left, bottom), Vector2.right, color);
        Debug.DrawRay(new Vector3(left, bottom), Vector2.up, color);
        // Bottom-Right corner
        Debug.DrawRay(new Vector3(right, bottom), Vector2.left, color);
        Debug.DrawRay(new Vector3(right, bottom), Vector2.up, color);
        // Center
        DebugHelper.drawRayFromPoint(center, color, 0);
    }
}
