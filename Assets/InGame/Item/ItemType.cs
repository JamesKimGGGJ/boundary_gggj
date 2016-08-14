public enum ItemType
{
    None,
    Rocket,
    JetPack,
    ColumnDrop,
}

public static partial class ItemHelper
{
    public static ItemType Random()
    {
        var itemTypes = new[] { ItemType.Rocket };
        return itemTypes[UnityEngine.Random.Range(0, itemTypes.Length)];
    }
}