using UnityEngine;

internal sealed class ImageEffect : MonoBehaviour
{
    private Vector3 m_DesiredPosition;

    private void Start()
    {
        m_DesiredPosition = transform.position;
        m_DesiredPosition += new Vector3(0, -10, 0);
    }

    private void Update()
    {
        if (transform.position == m_DesiredPosition)
            Destroy(this);

        transform.position = Vector3.Lerp(transform.position, m_DesiredPosition, Time.deltaTime * 2.5f);
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(Vector3.up, 180);
    }
}
