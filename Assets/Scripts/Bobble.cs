using UnityEngine;

public class Bobble : MonoBehaviour
{
    #region Public Variables

    public float HorizontalStrength;

    public float HorizontalPeriod;

    public float VerticalStrength;

    public float VerticalPeriod;

    #endregion

    #region Components

    private Transform m_Transform;

    #endregion

    #region Members

    private Vector3 m_StartPosition;
    private float m_HorizontalOffset;
    private float m_VerticalOffset;

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
    }

    void Start()
    {
        m_StartPosition = m_Transform.position;
        m_HorizontalOffset = 0.0f;
        m_VerticalOffset = 0.0f;
    }

    void Update()
    {
        m_HorizontalOffset = Mathf.Sin(HorizontalStrength * Time.time) * HorizontalPeriod;
        m_VerticalOffset = Mathf.Sin(VerticalPeriod * Time.time) * VerticalStrength;

        m_Transform.position = m_StartPosition + Vector3.right * m_HorizontalOffset + Vector3.up * m_VerticalOffset;
    }

    #endregion
}
