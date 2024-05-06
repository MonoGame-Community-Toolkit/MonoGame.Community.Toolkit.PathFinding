// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Community.Toolkit.PathFinding;

public interface IPathFinder
{
    SearchStatus SearchStatus { get; }
    TimeSpan TimeStep { get; set; }
    bool IsSearching { get; set; }
    int TotalSearchSteps { get; }

    void Update(GameTime gameTime);
    void Reset();
    public bool GetPath(out LinkedList<Point> path);
}
