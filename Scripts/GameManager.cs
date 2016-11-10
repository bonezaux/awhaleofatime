using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject player;
    private List<Actionable> actionables;
    private List<Actionable> remActionables;
    private List<GameObject> tiles;
    private List<GameObject> remTiles;
    private List<GameObject> doodads;
    private int combo_v;
    public int Combo {
        get {
            return combo_v;
        }
        private set {
            combo_v = value;
            comboText.text = "Combo: " + combo_v;

            if (MaxCombo != 0 && Combo > MaxCombo)
            { 
                for(int loop = MaxCombo; loop > Combo; loop++)
                {
                    Score += loop*0.2;
                }
                Combo = MaxCombo;
            }
        }
    }

    public int MaxCombo { get; set; }

    private double score_v;
    public double Score
    {
        get
        {
            return score_v;
        }
        private set
        {
            score_v = value;
            scoreText.text = "Score: " + score_v;
        }
    }

    public bool levelPaused { get; private set; }

    // Resource Prefabs
    public GameObject[] resourcePrefabs;

    // Tile Prefabs
    public GameObject[] tilePrefabs;

    public GameObject axePrefab;

    public GameObject levelEndPrefab;

    public static GameManager instance;

    public GameObject background;
    public Sprite[] backgroundSprites;

    public Text scoreText;
    public Text comboText;

    public Image SOLImage;
    public Text SOLText;
    public Image EOLImage;
    public Text EOLText;
    
    public Image head1;
    public Image head1lost;
    public Image head2;
    public Image head2lost;
    public Image head3;
    public Image head3lost;

    /// <summary>
    /// The GameObjects used for when something is hit.
    /// </summary>
    public GameObject[] hitText;

    public Canvas worldMap;
    public Image[] areaMaps;
    private MapNode[] mapNodes;
    private int curMapNode;
    public GameObject mapNodeUI;
    public Image nodeTextImage;
    public Image nodeImage;

    public Text debugModeText;

    private Area curArea_v = new Area(0);
    /// <summary>
    /// The current map Area.
    /// </summary>
    public Area CurArea {
        get
        {
            return curArea_v;
        }
        set
        {
            areaMaps[curArea_v.Background].gameObject.SetActive(false);
            curArea_v = value;
            areaMaps[curArea_v.Background].gameObject.SetActive(true);
        }
    }
    public Area[] Areas { get; private set; }

    public Level[] Levels { get; private set; }
    
    public int CurLevel { get; private set; }

    private Dictionary<string, double> LevelData { get; set; }
    public int Lives { get; private set; }
    public const bool debugMode = true;

    public GameManager()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        MaxCombo = 0;
        if (!debugMode)
            debugModeText.gameObject.SetActive(false);
        actionables = new List<Actionable>();
        remActionables = new List<Actionable>();
        tiles = new List<GameObject>();
        remTiles = new List<GameObject>();
        doodads = new List<GameObject>();
        
        EOLImage.gameObject.SetActive(false);
        EOLText.gameObject.SetActive(false);
        levelPaused = true;
        Areas = new Area[2];
        Areas[0] = new Area(0);
        Areas[1] = new Area(1);

        // 0 = cactus
        // 1 = oak
        // 2 = spruce
        // 3 = rock
        // 4 = flower
        // 5 = combo sign
        // 6 = nothing
        // 7 = green combo sign
        // 8 = blue combo sign
        // 9 = yew 
        // 10 = Combo post 20
        Levels = new Level[]
        {
            new Level(101, resourcePrefabs, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                "Press Z to throw axes at those darned cactuses.\n(Full frontal ramming is painful)", "", speedMod: 0.6f, minDist:3, background:1, tile:5),
            new Level(102, resourcePrefabs, new int[] { 0, 0, 3, 0, 1, 1, 3, 0, 1, 0, 3, 1, 0, 3, 3, 1, 1 }, "Don't throw axes at the rocks.\n" +
                "Whales can easily just plough through those, " +
                "when they are as mad as you are, at least.\n" +
                "If you hit 3 trees with your face, or three rocks with your axes, you lose the level.", "", speedMod: 0.8f, minDist:3, background:1, tile:5),
            new Level(103, resourcePrefabs, new int[] { 1, 1, 1, 3, 1, 1, 3, 0, 0, 1, 3, 1, 3, 3, 3, 1, 3, 1, 1, 3, 1, 1, 3 },
                "Higher combo means more points, but also more speed.\n" +
                "When your combo is broken, you lose all bonus points gained up to that level of Combo.", "", speedMod: 0.85f, minDist:3),
            new Level(104, resourcePrefabs, new int[] { 1, 1, 3, 1, 3, 2, 1, 1, 1, 3, 1, 2, 1, 3, 2, 1, 3, 1, 2 },
                "Spruces need two axe hits to fall", "", speedMod: 0.8f, minDist:3),
            new Level(105, resourcePrefabs, new int[] { 1, 1, 1, 1, 1, 1, 2, 1, 3, 1, 3, 1, 3, 6, 6,
                                             5, 2, 1, 3, 2, 2, 3, 1, 3, 2, 2, 1, 1, 3, 1, 6,
                                             5, 6, 1, 3, 1, 1, 2, 1, 3, 1, 1, 6, 5, 6, 2, 2, 2, 6, 2, 3},
                "Combo Signs present a choice: Axe them to lose half your combo, or barrel past them and retain your combo.\n"+
                "This also insures bonus points that are gained from combos above half your current combo, as they will not be lost if your combo is broken.",
                "", speedMod: 0.85f, minDist:3),
            new Level(106, resourcePrefabs, new int[] { 2, 2, 2, 3, 4, 1, 3, 3, 2, 3, 6, 5, 6, 4, 2, 1, 2, 3, 2, 2, 2, 2, 3, 2, 3, 6, 6, 6, 6, 6,
                                             5, 2, 1, 1, 3, 6, 3, 2, 3, 6, 4, 6, 6, 1, 2, 1, 3, 1, 2, 3, 2, 2, 2, 3, 1, 6, 6, 5, 1, 2, 1, 1, 1, 1,
                                             1, 1, 2, 1},
                                             "Don't axe the flowers.", "", speedMod: 0.85f, minDist:3), //Mountain Flower
            new Level(107, resourcePrefabs, new int[] { 1, 1, 1, 3, 1, 1, 3, 3, 1, 3, 2, 3, 1, 1, 1, 1, 3, 1, 1, 1, 3, 1, 1, 1, 3, 1, 1, 1,
                                             6, 5, 1, 3, 1, 1, 1, 6, 3, 1, 1, 3, 1, 1, 1, 3, 1, 3, 1, 1, 3, 1, 3, 1, 1, 1, 6, 6,
                                             5, 1, 3, 1, 3, 2, 1, 3, 1, 1, 1, 1, 3, 1}, "", "", speedMod: 0.9f, minDist:2),
            new Level(108, resourcePrefabs, new int[] { 1, 1, 1, 3, 1, 1, 3, 3, 1, 3, 2, 3, 1, 1, 1, 1, 3, 1, 1, 1, 3, 1, 1, 1, 3, 1, 1, 1,
                                             6, 5, 1, 3, 1, 1, 1, 6, 3, 1, 1, 3, 1, 1, 1, 3, 1, 3, 1, 1, 3, 1, 3, 1, 1, 1, 6, 6,
                                             5, 1, 3, 1, 3, 2, 1, 3, 1, 1, 1, 1, 3, 1}, "", "", speedMod: 0.9f, minDist:2),
            // End of first area.

            new Level(201, resourcePrefabs, new int[] { 0, 0, 3, 0, 1, 1, 0, 1, 0, 3, 2, 3, 3, 1, 3, 2, 3, 2, 2, 3, 0, 3, 1, 0, 3, 1,
                                             6, 5, 6, 2, 2, 2, 1, 1, 3, 1, 1, 1, 0, 1, 2, 6, 6, 5, 6, 1, 3, 1, 3, 1, 1, 1, 1, 6, 2, 1, 2,
                                             1, 3, 6, 1, 1, 0, 6, 6, 5, 6, 1, 2, 1, 1, 3, 1, 6, 6, 1, 3, 1, 2, 1, 3, 1, 6, 6, 5, 0, 0, 3, 6, 2, 1, 6, 2, 2, 3, 6, 1, 1, 0 }, "", "", speedMod: 1f, minDist:3),
            new Level(202, resourcePrefabs, new int[] { 2, 2, 2, 2, 2, 3, 6, 2, 2, 3, 1, 2, 6, 6, 6, 8, 6, 2, 1, 3, 6, 2, 2, 3, 6, 2, 6, 6, 6,
                                             8, 6, 2, 2, 2, 3, 6, 2, 2, 3, 1, 2, 1, 2, 3, 3, 6, 1, 3, 6, 2, 3, 3, 6, 1, 2, 1, 6, 6, 6,
                                             8, 4, 2, 3,6, 2, 2, 3, 6, 2}, "The Blue Combo Sign only halves your combo.", "", speedMod: 0.85f, minDist:3, locMod:0.9f),
            new Level(203, resourcePrefabs, new int[] { 3, 9, 3, 9, 3, 9, 6, 6, 6, 3, 9, 3, 9, 3, 9, 6, 6, 6, 3, 9, 3, 9, 3, 9,}, "", "",  minDist:2),
            new Level(204, resourcePrefabs, new int[] { 9, 9, 3, 2, 3, 2, 3, 6, 6, 3, 2, }, "", "", speedMod:1.0f,  minDist:4, distRoll:0, locMod:1.5f, tile:10),
            new Level(205, resourcePrefabs, new int[] { }, "", ""),
            new Level(206, resourcePrefabs, new int[] { }, "", ""),
            new Level(207, resourcePrefabs, new int[] { }, "", ""),
            new Level(208, resourcePrefabs, new int[] { }, "", ""),
            //End of Spoopy Forest
            
            new Level(1001, resourcePrefabs, new int[] { 4, 4, 3, 6, 1, 4, 2, 2, 4, 2, 6, 6, 5, 6, 2, 3, 6, 2, 1, 6, 1, 2, 6, 3, 6, 6, 5, 6,
                                                         4, 4, 4, 6, 2, 6, 6, 2, 3, 4, 6, 6, 5, 6, 2, 6, 2, 2, 2, 1, 1, 6, 1, 6, 1, 1, 1, 6, 1, 6, 1, 6,
                                                         1, 2, 6, 2, 6, 6, 5, 6, 2, 6, 2, 1, 2, 6, 2, 1, 4}, "", "", background:2, tile:11),
            //End of Void Levels

        };

        double totalScore = 0;
        double signScore = 0;
        for(int loop=0;loop<7;loop++)
        {
            totalScore += Levels[loop].PerfectScore;
            signScore += Levels[loop].SignScore;
        }
        Debug.Log("1-7 perfect score:" + totalScore);
        Debug.Log("1-7 par score:" + signScore);

        // Dirs: Up, right, down, left
        mapNodes = new MapNode[]
        {
            new MapNode(101, 588, 768-640, new int[] { 102, 102, 0, 0 }, Areas[0], 101, lockedTargets: new int[] {0, 1 }, title:"LEVEL 1.1"),
            new MapNode(102, 624, 768-604, new int[] { 103, 103, 101, 101 }, Areas[0], 102, lockedTargets: new int[] {0, 1 }, title:"LEVEL 1.2"),
            new MapNode(103, (292-128)*4, 768-143*4, new int[] { 104, 104, 102, 102 }, Areas[0], 103, lockedTargets: new int[] {0, 1 }, title:"LEVEL 1.3"),
            new MapNode(104, (301-128)*4, 768-134*4, new int[] { 105, 106, 103, 103 }, Areas[0], 104, lockedTargets: new int[] {1 }, title:"LEVEL 1.4"),
            new MapNodeConnector(105, (307-128)*4, 768-122*4, new int[] { 201, 0, 104, 0 }, Areas[0], "Tunnel", new int[] { 101, 102, 103, 104, 105, 106, 107 }, 
                400, 201, "Get 400 total score in levels 1.1-1.7 to open.\n" +
                "levels 1-7 par score: 458", new int[] { 0 }),
            new MapNode(106, (319-128)*4, 768-134*4, new int[] { 107, 0, 108, 104 }, Areas[0], 105, lockedTargets: new int[] {0, 2 }, title:"LEVEL 1.5"),
            new MapNode(107, (344-128)*4, 768-114*4, new int[] { 0, 109, 0, 106 }, Areas[0], 106, title:"LEVEL 1.6"),
            new MapNode(108, (329-128)*4, 768-144*4, new int[] { 106, 0, 0, 106 }, Areas[0], 107, title:"LEVEL 1.7"),
            new MapNode(109, (360-128)*4, 768-130*4, new int[] { 107, 0, 0, 107 }, Areas[0], 1001, title:"VOID LEVEL 1"),
            // End of The Shore.

            new MapNode(201, (299-128)*4, 768-111*4, new int[] { 202, 105, 105, 207 }, Areas[1], 201, lockedTargets: new int[] {0}, title:"LEVEL 2.1"),
            new MapNode(202, (297-128)*4, 768-81*4, new int[] { 203, 0, 201, 0 }, Areas[1], 202, lockedTargets: new int[] {0}, title:"LEVEL 2.2"),
            new MapNode(203, (309-128)*4, 768-59*4, new int[] { 205, 204, 202, 205 }, Areas[1], 203, lockedTargets: new int[] {1}, title:"LEVEL 2.3"),
            new MapNode(204, (334-128)*4, 768-68*4, new int[] { 203, 0, 0, 203 }, Areas[1], 204, title:"LEVEL 2.4"),
            new MapNode(205, (278-128)*4, 768-46*4, new int[] { 206, 203, 203, 206 }, Areas[1], 205, title:"LEVEL 2.5"),
            new MapNode(206, (255-128)*4, 768-40*4, new int[] { 0, 205, 205, 0 }, Areas[1], 206, title:"LEVEL 2.6"),
            new MapNode(207, (278-128)*4, 768-91*4, new int[] { 0, 201, 201, 208 }, Areas[1], 207, title:"LEVEL 2.7"),
            new MapNode(208, (262-128)*4, 768-97*4, new int[] { 209, 207, 0, 0 }, Areas[1], 208, title:"LEVEL 2.8"),
            new MapNodeConnector(209, (260-128)*4, 768-85*4, new int[] { 301, 0, 208, 0 }, Areas[1], "Palisade Gate", new int[] { 201, 202, 203, 204, },
                400, 301, "Get Bleigh total score in levels 2.1-2.8 to open.\n" +
                "levels 2.1-2.8 par score: Bleigh", new int[] { 0 }),
            // End of Spoopy Forest.
            
        };
        SetMapNode(101);
	}



    // Update is called once per frame
    void Update ()
    {
        if (!levelPaused)
        {
            player.transform.Translate(1 * GetPlayerSpeedMod() * Time.deltaTime * 64*7.5f, 0, 0);

            foreach (Actionable a in remActionables)
            {
                if(a != null)
                a.Die();
                actionables.Remove(a);
            }
            remActionables.Clear();

            UpdateTiles();
        }
    }
    private void UpdateTiles()
    {
        int maxX = -10;
        foreach(GameObject tile in tiles)
        {
            if (tile.transform.position.x/64 < player.transform.position.x/64-13)
            {
                remTiles.Add(tile);
            }
            if(tile.transform.position.x/64 > maxX)
            {
                maxX = (int)tile.transform.position.x/64;
            }
        }

        foreach(GameObject remTile in remTiles)
        {
            tiles.Remove(remTile);
            Destroy(remTile);
        }

        remTiles.Clear();

        for(int loop=maxX+1;loop<player.transform.position.x/64+25;loop++)
        {
            GameObject tileObject = Instantiate(tilePrefabs[GetLevel(CurLevel).Tile]);
            tileObject.transform.Translate(loop*64, -40, -4);
            tiles.Add(tileObject);
        }
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("z")) || Event.current.Equals(Event.KeyboardEvent("x")))
        {
            if(!levelPaused)
                ThrowAxe();
            else
            {
                if(SOLImage.gameObject.activeSelf)
                {
                    SOLImage.gameObject.SetActive(false);
                    SOLText.gameObject.SetActive(false);
                    levelPaused = false;
                }
                else if (EOLImage.gameObject.activeSelf)
                {
                    EOLImage.gameObject.SetActive(false);
                    EOLText.gameObject.SetActive(false);
                    worldMap.gameObject.SetActive(true);
                    UpdateMapNodeText();
                }
                else if (worldMap.gameObject.activeSelf)
                {
                    mapNodes[curMapNode].Perform();
                }

            }
        }
        if (Event.current.Equals(Event.KeyboardEvent("r")))
        {
            if(!levelPaused)
            {
                SetLevel(CurLevel);
            }
        }
        else if(worldMap.gameObject.activeSelf)
        {

            if (Event.current.Equals(Event.KeyboardEvent("up")))
            {
                int target = mapNodes[curMapNode].MoveTarget(0);
                SetMapNode(target);
            }
            else if (Event.current.Equals(Event.KeyboardEvent("right")))
            {
                int target = mapNodes[curMapNode].MoveTarget(1);
                SetMapNode(target);
            }
            else if (Event.current.Equals(Event.KeyboardEvent("down")))
            {
                int target = mapNodes[curMapNode].MoveTarget(2);
                SetMapNode(target);
            }
            else if (Event.current.Equals(Event.KeyboardEvent("left")))
            {
                int target = mapNodes[curMapNode].MoveTarget(3);
                SetMapNode(target);
            }
        }
    }
    public void SetMapNode(int target)
    {
        int mapNodeIndex = -1;
        for(int loop=0;loop<mapNodes.Length;loop++)
        {
            if (mapNodes[loop].Id == target)
            {
                mapNodeIndex = loop;
                break;
            }
        }

        Debug.Log("Target: " + target + ", Index: " + mapNodeIndex);

        if (mapNodeIndex != -1)
        {
            this.curMapNode = mapNodeIndex;
            mapNodeUI.transform.Translate(new Vector3(mapNodes[curMapNode].X, mapNodes[curMapNode].Y, 0) - mapNodeUI.transform.position);
            if (mapNodes[curMapNode].LocArea != CurArea)
            {
                CurArea = mapNodes[curMapNode].LocArea;
            }
            UpdateMapNodeText();
            Debug.Log("Ssni:" + mapNodes[curMapNode].ShowSelectedNodeImage);
            this.nodeImage.gameObject.SetActive(mapNodes[curMapNode].ShowSelectedNodeImage);
        }
    }
    public void UpdateMapNodeText()
    {
        if(this.nodeTextImage.gameObject.activeSelf)
        {
            GameObject.Find("MapNodeTitle").GetComponent<Text>().text = mapNodes[curMapNode].Title;
            GameObject.Find("MapNodeText").GetComponent<Text>().text = mapNodes[curMapNode].Text(); 
        }
    }
    /// <summary>
    /// Creates text that shows how well something was hit.
    /// 0=hit
    /// 1=great
    /// 2=perfect
    /// 3=FAIL
    /// </summary>
    /// <param name=""></param>
    /// <param name="hitText">The kind of text object to use.</param>
    /// <param name="location">Where the text should appear</param>
    public void HitText(int hitText, Vector3 location)
    {
        if(this.hitText[hitText] != null)
        {
            GameObject result = Instantiate(this.hitText[hitText]);
            result.transform.Translate(location);
            AddDoodad(result);
        }
    }

    public void RemoveActionable(Actionable actionable)
    {
        remActionables.Add(actionable);
        actionable.enabled = false;
    }
    public void AddDoodad(GameObject doodad)
    {
        doodads.Add(doodad);
    }

    public float GetPlayerSpeedMod()
    {
        return (1f + Combo * GetLevel(CurLevel).ComboSpeedMod)*GetLevel(CurLevel).SpeedMod;
    }
    
    /// <summary>
    /// Instantiates the objects of the given Level.
    /// </summary>
    /// <param name="level"></param>
    public void CreateLevel(Level level)
    {
        List<GameObject> levelObjects = level.PlacementList();
        int location = 7;
        System.Random r = new System.Random(level.Id);
        foreach(GameObject obj in levelObjects)
        {
            if(obj != null)
            {
                GameObject instantiateObject = Instantiate(obj);
                instantiateObject.transform.Translate(location * 64 * level.LocMod, 0, 0);
                actionables.Add(instantiateObject.GetComponent<Actionable>());
            }
            location += level.MinDist + r.Next(level.DistRoll);
        }
        location += 7;
        GameObject levelEnd = Instantiate(levelEndPrefab);
        levelEnd.transform.Translate(location*64*level.LocMod, 0, 0);
        doodads.Add(levelEnd);
    }
    /// <summary>
    /// Changes Level to a given Level id.
    /// </summary>
    /// <param name="level"></param>
    public void SetLevel(int level)
    {
        Debug.Log("LEVEL: " + level);
        ClearLevelData();
        worldMap.gameObject.SetActive(false);
        Debug.Log("New Level: " + level);
        CurLevel = level;
        Lives = GetLevel(level).Lives;
        UpdateLives();
        SOLImage.gameObject.SetActive(true);
        SOLText.text = "LEVEL " + level + "\n" +
            "----------------------------------------\n" +
            GetLevel(level).LevelStartText;
        SOLText.gameObject.SetActive(true);
        levelPaused = true;
        background.GetComponent<SpriteRenderer>().sprite = backgroundSprites[GetLevel(CurLevel).Background];

        UpdateTiles();
        CreateLevel(GetLevel(level));
    }
    /// <summary>
    /// Clears all doodads, tiles and the like in existence, and resets score and combo.
    /// </summary>
    private void ClearLevelData()
    {
        player.gameObject.transform.Translate(-player.gameObject.transform.position - new Vector3(0, 0, 0));
        MaxCombo = 0;
        foreach (GameObject doodad in doodads)
        {
            Destroy(doodad);
        }
        doodads.Clear();
        foreach (Actionable actionable in actionables)
        {
            Destroy(actionable.gameObject);
        }
        actionables.Clear();
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
        tiles.Clear();


        Combo = 0;
        Score = 0;
        LevelData = new Dictionary<string, double>();
        Lives = 3;
    }
    public void EndLevel(bool lost)
    {
        if(lost)
        {
            CurLevel -= 1;
        }
        else
        {
            if(Score > GetLevel(CurLevel).BestScore)
            {
                GetLevel(CurLevel).BestScore = Score;
            }
        }
        EOLImage.gameObject.SetActive(true);
        EOLText.text = "Level: " + CurLevel + "\n" +
            "----------------------------------------\n" +
            (LevelData.ContainsKey("Oak Logs") ? ("Oak logs gathered: " + LevelData["Oak Logs"] + "\n") : "") +
            (LevelData.ContainsKey("Spruce Logs") ? ("Spruce logs gathered: " + LevelData["Spruce Logs"] + "\n") : "") +
            (LevelData.ContainsKey("Cactus Needles") ? ("Cactus needles gathered: " + LevelData["Cactus Needles"] + "\n") : "") +
            (LevelData.ContainsKey("Broken Axes") ? ("Axes broken against rocks: " + LevelData["Broken Axes"] + "\n") : "") +
            (LevelData.ContainsKey("Full Frontal Rammings") ? ("Trees rammed frontally: " + LevelData["Full Frontal Rammings"] + "\n") : "") +
            "----------------------------------------\n" +
            (lost ? "Unfortunately, you lost." : GetLevel(CurLevel).LevelEndText);
        EOLText.gameObject.SetActive(true);
        levelPaused = true;
        ClearLevelData();
    }
    public Level GetLevel(int id)
    {
        foreach (Level level in Levels)
        {
            if (level.Id == id)
                return level;
        }
        return null;
    }

    /// <summary>
    /// Returns the perfect score for a Level.
    /// </summary>
    /// <param name="level">Level Id</param>
    /// <returns></returns>
    public double PerfectScore(Level level, bool comboSigns)
    {
        ClearLevelData();
        CreateLevel(level);
        foreach (Actionable actionable in actionables)
        {
            actionable.PerformSuccesfully(comboSigns);
        }
        double result = Score;
        ClearLevelData();
        return result;
    }
    public void UpdateLives()
    {
        if (Lives < 3)
        {
            head1.gameObject.SetActive(false);
            head1lost.gameObject.SetActive(true);
            if (Lives < 2)
            {
                head2.gameObject.SetActive(false);
                head2lost.gameObject.SetActive(true);
                if (Lives < 1)
                {
                      head3.gameObject.SetActive(false);
                      head3lost.gameObject.SetActive(true);
                }
            }
        }
        if (Lives > 2)
        {

            head3.gameObject.SetActive(true);
            head3lost.gameObject.SetActive(false);
            if (Lives > 1)
            {

                head2.gameObject.SetActive(true);
                head2lost.gameObject.SetActive(false);
                if (Lives > 0)
                {

                    head1.gameObject.SetActive(true);
                    head1lost.gameObject.SetActive(false);
                }
            }
        }
        if (Lives == 0)
            EndLevel(true);
    }
    public void ThrowAxe()
    {
        GameObject thrownAxe = Instantiate(axePrefab);
        thrownAxe.GetComponent<Axe>().SpeedMod = GetPlayerSpeedMod();
        thrownAxe.transform.Translate(player.transform.position);
        AddDoodad(thrownAxe);
    }

    public void AddLevelData(string data, double amount)
    {
        if (!LevelData.ContainsKey(data))
            LevelData[data] = 0;
        LevelData[data] += amount;
    }
    public void LoseLife()
    {
        Lives--;
        UpdateLives();
    }
    public void AddScore(double score, string message)
    {
        Score += score;
    }
    public void AddCombo(int combo)
    {
        Combo += combo;
    }
    public void ResetCombo()
    {
        double removedScore = 0;
        for (int loop = 0; loop < Combo; loop++)
        {
            removedScore += loop * 0.1;
        }
        AddScore(-removedScore, "nooo! you lost your Combo!");
        Combo = 0;
    }
}
