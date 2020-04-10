using DevelopTools;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
//using XInputDotNetPure;

public class Main : MonoBehaviour
{
    public static Main instance;

    [Header("Main Options")]
    public GenericBar bar;
    List<Action<IEnumerable<PlayObject>>> toload = new List<Action<IEnumerable<PlayObject>>>();
    ThreadRequestObject<PlayObject> req;
    public bool use_selector = true;
    bool gameisbegin;
    bool inmenu;


    [Header("Inspector References")]
    public LevelSystem levelsystem;
    public EventManager eventManager;
    [SerializeField] CharacterHead character;
    [SerializeField] PlayObject[] allentities;
    public UI_Menu ui_menu;

    public GameUI_controller gameUiController;

    [Header("Skills")]
    public GameObject model_skill_selector;
    GameObject selector;
    public SkillManager_Pasivas skillmanager_pasivas;
    public SkillManager_Activas skillmanager_activas;



    private void Awake()
    {
        instance = this;
        eventManager = new EventManager();
    }

    private SensorForEnemysInRoom mySensorRoom;
    BaseRoom _currentRoom;

    void Start()
    {
        if (use_selector)
        {
            selector = GameObject.Instantiate(model_skill_selector, gameUiController.MyCanvas.transform);
            selector.GetComponent<UI_BeginSkillSelector>().Initialize(SkillSelected);
        }
        else
        {
            LoadLevelPlayObjects();
        }

    }

    void SkillSelected(SkillType _skillType)
    {
        skillmanager_pasivas.Initialize();
        skillmanager_pasivas.SelectASkillType(_skillType);
        levelsystem.Initialize();
        LoadLevelPlayObjects();
        selector.gameObject.SetActive(false);
    }

    void LoadLevelPlayObjects()
    {
        toload.Add(AddToMainCollection);
        req = new ThreadRequestObject<PlayObject>(
            this,
            bar,
            toload.ToArray()
            );


        gameisbegin = true;
    }

    public void EVENT_OpenMenu()
    {
        if (gameisbegin)
        {
            if (!Ui_Is_Open())
            {
                gameUiController.Set_Opened_UI();
                ui_menu.Open();
            }
            else
            {
                gameUiController.Set_Closed_UI();
                ui_menu.Close();
            }
        }
    }
    public void CloseMenu()
    {
        if (gameisbegin)
        {
            Play();
            ui_menu.Close();
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



    public void Play() { foreach (var e in allentities) e.Resume(); }
    public void Pause() { foreach (var e in allentities) e.Pause(); }


    /////////////////////////////////////////////////////////////////////
    /// PUBLIC GETTERS
    /////////////////////////////////////////////////////////////////////
    public CharacterHead GetChar() => character;
    public List<DummyEnemy> GetEnemies() => GetListOf<DummyEnemy>();
    public List<Minion> GetMinions() => GetListOf<Minion>();
    public MyEventSystem GetMyEventSystem() => MyEventSystem.instance;
    public bool Ui_Is_Open() => gameUiController.openUI;
    public void SetRoom(BaseRoom newRoom) => _currentRoom = newRoom;
    public BaseRoom GetRoom() => _currentRoom;



}
