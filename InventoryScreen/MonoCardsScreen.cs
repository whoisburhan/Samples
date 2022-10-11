using System.Collections;
using System.Collections.Generic;
using Project.Framework.Graphs;
using Project.Framework.Pattern;
using Project.Game.Flows.Transitions;
using Project.Game.Presentation.Overlays;
using UnityEngine;
using Zenject;
using Project.Game.Presentation.Mocks;
using Sirenix.OdinInspector;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Game.Extensions;
using Project.Domain.Entities;
using Sirenix.Utilities;

namespace Project.Game.Overlays.Inventory
{
    public class MonoCardsScreen : MonoOverlayGraphImplementer, ICardsScreenGraphInterface, IRetainedPoolable<ICardGridDataSource>
    {
        #region Graph Interface
        public ISelectableGroupGraphInterface _cardsSelectablesGroup => CardsSelectionController;
        
        public NavSelectLeftButton _InvokeNavSelectLeftButtonStructure(SelectableInteractionStructure fromSelection)
        {
            throw new System.NotImplementedException();
        }

        public NavSelectRightButton _InvokeNavSelectRightButtonStructure(SelectableInteractionStructure fromSelection)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Poolable

        IMemoryPool IRetainablePoolableBase.Pool { get; set; }
        int IRetainablePoolableBase.RetainCount { get; set; }

        public void OnDespawned()
        {
            this.CardsSelectionController.AdditionalAction -= UpdateCardDetailsPanel;

            this.Mock.HideImmediate();

            this.DataSource.RemoveOnDataSourceChanged(OnSourceChanged);
            this.DataSource = null;
        }


        public void OnSpawned(ICardGridDataSource data)
        {
            this.Mock.HideImmediate();

            this.Mock.Title.Characters = titleName;
            //Sort Button work 
            //this.Mock.CardSortButton.Characters = "BY CHAPTER";
            
            SplitCardDataOnPages(data);


            VerticalSlideSetUp();   // Update Vertical Slide

            this.CardsSelectionController.AdditionalAction += UpdateCardDetailsPanel;
        }

        public void SetUp(ICardGridDataSource data)
        {
            this.Mock.HideImmediate();

            this.Mock.Title.Characters = titleName;
            //Sort Button work 
            //this.Mock.CardSortButton.Characters = "BY CHAPTER";
            
            SplitCardDataOnPages(data);


            VerticalSlideSetUp();   // Update Vertical Slide

            this.CardsSelectionController.AdditionalAction += UpdateCardDetailsPanel;
        }

        public class Factory : PlaceholderFactory<ICardGridDataSource, MonoCardsScreen>
        {
        }

        #endregion

        [ShowInInspector] private MockedCardsScreen Mock => this.GetComponentInChildren<MockedCardsScreen>(true);

        [ShowInInspector] private List<ICardGridDataSource> DataSourceList = new List<ICardGridDataSource>();
        [ShowInInspector] public ICardGridDataSource DataSource { get; set; }

        [Inject] private SelectableGroup CardsSelectionController { get; set; }
        [Inject] private CardScreenActions CardScreenActions { get; set; }

        [ShowInInspector] private ICardDetailsDataSource CardDetailsDataSource { get; set; }

        public static Action<ICardDetailsDataSource> UpdateCardDetailsAction;

        private string titleName = "INVENTORY SCREEN";

        private RectTransform verticalSlider;

        int totalPage, currentPage;
        int previousCardIndex = -1;

        float sliderHeight, sliderMinLimitOffset, sliderMaxLimitOffset;


        void Awake()
        {
            this.Mock.HideImmediate();

        }

        void OnEnable()
        {
            CardsSelectionController.AttachTo(this.Mock.CardGrid, CardScreenActions, this.Mock.CardGrid.Selectables);
        }

        void OnDisable()
        {
            CardsSelectionController.Detach();
        }

        private void OnSourceChanged()
        {
            //this.DataSource.RenderTo(this.Mock);
            this.DataSource.RenderTo(this.Mock);
        }

        private void SplitCardDataOnPages(ICardGridDataSource data)
        {
            data.FetchCardGridData();
            totalPage = (data.DatabaseCards.Count / this.Mock.CardGrid.Cards.Count) + 1;
            currentPage = 0;

            var cardSlots = this.Mock.CardGrid.Cards.Count;

            for (int i = 0; i < totalPage; i++)
            {
                var isCardSourceLimitCross = i * cardSlots + cardSlots > data.DatabaseCards.Count;

                var tempCardSourceList = data.DatabaseCards.GetRange(i * cardSlots, isCardSourceLimitCross ? data.DatabaseCards.Count - (i * cardSlots) : cardSlots);

                DataSourceList.Add(new NewCardGridDataSource(tempCardSourceList));
            }

            this.DataSource = data;
            this.DataSource.AddOnDataSourceChanged(OnSourceChanged);
            OnSourceChanged();

            if(data.DatabaseCards.Count > 0) UpdateCardDetailsFirstTime(data.DatabaseCards[0]);
            else HideCardDetails();
        }

        private int UpdateCardDetailsPanel(int cardSelectableIndex, int pageChange)
        {

            var isPageChanged = pageChange != 0;

            currentPage = pageChange == 0 ? currentPage : pageChange == 1 ? currentPage + 1 : currentPage - 1;

            if (currentPage >= totalPage)
            {
                currentPage = totalPage - 1;
                cardSelectableIndex = previousCardIndex;
                isPageChanged = false;
            }
            else if (currentPage < 0)
            {
                currentPage = 0;
                cardSelectableIndex = previousCardIndex;
                isPageChanged = false;
            }
            // currentPage = currentPage >= totalPage ? 0 : currentPage < 0 ? totalPage - 1 : currentPage;

            if (currentPage == totalPage - 1 && cardSelectableIndex >= DataSourceList[currentPage].DatabaseCards.Count)
            {
                cardSelectableIndex = previousCardIndex;
                //CardsSelectionController.AsSequencedSelectables()._ActiveSelectable = Mock.CardGrid.Selectables[cardSelectableIndex];
            }



            if (isPageChanged)
            {
                this.DataSourceList[currentPage].RenderTo(this.Mock);

                UpdateVerticalSliderPosition(); // Oppsoite Direction As in 2D Cordinate Up is negative and down in positive
            }


            var cardData = this.Mock.CardGrid.GameCards[cardSelectableIndex];

            cardData.CardFrameImage.color = Color.white;
            if (previousCardIndex != cardSelectableIndex)
            {
                this.Mock.CardGrid.GameCards[previousCardIndex].CardFrameImage.color = Color.grey;
            }

            ICardDetailsDataSource cardDetailData = new CardDetailsVisualDataSource(cardData);

            cardDetailData.RenderTo(this.Mock.CardDetail);

            previousCardIndex = cardSelectableIndex;

            return cardSelectableIndex;

        }

        private void UpdateCardDetailsFirstTime(ICardEntryDataSource cardData)
        {
            ICardDetailsDataSource cardDetailData = new CardDetailsVisualDataSource(cardData);

            cardDetailData.RenderTo(this.Mock.CardDetail);

            foreach (var card in this.Mock.CardGrid.GameCards)
            {
                card.CardFrameImage.color = Color.grey;       
            }
            
            this.Mock.CardGrid.GameCards[0].CardFrameImage.color = Color.white;
            previousCardIndex = 0; //help to track CardHighlights card index to remove that later
        }

        private void HideCardDetails()
        {
            this.Mock.CardDetail.gameObject.SetActive(false);
        }

        private void ShowCardDetails()
        {
            this.Mock.CardDetail.gameObject.SetActive(true);
        }

        /// Set Up Vertical Slider
        private void VerticalSlideSetUp()
        {
            verticalSlider = Mock.VerticalSlide.slider;
            sliderHeight = verticalSlider.rect.height / totalPage;
            verticalSlider.SetSize(new Vector2(verticalSlider.rect.width, sliderHeight));

            sliderMinLimitOffset = ((sliderHeight * totalPage) / 2) - sliderHeight / 2;
            sliderMaxLimitOffset = -((sliderHeight * totalPage) / 2) + sliderHeight / 2;
            
            verticalSlider.anchoredPosition = new Vector2(0, sliderMinLimitOffset);

        }

        /// Update Vertical Slide based on CardPage No
        private void UpdateVerticalSliderPosition()
        {
            verticalSlider.anchoredPosition= new Vector2(0, sliderMinLimitOffset - (currentPage) * sliderHeight);
        }
        #region  Private Classes
        private class CardDetailsVisualDataSource : ObservableDataSource, ICardDetailsDataSource
        {

            private string cardTitle;
            private string cardDescription;
            private string cardImgPath;
            private string cardFrameImgPath;
            private Sprite cardImage;
            private Sprite cardFrame;
            private int currentDurability;
            private string attackValue;
            private int maxDurability;
            private bool upgraded;
            private bool fortified;
            string ICardDetailsDataSource.CardTitle => cardTitle;

            string ICardDetailsDataSource.CardDescription => cardDescription;
            Sprite ICardDetailsDataSource.CardImage => cardImage;
            Sprite ICardDetailsDataSource.CardFrame => cardFrame;

            string ICardDetailsDataSource.CardImgPath => cardImgPath;

            string ICardDetailsDataSource.CardFrameImgPath => cardFrameImgPath;
            int ICardDetailsDataSource.CurrentDurability => currentDurability;
            int ICardDetailsDataSource.MaxDurability => maxDurability;
            string ICardDetailsDataSource.AttackValue => attackValue;
            bool ICardDetailsDataSource.Fortified => fortified;
            bool ICardDetailsDataSource.Upgraded => upgraded;

            public CardDetailsVisualDataSource(ICardEntryDataSource dataSource)
            {
                this.cardTitle = dataSource.CardTitle;
                this.cardDescription = dataSource.CardDescription;
                this.cardImgPath = dataSource.CardImgPath;
                this.currentDurability = dataSource.Durability;
                this.maxDurability = dataSource.MaxDurability;
                this.fortified = dataSource.Fortified;
                this.upgraded = dataSource.Upgraded;
                this.attackValue = dataSource.AttackText;
            }

            public CardDetailsVisualDataSource(ICardEntryDataTarget dataSource)
            {
                this.cardTitle = dataSource.CardTitle;
                this.cardDescription = dataSource.CardDescription;
                this.cardImage = dataSource.CardImage.sprite;
                this.currentDurability = dataSource.Durability;
                this.maxDurability = dataSource.MaxDurability;
                this.fortified = dataSource.Fortified;
                this.upgraded = dataSource.Upgraded;
                this.attackValue = dataSource.AttackField.Text.text;
            }
        }

        private class NewCardGridDataSource : ObservableDataSource, ICardGridDataSource
        {
            private List<ICardEntryDataSource> databaseCards;
            List<ICardEntryDataSource> ICardGridDataSource.DatabaseCards => databaseCards;
            public void FetchCardGridData()
            {
                throw new NotImplementedException();
            }

            public NewCardGridDataSource(List<ICardEntryDataSource> cards)
            {
                databaseCards = cards;
            }

        }
        #endregion
    }
}