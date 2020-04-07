using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools
{
    public class MyEventSystem : MonoBehaviour
    {
        public static MyEventSystem instance;
        EventSystem own;

        private void Awake()
        {
            own = GetComponent<EventSystem>();

            if (instance == null) instance = this;
            else throw new System.Exception("!!!!!!!!!!! Hay dos event system !!!!!!!!!!!!!!");
        }

        public void SelectGameObject(GameObject go)
        {
            own.SetSelectedGameObject(go);
        }
        public void DeselectGameObject(GameObject go)
        {
            //own.(go);
        }
    }
}

