using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
    #region Public Variables
    
    public float Lifetime = 2.5f;

    #endregion

    #region Members

    private float m_TotalLifetime;

    #endregion

    #region Methods

    void Start()
    {
        m_TotalLifetime = 0.0f;
    }

    void FixedUpdate()
    {
        m_TotalLifetime += Time.deltaTime;

        if (m_TotalLifetime >= Lifetime)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
