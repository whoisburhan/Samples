using Project.Framework.Graphs;
using Sirenix.OdinInspector;

namespace Project.Game.Flows.Transitions
{
    public struct NavSelectLeftButton : IConsumableRequest
    {
        [ShowInInspector] bool IConsumableRequest.Invoked { get; set; }
        [ShowInInspector] bool IConsumableRequest.Consumed { get; set; }      
    }
}