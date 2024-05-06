// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Community.Toolkit.PathFinding;

/// <summary>
/// Represents a single node in a search space.
/// </summary>
/// <param name="Position">The location of the node on the map.</param>
/// <param name="DistanceToGoal">The total estimated distance from this node to the goal.</param>
/// <param name="DistanceTraveled">The total distance traveled from the start to this node.</param>
public record SearchNode(Point Position, int DistanceToGoal, int DistanceTraveled)
{
    public static readonly SearchNode Empty = new SearchNode(new Point(), 0, 0);
}
