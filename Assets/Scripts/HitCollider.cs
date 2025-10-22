using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public int m_Damage;
    public EnemyController m_Enemy;

    public void Hit()
    {
        Debug.Log("Disparo acertado");
        m_Enemy.Hit(m_Damage);
    }
}