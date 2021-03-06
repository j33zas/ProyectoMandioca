﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngineInternal;

public class ThreadRequestObject<T> where T : MonoBehaviour
{
    MonoBehaviour m;
    GenericBar bar;
    Action<IEnumerable<T>>[] receivers;
    public ThreadRequestObject(
        MonoBehaviour _receptacle,
        GenericBar _bar,
        params Action<IEnumerable<T>>[] _receivers)
    {
        m = _receptacle;
        bar = _bar;
        receivers = _receivers;
        m.StartCoroutine(MainThread());

    }

    IEnumerator MainThread()
    {
        for (int i = 0; i < receivers.Length; i++)
        {
            yield return new WaitForEndOfFrame();
            yield return m.StartCoroutine(Find(receivers[i]));
        }
        yield return null;
    }

    public HashSet<T> hash = new HashSet<T>();

    IEnumerator Find<T>(Action<IEnumerable<T>> callbackCollection) where T : MonoBehaviour
    {
        var gos = UnityEngine
            .SceneManagement
            .SceneManager
            .GetActiveScene()
            .GetRootGameObjects();

        bar.Configure(gos.Length-1, 0.01f);

        List<T> col = new List<T>();

        for (int i = 0; i < gos.Length; i++)
        {
            //yield return new WaitForSeconds(0.01f);
            bar.SetValue((float)i);

            foreach (var child in gos[i].GetComponentsInChildren<T>())
            {
                if (!col.Contains(child))
                    col.Add(child);
            }
            var c = gos[i].GetComponent<T>();
            if (c != null)
            {
                if(!col.Contains(c))
                    col.Add(c);
            }
        }

        callbackCollection(col);

        bar.gameObject.SetActive(false);

        yield return null;
    }
}
