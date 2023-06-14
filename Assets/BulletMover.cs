using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward  *bulletSpeed * Time.deltaTime;    
    }
}
