using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneMainBase : MonoBehaviour
{

   // MessageScreen.Scene where;
   // MessageScreen.MessaggeType messagetype;
 //   string[] messages;
   // string sceneTogo;

    [Header("SpawnPoint")]
    public Transform spawn_point;

    bool canUpdate;

    //ScreenFade screenfade;

   // public MainMejorable mainMejorable;

    private void Awake()
    {
        //screenfade = FindObjectOfType<ScreenFade>();
        OnAwake();
    }
    private void Start()
    {
        //CompleteCameraController.instancia.ChangeToNormal();
        //CompleteCameraController.instancia.InstantAjust();

        //screenfade.Back(fadebackended);

        OnStart();
        Spawn();

        //if (mainMejorable) mainMejorable.EjecutarMejora();

        //MisionManager.instancia.allmisions.ForEach(x => x.PermanentConfigurations());
    }

    void fadebackended()
    {

        //canUpdate = true;
        //if (mainMejorable) mainMejorable.CheckearUpgradeNuevo();

        //OnFadeBackEnded();
    }


    private void Update()
    {
        if (canUpdate) OnUpdate();


        ////esto era por mero test, o para cuando se bugueaba
        //if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.B))
        //{
        //    TeleportBug();
        //}
    }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnPause();
    protected abstract void OnUpdate();
    protected abstract void OnFadeBackEnded();
    protected abstract void OnFadeGoEnded();
    public abstract void OnPlayerDeath();
    private void Spawn() //el spawn lo usaba para cargar los datos de esa escena, posicionar el character, la camara, etc etc etc
    {
        // 

        //if (GlobalData.Instance)
        //{
        //    Debug.Log("El Global data existe");

        //    if (GlobalData.Instance.LoadFromDisk)
        //    {
        //        Debug.Log("CARGANDO DESDE DISCO");
        //        var prof = GlobalData.Instance._savedata.profiles[GlobalData.Instance.currentIndex];

        //        CharBrain.instancia.gameObject.transform.position = new Vector3(
        //            prof.currentState.x_position,
        //            prof.currentState.y_position,
        //            prof.currentState.z_position);

        //        if (Scenes.GetActiveSceneName() == "MiCasa")
        //        {
        //            CharBrain.instancia.transform.position = spawn_point.transform.position;
        //        }

        //        GlobalData.Instance.LoadFromDisk = false;
        //    }
        //    else
        //    {
        //        Debug.Log("BUSCANDO SPAWNPOINTS");

        //        var spawnpoints = FindObjectsOfType<SpawnPoint>();
        //        foreach (var v in spawnpoints)
        //        {
        //            if (v.option == GameManager.instancia.transitionInformation.teleporOption)
        //            {
        //                Debug.Log("SE encontro el..." + v.option);
        //                var aux = new Vector3(v.transform.position.x, CharBrain.instancia.transform.position.y, v.transform.position.z);
        //                CharBrain.instancia.transform.position = aux;
        //                break;
        //            }
        //            else
        //            {
        //                CharBrain.instancia.transform.position = spawn_point.transform.position;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.Log("El Global data NO existe");
        //    CharBrain.instancia.transform.position = spawn_point.transform.position;
        //}

        //CompleteCameraController.instancia.InstantAjust();
    }

    //este lo usaba para cuando el character se bugeaba
    protected virtual void TeleportBug()
    {
        //CharBrain.instancia.transform.position = spawn_point.transform.position;
        //CompleteCameraController.instancia.InstantAjust();
    }

    public void Exit(/*MessageScreen.Scene where,
                     MessageScreen.MessaggeType message,
                     string scenetogo = "",
                     params string[] messages*/)
    {
        canUpdate = false;

        //si escribo algo en "scenetogo" voy a la escena Other, sino, la que me pidieron en "where"
        //this.where = where;
        //this.messagetype = message;
        //this.messages = messages;
        //this.sceneTogo = scenetogo;
        //screenfade.Go(EndAnimation);
    }

    public void EndAnimation()
    {
        OnFadeGoEnded();
       // MessageScreen.instancia.ShowAMessage(where, messagetype, sceneTogo, messages);
    }
}
