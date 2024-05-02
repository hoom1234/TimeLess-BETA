using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    public SO_item EMPTY_ITEM;
    public Transform slotPrefab;
    public Transform InventoryPanel;
    protected GridLayoutGroup gridLayoutGroup;
    [Space(5)]
    public int SlotAmount = 10;
    public InventorySlot[] inventorySlots;

    [Header("Mini canvas")]
    public RectTransform miniCanvas;
    [SerializeField] protected InventorySlot rightClickslot;

    void Start() // Corrected method name
    {
        gridLayoutGroup = InventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorySlots(); // Call the method to create slots when the script starts
    }

    #region Inventory Methods

    public void AddItem(SO_item item, int amount)
    {
        InventorySlot slot = IsEmptySlotLeft(item);
        if(slot == null)
        {
            //full
            DropItem(item , amount);
            return;
        }
        slot.MergethisSlot(item , amount);
    }

    public void UseItem() //Onclick Events
    {
        rightClickslot.UseItem();
        OnFinishMiniCanvas();
    }
    public void DestroyItem()//Onclick Events
        {
            rightClickslot.SetThislot(EMPTY_ITEM, 0);
            OnFinishMiniCanvas();
    } 
    public void DropItem() //Onclick Events
    {
        ItemSpawner.Instance.SpawnItem(rightClickslot.item, rightClickslot.stack);
        DestroyItem();

    } 

    public void DropItem(SO_item item, int amount)
    {
        ItemSpawner.Instance.SpawnItem(item, amount);


    }





    public void RemoveItem(InventorySlot slot)
    {
        slot.SetThislot(EMPTY_ITEM, 0);
    }

    public void SortItem(bool Ascending = true)
    {

    }

    public void CreateInventorySlots()
    {
        inventorySlots = new InventorySlot[SlotAmount];
        for (int i = 0; i < SlotAmount; i++)
        {
            Transform slot = Instantiate(slotPrefab, InventoryPanel);
            InventorySlot invSlot = slot.GetComponent<InventorySlot>();

            inventorySlots[i] = invSlot;
            invSlot.inventory = this;
            invSlot.SetThislot(EMPTY_ITEM, 0);
        }
    }

    public InventorySlot IsEmptySlotLeft(SO_item itemChecker = null, InventorySlot itemSlot = null)
    {
        InventorySlot firstEmptySlot = null;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot == itemSlot)
                continue;

            if (slot.item == itemChecker)
            {
                if (slot.stack < slot.item.maxStack)
                {
                    return slot;
                }
            }
            else if (slot.item == EMPTY_ITEM && firstEmptySlot == null)
                firstEmptySlot = slot;
        }
        return firstEmptySlot;
    }

    public void SetLayoutCotrolChild(bool isControlled)
    {
        gridLayoutGroup.enabled = isControlled;
    }
    #endregion



    #region
    public void SetRightClicksSlot(InventorySlot slot)
    {
        rightClickslot = slot;
    }

    public void OpenMiniCanvas(Vector2 clickPosition)
    {
        miniCanvas.position = clickPosition;
        miniCanvas.gameObject.SetActive(true);
    }

    public void OnFinishMiniCanvas()
    {
        rightClickslot = null;
        miniCanvas.gameObject.SetActive(false);
    }

    #endregion



}

