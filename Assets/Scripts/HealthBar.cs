using UnityEngine;

public class HealthBar : MonoBehaviour
{
    #region Public Variables

    public GameObject Subject;

    #endregion

    #region Components

    private Animation m_Animation;
    private Animator m_Animator;
    private Health m_Health;

    #endregion

    #region Members

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        m_Animation = GetComponent<Animation>();
        m_Animator = GetComponent<Animator>();

        m_Health = Subject.GetComponent<Health>();

        if (m_Health == null)
        {
            Debug.LogError("Subject " + Subject.name + " must have a Health script attached.");
        }
    }

    void Start()
    {
        m_Animator.speed = 0.0f;
    }

    #endregion
}
