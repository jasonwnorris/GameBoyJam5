using System;
using UnityEngine;

public class LockBoundary : MonoBehaviour
{
    [Serializable]
    public class ScanParameter
    {
        public DirectionEnum Direction;

        [Range(0.0f, 1024.0f)]
        public float Distance;
    }

    #region Public Variables

    public LayerMask LayerMask;

    [Range(0.0f, 1024.0f)]
    public float Size;

    public ScanParameter[] ScanParameters;

    #endregion

    #region Components

    private Transform m_Transform;

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        Vector2 boxSize = new Vector2(Size * 2.0f, Size * 2.0f);
        foreach (ScanParameter scanParameter in ScanParameters)
        {
            Vector2 position = Tools.ToVector2(m_Transform.position);
            Vector2 direction = DirectionUtil.AsVector2[scanParameter.Direction];
            float distance = scanParameter.Distance - Size;
            RaycastHit2D raycastHit = Physics2D.BoxCast(position, boxSize, 0.0f, direction, distance, LayerMask);
            if (raycastHit.collider != null)
            {
                float dot = Vector2.Dot(direction, raycastHit.normal);
                if (dot == -1.0f)
                {
                    m_Transform.Translate(-DirectionUtil.AsVector3[scanParameter.Direction] * (distance - raycastHit.distance));

                    Debug.DrawLine(m_Transform.position, raycastHit.point, Color.magenta, 0.0f, false);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * Size * 2.0f);
    }

    #endregion
}
