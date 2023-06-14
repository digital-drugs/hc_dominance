using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjController : MonoBehaviour
{
    [SerializeField] private float fireForce = 50f;
    [SerializeField] private GameObject destroyObjPrefab;

    [SerializeField] private Rigidbody _rb;

    [HideInInspector] public int ID = 0;

    public void SetID(int _id)
    {
        ID = _id;
    }

    private void OnEnable()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();

        _rb.isKinematic = false;
        StartCoroutine(OpenFire());
    }

    private IEnumerator OpenFire()
    {
        yield return new WaitForFixedUpdate();

        if (_rb != null) _rb.AddForce(transform.forward * fireForce);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<TargetStickman>())
        {
            collision.gameObject.GetComponent<TargetStickman>().UpdateHeal(ID, 1);

            if (destroyObjPrefab != null)
            {
                GameObject destroyObj = Instantiate(destroyObjPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);
                destroyObj.transform.position = gameObject.transform.position;
                destroyObj.transform.rotation = gameObject.transform.rotation;
            }

            _rb.isKinematic = true;
            gameObject.SetActive(false);
        }
        else
        if (!collision.isTrigger)
        {
            if (destroyObjPrefab != null)
            {
                GameObject destroyObj = Instantiate(destroyObjPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);
                destroyObj.transform.position = gameObject.transform.position;
                destroyObj.transform.rotation = gameObject.transform.rotation;
            }

            _rb.isKinematic = true;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.position.y < -5)
        {
            _rb.isKinematic = true;
            gameObject.SetActive(false);
        }
    }
}
