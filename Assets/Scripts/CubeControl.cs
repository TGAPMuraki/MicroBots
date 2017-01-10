using UnityEngine;

public class CubeControl
{
    private GameObject _gameObject;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private BoxCollider2DHelper _colliderHelper;
    private Vector2 _rotateAround;
    private Vector3 _oldEulerAngles;
    private bool _rotateRight;
    private bool _rotateLeft;
    private Vector3 _rotateVector;
    private const float rayCastOffset = 0.025f;
    private const string finisthTag = "Finish";

    public CubeControl(GameObject gameObject)
    {
        setGameObject(gameObject);
    }

    private void setGameObject(GameObject gameObject)
    {
        _gameObject = gameObject;
        _transform = gameObject.transform;
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public GameObject getGameObject()
    {
        return _gameObject;
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
        _rigidbody.gravityScale = 5;
        _rotateRight = false;
        _rotateLeft = false;
    }

    public bool isGrounded(float posXOffset)
    {
        float rayCastOriginPosY = _colliderHelper.bottom - rayCastOffset;
        Vector2 rayCastOrigin = new Vector2(_colliderHelper.center.x + posXOffset, rayCastOriginPosY);
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, rayCastOffset);
        return hit && hit.collider != _transform.GetComponent<BoxCollider2D>();
    }

    public bool isGrounded()
    {
        return isGrounded(0);
    }

    private bool isGroundedRight()
    {
        return isGrounded(_colliderHelper.width / 2 - rayCastOffset * 10);
    }

    private bool isGroundedLeft()
    {
        return isGrounded((_colliderHelper.width / 2 + rayCastOffset * 10) * -1);
    }

    private bool canRotate()
    {
        if (_rotateRight ? !isGroundedRight() : !isGroundedLeft())
            return false;

        float rayCastStartPosX = _rotateRight ? _colliderHelper.right + rayCastOffset : _colliderHelper.left - rayCastOffset;
        Vector2 rayCastOrigin = new Vector2(rayCastStartPosX, _colliderHelper.bottom + rayCastOffset * 10);
        Vector2 rayCastDirection = _rotateRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, rayCastDirection, _colliderHelper.height / 2);
        return !hit || (hit && hit.collider.tag == finisthTag);
    }

    private void checkInputForRotation()
    {
        float axis = Input.GetAxis("Horizontal");
        _rotateRight = axis == 1;
        _rotateLeft = _rotateRight ? false : axis == -1;

        if (isRotating() && canRotate())
        {
            _rigidbody.gravityScale = 0;
            _oldEulerAngles = _transform.eulerAngles;
            _rotateAround = new Vector2(_rotateRight ? _colliderHelper.right : _colliderHelper.left, _colliderHelper.bottom);
            _rotateVector = _rotateRight ? Vector3.back : Vector3.forward;
        }
        else
        {
            stopRotating();
        }
    }

    private void executeRotation()
    {
        _transform.RotateAround(_rotateAround, _rotateVector, 400 * Time.deltaTime);
        if (hasRotated90Degrees(_oldEulerAngles, _transform.eulerAngles))
        {
            stopRotating();
            GameObject newGameObject = Object.Instantiate(_gameObject, _transform.position, new Quaternion());
            GameObject oldGameObject = _gameObject;
            newGameObject.transform.localScale = new Vector3(_transform.localScale.y, _transform.localScale.x, _transform.localScale.z);
            GridManager.correctPositionToGrid(newGameObject.transform);
            newGameObject.name = oldGameObject.name;

            setGameObject(newGameObject);
            GameObject.Destroy(oldGameObject);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void Update()
    {
        _colliderHelper = new BoxCollider2DHelper(_gameObject.GetComponent<BoxCollider2D>());
        if (!isRotating())
        {
            if (isGrounded())
            {
                GridManager.correctPositionToGrid(_transform);
                _rigidbody.velocity = new Vector2();
            }
            else
            {
                GridManager.correctHorizontalPositionToGrid(_transform);
                if(_rigidbody.velocity.y > 5)
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 5);
            }
            _colliderHelper = new BoxCollider2DHelper(_gameObject.GetComponent<BoxCollider2D>());
            checkInputForRotation();
        }
        if (isRotating())
            executeRotation();
    }
}
