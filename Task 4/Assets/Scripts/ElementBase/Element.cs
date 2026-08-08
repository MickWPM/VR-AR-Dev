using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField] private ElementSO element;
    public ElementSO ElementType { get { return element; } }

    public void DestroyElement()
    {
        Destroy(gameObject);
    }
}
