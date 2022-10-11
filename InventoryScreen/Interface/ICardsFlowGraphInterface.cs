using NodeCanvas.Framework;
using Project.Framework.Graphs;
using Project.Game.Overlays.Confirmation;
using Project.Game.Overlays.Inventory;

namespace Project.Game.Flows.Inventoryflow
{
    public interface ICardsFlowGraphInterface : IGraphInterface
    {
        /// <summary>
        /// Self strategy.  We bind this to ourselves as default.
        /// </summary>
        ICardsFlowGraphInterface Self => this;
        
        ICardsScreenGraphInterface _CardsScreen { get; }

        Status CardsFlowSetup();

    }
}
