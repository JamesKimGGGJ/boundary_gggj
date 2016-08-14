using System;
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
        return (ItemType)UnityEngine.Random.Range(1,Enum.GetValues(typeof(ItemType)).Length);
    }
}