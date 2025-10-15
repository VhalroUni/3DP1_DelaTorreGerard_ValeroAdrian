using static UnityEditor.Progress;

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
        //falta implementar que se pueda recoger cuando le falten balas y to la paranoia
        return true;
    }
}