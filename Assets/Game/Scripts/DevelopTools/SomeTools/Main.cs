using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Tools;
using DevelopTools;
using XInputDotNetPure;


public class Main : MonoBehaviour
{
    public static Main instance;

    public EventManager eventManager;
    public LevelSystem levelsystem;

    GamePad gp;

    private void Awake()
    {
        instance = this;
        eventManager = new EventManager();

        gp = new GamePad();

    }

    public bool autofind;

    List<Action<IEnumerable<PlayObject>>> toload = new List<Action<IEnumerable<PlayObject>>>();
    [SerializeField] PlayObject[] allentities;
    [SerializeField] CharacterHead character;

    Dictionary<Type, List<PlayObject>> typedic = new Dictionary<Type, List<PlayObject>>();

    // CharacterHead;

    ThreadRequestObject<PlayObject> req;

    public GenericBar bar;

    bool openUI;

    void Start()
    {
        toload.Add(AddToMainCollection);
        req = new ThreadRequestObject<PlayObject>(
            this,
            bar,
            toload.ToArray()
            );
    }


    bool pause;
    private void Update()//test
    {
        pause = !pause;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                Pause();
            }
            else
            {
                Play();
            }
            
        }
    }

    void OnLoadEnded()
    {
        Play();
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

        OnLoadEnded();
        
    }

    public void OnPlayerDeath() { }

    public void PlayerDeath()
    {
        if (FindObjectOfType<SceneMainBase>())
        {
            FindObjectOfType<SceneMainBase>().OnPlayerDeath();
        }
    }

    public void Set_Opened_UI() { openUI = true; Pause(); }
    public void Set_Closed_UI() { openUI = false; Play(); }

    public void Play() { foreach (var e in allentities) e.Resume(); }
    public void Pause() { foreach (var e in allentities) e.Pause(); }


    /////////////////////////////////////////////////////////////////////
    /// PUBLIC GETTERS
    /////////////////////////////////////////////////////////////////////
    public CharacterHead GetChar() => character;
    public List<DummyEnemy> GetEnemies() => GetListOf<DummyEnemy>();
    public List<Minion> GetMinions() => GetListOf<Minion>();
    public MyEventSystem GetMyEventSystem() => MyEventSystem.instance;
    public bool Ui_Is_Open() => openUI;
    
}
