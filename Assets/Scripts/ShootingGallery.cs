using UnityEngine;

public class ShootingGallery : MonoBehaviour
{
    public int m_ScoreValue = 10;
    public void HitTarget()
    {
        GameManager.GetGameManager().AddScore(m_ScoreValue);
        GameObject.Destroy(gameObject);
    }
}