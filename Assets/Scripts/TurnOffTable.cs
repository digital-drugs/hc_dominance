using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOffTable : MonoBehaviour
{
    [SerializeField] private Button _tableButton;
    [SerializeField] private GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        Button tableButton = _tableButton.GetComponent<Button>();
        tableButton.onClick.AddListener(TurnOnTheTable);
    }

    private void TurnOnTheTable()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
