using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopTools
{
    public class ItemPrefab_test : MonoBehaviour, IPoolObjectIndex
    {
        public ObjetosDelPool objectType;

        public ObjetosDelPool TipoDePoolObject
        {
            get { return objectType; }
            set { objectType = value; }
        }
    }    

}

