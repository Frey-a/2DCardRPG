using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    protected CharStatus status = new CharStatus();

    public int id;
    public Image img;
    public RectTransform hp;

    protected abstract int GetMaxHp();

    public void Damage(int val)
    {
        status.hp -= val;

        if (status.hp < 0)
        {
            status.hp = 0;
        }

        UpdateHp();
    }

    public void Heal(int val)
    {
        status.hp = Mathf.Min(status.hp + val, GetMaxHp());

        UpdateHp();
    }

    public void UpdateHp()
    {
        Slider hpBar = hp.GetComponent<Slider>();
        hpBar.value = (float)status.hp / GetMaxHp();

        Text hpTxt = hp.GetComponentInChildren<Text>();
        hpTxt.text = status.hp + " / " + GetMaxHp();
    }
}
