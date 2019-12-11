using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoredImage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private GameObject imagePrefab;

    public void CancelOptions()
    {
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, false);
    }
    public void DeleteImage()
    {
        GetComponent<RawImage>().texture = null;

        UserInterfaceUtils.ToggleCanvasGroup(GetComponentInParent<CanvasGroup>(), false);
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, true);
    }

    public void CreateImage()
    {
        var image = Instantiate(imagePrefab, Camera.main.transform.position + Vector3.forward, Quaternion.identity);
        imagePrefab.GetComponent<Renderer>().sharedMaterial.mainTexture = GetComponent<RawImage>().texture;
        StartCoroutine(ScaleImageOverTime(image));
    }

    /// <summary>
    /// Increase the scale of <see cref="GameObject"/> using <see cref="Vector3.Lerp"/>  
    /// </summary>
    private static IEnumerator ScaleImageOverTime(GameObject image)
    {
        var targetScale = new Vector3(0.11f, 0.11f, 1);

        while (image.transform.localScale.x < 0.1f)
        {
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, targetScale, Time.deltaTime * 2.5f);
            yield return null;
        }
    }
}
