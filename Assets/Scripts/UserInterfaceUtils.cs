using UnityEngine;

public static class UserInterfaceUtils
{
    /// <summary>
    /// Pass in true or false to determine whether the canvas group
    /// options are enabled or disabled
    /// </summary>
    public static void ToggleCanvasGroup(CanvasGroup group, bool show)
    {
        group.alpha = show ? 1 : 0;
        group.blocksRaycasts = show;
        group.interactable = show;
    }
}