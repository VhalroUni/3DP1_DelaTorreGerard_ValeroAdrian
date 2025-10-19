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
        if (m_ShieldCount == 100) //Max de escudo
            return false;
        else
            return true;
    }
}