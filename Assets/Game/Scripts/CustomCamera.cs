using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Transform target;
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
        Vector3 desiredposition = target.position + offset;

        Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredposition, smooth * Time.deltaTime);
        transform.position = smoothedposition;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeShake = true;
            shakeDurationCurrent = shakeDuration;
            
        }
        if (activeShake)
        {
            Shake();
        }
        if (lookAt)
        {
            transform.LookAt(target);
        }
       
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
}
