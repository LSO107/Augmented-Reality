using UnityEngine;

internal sealed class TouchControl : MonoBehaviour
{
    private Camera MainCamera;

    private Vector3 screenPoint;
    private Vector3 offset;

    private bool resetTouch;

    private void Awake()
    {
        MainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            resetTouch = true;
        }
    }

    private void PinchScale()
    {
        resetTouch = false;

        var touchZero = Input.GetTouch(0);
        var touchOne = Input.GetTouch(1);

        var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        var prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        var difference = Mathf.Clamp(currentMagnitude - prevMagnitude, 0.075F, 1);

        var newScale = new Vector3(difference, difference, 1);
        gameObject.transform.localScale = Vector3.Lerp(transform.localScale, newScale, 3.5F * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        var position = transform.position;
        screenPoint = MainCamera.WorldToScreenPoint(position);

        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = position - MainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void OnMouseDrag()
    {
        if (Input.touchCount == 2)
        {
            PinchScale();
        }

        if (!resetTouch)
            return;

        var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        var cursorPosition = MainCamera.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;

        transform.LookAt(MainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}