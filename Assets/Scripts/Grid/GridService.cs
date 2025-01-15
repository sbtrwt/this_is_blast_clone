using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blaster.Grid
{
    public class GridService
    {
        private List<List<TileController>> _tiles;
        private int _rows;
        private int _columns;

        public GridService(int rows, int columns, TileView tileView, Transform container)
        {
            _rows = rows;
            _columns = columns;
            _tiles = new List<List<TileController>>();
            for (int i = 0; i < rows; i++)
            {
                _tiles.Add(new List<TileController>());
                for (int j = 0; j < columns; j++)
                { 
                    var tileController = new TileController(tileView, container);
                    _tiles[i].Add(tileController);
                    tileController.SetPosition(new Vector2(i, j));
                }
            }
        }

        public TileController GetTile(int row, int column)
        {
            return _tiles[row][column];
        }

        public List<TileController> GetRow(int row)
        {
            return _tiles[row];
        }

        public List<TileController> GetColumn(int column)
        {
            return _tiles.Select(x => x[column]).ToList();
        }

       
    }
}
