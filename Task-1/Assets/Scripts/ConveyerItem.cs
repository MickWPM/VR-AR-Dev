using UnityEngine;

public class ConveyerItem : MonoBehaviour
{
    public ItemType itemType = ItemType.Any;



    public enum ItemType
    {
        Any,
        Box,
        Pill,
        None
    }
}
