using UnityEngine;

public class TouchControl : MonoBehaviour
{
    private Camera MainCamera;

    private Vector3 screenPoint;
    private Vector3 offset;

    private void Awake()
    {
        MainCamera = Camera.main;
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
        var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        var cursorPosition = MainCamera.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;

        transform.LookAt(MainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
