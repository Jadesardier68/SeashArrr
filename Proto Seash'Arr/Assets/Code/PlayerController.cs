using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movementInput;
    public float health = 100f;
    public InputAction onMove;

    private void OnEnable()
    {
        onMove.Enable();
        onMove.performed += Move;  // Se d�clenche quand l'input change
        onMove.canceled += Move;   // R�initialise � 0 quand le joueur rel�che la touche
    }

    private void OnDisable()
    {
        onMove.performed -= Move;
        onMove.canceled -= Move;
        onMove.Disable();
    }

    private void Update()
    {
        Vector3 moveVector = new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime;
        transform.Translate(moveVector, Space.World); // Utilisation de Space.World pour �viter les probl�mes de rotation
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }
}
