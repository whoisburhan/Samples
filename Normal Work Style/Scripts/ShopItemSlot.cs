using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GS.FanstayWorld2D.UI
{
    public class ShopItemSlot : Selectable, ISelectHandler, IDeselectHandler
    {
        [Header("Item Data")]
        private ItemData itemData;
        private int shopItemSlotIndex;
        [SerializeField] private Image itemImg;

        private IPanel uIPanel;

        protected override void Awake()
        {
            base.Awake();
            uIPanel = GetComponent<IPanel>();
            itemImg = transform.GetChild(0).GetComponent<Image>();
            // if (uIPanel == null)
            //     uIPanel = this.gameObject.AddComponent<IPanel>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                uIPanel.HidePanel();
            if (Input.GetKeyDown(KeyCode.V))
                uIPanel.ShowPanel();
        }

        public void Init(ItemData itemData, int index, int itemsState)
        {
            shopItemSlotIndex = index;
            if (itemData != null)
            {
                this.itemData = itemData;
                itemImg.sprite = itemData.generalInfo.displayImg;
                itemData.shopData.itemStatus = (ItemStatus)itemsState;
                uIPanel.ShowPanel();
            }
            else
            {
                uIPanel.HidePanel();
            }
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            Debug.Log(this.gameObject.name + " was Deselected");
        }

        //Do this when the selectable UI object is selected.
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            Debug.Log(this.gameObject.name + " was selected");

            if (itemData != null)
            {
                StoreUI.OnUpdateDetailsPanel?.Invoke(itemData, shopItemSlotIndex);
            }
        }

    }
}