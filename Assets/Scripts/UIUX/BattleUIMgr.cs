using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class BattleUIMgr : MonoBehaviour
{
    public Transform turnImgParent;
    public Transform allies; // 캐릭터 생성 위치
    public Transform enemies; // 적 생성 위치
    public Transform deck;
    public Transform hand;
    public Transform graveyard;
    public Transform cardInfo;
    public Transform actionPanel; // 연출UI

    public void UpdateCntByChildren(Transform trans)
    {
        trans.parent.GetComponentInChildren<Text>().text = trans.childCount.ToString();
    }

    public void CreateOrderImg((bool isEnemy, int id) order)
    {
        GameObject turnImg = new GameObject("TurnImg", typeof(RectTransform), typeof(Image));
        turnImg.transform.SetParent(turnImgParent, false);

        Image img = turnImg.GetComponent<Image>();
        img.raycastTarget = false;
        //if(order.isEnemy)
        //{
        //    img.sprite = ;
        //}
        //else
        //{
        //    img.sprite = InfoMgr.Instance.database;
        //}
    }

    public void UpdateOrderImg()
    {
        turnImgParent.GetChild(0).SetSiblingIndex(turnImgParent.childCount);
    }

    public void AdjustRatio(Image img)
    {
        Sprite sprite = img.sprite;
        float w = sprite.rect.width;
        float h = sprite.rect.height;
        float ratio = h / w;

        float targetW = img.rectTransform.sizeDelta.x;
        float targetH = targetW * ratio;

        img.rectTransform.sizeDelta = new Vector2(targetW, targetH);

        if (!img.transform.parent.CompareTag("CharSlot")) // ActionPanel 미적용
        {
            RectTransform imgTrans = img.rectTransform;
            RectTransform parentTrans = img.transform.parent.GetComponent<RectTransform>();
            Vector3 pos = imgTrans.anchoredPosition;

            float targetY = (imgTrans.sizeDelta.y - parentTrans.sizeDelta.y) / 2;

            if(targetY < 0) // 포지션을 targetUI랑 맞추기 위해 아래로 내림
            {
                pos.y = targetY;
            }
            else
            {
                pos.y = -targetY;
            }

            imgTrans.anchoredPosition = pos;
        }
    }

    public void ActiveCardInfo(bool isActive, Transform card)
    {
        if(isActive) // 최적화 필요
        {
            cardInfo.GetChild(0).GetComponent<Image>().sprite = card.GetComponent<Card>().img.sprite;
            cardInfo.GetChild(1).GetComponent<Text>().text = card.GetComponent<Card>().cardName.text;
            cardInfo.GetChild(2).GetComponent<Text>().text = card.GetComponent<Card>().description.text;
        }

        cardInfo.gameObject.SetActive(isActive);
    }

    public void ActiveSlot(Transform slot, bool isactive)
    {
        Image img = slot.GetChild(0).GetComponent<Image>();

        if(img.enabled != isactive)
        {
            img.enabled = isactive;
        }
    }

    public void ActiveAction((bool isEnemy, int id) recentOrder, Transform target)
    {
        // 이미지 전부 idle상태 -> 조금 대기 -> 공격자 스프라이트 -> 애니메이션 실행 -> 종료시 피격자 스프라이트 및 이펙트 -> 종료
        actionPanel.gameObject.SetActive(true);

        foreach (Transform area in actionPanel)
        {
            if(recentOrder.isEnemy)
            {
                if (area.CompareTag("Enemy"))
                {
                    string spriteKey;

                    //Addressables.LoadAssetAsync<Sprite>(spriteKey).Completed += (AsyncOperationHandle<Sprite> handle) =>
                    //{
                    //    if (handle.Status == AsyncOperationStatus.Succeeded)
                    //    {
                    //        targetImage.sprite = handle.Result;
                    //    }
                    //    else
                    //    {
                    //        Debug.LogError($"스프라이트 로드 실패: {spriteKey}");
                    //    }
                    //};
                }
            }
            else
            {
                if (area.CompareTag("Ally"))
                {

                }
            }
        }
    }

    public void PlayMoveArrow(bool toRight, bool enemyFront)
    {
        //// 방향 설정 (오른쪽 true, 왼쪽 false)
        //Vector3 allyScale = allyArrow.localScale;
        //Vector3 enemyScale = enemyArrow.localScale;

        //float dir = toRight ? 1f : -1f;
        //allyScale.x = Mathf.Abs(allyScale.x) * dir;
        //enemyScale.x = Mathf.Abs(enemyScale.x) * dir;

        //allyArrow.localScale = allyScale;
        //enemyArrow.localScale = enemyScale;

        //// 겹쳤을 때 순서
        //if (!enemyFront) // last X -> 2번째자식
        //    allyArrow.SetAsLastSibling();
        //else
        //    enemyArrow.SetAsLastSibling();

        //// 애니메이션 실행
        //animator.Play("ArrowMove", -1, 0f);
    }
}
