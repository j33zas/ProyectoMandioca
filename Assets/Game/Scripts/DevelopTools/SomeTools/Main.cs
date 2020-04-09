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

    [Header("Main Options")]
    public GenericBar bar;
    List<Action<IEnumerable<PlayObject>>> toload = new List<Action<IEnumerable<PlayObject>>>();
    ThreadRequestObject<PlayObject> req;
    bool openUI;

    [Header("Inspector References")]
    public LevelSystem levelsystem;
    public EventManager eventManager;
    [SerializeField] CharacterHead character;
    [SerializeField] PlayObject[] allentities;

public GameUI_controller gameUiController;

    private void Awake()
    {
        instance = this;
        eventManager = new EventManager();
    }

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
    private void Update()//test//borrar
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
