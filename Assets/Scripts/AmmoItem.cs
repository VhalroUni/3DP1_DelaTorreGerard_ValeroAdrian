
public class AmmoItem : Item
{
    public int m_AmmoCount;
    public override void Pick()
    {
        base.Pick();
        GameManager.GetGameManager().GetPLayer().AddAmmo(m_AmmoCount);
    }
    public override bool CanPick()
    {
        PlayerController m_Player = GameManager.GetGameManager().GetPLayer();
        if (m_Player.m_Ammo >= 120) //Max de balas
            return false;
        else
        return true;
    }
}