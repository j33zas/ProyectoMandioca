using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ESTO ES SOLO UN EJEMPLO DE USO... HAY QUE BORRAR A LA MIERDA CUANDO SE LO AGREGUEMOS AL CHARACTER
public class EXAMPLE_USE_LIFE_SYSTEM : MonoBehaviour
{
    public CharacterLifeSystem lifesystemExample;

    public bool test;

    private void Start()
    {
        lifesystemExample.Config(100,OnLoseLife,OnGainLife,OnDeath, 100);
    }

    void OnLoseLife()
    {
        Debug.Log("Lose life");
    }
    void OnGainLife()
    {
        Debug.Log("Gain life");
    }
    void OnDeath()
    {
        Debug.Log("Death");
    }

    private void Update()
    {
        if (!test) return;

        if (Input.GetKeyDown(KeyCode.Z))//curar
        {
            lifesystemExample.AddHealth(20);
        }
        if (Input.GetKeyDown(KeyCode.X))//dañar
        {
            lifesystemExample.Hit(20);
        }
        if (Input.GetKeyDown(KeyCode.C))//resetear por default
        {
            lifesystemExample.ResetLife();
        }
        if (Input.GetKeyDown(KeyCode.V))//agregar salud maxima
        {
            lifesystemExample.IncreaseLife(20);
        }
    }

}
