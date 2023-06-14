//�������, ��� ������ if � ������, ��� ������ ����� �������������� ���. 
//������: ���������� ����� � �������� �� 1 000 000 if-�� �������������� �������� 6-10 ���. �� ������� ��������� ����������, � 0,5 ��� �� ����������.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class StaticGameController : MonoBehaviour
{
    private static StaticGameController instance;
    public static StaticGameController Instance => instance;

    [HideInInspector] public bool gameIsPlayed = false;

    [Header("Timer")]
    [SerializeField] private bool useTimer = false;
    [SerializeField] private Text TimeText;
    private int minuts = 0;
    private float second = 0;

    [Header("TimerGoDown (���� useTimer = true)")]
    [SerializeField] private bool useTimerDown = false;
    [SerializeField] private int startMinutCount = 5;

    [Header("TimerGoUp (���� useTimer = true)")]
    [SerializeField] private bool useTimerUp = false;
    [SerializeField] private int maximumMinutCount = 5;


    [Header("Point")]
    [SerializeField] private bool usePointToWin = false;// ����� �� ��, ��� �� ��� ������ ������������� ���������� ����� ����� ���������
    [SerializeField] private int needPointsToWin = 50;
    private int pointValue = 0;
    [SerializeField] private Text PointText;

    [Header("ALL POINT IN GAME")] 
    [SerializeField] private Text allPointText;
    private int allPointValue;

    [Header("Leage")]
    [SerializeField] private Text leageNomberText;
    private int leageNomber;

    [Header("Round Nomber")]
    [SerializeField] private Text roundNomberText;
    private int roundNomber;

    [Header("UI Menu Elements")]
    [SerializeField] private GameObject startLevelPanel;
    [SerializeField] private GameObject[] GamePlayedPanels = new GameObject[1];
    [SerializeField] private GameObject winGamePanel;
    [SerializeField] private GameObject loseGamePanel;

    [Header("��� ������ �� ����� ��� �������?")]
    [SerializeField] private GameObject winNextRoundPanel;
    [SerializeField] private GameObject winGotoMenuGamePanel;

    [Header("Audio")]
    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip pointUpdateAudio;

    [Header("������������ ������ ��� ������")]
    [SerializeField] private bool useStartTimer = false;
    [SerializeField] private float timerInStartValue = 3;
    [SerializeField] private Animator startTimerAnim;

    [Header("��������� ��� ������")]
    [SerializeField] private Animator[] gameStartAnim = new Animator[0];
    [SerializeField] private ParticleSystem[] gameStartParticle = new ParticleSystem[0];

    [Header("��������� ��� ����������")]
    [SerializeField] private Animator[] gameEndAnim = new Animator[0];
    [SerializeField] private ParticleSystem[] gameEndParticle = new ParticleSystem[0];

    [Header("ID ���������� ������")]
    [SerializeField] private int nextLevelID = 0;

    [SerializeField] private UnityEvent gameStarted;
    [SerializeField] private UnityEvent gameEnded;
    [SerializeField] private UnityEvent gameReset;

    public void SetLevelSetting(int _allPoint, int _roundLevel, int _leageLevel)
    {
        allPointValue = _allPoint;
        roundNomber = _roundLevel;
        leageNomber = _leageLevel;

        allPointText.text = allPointValue.ToString();
        roundNomberText.text = roundNomber.ToString();
        leageNomberText.text = leageNomber.ToString();

        SceneAssets.Instance.Init(roundNomber);
    }

    private void Awake()
    {
        instance = this;

        minuts = 0;
        second = 0;
        TimeText.text = $"{minuts}:00";

        if (PointText != null) PointText.text = "0";
        pointValue = 0;

        //��������� ������ ������ � �������� ������ ���������
        startLevelPanel.SetActive(true);
        for (int i = 0; i < GamePlayedPanels.Length; i++)
            GamePlayedPanels[i].SetActive(false);

        winGamePanel.SetActive(false);
        loseGamePanel.SetActive(false);

        //���� ���� ����� SoundManagerAllControll
        //if (SoundManagerAllControll.Instance && backgroundClip) SoundManagerAllControll.Instance.BackgroundClipPlay(backgroundClip);

        //��������
        if (useTimer)
        {
            if (TimeText == null) Debug.LogError("����� ���������� ���� ���� ����� ������������ �����-����");
            if (!useTimerDown && !useTimerUp) Debug.Log("���� ����������� �����, ����� ������� ���� �� ����� ��������� (useTimerUp/useTimerDown), ����� �� ������� ����� ��������� � +, ��� ����������� �� �������");
        }
    }

    private void Update()
    {
        if (!gameIsPlayed) return;

        if (useTimer) TimeGO();
    }

    public void GoToMenuLevel()//���������� �� ��� (������ UI �� ������ ������)
    {
        if (PlayerPrefs.HasKey("AllPoints"))
            PlayerPrefs.SetInt("AllPoints", PlayerPrefs.GetInt("AllPoints") + pointValue);
        else
            PlayerPrefs.SetInt("AllPoints", pointValue);

        SceneManager.LoadScene(0);
        //��� ������������� ��������� �� ���� ������� �������� (���� �������� ����, ��� �� ����� �������� ������ ������ ������������ ��������)
    }

    public void NextLevel()//���������� �� ��� (������ UI �� ������ ������)
    {
        if (PlayerPrefs.HasKey("AllPoints"))
            PlayerPrefs.SetInt("AllPoints", PlayerPrefs.GetInt("AllPoints") + pointValue);
        else
            PlayerPrefs.SetInt("AllPoints", pointValue);

        SceneManager.LoadScene(nextLevelID);
        //��� ������������� ��������� �� ���� ������� �������� (���� �������� ����, ��� �� ����� �������� ������ ������ ������������ ��������)
    }

    public void ResetLevel()//� ���� ��������� ������ ������������ ����� � ������ ���� ����� �������� � ����� ������ ������.
    {
        if (PlayerPrefs.HasKey("AllPoints"))
            PlayerPrefs.SetInt("AllPoints", PlayerPrefs.GetInt("AllPoints") + pointValue);
        else
            PlayerPrefs.SetInt("AllPoints", pointValue);

        SceneManager.LoadScene(SceneManager.sceneCount);//����� �� ��������, �� ����� ��� �����
    }

    public void GameStarted()
    {
        startLevelPanel.SetActive(false);
        for (int i = 0; i < GamePlayedPanels.Length; i++)
            GamePlayedPanels[i].SetActive(true);

        if (useTimer)
        {
            if (useTimerDown) minuts = startMinutCount;
            if (useTimerUp) minuts = 0;
            second = 0;
            TimeText.text = $"{minuts}:00";
        }

        if (PointText != null) PointText.text = "0";
        pointValue = 0;

        if (useStartTimer)
        {
            StartCoroutine(StartTimerActive());
        }
        else
        {
            gameIsPlayed = true;
            gameStarted.Invoke();

            if (gameStartAnim.Length >= 1)
                for (int i = 0; i < gameStartAnim.Length; i++)
                    gameStartAnim[i].SetTrigger("Start");

            if (gameStartParticle.Length >= 1)
                for (int i = 0; i < gameStartAnim.Length; i++)
                    gameStartParticle[i].Play();
        }
    }

    private IEnumerator StartTimerActive()
    {
        startTimerAnim.SetTrigger("Start");

        yield return new WaitForSeconds(timerInStartValue);

        gameIsPlayed = true;
        gameStarted.Invoke();

        if (gameStartAnim.Length >= 1)
            for (int i = 0; i < gameStartAnim.Length; i++)
                gameStartAnim[i].SetTrigger("Start");

        if (gameStartParticle.Length >= 1)
            for (int i = 0; i < gameStartParticle.Length; i++)
                gameStartParticle[i].Play();
    }

    public void GameEnded(bool win = false)
    {
        gameIsPlayed = false;

        gameEnded.Invoke();

        if (gameEndAnim.Length >= 1)
            for (int i = 0; i < gameEndAnim.Length; i++)
                gameEndAnim[i].SetTrigger("Start");

        if (gameEndParticle.Length >= 1)
            for (int i = 0; i < gameEndParticle.Length; i++)
                gameEndParticle[i].Play();

        if (win)
        {
            for (int i = 0; i < GamePlayedPanels.Length; i++)
                GamePlayedPanels[i].SetActive(false);
            winGamePanel.SetActive(true);

            if (roundNomber >= 3)
            {
                winGotoMenuGamePanel.SetActive(true);
                if (leageNomber < 10) PlayerPrefs.SetInt("LeageNomber", leageNomber + 1);
                PlayerPrefs.SetInt("RoundNomber", 1);
                winNextRoundPanel.SetActive(false);
            }
            else
            {
                winNextRoundPanel.SetActive(true);
                PlayerPrefs.SetInt("RoundNomber", roundNomber + 1);
                winGotoMenuGamePanel.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < GamePlayedPanels.Length; i++)
                GamePlayedPanels[i].SetActive(false);
            loseGamePanel.SetActive(true);
            if (leageNomber >= 2) PlayerPrefs.SetInt("LeageNomber", leageNomber - 1);
        }
    }

    public void GameReset()
    {
        SceneManager.LoadScene(SceneManager.sceneCount);//����� �� ��������, �� ����� ��� �����
    }

    [Header("��������� ���������� �����")]
    [SerializeField] private int convector = 1;
    public void UpdatePoint(int point)
    {
        if (!gameIsPlayed) return;

        pointValue += point * convector;
        if (PointText != null) PointText.text = pointValue.ToString();

        //���� ���� ����� SoundManagerAllControll
        //if (SoundManagerAllControll.Instance && pointUpdateAudio != null) SoundManagerAllControll.Instance.ClipPlay(pointUpdateAudio);

        if (usePointToWin && pointValue >= needPointsToWin)
            GameEnded(true);
    }

    private void TimeGO()
    {
        if (!gameIsPlayed) return;

        if (useTimerDown)
        {
            if (second <= 0)
            {
                minuts -= 1;
                second = 59;
            }
            else
                second = Mathf.Clamp(second - Time.deltaTime, 0, 60);

            if (second >= 10)
                TimeText.text = $"{minuts}:{Mathf.CeilToInt(second)}";
            else
                TimeText.text = $"{minuts}:0{Mathf.CeilToInt(second)}";

            if (minuts <= 0 && second <= 0)
                GameEnded();
        }
        else
        {
            if (useTimerUp)
            {
                if (second >= 60)
                {
                    minuts += 1;
                    second = 0;
                }
                else
                    second = Mathf.Clamp(second + Time.deltaTime, 0, 60);

                if (second >= 10)
                    TimeText.text = $"{minuts}:{Mathf.CeilToInt(second)}";
                else
                    TimeText.text = $"{minuts}:0{Mathf.CeilToInt(second)}";

                if (minuts >= maximumMinutCount)
                    GameEnded();
            }
            else
            {
                if (second >= 60)
                {
                    minuts += 1;
                    second = 0;
                }
                else
                    second = Mathf.Clamp(second + Time.deltaTime, 0, 60);

                if (second >= 9)
                    TimeText.text = $"{minuts}:{Mathf.CeilToInt(second)}";
                else
                    TimeText.text = $"{minuts}:0{Mathf.CeilToInt(second)}";
            }
        }
    }
}