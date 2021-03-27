using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnable : MonoBehaviour
{

    [SerializeField]
    float rotationSpeed;


    public void BeingTurned(float turnDirection)
    {

        transform.Rotate(-Vector3.forward * Time.deltaTime * turnDirection * rotationSpeed, Space.Self);

    }


}
