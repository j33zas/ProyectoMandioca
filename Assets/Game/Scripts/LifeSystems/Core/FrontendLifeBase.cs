using UnityEngine;

public abstract class FrontendStatBase : MonoBehaviour
{
    public abstract void OnLifeChange(int value, int max = 100, bool anim = false);
}
