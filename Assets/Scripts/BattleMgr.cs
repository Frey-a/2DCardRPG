using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public BattleUIMgr uiMgr;
    public BattleOrderMgr orderMgr;

    private void Start()
    {
        CreateScene();
    }

    private void CreateScene()
    {
        foreach(int charId in InfoMgr.Instance.GetCharIds()) // ally
        {
            GameObject charSlot = Instantiate(InfoMgr.Instance.charPrefab, uiMgr.allies);
            Transform character = charSlot.transform.GetChild(0);
            CharInfo info = character.GetComponent<CharInfo>();

            orderMgr.CreateSlot(charId, false);
            //info.SetData(charId);
            //uiMgr.UpdateStatus(info);
        }

        //enemy 생성

        foreach(int cardId in InfoMgr.Instance.GetCardIds()) // card
        {
            GameObject card = Instantiate(InfoMgr.Instance.cardPrefab, uiMgr.deck);
            CardInfo info = card.GetComponent<CardInfo>();

            //info.SetData(cardId);
            uiMgr.UpdateCntByChildren(uiMgr.deck);
        }

        StartBattle();
    }

    private void StartBattle()
    {
        orderMgr.RollDice();
    }
}
