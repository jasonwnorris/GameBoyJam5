using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants

    const string c_AnimationIdleLeft = "anim_player_idle_left";
    const string c_AnimationIdleRight = "anim_player_idle_right";
    const string c_AnimationRunLeft = "anim_player_run_left";
    const string c_AnimationRunRight = "anim_player_run_right";

    const string c_AxisHorizontal = "Horizontal";

    #endregion

    #region Public Variables

    public float HorizontalMovementSpeed = 1.0f;

    #endregion

    #region Members

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    private bool m_IsFacingRight;

    #endregion

    #region Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        m_IsFacingRight = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxis(c_AxisHorizontal);

        Vector2 force = Vector2.right * horizontal * HorizontalMovementSpeed;

        m_Rigidbody.AddRelativeForce(force, ForceMode2D.Impulse);

        if (horizontal > 0.0f)
        {
            m_Animator.Play(c_AnimationRunRight);
            m_IsFacingRight = true;
        }
        else if (horizontal < 0.0f)
        {
            m_Animator.Play(c_AnimationRunLeft);
            m_IsFacingRight = false;
        }
        else
        {
            if (m_IsFacingRight)
            {
                m_Animator.Play(c_AnimationIdleRight);
            }
            else
            {
                m_Animator.Play(c_AnimationIdleLeft);
            }
        }
    }

    #endregion
}
