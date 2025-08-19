using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public BattleUIMgr uiMgr;
    public DicePhaseMgr diceMgr;

    private void Start()
    {
        CreateScene();
    }

    private void CreateScene()
    {
        foreach(int charId in InfoMgr.Instance.getCharIds()) // ally
        {
            GameObject charSlot = Instantiate(InfoMgr.Instance.charPrefab, uiMgr.allies);
            Transform character = charSlot.transform.GetChild(0);
            CharInfo info = character.GetComponent<CharInfo>();

            diceMgr.CreateSlot(charId, false);
            info.SetData(charId);
            uiMgr.UpdateStatus(info);
        }

        //enemy 생성
    }

    private void StartBattle()
    {
        diceMgr.RollDice();
    }
}
