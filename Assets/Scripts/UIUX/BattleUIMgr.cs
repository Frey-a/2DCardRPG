using UnityEngine;
using UnityEngine.UI;

public class BattleUIMgr : MonoBehaviour
{
    public Transform turnImgParent;
    public Transform allies; // 캐릭터 생성 위치
    public Transform enemies; // 적 생성 위치
    public Transform deck;
    public Transform hand;
    public Transform graveyard;

    public void UpdateCntByChildren(Transform trans)
    {
        trans.parent.GetComponentInChildren<Text>().text = trans.childCount.ToString();
    }

    public void CreateTurnImg()
    {
        GameObject turnImg = new GameObject("TurnImg", typeof(RectTransform), typeof(Image));
        turnImg.transform.SetParent(turnImgParent, false);

        Image img = turnImg.GetComponent<Image>();
        img.raycastTarget = false;
        //if(recentOrder.isEnemy)
        //{
        //    img.sprite = ;
        //}
        //else
        //{
        //    img.sprite = ;
        //}
    }

    public void UpdateTurnImg()
    {
        turnImgParent.GetChild(0).SetSiblingIndex(turnImgParent.childCount);
    }

    public void ActiveSlot(Transform slot, bool isActive)
    {
        Image img = slot.GetComponent<Image>();
        img.enabled = isActive;
        img.raycastTarget = isActive;
    }
}
