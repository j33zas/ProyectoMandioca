using UnityEngine;
using TMPro;

public class DebugText : MonoBehaviour
{
    public TextMeshPro txttmp;
    public TextMesh txttm;

    public void Print(string s)
    {
                //Debug.Log(s);
        if (txttmp != null) txttmp.text = s;
        if (txttm != null) txttm.text = s;
    }
}
