using System.Collections.Generic;
using UnityEngine;

internal sealed class ImageLibrary : MonoBehaviour
{
    [SerializeField] private List<GameObject> storedImages = new List<GameObject>();

    [SerializeField] private UpdateLibrary updateLibrary;

    private CanvasGroup m_CanvasGroup;

    private void Start()
    {
        m_CanvasGroup = updateLibrary.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Adds the image to a list for stored images,
    /// then removes it from the list of temporary images
    /// </summary>
    public void StoreImage(GameObject image)
    {
        if (storedImages.Contains(image))
            return;

        storedImages.Add(image);
        GetComponent<HandleDisplay>().RemoveImageFromCollection(image);
        //updateLibrary.UpdateLibrary(image);
        updateLibrary.AddImage(image);
    }

    /// <summary>
    /// Removes gameObject from the collection, then destroys it
    /// </summary>
    public void RemoveImage(GameObject image)
    {
        if (storedImages.Contains(image))
        {
            storedImages.Remove(image);
        }

        Destroy(image);
    }

    /// <summary>
    /// Toggle image library user interface
    /// </summary>
    public void ToggleImageLibrary()
    {
        var show = !m_CanvasGroup.interactable;
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, show);
    }
}
