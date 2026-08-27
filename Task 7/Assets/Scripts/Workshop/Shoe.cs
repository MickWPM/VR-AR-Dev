using UnityEngine;

public class Shoe : MonoBehaviour
{
    public GameObject shoePrefab;

    private void Start()
    {
        AddShoe(this.transform);
    }

    public void AddShoe(Transform t)
    {
        if (t.childCount == 0)
        {
            Debug.Log($"Shoe added to {t.gameObject.name}", t.gameObject);
            Instantiate(shoePrefab, t.position, Quaternion.identity, t);
            return;
        }

        for (int i = 0; i < t.childCount; i++)
        {
            AddShoe(t.GetChild(i));
        }
    }
}