using TMPro;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public int boxCount, pillCount, lostCount;
    ItemConsumer[] itemConsumers;

    private void Awake()
    {
        itemConsumers = GameObject.FindObjectsByType<ItemConsumer>(FindObjectsSortMode.None);
        SubscribeToEvents();
    }

    private void Start()
    {
        UpdateResultsText();
    }

    private void UpdateResultsText()
    {
        textMeshPro.text = GetUpdatedString();
    }

    public string GetUpdatedString()
    {
        return $"--- RESULTS---\nBoxes: {boxCount}\nPills: {pillCount}\n\n\nLOST: {lostCount}";
    }

    public TextMeshProUGUI textMeshPro;
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
        UpdateResultsText();
    }
    private void Consumer_ItemDestroyedEvent(ConveyerItem item)
    {
        ++lostCount;
        UpdateResultsText();
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
            consumer.ItemDestroyedEvent += Consumer_ItemDestroyedEvent;
        }
    }


    private void UnsubscribeToEvents()
    {
        foreach (ItemConsumer consumer in itemConsumers)
        {
            consumer.ItemConsumedEvent -= Consumer_ItemConsumedEvent;
            consumer.ItemDestroyedEvent -= Consumer_ItemDestroyedEvent;
        }
    }

}
