using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIMgr : MonoBehaviour
{
    public Transform turnImgParent;
    public Transform allies; // 캐릭터 생성 위치
    public Transform enemies; // 적 생성 위치

    public void UpdateStatus(CharInfo info)
    {
        Text hpTxt = info.hp.GetComponentInChildren<Text>();
        hpTxt.text = info.GetStatus().hp + " / " + info.GetData().maxHp;
    }

    public void ChgImgAlpha()
    {
        List<Image> activeImges = new List<Image>();
        int imgCnt = 4; // 보여지는 최대 갯수

        foreach (Transform turnImg in turnImgParent)
        {
            if (turnImg.gameObject.activeSelf)
            {
                Image img = turnImg.GetComponent<Image>();

                if (img != null)
                {
                    activeImges.Add(img);
                }
            }
        }

        if (imgCnt > activeImges.Count)
        {
            imgCnt = activeImges.Count;
        }

        for (int i = 0; i < imgCnt; i++)
        {
            float alpha = Mathf.Lerp(1f, 0.1f, i * 0.3f);
            Image img = activeImges[i];

            if (img != null)
            {
                Color color = img.color;
                color.a = alpha;
                img.color = color;
            }
        }
    }
}
