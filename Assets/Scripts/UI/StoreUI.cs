using NaughtyAttributes;
using UnityEngine;

public class StoreUI : UIWindow
{
    # region Test Methods
    [Button("Test Show")]

    private void TestShow()
    {
        Show();
    }

    private void TestHide()
    {
        Hide();
    }
    #endregion
}
