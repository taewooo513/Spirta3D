using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;
    public Transform dropPosition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.player.addItem += AddItem;
        controller = CharacterManager.Instance.player.controller;
        condition = CharacterManager.Instance.player.condition;
        controller.inventory += Toggle;
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];
        dropPosition = CharacterManager.Instance.player.dropPosition;
        for (int i = 0; i < slotPanel.childCount; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            Debug.Log("여기인가");
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool isOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.player.itemData;

        if (data.isStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                CharacterManager.Instance.player.itemData = null;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.player.itemData = null;
            return;
        }
        ThrowItem(data);
        CharacterManager.Instance.player.itemData = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].quantity < data.maxStackAmount && slots[i].item == data)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null)
        {
            return;
        }
        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + '\n';
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + '\n';
        }
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }
    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {

            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
            ActionItem();
            RemoveSelectedItem();
        }
    }
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();

    }
    void ActionItem()
    {
        for (int j = 0; j < selectedItem.action.Length; j++)
        {
            selectedItem.action[j].Invoke();
        }
    }
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
            controller.moveSpeed -= slots[curEquipIndex].item.stat;
            controller.runSpeed -= slots[curEquipIndex].item.stat;
        }
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        controller.moveSpeed += slots[curEquipIndex].item.stat;
        controller.runSpeed += slots[curEquipIndex].item.stat;
        CharacterManager.Instance.player.equip.EquipNew(selectedItem);
        UpdateUI();
        SelectItem(selectedItemIndex);
    }
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }
    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

}
