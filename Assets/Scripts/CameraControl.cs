using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject bottomLeftCorner;
    public GameObject topRightCorner;

    private void adjustVisibleLevelSize(ref LevelHelper levelHelper, CameraHelper cameraHelper)
    {
        if (levelHelper.width < cameraHelper.width)
            levelHelper.right += cameraHelper.width - levelHelper.width;
        if (levelHelper.height < cameraHelper.height)
            levelHelper.top += cameraHelper.height - levelHelper.height;

        topRightCorner.transform.position = new Vector3(levelHelper.right + 1, levelHelper.top + 1, topRightCorner.transform.position.z);
        GridManager.correctPositionToGrid(topRightCorner.transform);
        levelHelper = new LevelHelper(bottomLeftCorner, topRightCorner);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if(bottomLeftCorner == null)
        {
            bottomLeftCorner = new GameObject();
            bottomLeftCorner.transform.position = new Vector3(0, 0);
        }
        if(topRightCorner == null)
        {
            topRightCorner = new GameObject();
            topRightCorner.transform.position = new Vector3(GridManager.getGrid().getWidth() + 1, GridManager.getGrid().getHeight() + 1);            
        }

        LevelHelper levelHelper = new LevelHelper(bottomLeftCorner, topRightCorner);
        CameraHelper cameraHelper = new CameraHelper(Camera.main);
        if (levelHelper.width < cameraHelper.width || levelHelper.height < cameraHelper.height)
            adjustVisibleLevelSize(ref levelHelper, cameraHelper);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * 50 * -1;
            float vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * 50 * -1;
            transform.Translate(new Vector3(horizontal, vertical));
        }

        CameraHelper cameraHelper = new CameraHelper(Camera.main);
        LevelHelper levelHelper = new LevelHelper(bottomLeftCorner, topRightCorner);

        if (cameraHelper.left < levelHelper.left)
            cameraHelper.center.x = levelHelper.left + cameraHelper.width / 2;
        else if (cameraHelper.right > levelHelper.right)
            cameraHelper.center.x = levelHelper.right - cameraHelper.width / 2;

        if (cameraHelper.top > levelHelper.top)
            cameraHelper.center.y = levelHelper.top - cameraHelper.height / 2;
        else if (cameraHelper.bottom < levelHelper.bottom)
            cameraHelper.center.y = levelHelper.bottom + cameraHelper.height / 2;

        transform.position = new Vector3(cameraHelper.center.x, cameraHelper.center.y, transform.position.z);
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        CameraHelper cameraHelper = new CameraHelper(Camera.main);
        cameraHelper.DrawHelperInfos(Color.green);

        LevelHelper levelHelper = new LevelHelper(bottomLeftCorner, topRightCorner);
        levelHelper.DrawHelperInfos(Color.red);
    }
}
