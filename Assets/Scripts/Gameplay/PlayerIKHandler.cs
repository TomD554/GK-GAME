using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIKHandler : MonoBehaviour
{
    public Animator animator;
    
    public bool ikActive = true;
    public Transform rightFootTarget;
    public Transform leftFootTarget;

    public bool useRightFoot = false; // Toggle for pass or shot foot

    private void Update()
    {
        rightFootTarget = GameObject.FindGameObjectWithTag("Ball").transform;
        leftFootTarget = GameObject.FindGameObjectWithTag("Ball").transform;
    }
    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || !ikActive) return;

        if (useRightFoot && rightFootTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
        }
        else if (!useRightFoot && leftFootTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);
        }
    }
}