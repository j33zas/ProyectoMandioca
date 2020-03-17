using UnityEngine;

public abstract class FrontendLifeBase : MonoBehaviour
{
    public abstract void OnLifeChange(int value, int max = 100, bool anim = false);
}
