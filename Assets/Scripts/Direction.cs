using System.Collections.Generic;
using UnityEngine;

public enum DirectionEnum
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionUtil
{
    public static readonly Dictionary<DirectionEnum, Vector2> AsVector2 = new Dictionary<DirectionEnum, Vector2>()
    {
        { DirectionEnum.Up, Vector2.up },
        { DirectionEnum.Down, Vector2.down },
        { DirectionEnum.Left, Vector2.left },
        { DirectionEnum.Right, Vector2.right }
    };

    public static readonly Dictionary<DirectionEnum, Vector3> AsVector3 = new Dictionary<DirectionEnum, Vector3>()
    {
        { DirectionEnum.Up, Vector3.up },
        { DirectionEnum.Down, Vector3.down },
        { DirectionEnum.Left, Vector3.left },
        { DirectionEnum.Right, Vector3.right }
    };
}