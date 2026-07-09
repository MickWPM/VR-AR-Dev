using UnityEngine;

public class ItemConsumer : MonoBehaviour
{
    public ConveyerItem.ItemType consumedItem = ConveyerItem.ItemType.None;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"On trigger enter: {other.gameObject}");

        ConveyerItem item = other.GetComponentInParent<ConveyerItem>();
        if (item == null) return;
        if (consumedItem == ConveyerItem.ItemType.None) return;

        if (consumedItem == ConveyerItem.ItemType.Any || item.itemType == consumedItem)
            ConsumeItem(item);
    }

    public event System.Action<ConveyerItem> ItemConsumedEvent;
    private void ConsumeItem(ConveyerItem item)
    {
        ItemConsumedEvent?.Invoke(item);
    }
}
