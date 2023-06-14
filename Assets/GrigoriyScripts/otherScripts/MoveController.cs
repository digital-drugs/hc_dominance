/* =======================================MoveController===================================
 * Класс контролирует движение персонажа
 * ========================================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [Header("Rotate")]
    [SerializeField] private GameObject visualPlayer;
    [SerializeField] private ParticleSystem deathParticle;

    [Header("MoveForward")]
    [SerializeField] private float speed = 5f;
    private float _speed = 5f;

    [SerializeField] private Animator[] anim;

    private Vector3 startPos;

    [Header("Присвоить индивид. ID")]
    [SerializeField] private int ID = 0;
    [SerializeField] private ShootGunController gun;
    [SerializeField] private float attackDistance = 15;

    private Transform target;
    public void NewTarget(Transform _target)
    {
        target = _target;
    }

    private void Awake()
    {
        startPos = gameObject.transform.position;
        _speed = speed;
        gun.Init(ID);
    }

    void FixedUpdate()
    {
        if (death || !StaticGameController.Instance.gameIsPlayed) return;

        if (target != null) FireControl();
        Move();
    }

    private bool fire = false;
    private void FireControl()
    {
        if (!fire && Vector3.Distance(gameObject.transform.position, target.position) < attackDistance)
            StartCoroutine(Fire());

    }

    private IEnumerator Fire()
    {
        fire = true;

        for (int i = 0; i < anim.Length; i++)
            anim[i].SetBool("Fire", true);

        gun.Fire();

        yield return new WaitForSeconds(1f);
        fire = false;

        for (int i = 0; i < anim.Length; i++)
            anim[i].SetBool("Fire", false);
    }

    private void Move()
    {
        float horizMove = -JoystickStick.Instance.VerticalAxis();
        float verticalMove = JoystickStick.Instance.HorizontalAxis();

        if (horizMove == 0.0f && verticalMove == 0.0f)
        {
            if (anim.Length != 0)
            {
                for (int i = 0; i < anim.Length; i++)
                {
                    anim[i].SetBool("Run", false);
                }
            }

            return;
        }

        float angle = Mathf.Atan2(JoystickStick.Instance.HorizontalAxis(), JoystickStick.Instance.VerticalAxis()) * Mathf.Rad2Deg;
        if (!fire) visualPlayer.transform.rotation = Quaternion.Euler(0, angle, 0);
        else
            visualPlayer.transform.LookAt(new Vector3(target.position.x, visualPlayer.transform.position.y, target.position.z));

        transform.transform.position = new Vector3(transform.transform.position.x + verticalMove * _speed, transform.transform.position.y, transform.transform.position.z + (-horizMove * _speed));//(new Vector3(verticalMove * speed, 0, -horizMove * speed), Space.World);

        if (anim.Length != 0)
        {
            for (int i = 0; i < anim.Length; i++)
                if (_speed == speed)
                    anim[i].SetBool("Run", true);
        }
    }

    public void WinGame()
    {
        if (anim.Length != 0)
        {
            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetBool("Run", false);
                anim[i].SetBool("WinGame", true);
            }
        }
    }

    public void LoseGame()
    {
        if (anim.Length != 0)
        {
            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetBool("Run", false);
                anim[i].SetBool("LoseGame", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (death || !StaticGameController.Instance.gameIsPlayed) return;

        //здесь пишем условие смерти и спавним на старт

        if (other.GetComponent<DamageObstacle>())
            StartCoroutine(Death());
    }

    private bool death = false;
    private IEnumerator Death()
    {
        death = true;

        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetBool("Run", false);
            anim[i].SetBool("Fire", false);
            anim[i].SetTrigger("Death");
        }

        if (deathParticle != null) deathParticle.Play();
        yield return new WaitForSeconds(2);

        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        visualPlayer.transform.rotation = Quaternion.identity;

        death = false;
    }
}
