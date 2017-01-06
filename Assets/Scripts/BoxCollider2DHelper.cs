using UnityEngine;

// Class that helps with calculation Collider-Positions
public class BoxCollider2DHelper
{
    public Vector3 center;
    public float top;
    public float bottom;
    public float left;
    public float right;
    public float width;
    public float height;

    public BoxCollider2DHelper(BoxCollider2D collider)
    {
        InitValues(collider);
    }

    private void calculateCenter(BoxCollider2D collider)
    {
        Transform transform = collider.transform;
        // Gets the center of the Collider (Scale independent)
        Vector3 colliderCenter = Vector3.Scale(transform.localScale, collider.offset);
        colliderCenter = transform.position + colliderCenter;

        // Repositions the Center if the Object is mirrored
        if (transform.rotation.y != 0)
            colliderCenter.x = colliderCenter.x - collider.offset.x;

        center = colliderCenter;
    }

    private void calculateSides(BoxCollider2D collider)
    {
        Transform transform = collider.transform;
        top = center.y + collider.size.y / 2 * transform.localScale.y;
        bottom = center.y - collider.size.y / 2 * transform.localScale.y;
        left = center.x - collider.size.x / 2 * transform.localScale.x;
        right = center.x + collider.size.x / 2 * transform.localScale.x;

        width = right - left;
        height = top - bottom;
    }

    // Sets the Values so they only need to be calculated once
    private void InitValues(BoxCollider2D collider)
    {
        calculateCenter(collider);
        calculateSides(collider);
    }
}
