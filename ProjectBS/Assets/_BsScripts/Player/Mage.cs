using UnityEngine;

public class Mage : PlayerComponent
{
    public override Job MyJob => Job.Mage;

    [SerializeField]private Transform handEffectPoint;
    [SerializeField] private GameObject BuffEffect;

    private void Start()
    {
        BuffEffect.SetActive(false);
        MyAnimEvent.SkillAct += OnSkillEffect;
        MyAnimEvent.SkillAct += OnLv7SkillEffect;
    }

    public void SetHandEffect(GameObject handEffect)
    {
        if(handEffectPoint == null)
        {
            handEffectPoint = transform.Find("HandEffect");
        }
        handEffect.transform.SetParent(handEffectPoint);
        handEffect.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void OnAttackPoint()
    {
        MagicEffect magic = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack, MyJobBless.MyStatus[Key.Size]) as MagicEffect;
        magic.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        magic.Shoot();
    }

    public void OnLv7SkillEffect(int i)
    {
        if (i == 2)
        {
            BuffEffect.SetActive(true);
            GameManager.Instance.Player.SetMoveStop(false); //임시처리
        }
    }
    public void OffLv7SkillEffect()
    {
        BuffEffect.SetActive(false);
    }

    public override void SetSkillAct(Player player)
    {
        player.OnSkillAct += OnSkill;
        player.OffSkillAct += OffSkill;
    }

    private void OnSkill()
    {
        MySkillEffect.Attack = MyJobBless.MyStatus[Key.SkillAttack];
        MySkillEffect.Size = MyJobBless.MyStatus[Key.SkillSize];
        GameManager.Instance.Player.SetMoveStop(true);
        GameManager.Instance.Player.RotatingBody.GetComponent<LookAtPoint>().SetRotSpeed(0.1f);
    }

    private void OffSkill()
    {
        GameManager.Instance.Player.SetMoveStop(false);
        GameManager.Instance.Player.RotatingBody.GetComponent<LookAtPoint>().ResetRotSpeed();
    }

}
