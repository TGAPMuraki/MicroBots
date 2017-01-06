using UnityEngine;

public class CubeControl
{
    private GameObject _gameObject;
    private Transform _transform;
    private BoxCollider2DHelper _colliderHelper;
    private Vector2 _rotateAround;
    private Vector3 _oldEulerAngles;
    private bool _rotateRight;
    private bool _rotateLeft;
    private Vector3 _rotateVector;
    private const float rayCastOffset = 0.02f;
    private const string finisthTag = "Finish";

    public CubeControl(GameObject gameObject)
    {
        setGameObject(gameObject);
    }

    private void setGameObject(GameObject gameObject)
    {
        _gameObject = gameObject;
        _transform = _gameObject.transform;
    }

    public GameObject getGameObject()
    {
        return _gameObject;
    }

    private float roundToGrid(float number)
    {
        float absNumber = Mathf.Abs(number);
        int truncated = (int)absNumber;
        float decimalNumber = (int)((absNumber - truncated) * 10);
        float roundedNumber = truncated;
        if (decimalNumber > 2 && decimalNumber < 8)
            roundedNumber += 0.5F;
        else if (decimalNumber >= 8)
            roundedNumber = truncated + 1;

        return number >= 0 ? roundedNumber : roundedNumber * -1;
    }

    private bool hasRotated90Degrees(Vector3 oldEuler, Vector3 currentEuler)
    {
        float oldAngle = Mathf.Abs(Mathf.Round(oldEuler.z));
        float currentAngle = Mathf.Abs(Mathf.Round(currentEuler.z));
        return Mathf.Abs(oldAngle - currentAngle) >= 90 && Mathf.Abs(oldAngle - currentAngle) <= 270;
    }

    public bool isRotating()
    {
        return _rotateRight || _rotateLeft;
    }

    private void stopRotating()
    {
        _gameObject.GetComponent<Rigidbody2D>().gravityScale = 5;
        _rotateRight = false;
        _rotateLeft = false;
    }

    private bool isGrounded(float posXOffset)
    {
        float rayCastOriginPosY = _colliderHelper.bottom - rayCastOffset;
        Vector2 rayCastOrigin = new Vector2(_colliderHelper.center.x + posXOffset, rayCastOriginPosY);
        Debug.DrawRay(rayCastOrigin, Vector2.down, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, rayCastOffset);
        return hit;
    }

    private bool isGroundedRight()
    {
        return isGrounded(_colliderHelper.width / 4);
    }

    private bool isGroundedLeft()
    {
        return isGrounded(_colliderHelper.width / 4 * -1);
    }

    private bool canRotate()
    {
        if (_rotateRight ? !isGroundedRight() : !isGroundedLeft())
            return false;

        float rayCastStartPosX = _rotateRight ? _colliderHelper.right + rayCastOffset : _colliderHelper.left - rayCastOffset;
        Vector2 rayCastOrigin = new Vector2(rayCastStartPosX, _colliderHelper.bottom + rayCastOffset);
        Vector2 rayCastDirection = _rotateRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, rayCastDirection, _colliderHelper.width / 2);
        return !hit || (hit && hit.collider.tag == finisthTag);
    }

    private void checkInputForRotation()
    {
        float axis = Input.GetAxis("Horizontal");
        _rotateRight = axis == 1;
        _rotateLeft = _rotateRight ? false : axis == -1;

        if (isRotating() && canRotate())
        {
            _oldEulerAngles = _transform.eulerAngles;
            _rotateAround = new Vector2(_rotateRight ? _colliderHelper.right : _colliderHelper.left, _colliderHelper.bottom);
            _rotateVector = _rotateRight ? Vector3.back : Vector3.forward;
        }
        else
        {
            stopRotating();
        }
    }

    private void correctPositionToGrid()
    {
        float posX = roundToGrid(_transform.position.x);
        float posY = _transform.position.y;

        _transform.position = new Vector3(posX, posY, _transform.position.z);
    }

    private void executeRotation()
    {
        _transform.RotateAround(_rotateAround, _rotateVector, 400 * Time.deltaTime);
        _gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (hasRotated90Degrees(_oldEulerAngles, _transform.eulerAngles))
        {
            correctPositionToGrid();
            GameObject newGameObject = Object.Instantiate(_gameObject, _transform.position, new Quaternion());
            GameObject oldGameObject = _gameObject;
            newGameObject.transform.localScale = new Vector3(_transform.localScale.y, _transform.localScale.x);
            newGameObject.name = oldGameObject.name;

            setGameObject(newGameObject);
            GameObject.Destroy(oldGameObject);

            stopRotating();
        }
    }

    private bool hitFinishGoald(Vector2 rayCastOrigin, Vector2 rayCastDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, rayCastDirection, rayCastOffset);
        return hit && hit.collider.tag == finisthTag;
    }

    public bool hasEnteredGoal()
    {
        Vector2 rayCastRightOrigin = new Vector2(_colliderHelper.right + rayCastOffset, _colliderHelper.center.y);
        Vector2 rayCastLeftOrigin = new Vector2(_colliderHelper.left - rayCastOffset, _colliderHelper.center.y);
        Vector2 rayCastTopOrigin = new Vector2(_colliderHelper.center.x, _colliderHelper.top + rayCastOffset);

        return hitFinishGoald(rayCastRightOrigin, Vector2.right) &&
                hitFinishGoald(rayCastLeftOrigin, Vector2.left) &&
                hitFinishGoald(rayCastTopOrigin, Vector2.up);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void Update()
    {
        _colliderHelper = new BoxCollider2DHelper(_gameObject.GetComponent<BoxCollider2D>());
        if (!isRotating())
            checkInputForRotation();
        if (isRotating())
            executeRotation();


    }
}
