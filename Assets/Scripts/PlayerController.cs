using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants

    const string c_AnimationFallLeft = "anim_player_jump_left";
    const string c_AnimationFallRight = "anim_player_jump_right";
    const string c_AnimationIdleLeft = "anim_player_idle_left";
    const string c_AnimationIdleRight = "anim_player_idle_right";
    const string c_AnimationJumpLeft = "anim_player_jump_left";
    const string c_AnimationJumpRight = "anim_player_jump_right";
    const string c_AnimationRunLeft = "anim_player_run_left";
    const string c_AnimationRunRight = "anim_player_run_right";
    const string c_AnimationShootLeft = "anim_player_shoot_left";
    const string c_AnimationShootRight = "anim_player_shoot_right";

    const string c_AxisHorizontal = "Horizontal";
    const string c_ButtonFire = "Fire1";
    const string c_ButtonJump = "Jump";

    const float c_RaycastDistance = 12.125f;

    const string c_FirePointLeft = "fire_point_left";
    const string c_FirePointRight = "fire_point_right";
    const string c_GroundSensor = "ground_sensor";

    #endregion

    #region Public Variables

    public LayerMask GroundLayerMask;

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

    public AudioClip JumpAudio;
    public AudioClip LandAudio;
    public AudioClip BlastAudio;

    #endregion

    #region Members

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private AudioSource m_AudioSource;

    private Transform m_LeftFireTransform;
    private Transform m_RightFireTransform;
    private BoxCollider2D m_GroundCollider;

    private bool m_IsFacingRight;
    private bool m_IsOnGround;
    private bool m_IsJumping;
    private float m_TotalJumpTime;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

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

        // Change states based on input.
        if (horizontalInput > 0.0f)
        {
            m_IsFacingRight = true;
        }
        else if (horizontalInput < 0.0f)
        {
            m_IsFacingRight = false;
        }

        // Store temporary of current velocity.
        Vector2 velocity = m_Rigidbody.velocity;

        // Check if on ground.
        if (m_GroundCollider.IsTouchingLayers(GroundLayerMask))
        {
            if (!m_IsOnGround)
            {
                PlaySound(LandAudio);
            }

            m_IsOnGround = true;
        }
        else
        {
            m_IsOnGround = false;
        }

        // Handle jump input.
        if (isJumpButtonDown && m_IsOnGround && !m_IsJumping)
        {
            m_IsJumping = true;
            m_TotalJumpTime = 0.0f;
            velocity.y = 0.0f;

            PlaySound(JumpAudio);
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
            ShootProjectile();
        }

        // Handle animations.
        if (m_IsOnGround)
        {
            if (horizontalInput > 0.0f)
            {
                PlayAnimation(c_AnimationRunRight);
            }
            else if (horizontalInput < 0.0f)
            {
                PlayAnimation(c_AnimationRunLeft);
            }
            else
            {
                PlayAnimation(m_IsFacingRight ? c_AnimationIdleRight : c_AnimationIdleLeft);
            }
        }
        else if (m_IsJumping)
        {
            PlayAnimation(m_IsFacingRight ? c_AnimationJumpRight : c_AnimationJumpLeft);
        }
        else
        {
            PlayAnimation(m_IsFacingRight ? c_AnimationFallRight : c_AnimationFallLeft);
        }
    }

    #endregion

    #region Helpers

    private void PlayAnimation(string p_AnimationName)
    {
        m_Animator.Play(p_AnimationName);
    }

    private void PlaySound(AudioClip p_AudioClip)
    {
        m_AudioSource.PlayOneShot(p_AudioClip);
    }

    private void ShootProjectile()
    {
        GameObject gameObject = (GameObject)Instantiate(ProjectilePrefab, (m_IsFacingRight ? m_RightFireTransform.position : m_LeftFireTransform.position), Quaternion.identity);
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.velocity = (m_IsFacingRight ? Vector2.right : Vector2.left) * ProjectileSpeed;

        PlayAnimation(m_IsFacingRight ? c_AnimationShootRight : c_AnimationShootLeft);
        PlaySound(BlastAudio);
    }

    #endregion
}
