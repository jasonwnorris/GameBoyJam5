using UnityEngine;

public class Tools
{
    public static bool IsInLayerMask(GameObject p_GameObject, LayerMask p_LayerMask)
    {
        return (p_LayerMask.value & (1 << p_GameObject.layer)) > 0;
    }

    public static Vector2 ToVector2(Vector3 p_Vector)
    {
        return new Vector2(p_Vector.x, p_Vector.y);
    }
}
