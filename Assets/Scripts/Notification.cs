using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification Instance { get; private set; }

    [SerializeField] private Text text;

    public void Awake()
    {
        if (Instance != null)
            Destroy(this);

        if (Instance == null)
            Instance = this;
    }

    public void SetNotification(bool success, string message)
    {
        StartCoroutine(success ? SuccessNotification(message) : ErrorNotification(message));
    }

    private IEnumerator SuccessNotification(string message)
    {
        text.text = message;
        text.color = Color.green;
        yield return new WaitForSeconds(1);
        text.text = string.Empty;
    }

    private IEnumerator ErrorNotification(string message)
    {
        text.text = message;
        text.color = Color.red;
        yield return new WaitForSeconds(1);
        text.text = string.Empty;
    }
}
