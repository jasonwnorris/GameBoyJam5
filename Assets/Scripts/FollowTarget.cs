using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    #region Public Variables

    public Transform Target;
    public float FollowFactor = 2.0f;

    #endregion

    #region Members

    private Transform m_Transform;

    #endregion

    #region Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector2 currentPosition = new Vector2(m_Transform.position.x, m_Transform.position.y);
        Vector2 targetPosition = new Vector2(Target.position.x, Target.position.y);

        Vector2 difference = targetPosition - currentPosition;

        Vector2 movePosition = currentPosition + difference / FollowFactor;

        m_Transform.position = new Vector3(movePosition.x, movePosition.y, m_Transform.position.z);
    }

    #endregion
}
