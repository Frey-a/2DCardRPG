using UnityEngine;
using UnityEngine.UI;

public class CharInfo : MonoBehaviour
{
    private CharData data;
    private CharStatus status;

    public Image img;
    public RectTransform hp;

    public void SetData(int charId)
    {
        data = InfoMgr.Instance.database.chars.Find(c => c.charId == charId);
        LoadStatus(charId);
    }

    public void LoadStatus(int charId)
    {
        CharStatus loadStatus = InfoMgr.Instance.LoadStatus(charId);

        if (loadStatus != null)
        {
            status = loadStatus;
        }
        else
        {
            status.charId = data.charId;
            status.hp = data.maxHp;
        }
    }

    public CharData GetData()
    {
        return data;
    }

    public CharStatus GetStatus()
    {
        return status;
    }

    public void SaveStatus()
    {

    }
}
