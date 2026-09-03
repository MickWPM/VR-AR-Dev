using Fusion;
using TMPro;
using UnityEngine;

public class BillboardController : NetworkBehaviour
{
    public TextMeshPro billboard;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void ChangeTextRPC(string text)
    {
        billboard.text = text;
    }

    public void ChangeText(string text)
    {
        ChangeTextRPC(text);
    }
}
