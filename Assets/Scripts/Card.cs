using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private BattleMgr battleMgr;
    private CardData data; // 정보
    private int idx; // hand index

    public int id;
    public Image img;
    public Text cardName;
    public Text description; // 효과 설명
    #region Exclusive
    public GameObject exclusive; // 전용 obj 부모
    public Image charImg;
    public Text charName;
    #endregion

    private void Start()
    {
        battleMgr = Object.FindFirstObjectByType<BattleMgr>();
    }

    public void SetData(int cardId)
    {
        data = InfoMgr.Instance.database.cards.Find(c => c.cardId == cardId);

        id = data.cardId;
        //img.sprite = 
        cardName.text = data.cardName;
        description.text = data.desc;

        if (!data.cardClass.Equals(""))
        {
            exclusive.SetActive(true);
            //charImg.sprite = 
            charName.text = data.cardClass;
        }
    }

    private void Hover(bool isActive)
    {
        battleMgr.uiMgr.ActiveCardInfo(isActive, transform);
    }

    private void Restore() // -> hand
    {
        transform.SetParent(battleMgr.uiMgr.hand);

        if (idx != transform.GetSiblingIndex()) // 제자리
        {
            transform.SetSiblingIndex(idx);
        }
    }

    private void ChkTargetType()
    {
        battleMgr.ActiveTarget(data.effectKey);
    }

    public void OnPointerEnter(PointerEventData eventData) => Hover(true);

    public void OnPointerExit(PointerEventData eventData) => Hover(false);

    public void OnPointerDown(PointerEventData eventData)
    {
        idx = transform.GetSiblingIndex();
        Hover(false);

        DragMgr.Instance.BeginDrag(GetComponent<RectTransform>());

        transform.SetParent(transform.parent.parent);
        transform.rotation = Quaternion.identity;

        ChkTargetType();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("CharSlot"))
            {
                //if (battleMgr.UseCard(effectData, result.gameObject.transform.GetChild(0)))
                //{
                //    transform.SetParent(battleMgr.uiMgr.graveyard); // hand -> trash
                //    battleMgr.uiMgr.UpdateCntByChildren(battleMgr.uiMgr.graveyard);
                //}
                //else
                //{
                //    Restore();
                //}
            }
            else
            {
                Restore();
            }
        }

        DragMgr.Instance.EndDrag();
        ChkTargetType();
    }
}
