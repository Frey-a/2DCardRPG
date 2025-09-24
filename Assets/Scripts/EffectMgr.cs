using System;
using UnityEngine;

public enum TargetType
{ 
    Enemy,
    Ally,
    Self
}

public enum EffectType
{
    Damage,
    Heal
}

public class EffectMgr : MonoBehaviour
{
    //public void Effect()
    //{
    //    if(Enum.TryParse(, true, out EffectType type))
    //    {
    //        switch (type)
    //        {
    //            case EffectType.Damage:
    //                Damage();
    //                break;

    //            case EffectType.Heal:
    //                Heal();
    //                break;

    //        }
    //    }
    //}
}
