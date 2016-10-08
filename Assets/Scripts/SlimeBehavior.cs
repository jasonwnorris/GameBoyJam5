using UnityEngine;

public class SlimeBehavior : MonoBehaviour
{
    #region Constants

    const string c_AnimationSlimeLeft = "anim_enemy_slime_left";
    const string c_AnimationSlimeRight = "anim_enemy_slime_right";

    const string c_GroundSensorLeft = "ground_sensor_left";
    const string c_GroundSensorRight = "ground_sensor_right";
    const string c_WallSensorLeft = "wall_sensor_left";
    const string c_WallSensorRight = "wall_sensor_right";

    #endregion

    #region Public Variables

    public LayerMask GroundLayerMask;
    public float MovementSpeed;

    #endregion

    #region Members

    private Transform m_Transform;
    private Animator m_Animator;

    private Transform m_GroundSensorLeftTransform;
    private Transform m_GroundSensorRightTransform;
    private Transform m_WallSensorLeftTransform;
    private Transform m_WallSensorRightTransform;

    private bool m_IsFacingRight;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();

        m_GroundSensorLeftTransform = m_Transform.Find(c_GroundSensorLeft).GetComponent<Transform>();
        m_GroundSensorRightTransform = m_Transform.Find(c_GroundSensorRight).GetComponent<Transform>();
        m_WallSensorLeftTransform = m_Transform.Find(c_WallSensorLeft).GetComponent<Transform>();
        m_WallSensorRightTransform = m_Transform.Find(c_WallSensorRight).GetComponent<Transform>();

        m_IsFacingRight = false;
    }

    void Update()
    {
        CheckCollision();
        ApplyMovement();
    }

    #endregion

    #region Private Methods

    private void ApplyMovement()
    {
        m_Transform.Translate((m_IsFacingRight ? Vector3.right : Vector3.left) * MovementSpeed * Time.deltaTime);
    }

    private void CheckCollision()
    {
        Vector2 groundSensorLeft = ToVector2(m_GroundSensorLeftTransform.position);
        Vector2 groundSensorRight = ToVector2(m_GroundSensorRightTransform.position);
        Vector2 wallSensorLeft = ToVector2(m_WallSensorLeftTransform.position);
        Vector2 wallSensorRight = ToVector2(m_WallSensorRightTransform.position);

        if (!Physics2D.OverlapPoint(groundSensorLeft, GroundLayerMask) || Physics2D.OverlapPoint(wallSensorLeft, GroundLayerMask))
        {
            FaceRight();
        }

        if (!Physics2D.OverlapPoint(groundSensorRight, GroundLayerMask) || Physics2D.OverlapPoint(wallSensorRight, GroundLayerMask))
        {
            FaceLeft();
        }
    }

    private void FaceLeft()
    {
        m_IsFacingRight = false;
        m_Animator.Play(c_AnimationSlimeLeft);
    }

    private void FaceRight()
    {
        m_IsFacingRight = true;
        m_Animator.Play(c_AnimationSlimeRight);
    }

    #endregion

    #region Helpers

    private Vector2 ToVector2(Vector3 p_Vector)
    {
        return new Vector2(p_Vector.x, p_Vector.y);
    }

    #endregion
}
