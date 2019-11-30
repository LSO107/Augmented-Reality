using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

internal sealed class HandleDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject imagePrefab;
    [SerializeField]
    private GameObject wikipediaPrefab;
    [SerializeField]
    private int picturesPerRow = 5;
    [SerializeField]
    private float imageOffset = 0.2f;
    [SerializeField]
    private Slider loadingBar;

    private float m_TotalDownloadProgress;

    private const int NumberOfImages = 10;

    private List<GameObject> m_InstantiatedImages = new List<GameObject>();
    private GameObject m_InstantiatedWiki;

    /// <summary>
    /// Instantiates image prefabs, sets positions and rotation relative to camera.
    /// Sets the texture from the byte array
    /// </summary>
    public void SetImages(List<byte[]> images, IEnumerable<string> imageContextLinks)
    {
        var column = 0;
        var row = 0;

        for (var i = 0; i < images.Count; i++)
        {
            var texture = new Texture2D(1, 1);
            texture.LoadImage(images[i]);

            if (column % picturesPerRow == 0)
            {
                column = 0;
                row++;
            }

            var pos = CalculateImageGridPosition(column, row);
            var img = Instantiate(imagePrefab, pos, Quaternion.identity, transform);
            m_InstantiatedImages.Add(img);
            StartCoroutine(ScaleImageOverTime(img));

            img.transform.LookAt(Camera.main.transform.position);
            img.transform.Rotate(Vector3.up, 180);

            img.GetComponent<Renderer>().material.mainTexture = texture;
            img.GetComponent<TouchControl>().StoreContextLinks(imageContextLinks.ToList()[i]);
            column++;
        }
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

    /// <summary>
    /// Gets the location to spawn relative to the camera
    /// </summary>
    private Vector3 CalculateImageGridPosition(int column, int row)
    {
        var cam = Camera.main.transform;
        var center = cam.position;
        var camRight = cam.right;
        var camForward = cam.forward;

        var start = center + camForward - Vector3.Scale(camRight, new Vector3(0.3f, 0, 0.3f));

        var x = start.x + (imageOffset * camRight.x) * column;
        var y = start.y + imageOffset * row;
        var z = start.z + (imageOffset * camRight.z) * column;

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Get position to spawn Wikipedia text relative to <see cref="Camera"/>
    /// </summary>
    /// <returns></returns>
    private static Vector3 CalculateWikiTextPosition()
    {
        var cam = Camera.main.transform;
        var center = cam.position;
        var camForward = cam.forward;

        var start = center + camForward;

        return start;
    }

    /// <summary>
    /// Download progress of all the images converted to
    /// a value between 0 and 1 for the slider
    /// </summary>
    public void UpdateLoadingBar(UnityWebRequest request)
    {
        m_TotalDownloadProgress += request.downloadProgress;
        loadingBar.value = (m_TotalDownloadProgress / NumberOfImages) * 1;
    }

    /// <summary>
    /// Instantiate wikipedia text prefab and set the <see cref="Text"/> component
    /// </summary>
    public void SetWikipediaText(string text)
    {
        m_InstantiatedWiki = Instantiate(wikipediaPrefab, CalculateWikiTextPosition(), Quaternion.identity, transform);
        m_InstantiatedWiki.GetComponentInChildren<Text>().text = text;
        m_InstantiatedWiki.transform.LookAt(Camera.main.transform.position);
        m_InstantiatedWiki.transform.Rotate(Vector3.up, 180);
    }

    /// <summary>
    /// Iterates over child objects and deletes them
    /// </summary>
    public void DeleteSearchResults()
    {
        foreach (var image in m_InstantiatedImages)
        {
            Destroy(image);
        }
        
        Destroy(m_InstantiatedWiki);

        m_TotalDownloadProgress = 0;
        loadingBar.value = 0;
    }

    public void RemoveImageFromCollection(GameObject image)
    {
        m_InstantiatedImages.Remove(image);
    }
}