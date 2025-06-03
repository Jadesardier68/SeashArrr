using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 720f;

    private Vector2 movementInput;
    public PlayerInput playerInput;

    private Rigidbody childRb;

    [SerializeField] private Transform childTransform; // assigné dans l’inspecteur ou trouvé dynamiquement

    private void Awake()
    {
        if (childTransform == null)
        {
            // Essaie de trouver automatiquement un Rigidbody dans les enfants
            childRb = GetComponent<Rigidbody>();
            if (childRb != null)
                childTransform = childRb.transform;
        }
        else
        {
            childRb = childTransform.GetComponent<Rigidbody>();
        }

        if (childRb == null)
        {
            Debug.LogError("❌ Aucun Rigidbody trouvé sur l'enfant !");
        }
    }

    private void OnEnable()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogWarning("❗ PlayerInput non trouvé sur ce GameObject.");
            return;
        }

        playerInput.actions.Enable();
        playerInput.actions["Move"].performed += Move;
        playerInput.actions["Move"].canceled += StopMoving;
    }
    private void OnDisable()
    {
        if (playerInput == null) return;

        playerInput.actions["Move"].performed -= Move;
        playerInput.actions["Move"].canceled -= StopMoving;

        playerInput.actions.Disable();
    }

    private void FixedUpdate()
    {
        if (childRb == null) return;

        Vector3 moveDir = new Vector3(-movementInput.y, 0f, movementInput.x);

        // Mouvement
        Vector3 targetPos = childRb.position + moveDir * speed * Time.fixedDeltaTime;
        childRb.MovePosition(targetPos);

        // Rotation
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(-moveDir);
            Quaternion newRot = Quaternion.RotateTowards(childRb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
            childRb.MoveRotation(newRot);
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void StopMoving(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }
}