using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

namespace workshop
{
    public class MoveAvatar : NetworkBehaviour
    {
        [SerializeField] private InputActionReference moveActionRef, jumpActionRef;

        private void OnEnable()
        {
            moveActionRef.action.Enable();
            jumpActionRef.action.Enable();

            jumpActionRef.action.performed += OnJump;
        }

        private void OnDisable()
        {
            moveActionRef.action.Disable();
            jumpActionRef.action.Disable();

            jumpActionRef.action.performed -= OnJump;
        }

        public float speed = 1, turnSpeed = 90;
        private void FixedUpdate()
        {
            if (Object.HasInputAuthority == false) return;

            Vector2 moveInput = moveActionRef.action.ReadValue<Vector2>();

            if (moveInput.sqrMagnitude > 0)
            {
                Debug.Log($"Moving: {moveInput}");
                transform.position += speed * Time.fixedDeltaTime * transform.forward * moveInput.y;
                transform.rotation *= Quaternion.AngleAxis(turnSpeed * Time.fixedDeltaTime * moveInput.x, Vector3.up);
            }
        }

        private BillboardController billboard;
        private CreateTexture textureCreate;
        private void OnJump(InputAction.CallbackContext context)
        {
            if (Object.HasInputAuthority == false) return;
            if (billboard == null)
            {
                billboard = GameObject.FindFirstObjectByType<BillboardController>();
            }
            if (textureCreate == null)
            {
                textureCreate = GameObject.FindFirstObjectByType<CreateTexture>();
            }
            billboard.ChangeText("Hello from " + Runner.LocalPlayer.PlayerId.ToString());
            textureCreate.ChangeTexture(Runner.LocalPlayer.PlayerId.ToString());

        }
    }
}