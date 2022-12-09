using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using UnityEngine;
using GS.FanstayWorld2D.Projectile;

namespace GS.FanstayWorld2D
{
    [CreateAssetMenu(fileName = "ItemData - 0", menuName = "GS/Player Item", order = 2)]
    public class ItemData : ScriptableObject
    {
        public GeneralInfo generalInfo;
        public ShopData shopData;
        public GameBenifits gameBenifits;
        public CharacterViwerData characterViwerData;
    }

    [Serializable]
    public class GeneralInfo
    {
        public string Name;
        public Sprite displayImg;
        public Sprite protraitImg;
        public ItemType itemType;

    }

    [Serializable]
    public class CharacterViwerData
    {
        public string Path;
        public Color Color1;
        public Color Color2;
        public Color Color3;
    }

    [Serializable]
    public class ShopData
    {
        public int Price;
        public ItemStatus itemStatus;
    }

    [Serializable]
    public class GameBenifits
    {
        public int Attack;
        public ProjectileType projectileType;
    }

    [Serializable]
    public enum ItemStatus
    {
        NOT_OWNED = 0, OWNED, EQUIPED
    }

    [SerializeField]
    public enum ItemType
    {
        Outfit, Sword, Bow, Wand, Mermaid
    }

}