using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopTools
{
    public abstract class MultipleObjectPool<T> : MonoBehaviour where T : Component, IPoolObjectIndex
{
     /// <summary>
     /// Singleton del pool
     /// </summary>
    public static MultipleObjectPool<T> Instance { get; private set; }
    /// <summary>
    /// lista de prefabs a poolear.
    /// Agregar aca a los prefabs que se quiera poner en pool
    /// </summary>
    [SerializeField] private List<T> prefabs = new List<T>();
    /// <summary>
    /// Lista de colas donde se alojan los objetos creados
    /// El IPoolObjectIndex sirve para que el pool sepa a cual de estas colas debe regresar el objeto
    /// </summary>
    //[HideInInspector] public List<Queue<T>> tracksModules = new List<Queue<T>>();
    private Dictionary<ObjetosDelPool, Queue<T>> registroDeObjetos = new Dictionary<ObjetosDelPool, Queue<T>>();
    /// <summary>
    /// cantidad de objetos a crear en cada queue
    /// </summary>
    [SerializeField] private int amountOfPreCreatedObjects;

    #region Init and set of pool

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitData();
        PreWarmPoolObjects();
    }

    /// <summary>
    /// Inicializa las estructuras para laburar
    /// </summary>
    private void InitData()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if(registroDeObjetos.ContainsKey(prefabs[i].TipoDePoolObject))
                continue;
            
            Queue<T> newQ = new Queue<T>();
            registroDeObjetos.Add(prefabs[i].TipoDePoolObject, newQ);
        }
        
        
//        for (int i = 0; i < prefabs.Count; i++)
//        {
//            Queue<T> nuevaCola = new Queue<T>();
//            tracksModules.Add(nuevaCola);
//        }
    }

    /// <summary>
    /// Objetos creados antes de arrancar para ya tener 
    /// </summary>
    public void PreWarmPoolObjects()
    {

        for (int i = 0; i < prefabs.Count; i++)
        {
            for (int j = 0; j < amountOfPreCreatedObjects; j++)
            {
                Debug.Log("entro");
                AddObjectToQueue(prefabs[i].TipoDePoolObject);
            }
        }
       //StartCoroutine(PreWarm_CoRoutine());

    }

    private IEnumerator PreWarm_CoRoutine()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            for (int j = 0; j < amountOfPreCreatedObjects; j++)
            {
                AddObjectToQueue(prefabs[i].TipoDePoolObject);
                yield return 0;
            }
        }
    }

    #endregion

    #region Pool Actions

    /// <summary>
    /// Te da un item del pool. Especificar de que queue lo queres
    /// </summary>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public T Get(ObjetosDelPool objectType)
    {
        if (registroDeObjetos[objectType].Count == 0)
        {
            AddObjectToQueue(objectType);
        }

        T poolObject = registroDeObjetos[objectType].Dequeue();
        poolObject.gameObject.SetActive(true);
        return poolObject;
    }

    /// <summary>
    /// Devuelve un objeto al pool. Solo hay que devolverlo, no se tiene que especificar de que queue era, para eso
    /// esta la interfaz que lo guarda
    /// </summary>
    /// <param name="objectToReturn"></param>
    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        ObjetosDelPool index = objectToReturn.GetComponent<IPoolObjectIndex>().TipoDePoolObject;
        
        registroDeObjetos[index].Enqueue(objectToReturn);
    }

    /// <summary>
    /// Agrega a la queue correspondiente un objeto y le dice a la interfaz de que queue era para luego poder guardarlo
    /// </summary>
    /// <param name="queueIndex"></param>
    private void AddObjectToQueue(ObjetosDelPool poolType)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].TipoDePoolObject == poolType)
            {
                var newObject = GameObject.Instantiate(prefabs[i], transform);
                newObject.gameObject.SetActive(false);
                registroDeObjetos[poolType].Enqueue(newObject);
                
            }
        }
    }
    

    #endregion

}

/// <summary>
/// Diferentes tipos de objetos del pool.
/// Ir agregando aca a medida que se van creando nuevos objetos para poolear
/// </summary>
public enum ObjetosDelPool {maza, espada};


}
