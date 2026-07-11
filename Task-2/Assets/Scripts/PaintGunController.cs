using UnityEngine;

public class PaintGunController : MonoBehaviour
{
    [SerializeField] private Transform muzzleExit;
    [SerializeField] private Painter projectilePrefab;

    public Color paintingColour = Color.white;

    public void Fire()
    {
        Fire(projectilePrefab);
    }

    public void Fire(Painter prefab)
    {
        var painter = Instantiate(prefab, muzzleExit.transform.position, muzzleExit.transform.rotation) as Painter;
        painter.SetColour(paintingColour);
    }
}
