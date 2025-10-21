using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShootingGalleryZone : MonoBehaviour
{
    public GameObject m_HUD;
    public Text m_ScoreText;
    public Text m_TimerText;
    public Text m_MessageText;
    public float m_TimerPlay = 30f;
    public int m_TargetScore = 100;
    public GameObject m_Door;
    public int m_Score = 0;

    private float m_Timer;
    private bool m_IsPlayerInside = false;
    private List<GameObject> m_Targets;

    private void Start()
    {
        m_HUD.SetActive(false);
        m_MessageText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (m_IsPlayerInside)
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

            m_TimerText.text = "Time: " + Mathf.CeilToInt(m_Timer); //Mathf.CeilToInt pregunta a ChatGPT. Pregunta "En Visual Studio para Unity3D como hago para que los segundos no salgan decimales?"

            if (Input.GetKeyDown(KeyCode.T))
                RestartShootingGallery();
        }
    }
    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            m_IsPlayerInside=true;
            m_HUD.SetActive(true);
            m_MessageText.gameObject.SetActive(true) ;
            m_MessageText.text = "Tienes 30 segundos! Pulsa T para reinciar";
            m_Timer = m_TimerPlay;
            UpdateScoreHUD();
        }
    } 

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            m_IsPlayerInside=false;
            m_HUD.SetActive(false);
        }
    }
    public void AddScore(int score)
    {
        m_Score += score;
        UpdateScoreHUD();
        if(m_Score >= m_TargetScore && m_Door != null)
        {
            m_Door.SetActive(false);
            Destroy(m_Door);
        }
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

    private void RestartTargets(int Count, GameObject PrefabElement)
    {
        m_Targets = new List<GameObject>();
        for (int i = 0; i < Count; i++)
        {
            GameObject l_GameObject = GameObject.Instantiate(PrefabElement);
            l_GameObject.SetActive(false);
            m_Targets.Add(l_GameObject);
        }
    }
}