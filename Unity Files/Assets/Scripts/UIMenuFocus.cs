using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenuFocus : MonoBehaviour
{
    public GameObject firstSelected;

    // this script helps the EventSystem decide which element to hover automatically
    // essential for controller UI
    private void OnEnable()
    {
        // clears old selection
        EventSystem.current.SetSelectedGameObject(null);

        // sets new focus for this menu
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}