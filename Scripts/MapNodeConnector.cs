using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MapNodeConnector : MapNode
{
    public string CostText { get; private set; }
    /// <summary>
    /// The levels that the connector looks up the score for. The sum of the best score of these levels must be above the required score.
    /// </summary>
    public int[] Levels { get; private set; }
    /// <summary>
    /// The score necessary to go through this connector.
    /// </summary>
    public double Score { get; private set; }
    /// <summary>
    /// The node you go to when you can go through the connector.
    /// </summary>
    public int TargetNode { get; private set; }
    /// <summary>
    /// The exit directions out of this Connector that are protected by score requirements.
    /// </summary>
    public int[] ProtectedDirections { get; private set; }

    public MapNodeConnector(int id, int x, int y, int[] targetIds, Area area, string title2, int[] levels, double score, int targetNode, string text, int[] protectedDirections)
        : base(id, x, y, targetIds, area, title:title2, showSelectedNodeImage: false)
    {
        CostText = text;
        Levels = levels;
        Score = score;
        TargetNode = targetNode;
        ProtectedDirections = protectedDirections;
    }

    public override void Perform()
    {
        if(CanPerform())
            GameManager.instance.SetMapNode(TargetNode);
    }

    public bool CanPerform()
    {
        double totalScore = 0;
        foreach (int level in Levels)
        {
            totalScore += GameManager.instance.GetLevel(level).BestScore;
        }
        if (totalScore > Score || GameManager.debugMode)
        {
            return true;
        }
        return false;
    }

    public override int MoveTarget(int direction)
    {
        if (ProtectedDirections.Contains(direction) && !CanPerform())
            return -1;
        if (TargetIds[direction] != -1)
        {
            return TargetIds[direction];
        }
        return -1;
    }

    public override string Text()
    {
        return CostText;
    }
}
