using UnityEngine;

public class Tools
{
    public static bool IsInLayerMask(GameObject p_GameObject, LayerMask p_LayerMask)
    {
        return (p_LayerMask.value & (1 << p_GameObject.layer)) > 0;
    }
}
