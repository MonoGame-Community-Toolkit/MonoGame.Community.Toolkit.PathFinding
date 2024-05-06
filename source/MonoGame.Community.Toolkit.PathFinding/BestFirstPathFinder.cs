// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Community.Toolkit.PathFinding;

public sealed class BestFirstPathFinder : PathFinder
{

    /// <summary>
    /// Initializes a new instance of the <see cref="BestFirstPathFinder"/> class.
    /// </summary>
    /// <param name="map">The map that will be searched for a path.</param>
    public BestFirstPathFinder(Map map) : base(map) { }

    protected override void Step()
    {
        TotalSearchSteps++;
        float smallestDistance = float.PositiveInfinity;
        float currentDistance;

        SearchNode openNode = null;
        foreach (SearchNode node in _openList)
        {
            currentDistance = node.DistanceToGoal;
            if (currentDistance < smallestDistance)
            {
                openNode = node;
                smallestDistance = currentDistance;
            }
        }

        if (openNode is null)
        {
            SearchStatus = SearchStatus.NoPath;
            return;
        }

        DoStep(openNode);
    }
}
