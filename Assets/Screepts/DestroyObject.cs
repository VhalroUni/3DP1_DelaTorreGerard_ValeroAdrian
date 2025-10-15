using System.Collections;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float m_DestroyOnTime = 0.3f;

   private void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }
    /*IEnumerator DestroyCoroutine()
    {
        Debug.Log("start coroutine");
        yield return new WaitForSeconds(m_DestroyOnTime);
        GameObject.Destroy(gameObject);
    }*/

    IEnumerator DestroyCoroutine()
    {
        Debug.Log("start coroutine");
        yield return new WaitForSeconds(m_DestroyOnTime);
        GameObject.Destroy(gameObject);
    }
}