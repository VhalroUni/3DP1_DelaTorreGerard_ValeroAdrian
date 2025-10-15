using UnityEngine;

public class LifeItem : Item
{
    public int m_LifeCount;
    public override void Pick()
    {
        base.Pick();
        GameManager.GetGameManager().GetPLayer().AddLife(m_LifeCount);
    }
    public override bool CanPick()
    {
        //falta implementar que se pueda recoger cuando le falten vidas y to la paranoia
        return true;
    }
}
