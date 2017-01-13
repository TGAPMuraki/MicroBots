using UnityEngine;

public static class DebugHelper
{

    public static void drawRayFromPoint(Vector2 point, Color color, float timer)
    {
        Vector2 topLeft = new Vector2(point.x, point.y);
        Debug.DrawRay(topLeft, Vector3.up, color, timer);
        Debug.DrawRay(topLeft, Vector3.down, color, timer);
        Debug.DrawRay(topLeft, Vector3.right, color, timer);
        Debug.DrawRay(topLeft, Vector3.left, color, timer);
    }

    public static void drawRayFromPoint(Vector2 point)
    {
        drawRayFromPoint(point, Color.red, 0);
    }

}