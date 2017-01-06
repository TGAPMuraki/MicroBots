using UnityEngine;

public class DebugScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
        BoxCollider2DHelper helper = new BoxCollider2DHelper(transform.GetComponent<BoxCollider2D>());

        //*****
        Vector2 topLeft = new Vector2(helper.left, helper.top);
        Debug.DrawRay(topLeft, Vector3.up, Color.red);
        Debug.DrawRay(topLeft, Vector3.down, Color.red);
        Debug.DrawRay(topLeft, Vector3.right, Color.red);
        Debug.DrawRay(topLeft, Vector3.left, Color.red);
        //*****
        Vector2 topRight = new Vector2(helper.right, helper.top);
        Debug.DrawRay(topRight, Vector3.up, Color.blue);
        Debug.DrawRay(topRight, Vector3.down, Color.blue);
        Debug.DrawRay(topRight, Vector3.right, Color.blue);
        Debug.DrawRay(topRight, Vector3.left, Color.blue);
        //*****
        Vector2 bottomLeft = new Vector2(helper.left, helper.bottom);
        Debug.DrawRay(bottomLeft, Vector3.up, Color.green);
        Debug.DrawRay(bottomLeft, Vector3.down, Color.green);
        Debug.DrawRay(bottomLeft, Vector3.right, Color.green);
        Debug.DrawRay(bottomLeft, Vector3.left, Color.green);
        //*****
        Vector2 bottomRight = new Vector2(helper.right, helper.bottom);
        Debug.DrawRay(bottomRight, Vector3.up, Color.magenta);
        Debug.DrawRay(bottomRight, Vector3.down, Color.magenta);
        Debug.DrawRay(bottomRight, Vector3.right, Color.magenta);
        Debug.DrawRay(bottomRight, Vector3.left, Color.magenta);
        //*****
        Debug.DrawRay(helper.center, Vector3.up, Color.black);
        Debug.DrawRay(helper.center, Vector3.down, Color.black);
        Debug.DrawRay(helper.center, Vector3.right, Color.black);
        Debug.DrawRay(helper.center, Vector3.left, Color.black);
        
    }
}
