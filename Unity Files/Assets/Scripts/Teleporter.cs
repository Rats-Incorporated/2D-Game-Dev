using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform GetDestination()
    {
        return destination;
    }

}
