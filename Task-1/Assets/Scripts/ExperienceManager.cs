using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public int boxCount, pillCount;
    ItemConsumer[] itemConsumers;

    private void Awake()
    {
        itemConsumers = GameObject.FindObjectsByType<ItemConsumer>(FindObjectsSortMode.None);
        SubscribeToEvents();
    }



    private void Consumer_ItemConsumedEvent(ConveyerItem item)
    {
        switch (item.itemType)
        {
            case ConveyerItem.ItemType.Box:
                ++boxCount;
                break;
            case ConveyerItem.ItemType.Pill:
                ++pillCount;
                break;
            case ConveyerItem.ItemType.Any:
            case ConveyerItem.ItemType.None:
                break;
            default:
                break;
        }

        Destroy(item.gameObject);
    }


    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        foreach (ItemConsumer consumer in itemConsumers)
        {
            consumer.ItemConsumedEvent += Consumer_ItemConsumedEvent;
        }
    }

    private void UnsubscribeToEvents()
    {
        foreach (ItemConsumer consumer in itemConsumers)
        {
            consumer.ItemConsumedEvent -= Consumer_ItemConsumedEvent;
        }
    }

}
