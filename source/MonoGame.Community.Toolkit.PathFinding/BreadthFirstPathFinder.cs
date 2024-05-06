// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Community.Toolkit.PathFinding;

public sealed class BreadthFirstPathFinder : PathFinder
{

    /// <summary>
    /// Initializes a new instance of the <see cref="BreadthFirstPathFinder"/> class.
    /// </summary>
    /// <param name="map">The map that will be searched for a path.</param>
    public BreadthFirstPathFinder(Map map) : base(map) { }

    protected override void Step()
    {
        TotalSearchSteps++;

        //  Breadth first look at every possible path in the order we see them. So we just grab the first open node
        //  and step with it.
        SearchNode node = _openList[0];
        DoStep(node);
    }
}
