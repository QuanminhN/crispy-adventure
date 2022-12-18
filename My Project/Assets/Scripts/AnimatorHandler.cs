using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    int vertical;
    int horizontal;
    public bool canRotate;

    public void Initialized()
    {
        anim = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        float v = animatorValues(verticalMovement);
        float h = animatorValues(horizontalMovement);

        anim.SetFloat(vertical, v, .1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, .1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, .2f);
    }

    private float animatorValues(float movementValue)
    {
        if (movementValue > 0 && movementValue < .55f)
        {
            return .5f;
        }
        else if (movementValue > .55f)
        {
            return 1;
        }
        else if (movementValue < 0 && movementValue > -.55f)
        {
            return -.5f;
        }
        else if (movementValue < -.55)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public void CanRotate()
    {
        canRotate = true;
    }
    public void StopRotation()
    {
        canRotate = false;
    }
}
