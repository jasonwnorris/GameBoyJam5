using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    #region Public Variables

    public Transform Target;

    [Range(0.001f, 1.0f)]
    public float FollowFactor;

    #endregion

    #region Members

    private Transform m_Transform;

    private float m_Z;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();

        m_Z = m_Transform.position.z;
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(Target.position.x, Target.position.y, m_Z);

        Vector3 difference = targetPosition - m_Transform.position;

        float speed = difference.magnitude * FollowFactor;

        if (difference.magnitude < speed)
        {
            m_Transform.Translate(difference);
        }
        else
        {
            m_Transform.Translate(difference.normalized * speed);
        }
    }

    #endregion
}
