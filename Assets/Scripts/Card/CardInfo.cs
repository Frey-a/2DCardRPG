using System;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    private CardData data; // 정보

    public int id;
    public Image[] imgs;
    public Text[] costs;
    public Text cardName;
    public Text description; // 효과 설명
    #region Exclusive
    public GameObject exclusive; // 전용 obj 부모
    public Image charImg;
    public Text charName;
    #endregion

    public enum TargetType
    {
        none,
        enemy,
        ally,
        self
    }

    public enum TargetRange
    {
        select,
        random,
        all,
        self
    }

    public void SetData(int cardId)
    {
        data = InfoMgr.Instance.database.cards.Find(c => c.cardId == cardId);

        id = data.cardId;
        //foreach(Image img in imgs)
        //{

        //}
        foreach (Text cost in costs)
        {
            cost.text = data.cost.ToString();
        }
        cardName.text = data.cardName;
        //description.text = 

        if (!data.charName.Equals(""))
        {
            exclusive.SetActive(true);
            //charImg.sprite = 
            charName.text = data.charName;
        }
    }

    public TargetType GetTargetType()
    {
        if(data.targetPos.Equals("self"))
        {
            return TargetType.self;
        }
        else
        {
            Enum.TryParse(data.targetType, out TargetType type);
            return type;
        }
    }

    public void Use()
    {
        char delimiter = '_';
        string[] target = data.targetPos.Split(delimiter);

        if (Enum.TryParse(target[0], out TargetRange range) && Enum.IsDefined(typeof(TargetRange), range))
        {
            switch(range)
            {
                case TargetRange.select:

                    break;
                case TargetRange.random:

                    break;
                case TargetRange.all:

                    break;
                case TargetRange.self:

                    break;
            }
        }
    }
}
