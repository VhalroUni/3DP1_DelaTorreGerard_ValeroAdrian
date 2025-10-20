using UnityEngine;

public class ShootingGallery : MonoBehaviour
{
    public int m_ScoreValue = 10;
    public ShootingGalleryZone m_Zone;
    public void HitTarget()
    {
        m_Zone.AddScore(m_ScoreValue);
        GameObject.Destroy(gameObject);
    }
}