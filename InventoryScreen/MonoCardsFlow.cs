using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using Project.Framework.Graphs;
using Project.Framework.Pattern;
using Project.Game.Overlays.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Project.Game.Flows.Inventoryflow
{
    public class MonoCardsFlow : MonoGraphImplementer, ICardsFlowGraphInterface,
        IRetainedPoolable<ICardGridDataSource>
    {
        #region Graph Interface
        ICardsScreenGraphInterface ICardsFlowGraphInterface._CardsScreen => cardsScreen;
        public Status CardsFlowSetup()
        {
            if(cardsScreen == null) cardsScreen = cardsScreenFactory.Create(dataSource);
            else cardsScreen.SetUp(dataSource);
            return Status.Success;
        }

        #endregion

        #region Poolable
        
        IMemoryPool IRetainablePoolableBase.Pool { get; set; }
        int IRetainablePoolableBase.RetainCount { get; set; }

        private ICardGridDataSource dataSource;
        public void OnSpawned(ICardGridDataSource cardData)
        {
            /*Debug.Log($"CHECK MonoCardsFLow OnSpawned IcardGridDataSource is null? {cardData == null}");
            this.cardsScreen = this.cardsScreenFactory.Create(cardData);
            dataSource = cardData;*/
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"AAA is CardScreen null ? {this.cardsScreen == null}");
            }
        }

        public void OnDespawned()
        {
            this.cardsScreen.NullSafeDispose();
            this.cardsScreen = null;
        }

        public class Factory : PlaceholderFactory<ICardGridDataSource, MonoCardsFlow>
        {}

        #endregion


        [ShowInInspector] MonoCardsScreen cardsScreen;
        
        private MonoCardsScreen.Factory cardsScreenFactory;

        [Inject]
        public void Construct(MonoCardsScreen.Factory cardsScreenFactory,
            ICardGridDataSource dataSource)
        {
           
            this.cardsScreenFactory = cardsScreenFactory;
            this.dataSource = dataSource;
        }
    }
}