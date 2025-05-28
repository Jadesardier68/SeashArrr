using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Animation_Chara : MonoBehaviour
{
  
    private PlayerInput playerInput;
    Animator animator;
    
    
    void Start()
    {
          
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        Debug.unityLogger.Log("Enable Animation Chara");
        
        //Move------------------------------------------
        playerInput.actions["Move"].performed += Move;
        playerInput.actions["Move"].canceled += StopMoving;
        //-----------------------------------------------
        //Manger------------------------------------------
        playerInput.actions["Manger"].started += Move;
        
    }

    private void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("Move", true);
        Debug.unityLogger.Log("Move");
    }

    private void StopMoving(InputAction.CallbackContext context)
    {
        animator.SetBool("Move", false);
    }
    void Update()
    {
       
    }
}
