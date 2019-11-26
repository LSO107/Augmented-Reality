using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class ImageLibrary : MonoBehaviour
{
    [SerializeField] private List<GameObject> storedImages = new List<GameObject>();
    [SerializeField] private GameObject content;

    private CanvasGroup m_CanvasGroup;

    private void Start()
    {
        m_CanvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void StoreImage(GameObject image)
    {
        if (storedImages.Contains(image))
            return;

        storedImages.Add(image);
        image.transform.SetParent(content.transform);
        RelocateImagePosition(image);
    }

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

    private void RelocateImagePosition(GameObject image)
    {
        // Move image position to image library
        // TODO: Find a starting position
        // TODO: Find the X axis of the last element, then add to the X axis
        image.transform.position = content.transform.position += new Vector3(-0.5f, 0, 0);

        // Reset the rotation to none (Quaternion identity)
        // TODO: Figure out why resetting the rotation is not working
        image.transform.rotation = Quaternion.identity;
    }

    public void ToggleCanvasGroup()
    {
        m_CanvasGroup.alpha = m_CanvasGroup.alpha == 1 ? 0 : 1;
        m_CanvasGroup.interactable = m_CanvasGroup.interactable != true;
        m_CanvasGroup.blocksRaycasts = m_CanvasGroup.blocksRaycasts != true;

        if (storedImages.Count == 1)
        {
            GetComponentInChildren<Canvas>().enabled = true;
        }
        else
        {
            Debug.Log("Library is empty!");
        }

        /*if (m_CanvasGroup.interactable)
        {
            StartCoroutine(LockLibraryPosition());
        }
        else
        {
            StopCoroutine(LockLibraryPosition());
        }*/
    }

    private IEnumerator LockLibraryPosition()
    {
        while (m_CanvasGroup.interactable)
        {
            // While image library is open

            // Lock position to top of the screen

            yield return null;
        }
    }
}
