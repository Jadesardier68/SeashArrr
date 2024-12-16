using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public enum Role
    {
        Captain,
        //Cook,
       // Carpenter,
       // Engineer,
        Doctor,
        Fighter,
        //Cannonier
    }

    public List<Material> CharactersRoles;
    private static int roleIndex = 0;

    [SerializeField] private int HPMax;
    public Role role;

    private int HP;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the good skin on the prefab
        switch (role)
        {
            case Role.Captain:
                break;
            //case Role.Cook:
              //  break;
            //case Role.Carpenter:
              //  break;
            //case Role.Engineer:
              //  break;
            case Role.Doctor:
                break;
            case Role.Fighter:
                break;
            //case Role.Cannonier:
              //  break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        HP = HPMax;
        AssignRole();
        AssignMaterial();

    }

    private void AssignRole()
    {
        // D�finir le r�le bas� sur l'index global
        role = (Role)roleIndex;

        // Incr�menter l'index global et revenir au d�but si n�cessaire
        roleIndex = (roleIndex + 1) % Enum.GetValues(typeof(Role)).Length;
    }

    private void AssignMaterial()
    {
        // V�rifier que la liste des mat�riaux contient suffisamment d'�l�ments
        if (CharactersRoles.Count > 0 && TryGetComponent<Renderer>(out Renderer renderer))
        {
            int roleMaterialIndex = (int)role;

            if (roleMaterialIndex >= 0 && roleMaterialIndex < CharactersRoles.Count)
            {
                // Appliquer le mat�riau correspondant au r�le
                renderer.material = CharactersRoles[roleMaterialIndex];
            }
            else
            {
                Debug.LogWarning("Pas de mat�riau disponible pour ce r�le.");
            }
        }
        else
        {
            Debug.LogWarning("Pas de mat�riaux ou Renderer non trouv�.");
        }
    }

    public int GetHP()
    {
        return HP;
    }

    public void SetHP(int newHP)
    {
        HP = newHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
