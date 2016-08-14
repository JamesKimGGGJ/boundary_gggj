using UnityEngine;
using UnityEngine.UI;

public class HUDPlayerItem : MonoBehaviour
{
    public Image frame;
    public Image icon;

    public static Color GetColorByPlayerOrder(int playerOrder)
    {
        switch (playerOrder)
        {
            case 1: return Color.red;
            case 2: return Color.blue;
            case 3: return Color.yellow;
            case 4: return Color.green;
            default:
                Debug.LogWarning("undefined playerOrder: " + playerOrder);
                return Color.white;
        }
    }

    public void SetColorWithPlayerOrder(int playerOrder)
    {
        frame.color = GetColorByPlayerOrder(playerOrder);
    }

    public void SetIcon(ItemType itemType)
    {
        icon.enabled = true;
        var imgIcon = Resources.Load<Sprite>("ItemIcon/" + itemType);
        icon.sprite = imgIcon;
    }

    public void ResetIcon()
    {
        icon.enabled = false;
    }
}
