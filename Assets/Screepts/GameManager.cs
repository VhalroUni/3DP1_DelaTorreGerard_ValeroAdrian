using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager m_GameManager;
    PlayerController m_Player;
    public Transform m_DestroyObjects;
    public int m_Score = 0;
    public Text m_ScoreText;
    public int m_Life = 0;
    public Text m_LifeText;
    public int m_Shield = 0;
    public Text m_ShieldText;


    private void Awake()
    {
        if (m_GameManager != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        m_GameManager = this;
        DontDestroyOnLoad(gameObject);
    }
    static public GameManager GetGameManager()
    {
        return m_GameManager;
    }
    public void RestartLevel()
    {
        for (int i = 0; i < m_DestroyObjects.childCount; i++)
            GameObject.Destroy(m_DestroyObjects.GetChild(i).gameObject);
        m_Player.Restart();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneManager.LoadSceneAsync("Level1Scene");
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneManager.LoadSceneAsync("Level2Scene");
    }
    public PlayerController GetPLayer()
    {
        return m_Player;
    }
    public void SetPlayer(PlayerController Player)
    {
        m_Player = Player;
    }
    public void AddScore(int value)
    {
        m_Score += value;
        UpdateScoreHUD();
    }
    void UpdateScoreHUD()
    {
        if (m_ScoreText != null)
            m_ScoreText.text = "Score: " + m_Score;
    }
    public void AddLife(int value)
    {
        m_Life += value;
        UpdateLifeHUD();
    }
    void UpdateLifeHUD()
    {
        if (m_LifeText != null)
            m_LifeText.text = "Life: " + m_Life;
    }
    public void AddShield(int value)
    {
        m_Shield += value;
        UpdateShieldHUD();
    }
    void UpdateShieldHUD()
    {
        if (m_ShieldText != null)
            m_ShieldText.text = "Shield: " + m_Shield;
    }

}