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
    public float BoxSize;

    public ScanParameter[] ScanParameters;

    #endregion

    #region Members

    private Transform m_Transform;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector2 size = new Vector2(BoxSize, BoxSize);
        float halfSize = BoxSize * 0.5f;
        foreach (ScanParameter sp in ScanParameters)
        {
            Vector2 position = Tools.ToVector2(m_Transform.position);
            RaycastHit2D raycastHit = Physics2D.BoxCast(position, size, 0.0f, c_RayDirections[sp.Direction], sp.Distance, LayerMask);
            if (raycastHit.collider != null)
            {
                Vector2 d = c_RayDirections[sp.Direction];
                Vector2 n = raycastHit.normal;
                Debug.Log(string.Format("[{0:0.00}] Direction: {1}, Normal: {2}, Dot: {3}, Distance: {4:0.000}", Time.time, d, n, Vector2.Dot(d, n), raycastHit.distance));
                m_Transform.Translate(c_OffsetDirections[sp.Direction] * (sp.Distance - raycastHit.distance - halfSize));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * BoxSize);
    }

    #endregion
}
