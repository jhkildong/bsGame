using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AnimParam
{
    public static int Attack = Animator.StringToHash("Attack");
    public static int isAttacking = Animator.StringToHash("isAttacking");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int isAttackMoving = Animator.StringToHash("isAttackMoving");
    public static int Death = Animator.StringToHash("Death");
    public static int x = Animator.StringToHash("x");
    public static int y = Animator.StringToHash("y");
}