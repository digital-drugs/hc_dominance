using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAndGameSetting : MonoBehaviour
{
    [Header("Должны быть на игроке и скрыты")]
    [SerializeField] private playerSkinnedSetting[] playerVisual = new playerSkinnedSetting[1];
    [SerializeField] private playerSkinnedSetting[] WeaponVisual = new playerSkinnedSetting[1];

    private void Start()
    {
        if (PlayerPrefs.HasKey("SkinID"))
        {
            int id = PlayerPrefs.GetInt("SkinID");
            for (int i = 0; i < playerVisual.Length; i++)
            {
                if (playerVisual[i].skin_ID == id)//внутри цикла при наличии нужного скина проводится еще одна проверка на активный в данный момент скин и отключает его, 
                                                  //что бы не получилось что два скина активированны
                {
                    for (int j = 0; j < playerVisual.Length; j++)
                        if (playerVisual[j].skin.activeInHierarchy)
                            playerVisual[j].skin.SetActive(false);

                    playerVisual[i].skin.SetActive(true);
                }
            }
        }


        if (PlayerPrefs.HasKey("WeaponID"))
        {
            int id = PlayerPrefs.GetInt("WeaponID");
            for (int i = 0; i < WeaponVisual.Length; i++)
            {
                if (WeaponVisual[i].skin_ID == id)
                {
                    for (int j = 0; j < WeaponVisual.Length; j++)
                        if (WeaponVisual[j].skin.activeInHierarchy)
                            WeaponVisual[j].skin.SetActive(false);

                    WeaponVisual[i].skin.SetActive(true);
                }
            }
        }


        int leage = 1;
        int round = 1;
        int coint = 0;

        if (PlayerPrefs.HasKey("LeageNomber"))
        {
            leage = PlayerPrefs.GetInt("LeageNomber");
        }
        else
           PlayerPrefs.SetInt("LeageNomber", leage);

        if (PlayerPrefs.HasKey("RoundNomber"))
        {
            round = PlayerPrefs.GetInt("RoundNomber");
        }
        else
            PlayerPrefs.SetInt("RoundNomber", round);

        if (PlayerPrefs.HasKey("AllPoint"))
        {
            coint = PlayerPrefs.GetInt("AllPoint");
        }
        else
            PlayerPrefs.SetInt("AllPoint", coint);


        StaticGameController.Instance.SetLevelSetting(coint, round, leage);

        // тут же ты можешь указать считывание характеристик которые игрок покупает в главном меню
    }
}

[Serializable]
public class playerSkinnedSetting
{
    public GameObject skin;
    public int skin_ID;
}