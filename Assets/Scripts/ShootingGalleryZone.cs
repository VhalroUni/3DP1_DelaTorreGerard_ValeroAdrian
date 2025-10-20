using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShootingGalleryZone : MonoBehaviour
{
    public GameObject m_HUD;
    public Text m_ScoreText;
    public Text m_TimerText;
    public float m_TimerPlay = 30f;
    public int m_TargetScore = 100;
    public GameObject m_Door;
    public int m_Score = 0;

    private float m_Timer;

    private void Start()
    {
        m_HUD.SetActive(false);
        m_Timer = m_TimerPlay;
    }
    private void Update()
    {
        if (m_Timer > 0f)
        {
            m_Timer -= Time.deltaTime;
        }
        else
        {
            m_Timer = 0f;
            EndShootingGallery();
        }
        if(m_TimerText != null)
            m_TimerText.text = "Time: " + m_Timer;

        if (Input.GetKeyDown(KeyCode.R))
            RestartShootingGallery();
    }
    private void OnTriggerEnter(Collider player)
    {
        if(player.CompareTag("Player"))
            m_HUD.SetActive(true);
            m_Timer = m_TimerPlay;
            UpdateScoreHUD();
    } 

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
            m_HUD.SetActive(false);
    }
    public void AddScore(int score)
    {
        m_Score += score;
        UpdateScoreHUD();
    }
    void UpdateScoreHUD()
    {
        if (m_ScoreText != null)
            m_ScoreText.text = "Score: " + m_Score;
    }
    private void RestartShootingGallery()
    {
        m_Score = 0;
        m_Timer = m_TimerPlay;
        UpdateScoreHUD();
    }
    private void EndShootingGallery()
    {
        m_HUD.SetActive(false );
    }
}