using UnityEngine;

public class ShootingGallery : MonoBehaviour
{
    public int scoreValue = 10;
    private bool isHit = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Reactivar la diana si está desactivada (hit)
            if (isHit)
            {
                gameObject.SetActive(true);
                isHit = false;
            }
        }
    }

    public void HitTarget()
    {
        // Aquí puedes añadir lógica de puntuación si quieres
        isHit = true;
        gameObject.SetActive(false);
    }
}