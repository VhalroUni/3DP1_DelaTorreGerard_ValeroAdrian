using UnityEngine;

public class ShootingGalleryZone : MonoBehaviour
{
    public GameObject m_ScoreHUD;

    private void OnTriggerEnter(Collider player)
    {
        if(player.CompareTag("Player"))
            m_ScoreHUD.SetActive(true);
    } 

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
            m_ScoreHUD.SetActive(false);
    }
}