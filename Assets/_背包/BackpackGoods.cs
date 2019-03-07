using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 背包示例
/// </summary>
public class BackpackGoods : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform rectTransform;
    Vector2 startPos;
    Image img;
    Color startColor;
    Transform[] childList;
    Transform melee;

    private void OnEnable()
    {
        rectTransform = transform as RectTransform;
        startPos = rectTransform.anchoredPosition;
        img = GetComponent<Image>();
        startColor = img.color;

        var parent = transform.parent;
        var num = parent.childCount;
        childList = new Transform[num];

        for (int i = 0; i < num; i++)
        {
            childList[i] = parent.GetChild(i);
        }
    }

    /// <summary>
    /// 清除跟近的物体
    /// </summary>
    private void ResetDistance()
    {
        if (melee != null)
        {
            melee.GetComponent<Image>().color = startColor;
            melee = null;
        }
    }

    /// <summary>
    /// 查询最近距离UI
    /// </summary>
    private void ClosestDistance()
    {
        ResetDistance();
        float f = float.MaxValue;
        for (int i = 0; i < childList.Length; i++)
        {
            var trans = childList[i];
            if (trans == transform)
            {
                continue;
            }

            var temp = Vector3.Distance(transform.position, trans.position);

            if (temp < f)
            {
                melee = trans;
                f = temp;
            }
        }

        if (melee != null)
        {
            var img1 = melee.GetComponent<Image>();
            img1.color = Color.yellow;
        }
    }

    private void FollowMouse(PointerEventData eventData)
    {
        if (eventData == null) return;

        if (eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
        {
            Vector3 globalMousePos;
            transform.SetAsLastSibling();
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rectTransform.position = globalMousePos;
            }
        }
    }

    /// <summary>
    /// 选中改变颜色
    /// </summary>
    /// <param name="color"></param>
    private void SelectedChangeColor(Color color)
    {
        if (img != null)
        {
            img.color = color;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ResetDistance();
        startPos = rectTransform.anchoredPosition;
        FollowMouse(eventData);
        SelectedChangeColor(Color.red);
    }

    public void OnDrag(PointerEventData eventData)
    {
        FollowMouse(eventData);
        SelectedChangeColor(Color.red);
        ClosestDistance();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (melee != null)
        {
            var txt1 = melee.GetChild(0).GetComponent<Text>();
            var txt2 = transform.GetChild(0).GetComponent<Text>();
            var str1 = txt1.text;
            var str2 = txt2.text;
            txt1.text = str2;
            txt2.text = str1;
        }
        ResetDistance();
        rectTransform.anchoredPosition = startPos;
        img.color = startColor;
    }
}
