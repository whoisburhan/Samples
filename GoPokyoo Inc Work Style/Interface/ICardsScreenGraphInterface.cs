using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Framework.Presentation;
using Project.Game.Flows.Transitions;
using Project.Game.Presentation.Overlays;

namespace Project.Game.Overlays.Inventory
{
    
    public interface ICardsScreenGraphInterface : IOverlayGraphInterface
    {
        ICardsScreenGraphInterface Self => this;
        
        /// <summary>
        /// The inner
        /// </summary>
        ISelectableGroupGraphInterface _cardsSelectablesGroup { get; }
        NavSelectLeftButton _InvokeNavSelectLeftButtonStructure(SelectableInteractionStructure fromSelection);
        NavSelectRightButton _InvokeNavSelectRightButtonStructure(SelectableInteractionStructure fromSelection);
        
        
    }
}