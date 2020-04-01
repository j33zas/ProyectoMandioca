using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopTools
{
    public class LePidoAlPool : MonoBehaviour
    {

        public Queue<ItemPrefab_test> maza = new Queue<ItemPrefab_test>();
        public Queue<ItemPrefab_test> espada = new Queue<ItemPrefab_test>();

        /// <summary>
        /// Aca puse un par de letras como input para probar sacar y poner cosas del pull
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                //Pido maza
                maza.Enqueue(MPool.Instance.Get(ObjetosDelPool.maza)); 
            }
        
            if (Input.GetKeyDown(KeyCode.D))
            {
                //Devuelvo Maza
                if (maza.Count > 0)
                {
                    var item = maza.Dequeue();
                    MPool.Instance.ReturnToPool(item);
                }
            }
        
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Pido espada
                espada.Enqueue(MPool.Instance.Get(ObjetosDelPool.espada));
            }
        
            if (Input.GetKeyDown(KeyCode.U))
            {
                //Devuelvo espada
                if (espada.Count > 0)
                {
                    var item = espada.Dequeue();
                    MPool.Instance.ReturnToPool(item);
                }
            
            }
        }
    }
    

}
