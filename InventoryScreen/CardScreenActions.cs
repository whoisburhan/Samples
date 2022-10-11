namespace Project.Game.Presentation.Overlays
{
    public class CardScreenActions: ISelectableGroupActions
    {
        string ISelectableGroupActions.UpAction => "UIUp";

        string ISelectableGroupActions.DownAction => "UIDown";

        string ISelectableGroupActions.LeftAction => "UILeft";

        string ISelectableGroupActions.RightAction => "UIRight";

        string ISelectableGroupActions.SubmitAction => "UISubmit";

        string ISelectableGroupActions.AltAction1 => "InventoryLeft";

        string ISelectableGroupActions.AltAction2 => "InventoryRight";

        string ISelectableGroupActions.AltAction3 => "CardSort";

        string ISelectableGroupActions.BackAction => "Cancel";
    }
}
