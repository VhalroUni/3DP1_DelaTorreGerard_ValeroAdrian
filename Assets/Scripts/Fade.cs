using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    bool m_IsFadeIn;
    public float m_FadeTime = 0.8f;
    float m_CurrentTime = 0.0f;
    public Image m_FadeImage;
    public delegate void OnFadeEndedFn();
    OnFadeEndedFn m_OnFadeEndedFn;

    private void Update()
    {
        UpdateFade();
    }
    void UpdateFade()
    {
        m_CurrentTime += Time.deltaTime;
        float l_Pct=Mathf.Min(1.0f, m_CurrentTime/m_FadeTime);
        m_FadeImage.color=new Color(0.0f, 0.0f,0.0f,m_IsFadeIn ? l_Pct : 1.0f-l_Pct);
        if (l_Pct == 1.0f)
            m_OnFadeEndedFn();
    }
    public void FadeIn(OnFadeEndedFn _OnFadeEndedFn)
    {
        _Fade(_OnFadeEndedFn, true);
    }
    public void FadeOut(OnFadeEndedFn _OnFadeEndedFn)
    {
        _Fade(_OnFadeEndedFn, false);
    }
    void _Fade(OnFadeEndedFn _OnFadeEndedFn, bool _IsFadeIn)
    {
        m_OnFadeEndedFn = _OnFadeEndedFn;
        m_CurrentTime = 0.0f;
        gameObject.SetActive(true);
        m_IsFadeIn= _IsFadeIn;
        UpdateFade();
    }
}