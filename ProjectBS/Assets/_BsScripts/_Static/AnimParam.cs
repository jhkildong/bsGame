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

    public static int OnSkill1 = Animator.StringToHash("OnSkill1");
    public static int OnSkill2 = Animator.StringToHash("OnSkill2");

    public static int Wait = Animator.StringToHash("Wait"); //ImpOnly

    public static int isBuilding = Animator.StringToHash("isBuilding");
    public static int isSkill = Animator.StringToHash("isSkill");
    public static int OnSkill = Animator.StringToHash("OnSkill");
    public static int AttackSpeed = Animator.StringToHash("AttackSpeed");
    public static int Lv7Skill = Animator.StringToHash("Lv7Skill");

}