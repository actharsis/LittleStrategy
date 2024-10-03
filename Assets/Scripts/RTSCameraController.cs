using UnityEngine;
using UnityEngine.EventSystems;

public class RtsCameraController : MonoBehaviour
{
    public static RtsCameraController instance;

    // item to follow
    // public void OnMouseDown(){
    //   CameraController.instance.followTransform = transform;
    // }

    [Header("General")]
    [SerializeField]
    private Transform _cameraTransform;
    public Transform FollowTransform;
    private Vector3 _newPosition;
    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;

    [Header("Optional Functionality")]
    [SerializeField] private bool _moveWithKeyboad;
    [SerializeField] private bool _moveWithEdgeScrolling;
    [SerializeField] private bool _moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] private float _normalSpeed = 0.01f;
    [SerializeField] private float _movementSensitivity = 1f;
    private float _movementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField]
    private float _edgeSize = 50f;

    private bool _isCursorSet = false;
    public Texture2D CursorArrowUp;
    public Texture2D CursorArrowDown;
    public Texture2D CursorArrowLeft;
    public Texture2D CursorArrowRight;

    private CursorArrow _currentCursor = CursorArrow.Default;

    private enum CursorArrow
    {
        Up,
        Down,
        Left,
        Right,
        Default
    }

    private void Start()
    {
        instance = this;

        _newPosition = transform.position;

        _movementSpeed = _normalSpeed;
    }

    private void Update()
    {
        // Allow Camera to follow Target
        if (FollowTransform != null)
        {
            transform.position = FollowTransform.position;
        }
        else
        {
            HandleCameraMovement();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FollowTransform = null;
        }
    }

    private void HandleCameraMovement()
    {
        // Mouse Drag
        if (_moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }

        // Keyboard Control
        if (_moveWithKeyboad)
        {
            _movementSpeed = _normalSpeed;

            if (Input.GetKey(KeyCode.UpArrow)) //questionable wasd camera movement
            {
                _newPosition += (transform.forward * _movementSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _newPosition += (transform.forward * -_movementSpeed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _newPosition += (transform.right * _movementSpeed);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _newPosition += (transform.right * -_movementSpeed);
            }
        }

        // Edge Scrolling
        if (_moveWithEdgeScrolling) //diagonal?
        {

            // Move Right
            if (Input.mousePosition.x > Screen.width - _edgeSize)
            {
                _newPosition += (transform.right * _movementSpeed);
                ChangeCursor(CursorArrow.Right);
                _isCursorSet = true;
            }

            // Move Left
            else if (Input.mousePosition.x < _edgeSize)
            {
                _newPosition += (transform.right * -_movementSpeed);
                ChangeCursor(CursorArrow.Left);
                _isCursorSet = true;
            }

            // Move Up
            else if (Input.mousePosition.y > Screen.height - _edgeSize)
            {
                _newPosition += (transform.forward * _movementSpeed);
                ChangeCursor(CursorArrow.Up);
                _isCursorSet = true;
            }

            // Move Down
            else if (Input.mousePosition.y < _edgeSize)
            {
                _newPosition += (transform.forward * -_movementSpeed);
                ChangeCursor(CursorArrow.Down);
                _isCursorSet = true;
            }
            else
            {
                if (_isCursorSet)
                {
                    ChangeCursor(CursorArrow.Default);
                    _isCursorSet = false;
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * _movementSensitivity);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ChangeCursor(CursorArrow newCursor)
    {
        if (_currentCursor == newCursor) return;
        switch (newCursor)
        {
            case CursorArrow.Up:
                Cursor.SetCursor(CursorArrowUp, Vector2.zero, CursorMode.Auto);
                break;
            case CursorArrow.Down:
                Cursor.SetCursor(CursorArrowDown, new Vector2(CursorArrowDown.width, CursorArrowDown.height), CursorMode.Auto);
                break;
            case CursorArrow.Left:
                Cursor.SetCursor(CursorArrowLeft, Vector2.zero, CursorMode.Auto);
                break;
            case CursorArrow.Right:
                Cursor.SetCursor(CursorArrowRight, new Vector2(CursorArrowRight.width, CursorArrowRight.height), CursorMode.Auto);
                break;
            case CursorArrow.Default:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }

        _currentCursor = newCursor;
    }



    private void HandleMouseDragInput()
    {
        if (Input.GetMouseButtonDown(2) && EventSystem.current.IsPointerOverGameObject() == false) //!IsPointerOverGameObject()?
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (plane.Raycast(ray, out var entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(2) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (!plane.Raycast(ray, out var entry)) return;
            _dragCurrentPosition = ray.GetPoint(entry);

            _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
        }
    }
}