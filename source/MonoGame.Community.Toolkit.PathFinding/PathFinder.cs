// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;


namespace MonoGame.Community.Toolkit.PathFinding;

public abstract class PathFinder
{
    protected readonly List<SearchNode> _openList;
    protected readonly List<SearchNode> _closedList;
    protected readonly Dictionary<Point, Point> _paths;
    protected readonly Map _map;
    private TimeSpan _timeSinceLastSearchStep;

    /// <inheritdoc />
    public SearchStatus SearchStatus { get; protected set; }

    /// <inheritdoc />
    public TimeSpan TimeStep { get; set; } = TimeSpan.FromSeconds(5);

    /// <inheritdoc />
    public bool IsSearching
    {
        get => SearchStatus == SearchStatus.Searching;
        set
        {
            if (SearchStatus == SearchStatus.Searching)
            {
                SearchStatus = SearchStatus.Stopped;
            }
            else if (SearchStatus == SearchStatus.Stopped)
            {
                SearchStatus = SearchStatus.Searching;
            }
        }
    }

    /// <inheritdoc />
    public int TotalSearchSteps { get; protected set; }

    internal PathFinder(Map map)
    {
        SearchStatus = SearchStatus.Stopped;
        _openList = new List<SearchNode>();
        _closedList = new List<SearchNode>();
        _paths = new Dictionary<Point, Point>();
        _timeSinceLastSearchStep = TimeSpan.Zero;
        _map = map;
    }

    /// <inheritdoc />
    public bool GetPath(out LinkedList<Point> path)
    {
        path = new LinkedList<Point>();

        if (SearchStatus != SearchStatus.PathFound)
        {
            return false;
        }

        Point curPrev = _map.EndTile;
        while (_paths.ContainsKey(curPrev))
        {
            curPrev = _paths[curPrev];
            path.AddFirst(curPrev);
        }

        return true;
    }

    /// <inheritdoc />
    public void Reset()
    {
        _openList.Clear();
        _closedList.Clear();
        _paths.Clear();
        _timeSinceLastSearchStep = TimeSpan.Zero;
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        Debug.Assert(gameTime is not null);

        if (SearchStatus is SearchStatus.Searching)
        {
            _timeSinceLastSearchStep += gameTime.ElapsedGameTime;
            if (_timeSinceLastSearchStep >= TimeStep)
            {
                if (_openList.Count > 0)
                {
                    TotalSearchSteps++;
                    Step();
                }
                _timeSinceLastSearchStep = TimeSpan.Zero;
            }
        }
    }

    protected abstract void Step();

    protected void DoStep(SearchNode openNode)
    {
        Debug.Assert(openNode is not null);

        Point curPosition = openNode.Position;
        foreach (Point point in _map.OpenMapTiles(curPosition))
        {
            //  Calculate the distance to the goal
            int dx = Math.Abs(curPosition.X - _map.EndTile.X);
            int dy = Math.Abs(curPosition.Y - _map.EndTile.Y);
            int distanceToGoal = (int)(_map.GetTileWeight(point) * (dx + dy));
            int distanceTraveled = openNode.DistanceTraveled + 1;
            SearchNode tile = new SearchNode(point, distanceToGoal, distanceTraveled);

            if(!InList(CollectionsMarshal.AsSpan(_openList), point) && !InList(CollectionsMarshal.AsSpan(_closedList), point))
            {
                _openList.Add(tile);
                _paths[point] = openNode.Position;
            }
        }

        if (curPosition == _map.EndTile)
        {
            SearchStatus = SearchStatus.PathFound;
        }

        _openList.Add(openNode);
        _closedList.Add(openNode);
    }

    private static bool InList(ReadOnlySpan<SearchNode> list, Point position)
    {
        for(int i = 0; i < list.Length; i++)
        {
            if(list[i].Position == position)
            {
                return true;
            }
        }

        return false;
    }
}
