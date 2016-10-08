using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants

    const string c_AnimationIdleLeft = "anim_player_idle_left";
    const string c_AnimationIdleRight = "anim_player_idle_right";
    const string c_AnimationRunLeft = "anim_player_run_left";
    const string c_AnimationRunRight = "anim_player_run_right";

    const string c_AxisHorizontal = "Horizontal";
    const string c_ButtonFire = "Fire1";
    const string c_ButtonJump = "Jump";

    const float c_RaycastDistance = 12.125f;

    const string c_FirePointLeft = "fire_point_left";
    const string c_FirePointRight = "fire_point_right";
    const string c_GroundSensor = "ground_sensor";

    #endregion

    #region Public Variables

    public float HorizontalAcceleration;
    public float HorizontalFriction;
    public float MaxHorizontalSpeed;
    public float Gravity;
    public float MaxFallSpeed;
    public float JumpingAcceleration;
    public float MinJumpTime;
    public float MaxJumpTime;
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;

    public LayerMask GroundLayerMask;

    #endregion

    #region Members

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    private Transform m_LeftFireTransform;
    private Transform m_RightFireTransform;
    private BoxCollider2D m_GroundCollider;

    private bool m_IsFacingRight;
    private bool m_IsOnGround;
    private bool m_IsJumping;
    private float m_TotalJumpTime;

    #endregion

    #region Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        m_LeftFireTransform = m_Transform.Find(c_FirePointLeft).GetComponent<Transform>();
        m_RightFireTransform = m_Transform.Find(c_FirePointRight).GetComponent<Transform>();
        m_GroundCollider = m_Transform.Find(c_GroundSensor).GetComponent<BoxCollider2D>();

        m_IsFacingRight = true;
        m_IsOnGround = false;
        m_IsJumping = false;
        m_TotalJumpTime = 0.0f;
    }

    void FixedUpdate()
    {
        // Read input.
        float horizontalInput = Input.GetAxis(c_AxisHorizontal);
        bool isFireButtonDown = Input.GetButtonDown(c_ButtonFire);
        bool isJumpButtonDown = Input.GetButton(c_ButtonJump);

        // Store temporary of current velocity.
        Vector2 velocity = m_Rigidbody.velocity;

        // Check if on ground.
        if (m_GroundCollider.IsTouchingLayers(GroundLayerMask))
        {
            m_IsOnGround = true;
        }
        else
        {
            m_IsOnGround = false;
        }

        // Handle jump input.
        if (isJumpButtonDown && m_IsOnGround)
        {
            m_IsJumping = true;
            m_TotalJumpTime = 0.0f;
            velocity.y = 0.0f;
        }

        // Handle horizontal movement.
        if (horizontalInput != 0.0f)
        {
            // Acceleration.
            velocity.x += horizontalInput * HorizontalAcceleration;

            velocity.x = Mathf.Clamp(velocity.x, -MaxHorizontalSpeed, MaxHorizontalSpeed);
        }
        else
        {
            // Friction.
            if (Mathf.Abs(velocity.x) < HorizontalFriction)
            {
                velocity.x = 0.0f;
            }
            else
            {
                velocity.x -= HorizontalFriction * Mathf.Sign(velocity.x);
            }
        }

        // Increase jumping over time.
        if (m_IsJumping)
        {
            velocity.y += JumpingAcceleration * Mathf.Sqrt(MaxJumpTime - Mathf.Min(m_TotalJumpTime, MaxJumpTime));

            m_TotalJumpTime += Time.deltaTime;
            if (m_TotalJumpTime >= MaxJumpTime || (!isJumpButtonDown && m_TotalJumpTime > MinJumpTime))
            {
                m_IsJumping = false;
            }
        }

        // Fall from gravity.
        if (!m_IsOnGround && !m_IsJumping)
        {
            velocity.y -= Gravity;

            if (velocity.y < -MaxFallSpeed)
            {
                velocity.y = -MaxFallSpeed;
            }
        }

        // Assign velocity to rigidbody.
        m_Rigidbody.velocity = velocity;

        // Handle firing.
        if (isFireButtonDown)
        {
            GameObject go = (GameObject)Instantiate(ProjectilePrefab, (m_IsFacingRight ? m_RightFireTransform.position : m_LeftFireTransform.position), Quaternion.identity);
            Rigidbody2D rigidbody = go.GetComponent<Rigidbody2D>();
            rigidbody.velocity = (m_IsFacingRight ? Vector2.right : Vector2.left) * ProjectileSpeed;
        }

        // Change states based on input.
        if (horizontalInput > 0.0f)
        {
            m_Animator.Play(c_AnimationRunRight);
            m_IsFacingRight = true;
        }
        else if (horizontalInput < 0.0f)
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
