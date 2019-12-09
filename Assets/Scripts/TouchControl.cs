using Assets.Scripts;
using UnityEngine;

internal sealed class TouchControl : MonoBehaviour
{
    private Camera m_MainCamera;

    private Vector3 m_ScreenPoint;
    private Vector3 m_Offset;

    private bool m_ResetTouch;
    private string m_ContextLink;

    private TouchStates m_CurrentState;

    private ImageLibrary m_ImageLibrary;

    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_ImageLibrary = FindObjectOfType<ImageLibrary>();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            m_ResetTouch = true;
            return;
        }

        if (Input.GetTouch(0).deltaTime > 2)
        {
            Debug.Log("Long Pressed for more than two seconds.");
        }
    }

    public void StoreContextLinks(string link)
    {
        m_ContextLink = link;
    }

    private void PinchScale()
    {
        m_ResetTouch = false;

        var touchZero = Input.GetTouch(0);
        var touchOne = Input.GetTouch(1);

        var firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        var secondTouchPosition = Camera.main.ScreenToWorldPoint(Input.touches[1].position);

        var firstRay = Camera.main.ScreenPointToRay(firstTouchPosition);
        var secondRay = Camera.main.ScreenPointToRay(secondTouchPosition);

        Physics.Raycast(firstRay, out var hit1);
        Physics.Raycast(firstRay, out var hit2);

        if (hit1.collider == hit2.collider)
        {
            var distance = Vector3.Distance(hit1.point, hit2.point);
            gameObject.transform.localScale = new Vector3(distance, distance, 1);
        }
        
        /*var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        var prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        var difference = Mathf.Clamp(currentMagnitude - prevMagnitude, 0.075F, 1);

        var newScale = new Vector3(difference, difference, 1);
        gameObject.transform.localScale = Vector3.Lerp(transform.localScale, newScale, 3.5F * Time.deltaTime);*/
    }

    private void DoubleTap()
    {
        m_ImageLibrary.StoreImage(gameObject);
        //Application.OpenURL(m_ContextLink);
        Debug.Log(m_ContextLink);
    }

    private void OnMouseDown()
    {
        if (InputWrapper.HasDoubleTapped())
        {
            DoubleTap();
            Debug.Log("JUST DOUBLE TAPPED !!");
        }

        var position = transform.position;
        m_ScreenPoint = m_MainCamera.WorldToScreenPoint(position);

        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);
        m_Offset = position - m_MainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void OnMouseDrag()
    {
        if (Input.touchCount == 2)
        {
            PinchScale();
        }

        if (!m_ResetTouch)
            return;

        var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);
        var cursorPosition = m_MainCamera.ScreenToWorldPoint(cursorPoint) + m_Offset;
        transform.position = cursorPosition;

        transform.LookAt(m_MainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}