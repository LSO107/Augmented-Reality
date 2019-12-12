using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLibrary : MonoBehaviour
{
    [SerializeField] private List<RawImage> images = new List<RawImage>();

    private void Start()
    {
        foreach (var rawImage in images)
        {
            rawImage.texture = null;
        }
    }

    public void AddImage(GameObject img)
    {
        var image = images.First(i => i.texture == null);
        image.texture = img.GetComponent<Renderer>().material.mainTexture;
        UserInterfaceUtils.ToggleCanvasGroup(image.GetComponent<CanvasGroup>(), true);
        Notification.Instance.SetNotification(true, "Image added to library");
    }
}
