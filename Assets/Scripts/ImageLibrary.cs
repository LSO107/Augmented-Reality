using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class ImageLibrary : MonoBehaviour
{
    [SerializeField] private List<GameObject> storedImages = new List<GameObject>();
    [SerializeField] private GameObject libraryInterface;

    private CanvasGroup m_CanvasGroup;

    private void Start()
    {
        m_CanvasGroup = GetComponentInChildren<CanvasGroup>();
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
    }

    /// <summary>
    /// Removes gameObject from the collection, then destroys it
    /// </summary>
    public void RemoveImage(GameObject image)
    {
        if (storedImages.Contains(image))
        {
            var index = storedImages.IndexOf(image);

            for (var i = index + 1; i < storedImages.Count; i++)
            {
                storedImages[i].transform.position += new Vector3(-0.2f, 0, 0);
            }

            storedImages.Remove(image);
        }

        Destroy(image);
    }

    /// <summary>
    /// Toggle <see cref="CanvasGroup"/> settings and calls <see cref="LockLibraryPosition"/>
    /// </summary>
    public void ToggleCanvasGroup()
    {
        m_CanvasGroup.alpha = m_CanvasGroup.alpha == 1 ? 0 : 1;
        m_CanvasGroup.interactable = m_CanvasGroup.interactable != true;
        m_CanvasGroup.blocksRaycasts = m_CanvasGroup.blocksRaycasts != true;

        if (GetComponentInChildren<Canvas>().enabled == false)
        {
            GetComponentInChildren<Canvas>().enabled = true;
        }

        StartCoroutine(LockLibraryPosition());
    }

    /// <summary>
    /// While <see cref="m_CanvasGroup"/> is active, lock position of <see cref="libraryInterface"/>
    /// </summary>
    private IEnumerator LockLibraryPosition()
    {
        while (m_CanvasGroup.interactable)
        {
            //libraryInterface.transform.position = GetLibraryPosition();
            libraryInterface.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            libraryInterface.transform.rotation = Camera.main.transform.rotation;
            yield return null;
        }
    }

    /// <summary>
    /// Returns the position for the library
    /// </summary>
    private static Vector3 GetLibraryPosition()
    {
        var cam = Camera.main.transform;
        var start = cam.position + new Vector3(0, 0.04f, 0);
        return start + cam.forward * 0.1f;
    }
}
