using UnityEngine;

public class ShieldItem : Item
{
    public int m_ShieldCount;
    public override void Pick()
    {
        base.Pick();
        GameManager.GetGameManager().GetPLayer().AddShield(m_ShieldCount);
    }
    public override bool CanPick()
    {
        PlayerController m_Player = GameManager.GetGameManager().GetPLayer();
        if (m_Player.m_Shield >= 100) //Max de escudo
            return false;
        else
            return true;
    }
}