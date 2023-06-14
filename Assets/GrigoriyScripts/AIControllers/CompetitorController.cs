/* Класс управления ботами, можно настроить скорость, собираемый вес и минимальную дистанцию */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompetitorController : MonoBehaviour
{
    private NavMeshAgent AI;

    [Header("AI Setting")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float minDistance = 2;

    [SerializeField] private Transform target;

    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem deathParticle;

    [SerializeField] private float stopTimeAtTarget = 0.1f;

    [Header("Присвоить индивид. ID")]
    [SerializeField] private int ID = 0;
    [SerializeField] private ShootGunController gun;

    private Vector3 startPos;

    private void Start()
    {
        startPos = gameObject.transform.position;

        AI = GetComponent<NavMeshAgent>();
        AI.speed = 0;

        gun.Init(ID);
    }

    public void GameStart()
    {
        fire = false;
        target = SceneAssets.Instance.ChekNewTarget();
        AI.destination = target.position;
        AI.speed = speed;
        anim.SetBool("Run", true);
    }

    public void EndGame(bool win)
    {
        AI.speed = 0;

        if (win)
            anim.SetBool("LoseGame", true);
        else
            anim.SetBool("WinGame", true);

        anim.SetBool("Run", false);
    }

    private float time = 0;
    private bool fire = false;
    private void FixedUpdate()
    {
        if (anim.GetBool("Fire"))
            anim.transform.parent.LookAt(target);
        else
            anim.transform.parent.rotation = anim.transform.parent.parent.rotation;

        if (death || !StaticGameController.Instance.gameIsPlayed)
        {
            AI.speed = 0;
            return;
        }

        if (!fire && (target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) < minDistance))
        {
            fire = true;
            StartCoroutine(WarriorFire());
        }
        else
        {
            if (target != null && time >= 5f)
            {
                target = SceneAssets.Instance.ChekNewTarget();
                AI.destination = target.position;
                time = 0;
            }
        }

        time += Time.deltaTime;
    }

    private IEnumerator WarriorFire()
    {
        AI.speed = 0;
        anim.SetBool("Fire", true);
        anim.SetBool("Run", false);
        gun.Fire();

        yield return new WaitForSeconds(stopTimeAtTarget);

        Transform newTarget = SceneAssets.Instance.ChekNewTarget();
        while (target.gameObject == newTarget.gameObject)
        {
            yield return new WaitForFixedUpdate();
            newTarget = SceneAssets.Instance.ChekNewTarget();
        }

        anim.SetBool("Run", true);
        AI.destination = newTarget.position;
        time = 0;
        AI.speed = speed;

        yield return new WaitForSeconds(0.1f);
        fire = false;
        target = newTarget;
        anim.SetBool("Fire", false);
    }

    private bool death = false;
    private void OnTriggerEnter(Collider other)
    {
        if (death || !StaticGameController.Instance.gameIsPlayed) return;
        
        //здесь пишем условие смерти и спавним на старт
     
        if (other.GetComponent<DamageObstacle>())
            StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        death = true;

        AI.destination = gameObject.transform.position;
        AI.speed = 0;

        anim.SetBool("Run", false);
        anim.SetBool("Fire", false);
        fire = false;

        anim.SetTrigger("Death");
        if (deathParticle != null) deathParticle.Play();
        yield return new WaitForSeconds(2);

        transform.position = startPos;
        transform.rotation = Quaternion.identity;

        death = false;

        GameStart();
    }
}
