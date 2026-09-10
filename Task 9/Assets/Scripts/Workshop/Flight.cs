using UnityEngine;

public class Flight : MonoBehaviour
{
    public Vector2 direction, position;
    public float speed = 0.02f;


    private void Update()
    {
        position = position + Time.deltaTime * speed * direction;
        Vector4 pos = new Vector4(position.x, position.y, 0, 0);
        GetComponent<MeshRenderer>().material.SetVector("_Offset", pos);
    }
}
