using UnityEngine;

public class Mage : PlayerComponent
{
    public override Job MyJob => Job.Mage;

    [SerializeField]private Transform handEffectPoint;

    private void Start()
    {
        MyAnimEvent.SkillAct += OnSkillEffect;
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
        MagicEffect magic = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as MagicEffect;
        magic.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        magic.Shoot();
    }

    public override void SetSkillAct(Player player)
    {
        player.OnSkillAct += OnSkill;
        player.OffSkillAct += OffSkill;
    }

    private void OnSkill()
    {
        GameManager.Instance.Player.SetOutOfControl(true);
        GameManager.Instance.Player.RotatingBody.GetComponent<LookAtPoint>().SetRotSpeed(0.1f);
    }

    private void OffSkill()
    {
        GameManager.Instance.Player.SetOutOfControl(false);
        GameManager.Instance.Player.RotatingBody.GetComponent<LookAtPoint>().ResetRotSpeed();
    }

}
