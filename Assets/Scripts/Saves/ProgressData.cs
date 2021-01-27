
[System.Serializable]
public class ProgressData
{
    public int coins;
    public bool[] skinsUnlocked;
    public int classicHighscore;
    public int hardcoreHighscore;
    public int hardcoreHighscoreTime;

    public ProgressData(int coins, bool[] skinsUnlocked, int classicHighscore, int hardcoreHighscore, int hardcoreHighscoreTime)
    {
        this.coins = coins;
        this.skinsUnlocked = skinsUnlocked;
        this.classicHighscore = classicHighscore;
        this.hardcoreHighscore = hardcoreHighscore;
        this.hardcoreHighscoreTime = hardcoreHighscoreTime;
    }

}
