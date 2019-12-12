using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;

    private void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenControls()
    {
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, true);
    }

    public void CloseControls()
    {
        UserInterfaceUtils.ToggleCanvasGroup(m_CanvasGroup, false);
    }
}
