using Fusion;
using UnityEngine;

public class JoinPlayer : SimulationBehaviour, IPlayerJoined
{
    public GameObject avatarPrefab;
    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log($"Player {player} joined {player.PlayerId}");
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(avatarPrefab, inputAuthority:player);
        }
    }
}
