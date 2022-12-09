using System.Collections.Generic;
using Game.Gui;
using Game.Gui.Enumerations;
using NPOI.SS.Formula.Functions;
using Project.Domain.Entities;
using Project.Framework.Pattern;
using Project.Framework.UI;
using Zenject;
using UnityEngine.UI;
using UnityEngine;
using Game.Gui.Utilities;
using Project.Game.Presentation.Mocks;

namespace Project.Game.Overlays.Inventory
{
    public enum CardVisibilityState
    {
        Show, Hide
    }
    public interface ICardGridDataSource : IObservableDataSource<ICardGridDataTarget>
    {
        List<ICardEntryDataSource> DatabaseCards { get; }

        void IRenderableDataSource<ICardGridDataTarget>.RenderTo(ICardGridDataTarget target)
        {

            if (DatabaseCards.Count >= 0 && DatabaseCards.Count <= target.Cards.Count)
            {
                // for (int x = 0; x < DatabaseCards.Count; x++)
                // {
                //     DatabaseCards[x].RenderTo(target.Cards[x]);
                // }

                for (int x = 0; x < target.Cards.Count; x++)
                {
                    if (x < DatabaseCards.Count)
                    {
                        target.Cards[x].CardVisibilityState = CardVisibilityState.Show;
                        DatabaseCards[x].RenderTo(target.Cards[x]);
                    }
                    else
                    {
                        target.Cards[x].CardVisibilityState = CardVisibilityState.Hide;
                    }
                }
            }
            else
            {
                Debug.LogError($"Not enough Cards slot for the cards... Card Amount: {DatabaseCards.Count}");
            }
        }

        public void FetchCardGridData();
        
        public class Factory : PlaceholderFactory<ICardGridDataSource>
        {

        }
    }

    public interface ICardGridDataTarget : IMonoDataTarget
    {

        public List<ICardEntryDataTarget> Cards { get; }

        public int TotalPage { get; set; }

        // public abstract void CardGenerator(int cardsAmount);

    }

    public interface ICardEntryDataSource : IObservableDataSource<ICardEntryDataTarget>
    {

        public string CardTitle { get; }
        public string CardDescription { get; }
        public string CardImgPath { get; }
        public string CardFrameImagePath { get; }
        public string CardBackerImagePath { get; }

        public string AttackBadgeImagePath { get; }
        public string AttackText { get; }
        public string RarityBadgeImagePath { get; }
        public string DurabilityBadgePath { get; }
        public int Durability { get; }
        public int MaxDurability { get; }

        public bool Upgraded { get; }
        public bool Fortified { get; }

        void IRenderableDataSource<ICardEntryDataTarget>.RenderTo(ICardEntryDataTarget target)
        {
            Debug.Log("ICardEntryDataSource : IObservableDataSource<ICardEntryDataTarget>");

            target.CardTitle = CardTitle;
            target.CardDescription = CardDescription;
            target.Durability = Durability;
            target.MaxDurability = MaxDurability;
            target.Upgraded = Upgraded;
            target.Fortified = Fortified;
            
            // Load Card AttackText
            if (!string.IsNullOrEmpty(AttackText))
            {
                target.AttackField.Characters = AttackText;
            }

            // Load CardImage
            if (!string.IsNullOrEmpty(CardImgPath))
            {
                TempUIFactory.FabricateCard(CardImgPath, (r) =>
                {
                    target.CardImage.sprite = r;
                });
            }

            // Load CardFrameImage
            if (!string.IsNullOrEmpty(CardFrameImagePath))
            {
                TempUIFactory.FabricateCard(CardFrameImagePath, (r) =>
                {
                    target.CardFrameImage.sprite = r;
                });
            }

            // Load CardBackerImage
            if (!string.IsNullOrEmpty(CardBackerImagePath))
            {
                TempUIFactory.FabricateCard(CardBackerImagePath, (r) =>
                {
                    target.CardBackerImage.sprite = r;
                });
            }

            // Load AttackBadgeImage
            if (!string.IsNullOrEmpty(AttackBadgeImagePath))
            {
                TempUIFactory.FabricateCard(AttackBadgeImagePath, (r) =>
                {
                    target.AttackBadgeImage.sprite = r;
                });
            }

            // Load RarityBadgeImage
            if (!string.IsNullOrEmpty(RarityBadgeImagePath))
            {
                TempUIFactory.FabricateCard(RarityBadgeImagePath, (r) =>
                {
                    target.RarityBadgeImage.sprite = r;
                });
            }

            // Load DurabilityBadge
            if (!string.IsNullOrEmpty(DurabilityBadgePath))
            {
                TempUIFactory.FabricateCard(DurabilityBadgePath, (r) =>
                {
                    target.DurabilityBadge.sprite = r;
                });
            }

            if (Upgraded == true)
            {
                target.AttackBadgeImage.color = GameConstants.CardUI_UpgradedColor;
            }
            else
            {
                target.AttackBadgeImage.color = GameConstants.CardUI_DefaultColor;
            }
        }

        public class Factory : PlaceholderFactory<ICardEntryDataSource>
        {
        }
    }
    public interface ICardEntryDataTarget : IMonoDataTarget
    {
        public Image CardImage => this.FindAncestor<Image>("img_card_card", includeInactive: true);
        public Image CardFrameImage => this.FindAncestor<Image>("img_card_frame", includeInactive: true);
        public Image CardBackerImage => this.FindAncestor<Image>("img_card_backer", includeInactive: true);
        public Image AttackBadgeImage => this.FindAncestor<Image>("img_card_attack_badge", includeInactive: true);
        public GoField AttackField => this.FindAncestor<GoField>("txt_damage_badge_field", includeInactive: true);
        public Image RarityBadgeImage => this.FindAncestor<Image>("img_card_rarity_badge", includeInactive: true);
        public Image DurabilityBadge => this.FindAncestor<Image>("invincible_badge", includeInactive: true);
        
        public int Durability { get; set; }
        public int MaxDurability { get; set; }
        public string CardTitle { get; set; }
        public string CardDescription { get; set; }
        public bool Upgraded { get; set; }
        public bool Fortified { get; set; }

        public CardVisibilityState CardVisibilityState { get; set; }

        public void DisableCard()
        {
            CardImage.enabled = false;
            CardFrameImage.enabled = false;
            AttackBadgeImage.enabled = false;
            AttackField.Text.text = "";
            RarityBadgeImage.enabled = false;
            DurabilityBadge.enabled = false;
        }

        public void EnableCard()
        {
            CardImage.enabled = true;
            CardFrameImage.enabled = true;
            AttackBadgeImage.enabled = true;
            AttackField.Text.text = "1";
            RarityBadgeImage.enabled = true;
            DurabilityBadge.enabled = true;
        }
    }

    #region  Card Details
    public interface ICardDetailsDataSource : IObservableDataSource<ICardDetailDataTarget>
    {

        public string CardTitle { get; }
        public string CardDescription { get; }
        public Sprite CardImage { get; }

        public Sprite CardFrame { get; }

        public string CardImgPath { get; }

        public string CardFrameImgPath { get; }

        public int CurrentDurability { get; }
        public int MaxDurability { get; }
        public string AttackValue { get; }

        public bool Fortified { get; }
        public bool Upgraded { get; }

        void IRenderableDataSource<ICardDetailDataTarget>.RenderTo(ICardDetailDataTarget target)
        {
            // Update Card Title
            target.CardTitle.Characters = CardTitle;

            // Update Card Descriptions
            target.CardDescription.Characters = CardDescription;

            // Load CardImage
            if (CardImage != null)
            {
                target.CardImage.sprite = CardImage;
            }

            if (!string.IsNullOrEmpty(CardImgPath))
            {
                TempUIFactory.FabricateCard(CardImgPath, (r) =>
                {
                    target.CardImage.sprite = r;
                });
            }


            // Load CardFrame
            if (CardFrame != null)
            {
                target.CardFrame.sprite = CardFrame;
            }

            if (!string.IsNullOrEmpty(CardFrameImgPath))
            {
                TempUIFactory.FabricateCard(CardFrameImgPath, (r) =>
                {
                    target.CardFrame.sprite = r;
                });
            }

            target.DurabilityBar.UpdateStatusBar(CurrentDurability,MaxDurability,Fortified);

            target.AttackField.Text.text = AttackValue;

            if (Upgraded)
            {
                target.AttackBadgeImage.color  = GameConstants.CardUI_UpgradedColor;
            }
            else
            {
                target.AttackBadgeImage.color  = GameConstants.CardUI_DefaultColor;
            }

        }

        public class Factory : PlaceholderFactory<ICardDetailDataTarget>
        {
        }
    }

    public interface ICardDetailDataTarget : IMonoDataTarget
    {
        public GoText CardTitle => this.FindAncestor<GoText>("txt_title", includeInactive: true);
        public GoText CardDescription => this.FindAncestor<GoText>("txt_description", includeInactive: true);
        public Image CardImage => this.FindAncestor<Image>("img_cardelement", includeInactive: true);
        public Image CardFrame => this.FindAncestor<Image>("img_cardframe", includeInactive: true);
        public Image CardBackerImage => this.FindAncestor<Image>("img_card_backer", includeInactive: true);
        public Image AttackBadgeImage => this.FindAncestor<Image>("img_card_attack_badge", includeInactive: true);
        public GoField AttackField => this.FindAncestor<GoField>("txt_damage_badge_field", includeInactive: true);

        public MockedStatusBar DurabilityBar { get; }

    }
    #endregion
}