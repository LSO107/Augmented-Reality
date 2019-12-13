using System.Collections;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    public static Notification Instance { get; private set; }

    [SerializeField] private GameObject notification;

    private CanvasGroup m_CanvasGroup;
    private TextMeshProUGUI m_Text;

    public void Awake()
    {
        if (Instance != null)
            Destroy(this);

        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        m_Text = notification.GetComponentInChildren<TextMeshProUGUI>();
        m_CanvasGroup = notification.GetComponent<CanvasGroup>();
    }

    public void SetNotification(bool success, string message)
    {
        StartCoroutine(success ? SuccessNotification(message) : ErrorNotification(message));
    }

    private IEnumerator SuccessNotification(string message)
    {
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, true);
        m_Text.text = message;
        m_Text.color = Color.green;
        yield return new WaitForSeconds(1);
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, false);
    }

    private IEnumerator ErrorNotification(string message)
    {
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, true);
        m_Text.text = message;
        m_Text.color = Color.red;
        yield return new WaitForSeconds(1);
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, false);
    }
}
