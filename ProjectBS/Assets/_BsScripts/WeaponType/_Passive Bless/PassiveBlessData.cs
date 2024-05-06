using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PassiveBless", menuName = "Bless/PassiveBless", order = 1)]
public class PassiveBlessData : BaseBlessData
{
    public Sprite Icon => _icon;
    [SerializeField] private Sprite _icon;

    enum PassiveBlessType
    {
        Attack,
        AkSpeed,
        HpMax,
        Speed,
        MagnetRange,
        ConstSpeed,
        RepairSpeed,
    }
    
    public int ShowRandomPassive(out string Description, out UnityAction action, params int[] exclude)
    {
        List<int> types = new List<int>();
        for(int i = 0; i < System.Enum.GetValues(typeof(PassiveBlessType)).Length; i++)
        {
            if(exclude.Contains(i))
                continue;
            types.Add(i);
        }

        PassiveBlessType random = (PassiveBlessType)types[Random.Range(0, types.Count)];
        int randomValue = Random.Range(1, 4);
        int result;
        StringBuilder sb = new StringBuilder();
        switch(random)
        {
            case PassiveBlessType.Attack:
                sb.Append("공격력이 ");
                result = 0;
                action = () => GameManager.Instance.Player.getBuff.atkBuffDict["PassiveAttack"] += randomValue * 0.1f;
                break;
            case PassiveBlessType.AkSpeed:
                sb.Append("공격속도가 ");
                action = () => GameManager.Instance.Player.getBuff.asBuffDict["PassiveAkSpeed"] += randomValue * 0.1f;
                result = 1;
                break;
            case PassiveBlessType.HpMax:
                sb.Append("체력이 ");
                action = () => GameManager.Instance.Player.getBuff.hpBuffDict["PassiveHpMax"] += randomValue * 0.1f;
                result = 2;
                break;
            case PassiveBlessType.Speed:
                sb.Append("이동속도가 ");
                action = () => GameManager.Instance.Player.getBuff.msBuffDict["PassiveSpeed"] += randomValue * 0.1f;
                result = 3;
                break;
            case PassiveBlessType.MagnetRange:
                sb.Append("자석의 범위가 ");
                action = () => GameManager.Instance.Player.getBuff.rangeBuffDict["PassiveMagnetRange"] += randomValue * 0.1f;
                result = 4;
                break;
            case PassiveBlessType.ConstSpeed:
                sb.Append("건설 속도가 ");
                action = () => GameManager.Instance.Player.ConstSpeedBuff += randomValue * 0.1f;
                result = 5;
                break;
            case PassiveBlessType.RepairSpeed:
                sb.Append("수리 속도가 ");
                action = () => GameManager.Instance.Player.RepairSpeedBuff += randomValue * 0.1f;
                result = 6;
                break;
            default:
                Description = "";
                action = null;
                return -1;
        }
        sb.Append(randomValue * 10);
        sb.Append("% 증가합니다.");
        Description = sb.ToString();
        return result;
    }
    
}
