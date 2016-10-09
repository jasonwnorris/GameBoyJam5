using UnityEngine;

public class Damage : MonoBehaviour
{
    #region Public Variables

    [Range(0, 100)]
    public int Health;

    [Range(0.0f, float.MaxValue)]
    public float Knockback;

    #endregion
}
