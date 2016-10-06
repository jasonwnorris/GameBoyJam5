using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants

    const string c_AnimationIdleLeft = "anim_player_idle_left";
    const string c_AnimationIdleRight = "anim_player_idle_right";
    const string c_AnimationRunLeft = "anim_player_run_left";
    const string c_AnimationRunRight = "anim_player_run_right";

    const string c_AxisHorizontal = "Horizontal";
    const string c_ButtonJump = "Jump";

    const float c_RaycastDistance = 12.125f;

    #endregion

    #region Public Variables

    public float HorizontalMovementSpeed = 1.0f;
    public float JumpingSpeed = 2.5f;
    public float MinimumJumpTime = 0.15f;
    public float MaximumJumpTime = 0.5f;

    #endregion

    #region Members

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    private bool m_IsFacingRight;
    private bool m_IsOnGround;
    private bool m_IsJumping;
    private float m_JumpTime;

    #endregion

    #region Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        m_IsFacingRight = true;
        m_IsOnGround = false;
        m_IsJumping = false;
        m_JumpTime = 0.0f;
    }

    void Update()
    {
        // Raycast to check if on ground.
        RaycastHit2D raycastHit = Physics2D.Raycast(m_Transform.position, Vector2.down, c_RaycastDistance);
        if (raycastHit)
        {
            m_IsOnGround = true;
        }
        else
        {
            m_IsOnGround = false;
        }

        // Read input.
        float horizontal = Input.GetAxis(c_AxisHorizontal);
        bool isJumpButtonDown = Input.GetButton(c_ButtonJump);

        // Handle movement.
        Vector2 horizontalForce = Vector2.right * horizontal * HorizontalMovementSpeed;
        m_Rigidbody.AddRelativeForce(horizontalForce, ForceMode2D.Impulse);

        // Handle jump input.
        if (isJumpButtonDown && m_IsOnGround)
        {
            if (!m_IsJumping)
            {
                m_IsJumping = true;
                m_JumpTime = 0.0f;
            }
        }

        // Increase jumping over time.
        if (m_IsJumping)
        {
            Vector2 verticalForce = Vector2.up * JumpingSpeed;
            m_Rigidbody.AddRelativeForce(verticalForce, ForceMode2D.Impulse);

            m_JumpTime += Time.deltaTime;

            if (m_JumpTime >= MaximumJumpTime || (!isJumpButtonDown && m_JumpTime > MinimumJumpTime))
            {
                m_IsJumping = false;
            }
        }

        // Change states based on input.
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
