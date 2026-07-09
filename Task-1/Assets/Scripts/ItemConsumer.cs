using UnityEngine;

public class ItemConsumer : MonoBehaviour
{
    public ConveyerItem.ItemType consumedItem = ConveyerItem.ItemType.None;
    public bool deleteOnWrong = false;
    private void OnTriggerEnter(Collider other)
    {
        ConveyerItem item = other.GetComponentInParent<ConveyerItem>();
        if (item == null) return;

        if (consumedItem == ConveyerItem.ItemType.Any || item.itemType == consumedItem)
        {
            ConsumeItem(item);
            return;
        }

        if (deleteOnWrong)
            DeleteObject(item);
    }

    public event System.Action<ConveyerItem> ItemDestroyedEvent;
    private void DeleteObject(ConveyerItem item)
    {
        ItemDestroyedEvent?.Invoke(item);
        Destroy(item.gameObject);   
    }

    public event System.Action<ConveyerItem> ItemConsumedEvent;
    private void ConsumeItem(ConveyerItem item)
    {
        ItemConsumedEvent?.Invoke(item);
    }
}
