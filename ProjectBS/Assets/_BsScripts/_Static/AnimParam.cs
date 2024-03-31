using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AnimParam
{
    public static int Attack = Animator.StringToHash("Attack");
    public static int isAttacking = Animator.StringToHash("isAttacking");
    public static int isComboReady = Animator.StringToHash("isComboReady");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int Death = Animator.StringToHash("Death");
    public static int isDead = Animator.StringToHash("isDead");
    public static int x = Animator.StringToHash("x");
    public static int y = Animator.StringToHash("y");
}