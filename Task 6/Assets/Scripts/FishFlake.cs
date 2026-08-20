using Unity.VisualScripting;
using UnityEngine;

public class FishFlake : MonoBehaviour
{
    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearDamping = Random.Range(1f, 5f);
    }
}
