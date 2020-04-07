using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Main : MonoBehaviour
{
    public static Main instance;
    private void Awake() => instance = this;

    public bool autofind;

    List<Action<IEnumerable<PlayObject>>> toload = new List<Action<IEnumerable<PlayObject>>>();
    [SerializeField] PlayObject[] allentities;
    [SerializeField] CharacterHead character;

    Dictionary<Type, List<PlayObject>> typedic = new Dictionary<Type, List<PlayObject>>();

    // CharacterHead;

    ThreadRequestObject<PlayObject> req;

    public GenericBar bar;

    void Start()
    {
        toload.Add(AddToMainCollection);
        req = new ThreadRequestObject<PlayObject>(
            this,
            bar,
            toload.ToArray()
            );
    }

    void AddType(Type type, PlayObject playobject)
    {
        if (!typedic.ContainsKey(type))
        {
            var list = new List<PlayObject>();
            list.Add(playobject);
            typedic.Add(type, list);
        }
        else
        {
            if (!typedic[type].Contains(playobject))
            {
                typedic[type].Add(playobject);
            }
        }
    }

    public List<T> GetListOf<T>() where T : PlayObject
    {
        List<T> aux = new List<T>();
        foreach (var obj in allentities)
        {
            if (obj.GetType() == typeof(T))
            {
                aux.Add((T)obj);
            }
        }
        return aux;
    }

    void AddToMainCollection(IEnumerable<PlayObject> col)
    {
        Debug.Log("Addto main collection");
        allentities = col.ToArray(); 
    }

    public void OnPlayerDeath() { }

    public void PlayerDeath()
    {
        if (FindObjectOfType<SceneMainBase>())
        {
            FindObjectOfType<SceneMainBase>().OnPlayerDeath();
        }
    }


    /////////////////////////////////////////////////////////////////////
    /// PUBLIC GETTERS
    /////////////////////////////////////////////////////////////////////
    public CharacterHead GetChar() => character;
    public List<DummyEnemy> GetEnemies() => GetListOf<DummyEnemy>();
    public List<Minion> GetMinions() => GetListOf<Minion>();
    
}
