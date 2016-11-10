using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{
    public GameObject[] ObjectsToPlace { get; private set; }
    public string LevelStartText { get; private set; }
    public string LevelEndText { get; private set; }
    public float SpeedMod { get; private set; }
    public float ComboSpeedMod { get; private set; }
    public int MinDist { get; private set; }
    public int DistRoll { get; private set; }
    public int Lives { get; private set; }
    public double BestScore { get; set; }
    public float LocMod { get; private set; }

    public int[] Placements { get; private set; }
    public int Id { get; private set; }
    public double PerfectScore { get; private set; }
    public double SignScore { get; private set; }
    /// <summary>
    /// Which tile should be used when in this Level.
    /// </summary>
    public int Tile { get; private set; }
    /// <summary>
    /// Which background should be used when in this Level.
    /// </summary>
    public int Background { get; private set; }
    public Level(int id, GameObject[] objectsToPlace, int[] placements, string levelStartText, string levelEndText, float speedMod = 1, int minDist = 2, int distRoll = 3, float comboSpeedMod=0.03f,
        int lives = 3, float locMod=1f, int tile=0, int background=0)
    {
        Id = id;
        LevelStartText = levelStartText;
        LevelEndText = levelEndText;
        SpeedMod = speedMod;
        MinDist = minDist;
        DistRoll = distRoll;
        ComboSpeedMod = comboSpeedMod;
        Lives = lives;
        BestScore = 0;
        LocMod = locMod;

        ObjectsToPlace = objectsToPlace;
        Placements = placements;
        PerfectScore = GameManager.instance.PerfectScore(this, false);
        SignScore = GameManager.instance.PerfectScore(this, true);
        Tile = tile;
        Background = background;
    }

    public List<GameObject> PlacementList()
    {
        List<GameObject> result = new List<GameObject>();

        foreach(int placement in Placements)
        {
            result.Add(ObjectsToPlace[placement]);
        }
        return result;
    }

}
