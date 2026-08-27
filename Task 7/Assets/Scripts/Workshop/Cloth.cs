using System;
using UnityEngine;

public class Cloth : MonoBehaviour
{
    public GameObject springTemplate;
    public GameObject nodeTemplate;
    public int size = 3;

    GameObject[,] nodes;

    public static float deltaTime = 0.01f;

    private void Start()
    {
        CreateCloth();
    }

    private void CreateCloth()
    {
        nodes = new GameObject[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3 position = new (x -  size / 2, 0, y - size / 2);
                GameObject node = Instantiate(nodeTemplate, position, Quaternion.identity, transform);
                nodes[x, y] = node;
                if (x == 0)
                {
                    node.GetComponent<Node>().dynamic = false;
                }
            }
        }

        GameObject springsContainer = new GameObject("Springs");


        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (x < size - 1)
                {
                    GameObject springObjectH = Instantiate(springTemplate, springsContainer.transform.position, Quaternion.identity, springsContainer.transform);
                    Spring springH = springObjectH.GetComponent<Spring>();
                    springH.end1 = nodes[x, y].transform;
                    springH.end2 = nodes[x + 1, y].transform;
                    nodes[x, y].GetComponent<Node>().AddSpring(springH); 
                    nodes[x + 1, y].GetComponent<Node>().AddSpring(springH);
                }
                if (y < size - 1)
                {
                    GameObject springObjectV = Instantiate(springTemplate, springsContainer.transform.position, Quaternion.identity, springsContainer.transform);
                    Spring springV = springObjectV.GetComponent<Spring>();
                    springV.end1 = nodes[x, y].transform;
                    springV.end2 = nodes[x, y + 1].transform;
                    nodes[x, y].GetComponent<Node>().AddSpring(springV); 
                    nodes[x, y + 1].GetComponent<Node>().AddSpring(springV);
                }
            }
        }
    }

    private void Update()
    {
        for (int X = 0; X < size; X++)
        {
            for (int Y = 0; Y < size; Y++)
            {
                nodes[X, Y].GetComponent<Node>().DoDynamics();
            }
        }
        for (int X = 0; X < size; X++)
        {
            for (int Y = 0; Y < size; Y++)
            {
                nodes[X, Y].GetComponent<Node>().StepTime();
            }
        }
    }

}
