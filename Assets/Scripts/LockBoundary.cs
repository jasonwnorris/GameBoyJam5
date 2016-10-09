using System;
using System.Collections.Generic;
using UnityEngine;

public class LockBoundary : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [Serializable]
    public class ScanParameter
    {
        public Direction Direction;

        [Range(0.0f, 1024.0f)]
        public float Distance;
    }

    #region Constants

    private static readonly Dictionary<Direction, Vector2> c_RayDirections = new Dictionary<Direction, Vector2>()
    {
        { Direction.Up, Vector2.up },
        { Direction.Down, Vector2.down },
        { Direction.Left, Vector2.left },
        { Direction.Right, Vector2.right }
    };

    private static readonly Dictionary<Direction, Vector3> c_OffsetDirections = new Dictionary<Direction, Vector3>()
    {
        { Direction.Up, Vector3.down },
        { Direction.Down, Vector3.up },
        { Direction.Left, Vector3.right },
        { Direction.Right, Vector3.left }
    };

    #endregion

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

    void Update()
    {
        Vector2 boxSize = new Vector2(Size * 2.0f, Size * 2.0f);
        foreach (ScanParameter scanParameter in ScanParameters)
        {
            Vector2 position = Tools.ToVector2(m_Transform.position);
            Vector2 direction = c_RayDirections[scanParameter.Direction];
            float distance = scanParameter.Distance - Size;
            RaycastHit2D raycastHit = Physics2D.BoxCast(position, boxSize, 0.0f, direction, distance, LayerMask);
            if (raycastHit.collider != null)
            {
                float dot = Vector2.Dot(direction, raycastHit.normal);
                if (dot == -1.0f)
                {
                    m_Transform.Translate(c_OffsetDirections[scanParameter.Direction] * (distance - raycastHit.distance));

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
