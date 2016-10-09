using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public enum RestPeriod
    {
        ShortDelay,
        LongDelay
    }

    #region Public Variables

    [Header("Projectile")]

    public Transform Origin;

    public GameObject Prefab;

    public DirectionEnum Direction;

    [Range(0.0f, 1000.0f)]
    public float Speed;

    [Header("Pattern")]

    [Range(1, 10)]
    public int FireCount;

    [Range(0.0f, 10.0f)]
    public float ShortDelayTime;

    [Range(0.0f, 60.0f)]
    public float LongDelayTime;

    #endregion

    #region Members

    private RestPeriod m_RestPeriod;
    private int m_FireCount;
    private float m_TotalTime;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        m_RestPeriod = RestPeriod.LongDelay;
        m_FireCount = 0;
        m_TotalTime = 0.0f;
    }

    void Update()
    {
        m_TotalTime += Time.deltaTime;

        switch (m_RestPeriod)
        {
            case RestPeriod.ShortDelay:
                ProcessShortDelay();
                break;
            case RestPeriod.LongDelay:
                ProcessLongDelay();
                break;
        }
    }

    #endregion

    #region Private Methods

    void ProcessShortDelay()
    {
        if (m_TotalTime >= ShortDelayTime)
        {
            GameObject gameObject = (GameObject)Instantiate(Prefab, Origin.position, Quaternion.identity);
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.velocity = DirectionUtil.AsVector3[Direction] * Speed;

            ++m_FireCount;
            if (m_FireCount >= FireCount)
            {
                m_RestPeriod = RestPeriod.LongDelay;
                m_FireCount = 0;
            }

            m_TotalTime = 0.0f;
        }
    }

    void ProcessLongDelay()
    {
        if (m_TotalTime >= LongDelayTime)
        {
            m_RestPeriod = RestPeriod.ShortDelay;
            m_TotalTime = 0.0f;
        }
    }

    #endregion
}
