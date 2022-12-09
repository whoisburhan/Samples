using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GS.FanstayWorld2D
{
    [System.Serializable]
    public class SaveState
    {
        public int[] Outfits = new int[4] { 2, 0, 0, 0 };

        public int[] Swords = new int[5] { 2, 0, 0, 0, 0 };
        public int[] Bow = new int[3] { 2, 0, 0 };
        public int[] Wand = new int[5] { 2, 0, 0, 0, 0 };
        public int[] Mermaid = new int[2] { 2, 0 };

        //----------------------------------------------------------------------------
        public int[] Balls = new int[10] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] Grounds = new int[8] { 2, 0, 0, 0, 0, 0, 0, 0 };

        public int[] Puds = new int[38] { 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int HighScore { get; set; }

        public int SelectedLanguage { get; set; }

        public int TotalCoin { get; set; }

    }

    public class SelectedStoreData
    {
        public ItemData Outfit;
        public ItemData Sword;
        public ItemData Bow;
        public ItemData Wand;
        public ItemData MermaidOutfit;
    }

    public class GameData : MonoBehaviour
    {
        public static GameData Instance { get; private set; }

        public static Action<SelectedStoreData> OnLoadData, OnSaveData;
        public SaveState State { get => state; set => state = value; }

        [SerializeField] public StoreData storeData;

        public int CurrentlySelectedBallIndex = 0;
        public int CurrentlySelectedFieldIndex = 0;
        public int CurrentlySelectedPudIndex = 2;
        [Space]
        public int CurrentlySelectedOutfitIndex = 0;
        public int CurrentlySelectedSwordIndex = 0;
        public int CurrentlySelectedBowIndex = 0;
        public int CurrentlySelectedWandIndex = 0;
        public int CurrentlySelectedMermaidOutfitIndex = 0;

        private bool updateShopItemStatusPermissible = true;
        private SelectedStoreData selectedStoreData = new SelectedStoreData();

        [Header("Logic")]

        [SerializeField] public string SaveFileName = "data.GS";
        private string saveFileName;
        [SerializeField] private bool loadOnStart = true;

        private SaveState state;
        private BinaryFormatter formatter;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            state = new SaveState();
            saveFileName = Application.persistentDataPath + "/" + SaveFileName;
            Debug.Log(saveFileName);
            formatter = new BinaryFormatter();
            Load();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) OnLoadData?.Invoke(selectedStoreData);
        }

        #region  File READ/WRITE
        public void Save()
        {
            //If there no previous state loaded, create a new one
            if (State == null)
            {
                State = new SaveState();
            }

            var file = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, State);
            file.Close();

            UpdateStoreData();
            //OnSaveData?.Invoke(selectedStoreData);
        }

        public void Load()
        {
            // Open a physical file, on your disk to hold the save


            try
            {
                // If we found the file, open and read it
                var file = new FileStream(saveFileName, FileMode.Open, FileAccess.Read);
                State = (SaveState)formatter.Deserialize(file);
                file.Close();
                SelectedItemFinder();

            }
            catch
            {
                Debug.Log("No file found, creating new entry...");
                state.TotalCoin = 2500;
                // UIManager.Instance.FirstTimeGameOn = true;
                Save();
            }
        }

        #endregion

        #region  UPDATE SAVE STATE

      
        // Used By Store to save / select
        public void UpdateSaveState(ItemData itemData, int slotNo)
        {
            switch (itemData.generalInfo.itemType)
            {
                case ItemType.Outfit:
                    state.Outfits[CurrentlySelectedOutfitIndex] = 1; // UnEquiping ------ 1
                    CurrentlySelectedOutfitIndex = slotNo;
                    state.Outfits[slotNo] = 2; // Equipe new ----- 2
                    break;
                case ItemType.Sword:
                    state.Swords[CurrentlySelectedSwordIndex] = 1;
                    CurrentlySelectedSwordIndex = slotNo;
                    state.Swords[slotNo] = 2;
                    break;
                case ItemType.Bow:
                    state.Bow[CurrentlySelectedBowIndex] = 1;
                    CurrentlySelectedBowIndex = slotNo;
                    state.Bow[slotNo] = 2;
                    break;
                case ItemType.Wand:
                    state.Wand[CurrentlySelectedWandIndex] = 1;
                    CurrentlySelectedWandIndex = slotNo;
                    state.Wand[slotNo] = 2;
                    break;
                case ItemType.Mermaid:
                    state.Mermaid[CurrentlySelectedMermaidOutfitIndex] = 1;
                    CurrentlySelectedMermaidOutfitIndex = slotNo;
                    state.Mermaid[slotNo] = 2;
                    break;
            }

            Save(); // Save data into the file, A costly function
        }

        #endregion
        private void SelectedItemFinder()
        {
            CurrentlySelectedOutfitIndex = GetSelectedItemIndex(State.Outfits);
            CurrentlySelectedSwordIndex = GetSelectedItemIndex(State.Swords);
            CurrentlySelectedBowIndex = GetSelectedItemIndex(State.Bow);
            CurrentlySelectedWandIndex = GetSelectedItemIndex(State.Wand);
            CurrentlySelectedMermaidOutfitIndex = GetSelectedItemIndex(State.Mermaid);

            // Update For Once In A Game Session
            if(updateShopItemStatusPermissible)
            {
                UpdateShopItemStatus();
                updateShopItemStatusPermissible = false;
            }
            //

            UpdateStoreData();
        }

        private void UpdateShopItemStatus()
        {
            updateShopItemStatusFunc(storeData.Outfits,state.Outfits);
            updateShopItemStatusFunc(storeData.Swords,state.Swords);
            updateShopItemStatusFunc(storeData.Bows,state.Bow);
            updateShopItemStatusFunc(storeData.Wands,state.Wand);
            //updateShopItemStatusFunc(storeData.Mermaids,state.Mermaid);
        }
        private void updateShopItemStatusFunc(List<ItemData> itemDatas, int[] saveStatusArr)
        {
            for(int i = 0; i < itemDatas.Count; i++)
            {
                itemDatas[i].shopData.itemStatus = (ItemStatus) saveStatusArr[i];
            }
        }
        private int GetSelectedItemIndex(int[] itemList)
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i] == 2)
                    return i;
            }

            return -1; // Not selected anythings ; Used for locked Assets;
        }

        private void UpdateStoreData()
        {
            selectedStoreData.Outfit = CurrentlySelectedOutfitIndex == -1 ? null : storeData.Outfits[CurrentlySelectedOutfitIndex];
            selectedStoreData.Sword = CurrentlySelectedSwordIndex == -1 ? null : storeData.Swords[CurrentlySelectedSwordIndex];
            selectedStoreData.Bow = CurrentlySelectedBowIndex == -1 ? null : storeData.Bows[CurrentlySelectedBowIndex];
            selectedStoreData.Wand = CurrentlySelectedWandIndex == -1 ? null : storeData.Wands[CurrentlySelectedWandIndex];
            // selectedStoreData.MermaidOutfit = CurrentlySelectedMermaidOutfitIndex == -1 ? null : storeData.Mermaids[CurrentlySelectedMermaidOutfitIndex];

            OnLoadData?.Invoke(selectedStoreData);
        }

    }
}