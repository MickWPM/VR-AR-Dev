using Fusion;
using UnityEngine;

namespace workshop
{

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

        /*
         * spawnn avatar
         * is vr active? - SetupVRPlayer if so
         * Is xr rig available?
         * transform of XR rig to avatar position/rotation & parenting
         */


    }

}