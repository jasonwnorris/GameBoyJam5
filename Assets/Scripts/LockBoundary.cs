using System;
using UnityEngine;

public class LockBoundary : MonoBehaviour
{
    private class Lock
    {
        public Vector2 RayDirection { get; set; }
        public Vector3 AdjustmentDirection { get; set; }
        public Func<float> DistanceFunction { get; set; }
    }

    #region Public Variables

    public LayerMask LayerMask;

    [Range(0.0f, 1024.0f)]
    public float TopDistance;

    [Range(0.0f, 1024.0f)]
    public float BottomDistance;

    [Range(0.0f, 1024.0f)]
    public float LeftDistance;

    [Range(0.0f, 1024.0f)]
    public float RightDistance;

    #endregion

    #region Members

    private Transform m_Transform;

    private Lock[] m_Locks;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();

        m_Locks = new Lock[]
        {
            new Lock()
            {
                RayDirection = Vector2.up,
                AdjustmentDirection = Vector3.down,
                DistanceFunction = () => { return TopDistance; }
            },
            new Lock()
            {
                RayDirection = Vector2.down,
                AdjustmentDirection = Vector3.up,
                DistanceFunction = () => { return BottomDistance; }
            },
            new Lock()
            {
                RayDirection = Vector2.left,
                AdjustmentDirection = Vector3.right,
                DistanceFunction = () => { return LeftDistance; }
            },
            new Lock()
            {
                RayDirection = Vector2.right,
                AdjustmentDirection = Vector3.left,
                DistanceFunction = () => { return RightDistance; }
            }
        };
    }

    void Update()
    {
        Vector2 position = new Vector2(m_Transform.position.x, m_Transform.position.y);

        foreach (Lock l in m_Locks)
        {
            float distance = l.DistanceFunction();
            RaycastHit2D raycastHit = Physics2D.Raycast(position, l.RayDirection, distance, LayerMask);
            if (raycastHit.collider != null)
            {
                m_Transform.Translate(l.AdjustmentDirection * (distance - raycastHit.distance));
            }
        }
    }

    #endregion
}
