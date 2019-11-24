using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

internal sealed class DisplayHandler : MonoBehaviour
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

    private int m_CurrentRow;
    private int m_CurrentColumn;
    private float m_TotalDownloadProgress;

    private const int NumberOfImages = 10;

    /// <summary>
    /// Instantiates image prefabs, sets positions and rotation relative to camera.
    /// Sets the texture from the byte array
    /// </summary>
    public void SetImages(List<byte[]> images, IEnumerable<string> imageContextLinks)
    {
        for (var i = 0; i < images.Count; i++)
        {
            var texture = new Texture2D(1, 1);
            texture.LoadImage(images[i]);

            if (m_CurrentColumn % picturesPerRow == 0)
            {
                m_CurrentColumn = 0;
                m_CurrentRow++;
            }

            var pos = GetSpawnPosition();

            var img = Instantiate(imagePrefab, new Vector3(pos.x, pos.y + 10, pos.z), Quaternion.identity, transform);
            //img.transform.LookAt(Camera.main.transform.position);
            //img.transform.Rotate(Vector3.up, 180);

            img.GetComponent<Renderer>().material.mainTexture = texture;
            img.GetComponent<TouchControl>().StoreContextLinks(imageContextLinks.ToList()[i]);
            m_CurrentColumn++;
        }
    }

    /// <summary>
    /// Gets the location to spawn relative to the camera
    /// </summary>
    private Vector3 GetSpawnPosition()
    {
        if (Camera.main == null)
            throw new NullReferenceException("Camera is null.");

        var cam = Camera.main.transform;
        var center = cam.position;
        var camRight = cam.right;
        var camForward = cam.forward;

        var start = center + camForward - Vector3.Scale(camRight, new Vector3(0.3f, 0, 0.3f));

        var x = start.x + (imageOffset * camRight.x) * m_CurrentColumn;
        var y = start.y + imageOffset * m_CurrentRow;
        var z = start.z + (imageOffset * camRight.z) * m_CurrentColumn;

        return new Vector3(x, y, z);
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
        var pos = GetSpawnPosition();
        var wiki = Instantiate(wikipediaPrefab, new Vector3(0, 0, pos.z), Quaternion.identity, transform);
        wiki.GetComponentInChildren<Text>().text = text;
        wiki.transform.LookAt(Camera.main.transform.position);
        wiki.transform.Rotate(Vector3.up, 180);
    }

    /// <summary>
    /// Iterates over child objects and deletes them
    /// </summary>
    public void DeleteSearchResults()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        m_TotalDownloadProgress = 0;
        loadingBar.value = 0;
        m_CurrentColumn = 0;
        m_CurrentRow = 0;
    }
}