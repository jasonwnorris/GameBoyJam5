using UnityEngine;

public class BatBehavior : MonoBehaviour
{
    #region Constants

    const string c_AnimationFly = "anim_enemy_bat_fly";
    const string c_AnimationIdle = "anim_enemy_bat_idle";

    #endregion

    #region Public Variables

    public float MovementSpeed;
    public LayerMask AttackLayerMask;
    public float AttackRadius;

    #endregion

    #region Components

    private Transform m_Transform;
    private Animator m_Animator;

    #endregion

    #region Members

    private Transform m_AttackTarget;

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
    }

    void Start()
    {
        m_AttackTarget = null;
    }

    void Update()
    {
        CheckAttackRange();
        MoveTowardTarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }

    #endregion

    #region Private Methods

    private void CheckAttackRange()
    {
        Vector2 position = Tools.ToVector2(m_Transform.position);
        Collider2D collider = Physics2D.OverlapCircle(position, AttackRadius, AttackLayerMask);
        if (collider != null)
        {
            m_AttackTarget = collider.gameObject.transform;
            m_Animator.Play(c_AnimationFly);
        }
        else
        {
            m_AttackTarget = null;
            m_Animator.Play(c_AnimationIdle);
        }
    }

    private void MoveTowardTarget()
    {
        if (m_AttackTarget != null)
        {
            float speed = MovementSpeed * Time.deltaTime;
            Vector3 difference = m_AttackTarget.position - m_Transform.position;

            if (difference.magnitude < speed)
            {
                m_Transform.Translate(difference);
            }
            else
            {
                m_Transform.Translate(difference.normalized * speed);
            }
        }
    }

    #endregion
}
