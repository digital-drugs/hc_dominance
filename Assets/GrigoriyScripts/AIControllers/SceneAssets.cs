using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssets : MonoBehaviour
{
    private static SceneAssets instance;
    public static SceneAssets Instance => instance;

    [Header("ПУСТЫХ БЫТЬ НЕ ДОЛЖНО!")]
    [SerializeField] private GameObject[] sceneObstacle = new GameObject[1];

    [Header("Стикманы мишени")]
    [SerializeField] private TargetStickman[] targetObject = new TargetStickman[1];
    [SerializeField] private MoveController player;
    [Header("Указать радиус вокруг игрока, где будут бегать боты")]
    [SerializeField] private float visibleDistance = 15;

    [Header("Обязательно подписать этот класс на GameController")]

    [Header("Для теста")]
    [Range(1, 3)] [SerializeField] private int sceneRoundLevel = 1;
    [SerializeField] private int winPlayersCount = 2;

    public List<TargetStickman> activeTargetStickman = new List<TargetStickman>();

    private void Awake()
    {
        instance = this;
    }
    
    public void Init(int roundLevel)
    {
        sceneRoundLevel = roundLevel;

        if (winPlayersCount < targetObject.Length) Debug.LogError("!!! Количество препядствий на сцене не должно быть меньше количества прошедших игроков");

        for (int i = 0; i < targetObject.Length; i++)
            targetObject[i].gameObject.SetActive(false);
        for (int i = 0; i < sceneObstacle.Length; i++)
            sceneObstacle[i].SetActive(false);

        for (int i = 0; i < winPlayersCount + 1; i++)
            if (!targetObject[i].gameObject.activeInHierarchy)
                targetObject[i].gameObject.SetActive(true);

        int objCount = 1;
        switch (sceneRoundLevel)
        {
            case 1:
                objCount = sceneObstacle.Length;
                if (objCount <= 0) objCount = 1;
                if (objCount >= sceneObstacle.Length) objCount = sceneObstacle.Length - 1;
                break;
            case 2:
                objCount = Mathf.CeilToInt(sceneObstacle.Length / 2);
                if (objCount <= 0) objCount = 1;
                if (objCount >= sceneObstacle.Length) objCount = sceneObstacle.Length - 1;
                break;
            case 3:
                objCount = Mathf.CeilToInt(sceneObstacle.Length / 3);
                if (objCount <= 0) objCount = 1;
                if (objCount >= sceneObstacle.Length) objCount = sceneObstacle.Length - 1;
                break;
        }


         for (int i = 0; i < objCount; i++)
             if (!sceneObstacle[i].activeInHierarchy)
                 sceneObstacle[i].SetActive(true);
    }

    public void GameStarted()
    {
        StartCoroutine(CountControl());
    }

    private IEnumerator CountControl()
    {
        if (StaticGameController.Instance.gameIsPlayed)
        {
            yield return new WaitForSeconds(0.05f);

            float distance = 1000;
            int nomber = 0;
            for (int i = 0; i < targetObject.Length; i++)
            {
                if (targetObject[i].gameObject.activeInHierarchy)
                {
                    if (Vector3.Distance(player.transform.position, targetObject[i].gameObject.transform.position) <= visibleDistance)
                    {
                        float factDistance = Vector3.Distance(player.transform.position, targetObject[i].gameObject.transform.position);
                        if (factDistance <= distance)//определяем цель для игрока
                        {
                            distance = factDistance;
                            nomber = i;
                        }

                        if (!activeTargetStickman.Contains(targetObject[i]))//определяем цель для ботов
                            activeTargetStickman.Add(targetObject[i]);
                    }
                    else
                        activeTargetStickman.Remove(targetObject[i]);
                }
                else
                    activeTargetStickman.Remove(targetObject[i]);
            }

            player.NewTarget(targetObject[nomber].transform);

            StartCoroutine(CountControl());
        }
        else
        {
            yield return new WaitForEndOfFrame();
            activeTargetStickman.RemoveRange(0, activeTargetStickman.Count - 1);
        }
    }

    public Transform ChekNewTarget()
    {
        if (activeTargetStickman.Count == 0)
            return targetObject[Random.Range(0, targetObject.Length)].gameObject.transform;

        Transform newTarget = activeTargetStickman[Random.Range(0, activeTargetStickman.Count)].gameObject.transform;
        return newTarget;
    }

    public int GetActiveTarget()
    {
        int nomber = 0;
        for (int i = 0; i < targetObject.Length; i++)
            if (targetObject[i].gameObject.activeInHierarchy)
                nomber += 1;

        return nomber;
    }
}
