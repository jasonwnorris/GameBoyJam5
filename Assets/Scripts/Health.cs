using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour
{
    #region Constants

    private const float c_FlickerTime = 0.05f;

    #endregion

    #region Public Variables

    public LayerMask HurtByLayerMask;

    [Range(0, 100)]
    public int MaximumHealth;

    [Range(0.0f, 60.0f)]
    public float InvulnerabilityTime;

    public AudioClip TakeDamageAudio;

    #endregion

    #region Members

    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer m_SpriteRenderer;
    private AudioSource m_AudioSource;

    private int m_Health;
    private bool m_IsVulnerable;
    private float m_TotalInvulnerableTime;
    private float m_TotalFlickerTime;

    #endregion

    #region Properties

    public int CurrentHealth
    {
        get { return m_Health; }
    }

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_AudioSource = GetComponent<AudioSource>();

        if (m_Rigidbody == null)
        {
            Debug.LogWarning("Entity " + gameObject.name + " is using Health script with no Rigidbody2D attached.");
        }

        if (m_AudioSource == null)
        {
            Debug.LogWarning("Entity " + gameObject.name + " is using Health script with no AudioSource attached.");
        }
    }

    void Start()
    {
        m_Health = MaximumHealth;
        m_IsVulnerable = true;
        m_TotalInvulnerableTime = 0.0f;
        m_TotalFlickerTime = 0.0f;
    }

    void Update()
    {
        if (!m_IsVulnerable)
        {
            m_TotalInvulnerableTime += Time.deltaTime;
            if (m_TotalInvulnerableTime >= InvulnerabilityTime)
            {
                m_IsVulnerable = true;
                m_SpriteRenderer.enabled = true;
            }
            else
            {
                m_TotalFlickerTime += Time.deltaTime;
                if (m_TotalFlickerTime >= c_FlickerTime)
                {
                    m_TotalFlickerTime = 0.0f;
                    m_SpriteRenderer.enabled = !m_SpriteRenderer.enabled;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D p_Collider)
    {
        if (m_IsVulnerable && Tools.IsInLayerMask(p_Collider.gameObject, HurtByLayerMask))
        {
            TakeDamage(p_Collider.gameObject.GetComponent<Damage>());
        }
    }

    #endregion

    #region Helpers

    private void TakeDamage(Damage p_Damage)
    {
        m_IsVulnerable = false;
        m_TotalInvulnerableTime = 0.0f;
        m_TotalFlickerTime = 0.0f;
        m_SpriteRenderer.enabled = false;

        if (m_AudioSource != null)
        {
            m_AudioSource.PlayOneShot(TakeDamageAudio);
        }

        TakeHealthDamage(p_Damage.Health);
        TakeKnockbackDamage(p_Damage.Knockback);
    }

    private void TakeHealthDamage(int p_Amount)
    {
        m_Health -= p_Amount;

        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void TakeKnockbackDamage(float p_Amount)
    {
        if (m_Rigidbody != null)
        {
        }
    }

    #endregion
}
