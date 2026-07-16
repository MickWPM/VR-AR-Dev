using UnityEngine;
using System.Collections.Generic;

public class PainterCanvas : MonoBehaviour
{
    private List<GameObject> paintObjects = new List<GameObject>();
    

    public void AddToCanvas(GameObject go)
    {
        paintObjects.Add(go);
    }

    public void ResetCanvas()
    {
        if (paintObjects.Count == 0) return;
        foreach (var obj in paintObjects)
        {
            Destroy(obj);
        }
        paintObjects.Clear();
    }
}
