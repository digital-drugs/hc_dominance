using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float coolDown;
    [SerializeField] private GameObject playerTransform;
    private bool canShoot = true;

    private IEnumerator ShootTheBullet()
    {
        GameObject bullet = Instantiate(projectile, startPoint.position, startPoint.rotation);
        //bullet.GetComponent<Rigidbody>().AddForce(direction * 100);
        canShoot = false;
        Destroy(bullet, 2);
        yield return new WaitForSeconds(coolDown);
        canShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       

        if (other.gameObject.GetComponent<DestroyDummy>() && canShoot)
        {
           
            StartCoroutine(ShootTheBullet());
        }
    }

    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.GetComponent<DestroyDummy>() && canShoot)
        {
            
            StartCoroutine(ShootTheBullet());
        }
    }
}
