using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime;

    private void Awake()
    {
        destroyTime = Mathf.Max(0, destroyTime);
        Destroy(gameObject, destroyTime);
    }
}
