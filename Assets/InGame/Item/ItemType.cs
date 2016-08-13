public enum ItemType
{
    None,
    Missle,
}

public static partial class ItemHelper
{
    public static ItemType Random()
    {
        var itemTypes = new[] { ItemType.Missle };
        return itemTypes[UnityEngine.Random.Range(0, itemTypes.Length)];
    }
}