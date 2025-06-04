using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class Anim_Nav : MonoBehaviour
{
  
    public PlayerInput playerInput;
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
        //playerInput = GetComponent<PlayerInput>();
        Debug.unityLogger.Log("Enable Animation Chara");
        
        //Move------------------------------------------
        playerInput.actions["Move"].performed += Move;
        playerInput.actions["Move"].canceled += ctx => StopMoving();
        //------------------------------------------------

        //Manger------------------------------------------
        /* playerInput.actions["Manger"].started += Manger;
         //------------------------------------------------

         //Cuisiner------------------------------------------
         playerInput.actions["CuisinerRagout"].started += Cuisiner;

         //Canon------------------------------------------
         playerInput.actions["RepCanon"].started += Canon;

         //Inge------------------------------------------
         playerInput.actions["AmeBateau"].started += Ingenieur;


         playerInput.actions["Attaquer"].performed += Attaquer;
         playerInput.actions["Soigner"].performed += Soigner;
         playerInput.actions["Réparer"].performed += Reparer;*/
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("Cooking", false);
        animator.SetBool("Eat", false);
        animator.SetBool("Move", true);
        
        
        //Quand le perso marche ; se déplace
    }

    public void StopMoving()
    {
        animator.SetBool("Move", false);
        //Quand le perso ne marche plus ; Idle
    }

    public void Manger()
    {
        if (piqueNiqueActive)
        {
            animator.SetBool("Eat", true);
            //Quand le perso Mange
        }
    }

    public void Cuisiner()
    {
        if (cuisineActive)
        {
            animator.SetBool("Cooking", true);
            //Quand le perso Cuisine
        }
    }
    
    public void Canon()
    {
        if (canonActive)
        {
            animator.SetBool("Cooking", true);
            //Quand le perso Repare le Canon
        }
    }
    
    public void Ingenieur()
    {
        if (tableIngenieurActive)
        {
            animator.SetBool("Cooking", true);
            //Quand le perso ameliore 
        }
    }

    public void Attaquer()
    {
        animator.SetBool("Shoot", true);
        StartCoroutine(ResetBool("Shoot", 1f));
    }

    public void Soigner()
    {
        animator.SetBool("Heal", true);
        StartCoroutine(ResetBool("Heal", 1f));
    }

    public void Reparer()
    {
        animator.SetBool("Cooking", true);
        StartCoroutine(ResetBool("Cooking", 1f));
    }

    private IEnumerator ResetBool(string param, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(param, false);
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


    
    
    
    
    
    
    

    

