using UnityEngine;
using UnityEngine.UI;

public class HUDPlayerItem : MonoBehaviour
{
    public Image frame;
    public Image icon;

    public static Color GetColorByPlayerOrder(int playerId)
    {
        switch (playerId)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.white;
            default:
                Debug.LogWarning("undefined playerOrder: " + playerId);
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
