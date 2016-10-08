using UnityEngine;

public class InitializeScreenResolution : MonoBehaviour
{
    #region Public Variables

    public int Width;
    public int Height;
    public bool IsFullscreen;

    #endregion

    #region Methods

    void Awake()
    {
        Screen.SetResolution(Width, Height, IsFullscreen);
    }

    #endregion
}
