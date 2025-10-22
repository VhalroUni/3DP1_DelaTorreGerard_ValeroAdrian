using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public int m_Damage;
    public EnemyController m_Enemy;
    void Awake()
    {
        if (m_Enemy == null)
            m_Enemy = GetComponentInParent<EnemyController>();
    }
    public void Hit()
    {
        if (m_Enemy != null)
        {
            Debug.Log("Disparo acertado");
            m_Enemy.Hit(m_Damage);
        }
        else
            Debug.Log("No acertado");
     
    }
}