using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    public int depth = 1;
    public string axiom;
    public GameObject treeBranch;

    private void Start()
    {
        string tree = Generate(axiom, depth);
        Debug.Log($"Tree: {tree}");
        Interpret(tree);
    }


    private string Generate(string current, int depth)
    {
        if (depth == 0) return current;
        
        string result = "";
        foreach (string token in current.Split(' '))
        {
            if (result != "")
            {
                result += " ";
            }
            result += Replace(token);
        }
        depth--;
        
        return Generate(result, depth);
    }

    private string Replace(string token)
    {
        foreach (ProductionRule rule in productionRules)
        {
            if (rule.lhs.Equals(token))
            {
                //Simple version - should collate all applicable and select a random one
                return rule.rhs;
            }
        }
        return token;
    }


    private void Interpret(string tree)
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        Stack<(Vector3, Quaternion)> stack = new Stack<(Vector3, Quaternion)> ();
        foreach (string token in tree.Split(' '))
        {
            switch (token)
            {
                case "f":
                    GameObject g = Instantiate(treeBranch, position, rotation);
                    position += g.transform.up * 1;
                    break;
                case "l":
                    //Rotation etc could be more customisable
                    rotation *= Quaternion.AngleAxis(Random.Range(20f,30f), Vector3.forward);
                    break;
                case "r":
                    //Rotation etc could be more customisable
                    rotation *= Quaternion.AngleAxis(-1*Random.Range(20f, 30f), Vector3.forward);
                    break;
                case "[":
                    stack.Push((position, rotation));
                    break;
                case "]":
                    (position, rotation) = stack.Pop();
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public class ProductionRule
    {
        public string lhs;
        public string rhs;
    }
    public List<ProductionRule> productionRules;
}