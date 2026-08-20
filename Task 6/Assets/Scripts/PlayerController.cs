using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction spawnFishAction, spawnFoodAction;
    [SerializeField] private Transform fishSpawnTransform;
    [SerializeField] private Transform foodSpawnTransform;
    [SerializeField] private Fish foodPrefab;
    [SerializeField] private FishBrain fishPrefab;
    [SerializeField] private FishTank fishTank;

    private void Awake()
    {
        spawnFishAction = InputSystem.actions.FindAction("SpawnFish");
        spawnFoodAction = InputSystem.actions.FindAction("SpawnFood");

        spawnFishAction.performed += SpawnFishAction_performed;
        spawnFoodAction.performed += SpawnFoodAction_performed;
    }

    private void SpawnFoodAction_performed(InputAction.CallbackContext obj)
    {
        Instantiate(foodPrefab, foodSpawnTransform.position, foodSpawnTransform.rotation);
    }

    private void SpawnFishAction_performed(InputAction.CallbackContext obj)
    {
        var fishBrain = Instantiate(fishPrefab, foodSpawnTransform.position, foodSpawnTransform.rotation);
        fishBrain.fishTank = fishTank;
    }

    private void Update()
    {
        
    }
}
