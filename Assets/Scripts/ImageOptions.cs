using UnityEngine;

internal sealed class ImageOptions : MonoBehaviour
{
    private ImageLibrary m_Library;
    private CanvasGroup m_Canvas;

    public void SaveImage() => m_Library.StoreImage(gameObject);
    public void DeleteImage() => m_Library.RemoveImage(gameObject);

    private void Start()
    {
        m_Library = FindObjectOfType<ImageLibrary>();
        m_Canvas = GetComponentInChildren<CanvasGroup>();
    }

    public void HideImage()
    {
        UserInterfaceUtils.ToggleCanvasGroup(m_Canvas, false);
    }
}
