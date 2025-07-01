using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;

    private bool _isDragging;

    private void Awake()
    {
        _mainCamera = Camera.main;
        zoom = _mainCamera.orthographicSize;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _origin = _mainCamera.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());
        }
        _isDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate()
    {
        if(!_isDragging)
        {
            return;
        }

        _difference = GetMousePosition() - transform.position;
        transform.position = _origin - _difference;
    }

    private Vector3 GetMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());
    }

    private float zoom;
    private float zoomMultiplier = 4f;
    private const float minZoom = 2f;
    private const float maxZoom = 5.5f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        _mainCamera.orthographicSize = Mathf.SmoothDamp(_mainCamera.orthographicSize, zoom, ref velocity, smoothTime);
    }

    public void Scrollo(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<float>());
    }

    public void resetCamPos()
    {
        transform.position = new Vector3(9.5f, -5.75f, -10.0f);
        //_mainCamera.orthographicSize = 5.1f;
    }

}
