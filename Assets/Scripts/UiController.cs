using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject _gamePlayUI;
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private Button _tableButton;

    private void Start()
    {
        Button tableButton = _tableButton.GetComponent<Button>();
        GameObject gamePlayUI = _gamePlayUI.GetComponent<GameObject>();
        GameObject mainMenuUI = _mainMenuUI.GetComponent<GameObject>();
        mainMenuUI.SetActive(true);
    }

}
