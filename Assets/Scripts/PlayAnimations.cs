using UnityEngine;
using StarterAssets;
using PurrNet;

public class PlayAnimations : MonoBehaviour
{
    [SerializeField] StarterAssetsInputs input;
    [SerializeField] NetworkAnimator animator;

    // Update is called once per frame
    void Update()
    {
        if (input.move.y > 0)
        {
            animator.SetTrigger("StartWalk");
        }
        else
        {
            animator.SetTrigger("StopWalk");
        }
    }
}
