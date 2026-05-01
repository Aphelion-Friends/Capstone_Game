using UnityEngine;

public enum FlashLightReaction
{
    None,
    Flee,
    Attracted
}

public class FlashlightSense : MonoBehaviour
{
    public FlashLightReaction reaction = FlashLightReaction.None;
}
