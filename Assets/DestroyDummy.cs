using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class DestroyDummy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
     if(collision.transform.CompareTag("Enemy"))   
            Debug.Log("Попал");
            Destroy(gameObject);
        
    }
}
