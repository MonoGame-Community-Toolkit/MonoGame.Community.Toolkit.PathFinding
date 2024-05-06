// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Community.Toolkit.PathFinding;

public sealed class Map
{
    private readonly float[] _tiles;

    /// <summary>
    /// Gets the total number of Rows in this map.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the total number of columns in this map.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets or Sets the starting position on the map to begin finding a path from.
    /// </summary>
    public Point StartTile { get; set; }

    /// <summary>
    /// Gets or Sets the ending position on the map to find the path too.
    /// </summary>
    public Point EndTile { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Map"/> class.
    /// </summary>
    /// <param name="columns">The total number of columns in the map.</param>
    /// <param name="rows">The total number of rows in the map.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="columns"/> parameter is less than or equal to zero
    ///
    /// -or -
    ///
    /// Thrown if the <paramref name="rows"/> parameter is less than or equal to zero.
    /// </exception>
    public Map(int columns, int rows)
    {
        if (columns <= 0)
            throw new ArgumentException($"{nameof(columns)} must be greater than zero");

        if (rows <= 0)
            throw new ArgumentException($"{nameof(rows)} must be greater than zero");

        Columns = columns;
        Rows = rows;
        _tiles = new float[columns * rows];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Map"/> class.
    /// </summary>
    /// <param name="columns">The total number of columns in the map.</param>
    /// <param name="rows">The total number of rows in the map.</param>
    /// <param name="tiles">A predefined array representing the tiles of this map.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="columns"/> parameter is less than or equal to zero
    ///
    /// -or -
    ///
    /// Thrown if the <paramref name="rows"/> parameter is less than or equal to zero.
    ///
    /// -or-
    ///
    /// Thrown if the length of the <paramref name="tiles"/> parameter is not equal to column * rows.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Throw if the <paramref name="tiles"/> parameter is <see langword="null"/>
    /// </exception>
    public Map(int columns, int rows, float[] tiles)
    {
        if (columns <= 0)
            throw new ArgumentException($"{nameof(columns)} must be greater than zero");

        if (rows <= 0)
            throw new ArgumentException($"{nameof(rows)} must be greater than zero");

        if (tiles is null)
            throw new ArgumentNullException($"{nameof(tiles)}", $"{nameof(tiles)} cannot be null");

        if (tiles.Length != columns * rows)
            throw new ArgumentException($"{nameof(tiles)} length of {columns * rows} does not match {nameof(columns)} * {nameof(rows)}");


        Columns = columns;
        Rows = rows;
        _tiles = new float[columns * rows];
    }

    /// <summary>
    /// Sets the weight of the tile with the specified id.
    /// </summary>
    /// <remarks>
    /// The weighted value of a tile is clamped between 0.0f and 1.0f. It determines how passable the tile is in
    /// heuristic calculations.  A value of 0.0f means that the tile is passable with no issue, and a value of 1.0f
    /// means the tile is completely impassable.
    /// </remarks>
    /// <param name="tileID">The id of the tile to set the weight of.</param>
    /// <param name="value">The weighted value to set for the tile. Value will be clamped between 0.0f an 1.0f.</param>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the <paramref name="tileID"/> is out of bounds for this map.
    /// </exception>
    public void SetTileWeight(int tileID, float value) => _tiles[tileID] = Math.Max(0.0f, Math.Min(1.0f, value));

    /// <summary>
    /// Sets the weight of the tile at the specified column and row.
    /// </summary>
    /// <remarks>
    /// The weighted value of a tile is clamped between 0.0f and 1.0f. It determines how passable the tile is in
    /// heuristic calculations.  A value of 0.0f means that the tile is passable with no issue, and a value of 1.0f
    /// means the tile is completely impassable.
    /// </remarks>
    /// <param name="column">The column of the tile in this map.</param>
    /// <param name="row">The row of the tile in this map.</param>
    /// <param name="value">The weighted value to set for the tile. Value will be clamped between 0.0f an 1.0f.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="column"/> parameter is less than or equal to zero.
    ///
    /// -or-
    ///
    /// Thrown if the <paramref name="row"/> parameter is less than or equal to zero.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the <paramref name="column"/> and <paramref name="row"/> parameters are positions that are out of
    /// bounds of this map.
    /// </exception>
    public void SetTileWeight(int column, int row, float value)
    {
        if (column < 0)
            throw new ArgumentException($"{nameof(column)} must be greater than zero");

        if (row < 0)
            throw new ArgumentException($"{nameof(row)} must be greater than zero");

        int id = row * Columns + column;
        SetTileWeight(id, value);
    }

    /// <summary>
    /// Sets the weight of the tile at the specified location.
    /// </summary>
    /// <remarks>
    /// The weighted value of a tile is clamped between 0.0f and 1.0f. It determines how passable the tile is in
    /// heuristic calculations.  A value of 0.0f means that the tile is passable with no issue, and a value of 1.0f
    /// means the tile is completely impassable.
    /// </remarks>
    /// <param name="location">The location of the tile in this map.</param>
    /// <param name="value">The weighted value to set for the tile. Value will be clamped between 0.0f an 1.0f.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the X component of the <paramref name="location"/> parameter is less than or equal to zero.
    ///
    /// -or-
    ///
    /// Thrown if the Y component of the <paramref name="location"/> parameter is less than or equal to zero.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the X and/or Y components of the <paramref name="location"/> parameter are out of bounds of this map.
    /// </exception>
    public void SetTileWeight(Point location, float value)
    {
        if (location.X < 0)
            throw new ArgumentException($"{nameof(location)} X coordinate must be greater than zero");
        if (location.Y < 0)
            throw new ArgumentException($"{nameof(location)} Y coordinate must be greater than zero");

        int id = location.X * Columns + location.Y;
        SetTileWeight(id, value);
    }

    /// <summary>
    /// Gets the weight of the tile with the specified id.
    /// </summary>
    /// <param name="tileID">The id of the tile to get the weight of.</param>
    /// <returns>The weight of the tile with the specified id.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the <paramref name="tileID"/> parameter is out the bounds of this map.
    /// </exception>
    public float GetTileWeight(int tileID) => _tiles[tileID];

    /// <summary>
    /// Gets the weight of the tile at the specified column and row.
    /// </summary>
    /// <param name="column">The column of the tile.</param>
    /// <param name="row">The row of the tile.</param>
    /// <returns>The weight of the tile at the specified column and row.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="column"/> parameter is less than or equal to zero.
    ///
    /// -or-
    ///
    /// Thrown if the <paramref name="row"/> parameter is less than or equal to zero.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the <paramref name="column"/> and <paramref name="row"/> parameters are positions that are out of
    /// bounds of this map.
    /// </exception>
    public float GetTileWeight(int column, int row)
    {
        if (column < 0)
            throw new ArgumentException($"{nameof(column)} must be greater than zero");

        if (row < 0)
            throw new ArgumentException($"{nameof(row)} must be greater than zero");

        int id = row * Columns + column;
        return GetTileWeight(id);
    }

    /// <summary>
    /// Gets the weight of the tile at the specified location.
    /// </summary>
    /// <param name="location">The location of the tile in this map.</param>
    /// <returns>The weight of the tile at the specified location.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the X component of the <paramref name="location"/> parameter is less than or equal to zero.
    ///
    /// -or-
    ///
    /// Thrown if the Y component of the <paramref name="location"/> parameter is less than or equal to zero.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the X and/or Y components of the <paramref name="location"/> parameter are out of bounds of this map.
    /// </exception>
    public float GetTileWeight(Point location)
    {
        if (location.X < 0)
            throw new ArgumentException($"{nameof(location)} X coordinate must be greater than zero");

        if (location.Y < 0)
            throw new ArgumentException($"{nameof(location)} Y coordinate must be greater than zero");

        int id = location.X * Columns + location.Y;
        return GetTileWeight(id);
    }

    /// <summary>
    /// Returns a value that indicates whether the specified tile id is contained within this map.
    /// </summary>
    /// <param name="tileID">The id of the tile to check.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="tileID"/> parameter specified is contained within the bounds of
    /// this map; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Contains(int tileID)
    {
        return tileID >= 0 && tileID < _tiles.Length;
    }

    /// <summary>
    /// Returns a value that indicates if the specific column and row are contained within this map.
    /// </summary>
    /// <param name="column">The column to check.</param>
    /// <param name="row">The row to check.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="column"/> and <paramref name="row"/> specified are contained
    /// within the bounds of this map; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Contains(int column, int row)
    {
        return column >= 0 && column <= Columns &&
               row >= 0 && row <= Rows;
    }

    /// <summary>
    /// Returns a value that indicates if the specified location is contained within this map.
    /// </summary>
    /// <param name="location">The location to check.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="location"/> parameter specified is contained within the bounds of
    /// this map; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Contains(Point location) => Contains(location.X, location.Y);

    /// <summary>
    /// Returns a value that indicates if the tile with the specified id is considered open.
    /// </summary>
    /// <remarks>
    /// An open tile is any tile that is passable, meaning it has a weighted value that is less than 1.0f.
    /// </remarks>
    /// <param name="tileID">THe id of the tile to check.</param>
    /// <returns>
    /// <see langword="true"/> if the tile the specified <paramref name="tileID"/> parameter is passable; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    private bool IsOpen(int tileID)
    {
        return Contains(tileID) && GetTileWeight(tileID) < 1.0f;
    }

    /// <summary>
    /// Returns a value that indicates if the tile at the specified column and row is considered open.
    /// </summary>
    /// <remarks>
    /// An open tile is nay tile that is passable, meaning it has a weighted value that is less than 1.0f.
    /// </remarks>
    /// <param name="column">The column of the tile to check.</param>
    /// <param name="row">The row of the tile to check</param>
    /// <returns>
    /// <see langword="true"/> if the tile at the specified <paramref name="column"/> and <paramref name="row"/>
    /// parameters is open; otherwise, <see langword="false"/>.
    /// </returns>
    private bool IsOpen(int column, int row)
    {
        return Contains(column, row) && GetTileWeight(column, row) < 1.0f;
    }

    /// <summary>
    /// Returns a value that indicates whether the tile at the specified location is open.
    /// </summary>
    /// <param name="location">The location of the tile to check.</param>
    /// <returns>
    /// <see langword="true"/> if the tile at the specified <paramref name="location"/> parameter is open; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    private bool IsOpen(Point location)
    {
        return Contains(location) && GetTileWeight(location) < 1.0f;
    }

    /// <summary>
    /// Enumerates all map locations that can be entered from the specified location.
    /// </summary>
    /// <param name="location">The location to check.</param>
    /// <returns>
    /// An enumerator that can be used to enumerate all map locations that can be entered from the
    /// <paramref name="location"/> specified.
    /// </returns>
    public IEnumerable<Point> OpenMapTiles(Point location)
    {
        if (IsOpen(location.X, location.Y + 1))
        {
            yield return new Point(location.X, location.Y + 1);
        }

        if (IsOpen(location.X, location.Y - 1))
        {
            yield return new Point(location.X, location.Y - 1);
        }

        if (IsOpen(location.X + 1, location.Y))
        {
            yield return new Point(location.X + 1, location.Y);
        }

        if (IsOpen(location.X - 1, location.Y))
        {
            yield return new Point(location.X - 1, location.Y);
        }
    }

    /// <summary>
    /// Returns the minimum number of steps to move from one point to another assuming there are no barriers in the way.
    /// </summary>
    /// <param name="from">The point to move from.</param>
    /// <param name="to">The point to move to.</param>
    /// <returns>The minimum number of steps to move.</returns>
    public static int StepDistance(Point from, Point to)
    {
        int dx = Math.Abs(from.X - to.X);
        int dy = Math.Abs(from.Y - to.Y);
        return dx + dy;
    }
}
