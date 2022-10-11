using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Framework.HID;
using Project.Framework.Pattern;
using Project.Framework.UI;
using Zenject;
using Project.Game.Overlays.Inventory;
using Project.Domain.Entities;
using Project.Game.Presentation.Mocks;

namespace Project.Game.Presentation.Overlays
{
    public class CardsScreenSelectableGroupController : SelectableGroup,
        IDisposable
    {
        private const int CardGridWidth = 5;
        private List<IGoSelectable> selectables;
        private IRetainedPoolable<List<IGoSelectable>, IGoSelectable> retainedPoolableImplementation;


        public CardsScreenSelectableGroupController(InputService inputService) : base(inputService)
        {
        }


        protected override UniTask OnProcessSelection(IGoSelectable selection, CancellationToken token)
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask<IGoSelectable> OnProcessLeft(IGoSelectable from, CancellationToken token)
        {
            var condition = selectables.IndexOf(from) > 0;

            var i = condition ? selectables.IndexOf(from) - 1 : selectables.Count - 1;

            var pageChange = condition ? 0 : -1;

            i = AdditionalAction.Invoke(i, pageChange);

            return UniTask.FromResult(selectables[i]);


        }

        protected override UniTask<IGoSelectable> OnProcessRight(IGoSelectable from, CancellationToken token)
        {
            var condition = selectables.IndexOf(from) < selectables.Count - 1;

            var i = condition ? selectables.IndexOf(from) + 1 : 0;

            var pageChange = condition ? 0 : 1;

            i = AdditionalAction.Invoke(i, pageChange);

            return UniTask.FromResult(selectables[i]);
        }

        protected override UniTask<IGoSelectable> OnProcessUp(IGoSelectable from, CancellationToken token)
        {
            var condition = selectables.IndexOf(from) / CardGridWidth > 0;

            var i = condition ? selectables.IndexOf(from) - CardGridWidth : selectables.IndexOf(from) - CardGridWidth + selectables.Count;

            var pageChange = condition ? 0 : -1;

            i = AdditionalAction.Invoke(i, pageChange);

            return UniTask.FromResult(selectables[i]);
        }

        protected override UniTask<IGoSelectable> OnProcessDown(IGoSelectable from, CancellationToken token)
        {
            var condition = selectables.IndexOf(from) / CardGridWidth < (selectables.Count / CardGridWidth) - 1;

            var i = condition ? selectables.IndexOf(from) + CardGridWidth : (selectables.IndexOf(from) + CardGridWidth) - selectables.Count;

            var pageChange = condition ? 0 : 1;

            i = AdditionalAction.Invoke(i, pageChange);

            return UniTask.FromResult(selectables[i]);
        }

        protected override UniTask<IGoSelectable> OnProcessAction(IGoSelectable from, string actionName, CancellationToken token)
        {
            return UniTask.FromResult(from);
        }

        public override void AttachTo(MockedElement element, ISelectableGroupActions actions, List<IGoSelectable> selectables, IGoSelectable active = null)
        {
            base.AttachTo(element, actions, selectables, active);

            if (active == null) active = selectables.First();
            
            this.AsSequencedSelectables()._ActiveSelectable = active;
            this.selectables = selectables;
            
        }


        public void Dispose()
        {
        }
    }
}