using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public event Action OnSpriteChangeFinished;

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

        Transform trans = order.isEnemy ? enemies : allies;
        foreach (Transform slot in trans)
        {
            if (slot.childCount < 1)
            {
                break;
            }

            if(order.isEnemy)
            {
                Monster monster = slot.GetComponentInChildren<Monster>();

                if (monster.id == order.id)
                {
                    _ = SetSprite(img, monster.spriteRoot + "Idle");
                }
            }
            else
            {
                PlayableChar character = slot.GetComponentInChildren<PlayableChar>();

                if (character.id == order.id)
                {
                    _ = SetSprite(img, character.spriteRoot + "Idle");
                }
            }
        }
    }

    public void UpdateOrderImg()
    {
        turnImgParent.GetChild(0).SetSiblingIndex(turnImgParent.childCount);
    }

    public void DelOrderImg(string spriteRoot)
    {
        foreach(Transform turnImg in turnImgParent)
        {
            Image img = turnImg.GetComponent<Image>();

            if(img.sprite.name.Contains(spriteRoot))
            {
                Destroy(turnImg.gameObject);
                break;
            }
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

    public async void ActiveAction(Transform attacker, List<Transform> targets) // spriteKey 받아야함
    {
        RectTransform attackerRectTrans = attacker.GetComponent<RectTransform>();
        Vector2 originSize = attackerRectTrans.sizeDelta; // Idle 크기 복구용
        Image attakerImg = attacker.GetChild(0).GetComponent<Image>();
        string spriteRoot = attacker.GetComponent<Character>().spriteRoot;

        await SetSprite(attakerImg, spriteRoot + "Attack");
        AdjustXRatio(attakerImg);

        List<RectTransform> targetRectTranses = new List<RectTransform>();
        List<Vector2> targetOriginSizes = new List<Vector2>();
        List<Image> targetImges = new List<Image>();
        List<string> spriteRoots = new List<string>();

        for (int i = 0; i < targets.Count; i++)
        {
            targetRectTranses.Add(targets[i].GetComponent<RectTransform>());
            targetOriginSizes.Add(targetRectTranses[i].sizeDelta);
            targetImges.Add(targets[i].GetChild(0).GetComponent<Image>());
            spriteRoots.Add(targets[i].GetComponent<Character>().spriteRoot);

            await SetSprite(targetImges[i], spriteRoots[i] + "Hit");
            AdjustXRatio(targetImges[i]);
        }

        await Task.Delay(1000); // 1초 대기

        attackerRectTrans.sizeDelta = originSize;
        await SetSprite(attakerImg, spriteRoot + "Idle");

        OnSpriteChangeFinished?.Invoke();

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                targetRectTranses[i].sizeDelta = targetOriginSizes[i];
                await SetSprite(targetImges[i], spriteRoots[i] + "Idle");
            }
        }
    }

    public async Task SetSprite(Image img, string spriteKey)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(spriteKey);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            img.sprite = handle.Result;
        }
        else
        {
            Debug.LogWarning($"[Addressables] Sprite Load 실패: {spriteKey}");
        }
    }

    private void AdjustXRatio(Image img)
    {
        RectTransform imgTrans = img.rectTransform;
        Vector2 originSize = imgTrans.sizeDelta;

        float ratioW = img.sprite.rect.width / 1000f;

        float targetW = originSize.x * ratioW;

        imgTrans.sizeDelta = new Vector2(targetW, originSize.y);
    }
}
