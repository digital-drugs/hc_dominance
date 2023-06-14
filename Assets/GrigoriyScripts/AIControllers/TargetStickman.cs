using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStickman : MonoBehaviour
{
    [SerializeField] private int healPoint = 1;
    [SerializeField] private int pointForDestroy = 1;

    [Header("Визуал при падении")]
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem particle;

    private bool death = false;

    public void UpdateHeal(int ID, int point)
    {
        healPoint -= point;

        if (!death && healPoint <= 0)
        {
            death = true;
            Debug.LogWarning("Player with ID = " + ID + "Have + 1 Point"); 
            StartCoroutine(Death());
            //обновление таблицы результатов и отправка ID + количество заработанных очков pointForDestroy
        }
    }

    private IEnumerator Death()
    {
        anim.SetTrigger("Death");
        if (particle != null) particle.Play();

        yield return new WaitForSeconds(2);

        gameObject.SetActive(false);
        if (SceneAssets.Instance.GetActiveTarget() <= 0)
            //if (таблица вернула 1, то передаем win)
            StaticGameController.Instance.GameEnded(true);
    }
}
