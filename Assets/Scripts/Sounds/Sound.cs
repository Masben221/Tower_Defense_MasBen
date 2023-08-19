namespace TowerDefense
{
    public enum Sound

    {        
        ArrowShoot = 0,
        ArrowHit = 1,
        
        EnemyDie = 2,
        EnemyWin = 3,
        PlayerWin = 4,
        PlayerLose = 5,
        BGM = 6,
        MagicShoot = 7,
        MagicHit = 8,
        StoneShoot = 9,
        StoneHit = 10,
        BombShoot = 11,
        BombHit = 12,
        BGM_1 = 13,
        BGM_2 = 14,
        BGM_3 = 15,
        BGM_4 = 16,
        FIRE_1 = 17,
        FIRE_2 = 18,
        Slow = 19
    }

    public static class SoundExtensions
    {
        public static void Play(this Sound sound)
        {
            SoundPlayer.Instance.Play(sound);
        }
        public static void Stop(this Sound sound)
        {
            SoundPlayer.Instance.Stop(sound);
        }


    }
}