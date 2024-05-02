using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [Header("Inventory Detail")]
    public Inventory inventory;

    [Header("Slot Detail")]
    public SO_item item;
    public int stack;

    [Header("UI")]
    public Color emptyColor;
    public Color itemColor;
    public Image icon;
    public TextMeshProUGUI stacktext;

    [Header("Drag and Drop")]
    public int siblingIndex;
    public RectTransform draggable;
    Canvas canvas;
    CanvasGroup canvasGroup;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        siblingIndex = transform.GetSiblingIndex();
    }

    #region Drag and Drop Methods

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
        inventory.SetLayoutCotrolChild(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggable.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        draggable.anchoredPosition = Vector2.zero;
        transform.SetSiblingIndex(siblingIndex);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();
            if(slot != null)
                if(slot.item == item)
                {
                    //merge
                }
                else
                {
                    SwapSlot(slot);
                }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == inventory.EMPTY_ITEM)
                return;

            //inventory open mini canvas
            inventory.OpenMiniCanvas(eventData.position);
            inventory.SetRightClicksSlot(this);
        }
    }

    #endregion


    public void UseItem()
        {
        stack = Mathf.Clamp(stack - 1, 0, item.maxStack);
        if (stack > 0)
            CheckShowText();
        else
            inventory.RemoveItem(this);
        } 
    public void SwapSlot(InventorySlot newSlot)
    {
        SO_item keepItem;
        int keepstack;

        keepItem = item;
        keepstack = stack;

        SetSwap(newSlot.item, newSlot.stack);

        newSlot.SetSwap(keepItem, keepstack);
    }

    public void SetSwap(SO_item swapItem, int amount)
    {
        item = swapItem;
        stack = amount;
        icon.sprite = swapItem.icon;

        CheckShowText();
    }

    public void MergethisSlot(InventorySlot mergeSlot)
    {
        if (stack == item.maxStack || mergeSlot.stack == mergeSlot.item.maxStack)
        {
            SwapSlot(mergeSlot);
            return;
        }
        int ItemAmount = stack + mergeSlot.stack;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack);
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            mergeSlot.SetThislot(mergeSlot.item, amountLeft);
        }
        else
        {
            //remove
            inventory.RemoveItem(mergeSlot);
        }

            

    }

    public void MergethisSlot(SO_item mergeItme, int mergeamount)
    {
        item = mergeItme;
        icon.sprite = mergeItme.icon;

        int ItemAmount = stack + mergeamount;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack);
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = inventory.IsEmptySlotLeft(mergeItme, this);
            if(slot == null)
            {
                inventory.DropItem(mergeItme, amountLeft);
                return;
            }
            else
            {
               slot.MergethisSlot(mergeItme, amountLeft);
            }
        }
        



    }



    public void SetThislot(SO_item newItem, int amount)
    {
        item = newItem;
        icon.sprite = newItem.icon;

        int ItemAmount = amount;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, newItem.maxStack);
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if(amountLeft > 0)
        {

            InventorySlot slot = inventory.IsEmptySlotLeft(newItem, this); 
            if(slot == null)
            {
                //Drop Item
                return;
            } else
            {
                slot.SetThislot(newItem, amountLeft);
            }
        }
    }



    public void CheckShowText()
    {
        UpdateColorSlot();
        stacktext.text = stack.ToString();
        if(item.maxStack < 2)
        {
            stacktext.gameObject.SetActive(false);
        }
        else
        {
            if (stack > 1)
                stacktext.gameObject.SetActive(true);
            else
                stacktext.gameObject.SetActive(false);
        }
    }

  public void UpdateColorSlot()
    {
        if (item == inventory.EMPTY_ITEM)
            icon.color = emptyColor;
        else
            icon.color = itemColor;
    }

    
}
