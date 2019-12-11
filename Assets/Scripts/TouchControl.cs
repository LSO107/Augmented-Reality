using System.Collections;
using UnityEngine;

internal sealed class TouchControl : MonoBehaviour
{
    private Camera m_MainCamera;
    private CanvasGroup m_Canvas;

    private Vector3 m_ScreenPoint;
    private Vector3 m_Offset;

    private string m_ContextLink;
    private float m_Timer;

    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_Canvas = GetComponentInChildren<CanvasGroup>();
    }

    public void StoreContextLinks(string link)
    {
        m_ContextLink = link;
    }

    /// <summary>
    /// Scales the game object based on the distance between two touches
    /// </summary>
    private void PinchScale()
    {
        var touchZero = Input.GetTouch(0);
        var touchOne = Input.GetTouch(1);

        var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        var prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        var difference = Mathf.Clamp(currentMagnitude - prevMagnitude, 0.075F, 0.75f);

        var newScale = new Vector3(difference, difference, 1);
        gameObject.transform.localScale = Vector3.Lerp(transform.localScale, newScale, 3.5F * Time.deltaTime);
    }

    /// <summary>
    /// Opens context link URL via <see cref="Application"/>
    /// </summary>
    private void DoubleTap()
    {
        Application.OpenURL(m_ContextLink);
        Debug.Log(m_ContextLink);
    }

    /// <summary>
    /// Updates position based on touch/cursor position in world space
    /// </summary>
    private void Drag()
    {
        var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);
        var cursorPosition = m_MainCamera.ScreenToWorldPoint(cursorPoint) + m_Offset;
        transform.position = cursorPosition;

        transform.LookAt(m_MainCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    /// <summary>
    /// Opens user interface if the touch has been longer than 0.5 seconds
    /// </summary>
    private IEnumerator LongPress(Touch touch)
    {
        if (touch.phase != TouchPhase.Stationary)
        {
            m_Timer = 0;
            yield return null;
        }

        m_Timer += Time.deltaTime;

        if (m_Timer >= 0.5f)
        {
            UserInterfaceUtils.ToggleCanvasGroup(m_Canvas, true);
            m_Timer = 0;
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetTouch(0).tapCount == 2)
        {
            DoubleTap();
        }

        var position = transform.position;
        m_ScreenPoint = m_MainCamera.WorldToScreenPoint(position);

        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);
        m_Offset = position - m_MainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void OnMouseDrag()
    {
        var touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Moved:
                Drag();
                break;
            case TouchPhase.Stationary:
                StartCoroutine(LongPress(touch));
                break;
        }

        if (Input.touchCount == 2)
        {
            PinchScale();
        }
    }
}