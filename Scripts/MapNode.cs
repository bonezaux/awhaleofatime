using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MapNode
{
    public int Id { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int[] TargetIds { get; private set; }
    public int[] LockedTargets { get; private set; }
    public int EntryLevel { get; private set; }
    public bool ShowSelectedNodeImage { get; private set; }
    public string Title { get; private set; }
    public Area LocArea { get; private set; }

    public MapNode(int id, int x, int y, int[] targetIds, Area area, int entryLevel=-1, bool showSelectedNodeImage=true, string title = "", int[] lockedTargets = null)
    {
        Id = id;
        X = x;
        Y = y;
        TargetIds = targetIds;
        EntryLevel = entryLevel;
        ShowSelectedNodeImage = showSelectedNodeImage;
        Title = title;
        if (title == "")
        {
            Title = "Level " + entryLevel;
        }
        LocArea = area;
        LockedTargets = lockedTargets;
        if (lockedTargets == null)
            LockedTargets = new int[] { };
    }

    public virtual void Perform()
    {
        GameManager.instance.SetLevel(EntryLevel);
    }

    public virtual int MoveTarget(int direction)
    {
        if (LockedTargets.Contains(direction) && (GameManager.instance.GetLevel(EntryLevel).BestScore == 0  && !GameManager.debugMode))
            return -1;
        if(TargetIds[direction] != -1)
        {
            return TargetIds[direction];
        }
        return -1;
    }

    public virtual string Text()
    {
        return "Best Score: " + GameManager.instance.GetLevel(EntryLevel).BestScore +
            "\nPar Score: " + GameManager.instance.GetLevel(EntryLevel).SignScore +
            (GameManager.instance.GetLevel(EntryLevel).BestScore == GameManager.instance.GetLevel(EntryLevel).PerfectScore ? 
            "\nPERFECT SCORE" : "");
    }
}
