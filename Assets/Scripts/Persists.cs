using UnityEngine;

public class Persists : MonoBehaviour
{
    #region MonoBehaviour Methods

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion
}
