public class Monster : Character
{
    private MonsterData data;

    public void SetData(int monsterId)
    {
        data = InfoMgr.Instance.database.monsters.Find(c => c.monsterId == monsterId);
        id = data.monsterId;
        status.hp = data.maxHp;
    }

    protected override int GetMaxHp()
    {
        return data.maxHp;
    }
}
