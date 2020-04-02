using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMainManager : MonoBehaviour
{
    //public void Exit(/*MessageScreen.Scene where, MessageScreen.MessaggeType messagetype, */string scenetogo = "", params string[] messages)
    //{
    //    //if (FindObjectOfType<SceneMainBase>())
    //    //    FindObjectOfType<SceneMainBase>()
    //    //        .Exit(scenetogo == "" ? where : MessageScreen.Scene.other, messagetype, scenetogo, messages);
    //}
    public void Exit(string sceneToGo, params string[] messages)
    {
        //if (FindObjectOfType<SceneMainBase>())
        //    FindObjectOfType<SceneMainBase>()
        //        .Exit(MessageScreen.Scene.other, MessageScreen.MessaggeType.commonMessage, sceneToGo, messages);
    }

    public void PlayerDeath()
    {
        if (FindObjectOfType<SceneMainBase>())
        {
            FindObjectOfType<SceneMainBase>().OnPlayerDeath();
        }
    }
}
