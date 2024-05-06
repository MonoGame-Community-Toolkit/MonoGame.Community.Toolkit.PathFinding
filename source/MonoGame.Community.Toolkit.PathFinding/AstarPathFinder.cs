// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Community.Toolkit.PathFinding;

public sealed class AstarPathFinder : PathFinder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BreadthFirstPathFinder"/> class.
    /// </summary>
    /// <param name="map">The map that will be searched for a path.</param>
    public AstarPathFinder(Map map) : base(map) { }

    protected override void Step()
    {
        SearchNode stepNode = null;

        TotalSearchSteps++;
        float smallestDistance = float.PositiveInfinity;
        float currentDistance;
        foreach (SearchNode node in _openList)
        {
            currentDistance = Heuristic(node);
            if (currentDistance <= smallestDistance)
            {
                if (currentDistance < smallestDistance)
                {
                    stepNode = node;
                    smallestDistance = currentDistance;
                }
                else if (currentDistance == smallestDistance)
                {
                    if ((stepNode is not null && node.DistanceTraveled > stepNode.DistanceTraveled) ||
                        (stepNode is null && node.DistanceTraveled > 0))
                    {
                        stepNode = node;
                        smallestDistance = currentDistance;

                    }
                }
            }

            if (stepNode is not null)
            {
                DoStep(stepNode);
            }
        }
    }

    private static float Heuristic(SearchNode location)
    {
        return location.DistanceTraveled + location.DistanceToGoal;
    }
}
