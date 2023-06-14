using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootGunController : MonoBehaviour
{
    [SerializeField] private Transform gunPos;
    [SerializeField] private GameObject fireObjPrefab;
    private List<GameObject> objPool = new List<GameObject>();

    [Header("miniCooldown Fire Time")]
    [SerializeField] private float fireCooldownTime = 0.5f;

    [SerializeField] private int gunMagazineCount = 10;
    private int magazine;

    private bool fireOpen = false;

    public void Init(int ID)
    {
        magazine = gunMagazineCount;

        for (int i = 0; i < 5; i++)
        {
            GameObject _go = Instantiate(fireObjPrefab, Vector3.zero, Quaternion.identity, gameObject.transform.parent);
            objPool.Add(_go);
            _go.SetActive(false);
            _go.GetComponent<FireObjController>().SetID(ID);
        }
    }

    public void Fire()
    {
        StartCoroutine(FireOpen());
    }

    private IEnumerator FireOpen()
    {
        if (fireOpen) yield break;

        fireOpen = true;

        while (magazine > 0)
        {
            magazine -= 1;

                for (int i = 0; i < objPool.Count; i++)
                {
                    if (!objPool[i].activeInHierarchy)
                    {
                        objPool[i].SetActive(true);

                        objPool[i].transform.position = gunPos.position;
                        objPool[i].transform.rotation = gunPos.rotation;
                        break;
                    }
                    else
                        if (i == objPool.Count - 1)
                    {
                        objPool[0].SetActive(false);
                        objPool[0].SetActive(true);

                        objPool[0].transform.position = gunPos.position;
                        objPool[0].transform.rotation = gunPos.rotation;
                        break;
                    }
                }

            yield return new WaitForSeconds(fireCooldownTime);
        }

        fireOpen = false;
        magazine = gunMagazineCount;

        //stat.Instance.FireEnd();
    }
}