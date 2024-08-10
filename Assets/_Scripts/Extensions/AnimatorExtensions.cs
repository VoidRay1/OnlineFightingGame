using UnityEngine;

public static class AnimatorExtensions 
{
    public static bool IsAnimationPlaying(this Animator animator, int layerIndex)
    {
        Debug.Log("Transition: " + animator.IsInTransition(layerIndex));
        Debug.Log("Normalized time: " + animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime);
        Debug.Log("Animation speed: " + animator.GetCurrentAnimatorStateInfo(layerIndex).speed);
        Debug.Log("Current animation: " + animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Fighting Idle"));
        return animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime > 1.0f;
    }
}