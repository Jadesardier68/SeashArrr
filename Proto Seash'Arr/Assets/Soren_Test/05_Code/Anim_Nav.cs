using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class Anim_Nav : MonoBehaviour
{
  
    private PlayerInput playerInput;
    Animator animator;
    public bool cuisineActive; 
    public bool tableIngenieurActive;
    public bool canonActive;
    public bool piqueNiqueActive;
    public bool ancreActive;
    
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
        //------------------------------------------------
        
        //Manger------------------------------------------
        playerInput.actions["Manger"].started += Manger;
        //------------------------------------------------
        
        //Cuisiner------------------------------------------
        playerInput.actions["CuisinerRagout"].started += Cuisiner;
        
        //Canon------------------------------------------
        playerInput.actions["RepCanon"].started += Canon;

        //Inge------------------------------------------
        playerInput.actions["AmeBateau"].started += Ingenieur;
        
        
        playerInput.actions["Attaquer"].performed += Attaquer;
        playerInput.actions["Soigner"].performed += Soigner;
        playerInput.actions["Canon"].started += CanonCombat;
        playerInput.actions["Réparer"].performed += Reparer;
    }

    private void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("Cooking", false);
        animator.SetBool("Eat", false);
        animator.SetBool("Move", true);
        
        
        //Quand le perso marche ; se déplace
    }

    private void StopMoving(InputAction.CallbackContext context)
    {
        animator.SetBool("Move", false);
        //Quand le perso ne marche plus ; Idle
    }

    private void Manger(InputAction.CallbackContext context)
    {
        if (piqueNiqueActive)
        {
            animator.SetBool("Eat", true);
            //Quand le perso Mange
        }
    }

    private void Cuisiner(InputAction.CallbackContext context)
    {
        if (cuisineActive)
        {
            animator.SetBool("Cooking", true);
            //Quand le perso Cuisine
        }
    }
    
    private void Canon(InputAction.CallbackContext context)
    {
        if (canonActive)
        {
            animator.SetBool("Cooking", true);
            //Quand le perso Repare le Canon
        }
    }
    
    private void Ingenieur(InputAction.CallbackContext context)
    {
        if (tableIngenieurActive)
        {
            animator.SetBool("Cooking", true);
            //Quand le perso ameliore 
        }
    }

    private void Attaquer(InputAction.CallbackContext context)
    {
        if (RandomFight.Fight == true)
        {
            animator.SetBool("Attaquer", true);
        }
    }

    private void Soigner(InputAction.CallbackContext context)
    {
        if (RandomFight.Fight == true)
        {
            animator.SetBool("Heal", true);
        }
    }
    
    private void Reparer(InputAction.CallbackContext context)
    {
        if (RandomFight.Fight == true)
        {
            animator.SetBool("Cooking", true);
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Cuisine"))
            {
                cuisineActive = true;
                Debug.Log("Cuisine");
            }
            else if (other.CompareTag("TableIngenieur"))
            {
                tableIngenieurActive = true;
                Debug.Log("Ingenieur");
            }
            else if (other.CompareTag("Canon"))
            {
                canonActive = true;
                Debug.Log("Canon");
            }
            else if (other.CompareTag("PiqueNique"))
            {
                piqueNiqueActive = true;
                Debug.Log("PiqueNique");
            }
            else if (other.CompareTag("Ancre"))
            {
                ancreActive = true;
            }
        }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cuisine"))
        {
            cuisineActive = false;
        }
        else if (other.CompareTag("TableIngenieur"))
        {
            tableIngenieurActive = false;
        }
        else if (other.CompareTag("Canon"))
        {
            canonActive = false;
        }
        else if (other.CompareTag("PiqueNique"))
        {
            piqueNiqueActive = false;
        }
        else if (other.CompareTag("Ancre"))
        {
            ancreActive = false;
        }
    }
    void Update()
    {
       
    }
}


    
    
    
    
    
    
    

    

