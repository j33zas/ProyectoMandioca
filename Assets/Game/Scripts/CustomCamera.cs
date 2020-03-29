using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity = Vector3.zero;
    public float smooth = 1f;
    public Vector3 offset;
    public bool lookAt;
    public float shakeAmmount;
    private float shakeDurationCurrent;
    public float shakeDuration;
    private bool activeShake;

    private void Start()
    {
        shakeDurationCurrent = shakeDuration;
    }
    private void LateUpdate()
    {
        if (activeShake)
        {
            Shake();
        }
        
    }

    private void FixedUpdate()
    {
        Vector3 desiredposition = target.position + offset;

        Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredposition, smooth * Time.deltaTime);
        transform.position = smoothedposition;
       
        
        if (lookAt)
        {
            transform.LookAt(target);
        }
    }

    public void BeginShakeCamera()
    {
        activeShake = true;
        shakeDurationCurrent = shakeDuration;
    }

    private void Shake()
    {
        if (shakeDurationCurrent > 0)
        {
            transform.position += Random.insideUnitSphere * shakeAmmount;
            shakeDurationCurrent -= Time.deltaTime;
        }
        else
        {
            shakeDurationCurrent = 0;
            activeShake = false;
        }

    }
    void SmoothDump()
    {
        transform.position = Vector3.SmoothDamp(transform.position,target.position,ref velocity,smooth);
                                                      
    }
}
