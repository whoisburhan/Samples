using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GS.FanstayWorld2D.Player;

namespace GS.FanstayWorld2D.UI
{
    public class StoreUI : UIPanel
    {
        public static Action<ItemData, int> OnUpdateDetailsPanel;

        #region Sub-Store Menu

        [Header("Sub-Store Menu Button")]
        [SerializeField] private Button outfitPanelBtn;
        [SerializeField] private Button swordPanelBtn, bowPanelBtn, wandPanelBtn, mermaidPanelBtn;

        #endregion
        #region Item Details
        [Header("Item Details")]
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text priceText;
        [SerializeField] private Text statusText;
        [SerializeField] private Text attackText;
        [Space]
        [SerializeField] private Image displayImg;
        [Space]
        [SerializeField] private Button buyOrEquipeButton;
        [SerializeField] private Image buyOrEquipeButtonImg;
        [SerializeField] private Text buyOrEquipeButtonText;
        [Space]
        [SerializeField] private Sprite greenButtonSprite;
        [SerializeField] private Sprite goldenButtonSprite;
        #endregion

        #region Item Slots

        [Header("ItemSlots")]
        private ShopItemSlot[] itemSlots;

        #endregion

        private int selectedItemSlot;
        private ItemData selectedItemData, equipedItemData;

        #region Override Unity Func

        protected override void OnAwakeCall()
        {
            base.OnAwakeCall();
            itemSlots = GetComponentsInChildren<ShopItemSlot>(true);
            InitSubStoreMenuBtns();
            InitBuyOrEquipeBtn();
        }
        protected override void OnStartCall()
        {
            base.OnStartCall();
        }

        protected override void OnEnableCall()
        {
            base.OnEnableCall();
            OnUpdateDetailsPanel += UpdateDetailsPanel;
        }

        protected override void OnDisableCall()
        {
            base.OnDisableCall();
            OnUpdateDetailsPanel -= UpdateDetailsPanel;
        }

        protected override void OnUpdateCall()
        {
            base.OnUpdateCall();

            // if (Input.GetKeyDown(KeyCode.Alpha1))
            // {
            //     UpdateShopSlots(GameData.Instance.storeData.Outfits);
            // }
            // if (Input.GetKeyDown(KeyCode.Alpha2))
            // {
            //     UpdateShopSlots(GameData.Instance.storeData.Swords);
            // }
            // if (Input.GetKeyDown(KeyCode.Alpha3))
            // {
            //     UpdateShopSlots(GameData.Instance.storeData.Bows);
            // }
            // if (Input.GetKeyDown(KeyCode.Alpha4))
            // {
            //     UpdateShopSlots(GameData.Instance.storeData.Wands);
            // }
        }

        protected override void OnShowPanel()
        {
            base.OnShowPanel();
            UpdateShopSlots(GameData.Instance.storeData.Outfits, GameData.Instance.State.Outfits, GameData.Instance.CurrentlySelectedOutfitIndex);
        }
        #endregion

        private void InitBuyOrEquipeBtn()
        {
            buyOrEquipeButton.onClick.AddListener(() => BuyOrEquipeButtonFunc());
        }

        private void BuyOrEquipeButtonFunc()
        {
            if (selectedItemData != equipedItemData)
            {
                // Here we decide either buy or equipe
                // For now lets skip the buy things
                // if(selectedItemData.shopData.itemStatus == ItemStatus.NOT_OWNED)
                // {
                //     // we try to buy it
                // }

                if (selectedItemData.shopData.itemStatus == ItemStatus.NOT_OWNED || selectedItemData.shopData.itemStatus == ItemStatus.OWNED)
                {
                    equipedItemData.shopData.itemStatus = ItemStatus.OWNED;
                    selectedItemData.shopData.itemStatus = ItemStatus.EQUIPED;
                    equipedItemData = selectedItemData;

                    UpdateDetailsPanel(selectedItemData, selectedItemSlot);
                    GameData.Instance.UpdateSaveState(selectedItemData, selectedItemSlot);
                }
            }
        }
        private void InitSubStoreMenuBtns()
        {
            outfitPanelBtn.onClick.AddListener(() => UpdateShopSlots(GameData.Instance.storeData.Outfits, GameData.Instance.State.Outfits, GameData.Instance.CurrentlySelectedOutfitIndex));
            swordPanelBtn.onClick.AddListener(() => UpdateShopSlots(GameData.Instance.storeData.Swords, GameData.Instance.State.Swords, GameData.Instance.CurrentlySelectedSwordIndex));
            bowPanelBtn.onClick.AddListener(() => UpdateShopSlots(GameData.Instance.storeData.Bows, GameData.Instance.State.Bow, GameData.Instance.CurrentlySelectedBowIndex));
            wandPanelBtn.onClick.AddListener(() => UpdateShopSlots(GameData.Instance.storeData.Wands, GameData.Instance.State.Wand, GameData.Instance.CurrentlySelectedWandIndex));
            mermaidPanelBtn.onClick.AddListener(() => UpdateShopSlots(GameData.Instance.storeData.Mermaids, GameData.Instance.State.Mermaid, GameData.Instance.CurrentlySelectedMermaidOutfitIndex));
        }

        #region  Item Details Func

        private void UpdateDetailsPanel(ItemData itemData, int slotNo)
        {
            selectedItemData = itemData;
            selectedItemSlot = slotNo;

            UpdateItemName(itemData.generalInfo.Name);
            UpdatePrice(itemData.shopData.Price);
            UpdateItemStatus(itemData.shopData.itemStatus);
            UpdateAttackText(itemData.gameBenifits.Attack);
            UpdateDisplayImg(itemData.generalInfo.displayImg);
            UpdateBuyOrEquipeButtonAppearance(itemData.shopData.itemStatus);
        }

        private void UpdateItemName(string itemName)
        {
            itemNameText.text = itemName;
        }
        private void UpdatePrice(int price)
        {
            priceText.text = $"PRICE : {price}";
        }

        private void UpdateItemStatus(ItemStatus itemStatus)
        {
            switch (itemStatus)
            {
                case ItemStatus.NOT_OWNED:
                    statusText.text = "NOT OWNED";
                    statusText.color = Color.red;
                    break;
                case ItemStatus.OWNED:
                    statusText.text = "OWNED";
                    statusText.color = Color.green;
                    break;
                case ItemStatus.EQUIPED:
                    statusText.text = "Equiped";
                    statusText.color = Color.green;
                    break;

            }
        }

        private void UpdateAttackText(int attackValue)
        {
            attackText.text = $"ATK : {attackValue}";
        }

        private void UpdateDisplayImg(Sprite display)
        {
            displayImg.sprite = display;
        }

        private void UpdateBuyOrEquipeButtonAppearance(ItemStatus itemStatus)
        {
            switch (itemStatus)
            {
                case ItemStatus.NOT_OWNED:
                    buyOrEquipeButtonImg.sprite = greenButtonSprite;
                    buyOrEquipeButtonText.text = "BUY";
                    break;

                case ItemStatus.OWNED:
                    buyOrEquipeButtonImg.sprite = greenButtonSprite;
                    buyOrEquipeButtonText.text = "EQUIP";
                    break;

                case ItemStatus.EQUIPED:
                    buyOrEquipeButtonImg.sprite = goldenButtonSprite;
                    buyOrEquipeButtonText.text = "EQUIPED";
                    break;
            }
        }
        #endregion


        #region  Update Shop Slots

        private void UpdateShopSlots(List<ItemData> items, int[] itemsState, int currentlyEqauipedItemIndex)
        {
            equipedItemData = items[currentlyEqauipedItemIndex];

            UpdateDetailsPanel(items[0], 0);    // By Default Show first Item from the Item Slot

            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].Init(items.Count > i ? items[i] : null, i, itemsState.Length > i ? itemsState[i] : 0);
            }
        }

        #endregion

    }
}