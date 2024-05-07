using UnityEngine;

public class ExpItem : Item
{
    // Start is called before the first frame update
    public ExpItemData ExpItemData { get; private set; }

    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            int step = (int)(value * 0.02f); 
            transform.localScale = Vector3.one * ((1.0f + step * 0.5f));
        }
    }
    private int _exp;

    public override void Init(ItemData data)
    {
        base.Init(data);
        ExpItemData = (data as ExpItemData);
    }

    protected override void Eat()
    {
        base.Eat();
        GameManager.Instance.ChangeExp(Exp * (1+GameManager.Instance.SaveData.ExpBonus));
    }
}