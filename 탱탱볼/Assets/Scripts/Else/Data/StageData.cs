public class StageData
{
    public int GotCoins;
    public bool Cleared;
    public bool Opened;

    public StageData(bool opened, int gotCoin = 0, bool cleared = false)
    {
        GotCoins = gotCoin;
        Cleared = cleared;
        Opened = opened;
    }
}
