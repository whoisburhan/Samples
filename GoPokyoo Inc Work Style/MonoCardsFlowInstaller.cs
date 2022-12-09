using System.Collections.Generic;
using Game.Providers;
using Project.Framework.Go;
using Project.Framework.Graphs;
using Project.Framework.Pattern;
using Project.Framework.Presentation;
using Project.Framework.UI;
using Project.Game.DataSource;
using Project.Game.Flows.Transitions;
using Project.Game.Overlays.Confirmation;
using Project.Game.Overlays.Profile;
using Project.Game.Overlays.Inventory;
using Project.Game.Presentation.Mocks;
using Project.Game.Presentation.Overlays;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Project.Domain.Entities;
using Project.Game.Flows.Profileflow;
using Project.Framework.HID;

namespace Project.Game.Flows.Inventoryflow
{
    #if UNITY_EDITOR
    /// <summary>
    /// For testing
    /// </summary>
    public class WorkspaceInitializer : InstallerInitializer, IInitializable, IEdtiorBlackboardBindSource
    {

        public class DummyCardGridDataSource : ObservableDataSource, ICardGridDataSource
        {
            private List<ICardEntryDataSource> databaseCards;

            private ICardEntryDataSource tempDataSource;
            List<ICardEntryDataSource> ICardGridDataSource.DatabaseCards => databaseCards;

            public DummyCardGridDataSource()
            {
                databaseCards = new List<ICardEntryDataSource>();

                var cardCount = DatabaseCard.CountEntities;

                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < cardCount; i++)
                    {
                        var card = DatabaseCard.GetEntity(i);

                        var tempDatabaseCardSource = new DummyCardEntryDataSource(card.CardName, card.Description, card.ImagePath, "", "", "", card.AttackValue.ToString(), "", "",0);

                        databaseCards.Add(tempDatabaseCardSource);

                        Debug.Log($"databaseCards ABC? {databaseCards[i].CardImgPath}");
                    }
                }

            }
            
            public void FetchCardGridData()
            {
                if (PlayerDataProvider.Instantiated == true)
                {
                    databaseCards = new List<ICardEntryDataSource>();
                    
                    var cardCount = PlayerDataProvider.Instance.PlayerCardBag.Deck.Count;

                    for (int i = 0; i < cardCount; i++)
                    {
                        var card = PlayerDataProvider.Instance.PlayerCardBag.Deck[i];
                        if (card.Durability > 0)
                        {
                            var tempDatabaseCardSource = new DummyCardEntryDataSource(card);

                            databaseCards.Add(tempDatabaseCardSource);

                            Debug.Log($"databaseCards ABC? {databaseCards[i].CardImgPath}");
                        }
                    }
                }
                else
                {
                    Debug.LogError("GridDataSource can't fetch data PlayerDataProvider is not initialized");
                }
            }

        }

        private class DummyCardEntryDataSource : ObservableDataSource, ICardEntryDataSource
        {

            private string cardTitle;
            private string cardDescription;
            private string cardImgPath;
            private string cardFrameImagePath;
            private string cardBackerImagePath;
            private string attackBadgeImagePath;
            private string attackText;
            private string rarityBadgeImagePath;
            private string durabilityBadgePath;
            private int durability;
            private int maxDurability = 10;

            private bool upgraded;
            private bool fortified;

            string ICardEntryDataSource.CardImgPath => cardImgPath;
            string ICardEntryDataSource.CardFrameImagePath => cardFrameImagePath;
            string ICardEntryDataSource.CardBackerImagePath => cardBackerImagePath;
            string ICardEntryDataSource.AttackBadgeImagePath => attackBadgeImagePath;
            string ICardEntryDataSource.AttackText => attackText;
            string ICardEntryDataSource.RarityBadgeImagePath => rarityBadgeImagePath;
            string ICardEntryDataSource.DurabilityBadgePath => durabilityBadgePath; 
            int ICardEntryDataSource.Durability => durability;
            int ICardEntryDataSource.MaxDurability => maxDurability;
            bool ICardEntryDataSource.Upgraded => upgraded;
            bool ICardEntryDataSource.Fortified => fortified;

            string ICardEntryDataSource.CardTitle => cardTitle;
            string ICardEntryDataSource.CardDescription => cardDescription;

            public DummyCardEntryDataSource(
                string cardTitle, string cardDescription,
                string cardImgPath = null, string cardFrameImagePath = null, string cardBackerImagePath = null,
                string attackBadgeImagePath = null, string attackText = null, string rarityBadgeImagePath = null,
                string durabilityBadgePath = null,
                int durability = 0,
                int maxDurability = 0)
            {
                this.cardTitle = cardTitle;
                this.cardDescription = cardDescription;
                this.cardImgPath = cardImgPath;
                this.cardFrameImagePath = cardFrameImagePath;
                this.cardBackerImagePath = cardBackerImagePath;
                this.attackBadgeImagePath = attackBadgeImagePath;
                this.attackText = attackText;
                this.rarityBadgeImagePath = rarityBadgeImagePath;
                this.durabilityBadgePath = durabilityBadgePath;
                this.durability = durability;
                this.maxDurability = maxDurability;
            }

            public DummyCardEntryDataSource(BaseCard baseCard)
            {
                this.cardTitle = baseCard.Name;
                this.cardDescription = baseCard.Description;
                this.cardImgPath = baseCard.imagePath;
                this.attackText = baseCard.attackValue.ToString();
                this.maxDurability = baseCard.MaxDurability;
                this.durability = baseCard.Durability;
                this.upgraded = baseCard.attackUpgraded;
                this.fortified = baseCard.fortified;
            }
        }


        public bool IsLeftButtonPressed => false;
        public bool IsRightButtonPressed => false;
        // public bool IsCreatePath => true;
        // public bool IsContinuePath => false;

        // public NavBackStructure NavBack;
        // public NavSelectProfileStructure NavSelectProfile;
        // public NavDeleteProfileStructure NavDeleteProfile;
        // public NavCreateProfileStructure NavCreateProfile;


        public override void Initialize()
        {
            Debug.Log("TEST");
            GameObject.FindObjectOfType<MonoCardsFlow>().OnSpawned(new DummyCardGridDataSource());
        }
    }
    
    #endif

    public class MonoCardsFlowInstaller : MonoInstaller
    {
        public MonoCardsScreen cardsScreenPrefab;

        public override void InstallBindings()
        {
            var g = new GameObject("CanvasRoot");
            var canvas = g.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var canvasScale = g.AddComponent<CanvasScaler>();
            canvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScale.referenceResolution = new Vector2(1920, 1080);

            Container.BindFactory<
                    ICardGridDataSource,
                    MonoCardsScreen,
                    MonoCardsScreen.Factory>()
                .FromMonoPoolableMemoryPool(x => x.WithInitialSize(1)
                    .FromComponentInNewPrefab(cardsScreenPrefab)
                    .UnderTransform(g.transform));

            Container.Bind<SelectableGroup>().To<CardsScreenSelectableGroupController>().AsTransient().WhenInjectedInto<MonoCardsScreen>();
            Container.Bind<CardScreenActions>().AsSingle().IfNotBound();
            
            Container.Bind<MonoOverlayGraphImplementer>().To<MonoCardsScreen>().AsCached().IfNotBound();
            
            Container.Bind<ICardGridDataSource>().To<WorkspaceInitializer.DummyCardGridDataSource>()
                .FromInstance(new WorkspaceInitializer.DummyCardGridDataSource()).AsCached();
        }

    }
}
