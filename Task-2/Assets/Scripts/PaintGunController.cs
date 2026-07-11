using UnityEngine;

public class PaintGunController : MonoBehaviour
{
    [SerializeField] private Transform muzzleExit;
    [SerializeField] private Transform projectilePrefab;

    public void Fire()
    {
        Fire(projectilePrefab);
    }

    public void Fire(Transform prefab)
    {
        Instantiate(prefab, muzzleExit.transform.position, muzzleExit.transform.rotation);
    }
}
