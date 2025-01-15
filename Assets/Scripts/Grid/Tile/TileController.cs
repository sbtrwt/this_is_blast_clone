using UnityEngine;

namespace Blaster.Grid
{
    public class TileController
    {
        private TileView _view;
        private Vector3 _position;
        private bool _isOccupied;
        public TileController(TileView view, Transform parent) { 
            _view = GameObject.Instantiate(view, parent);
            _view.Controller = this;    
        }
        public void SetPosition(Vector2 position)
        {
            _position =  new Vector3 (position.x, 0, position.y);
            Debug.Log(_position);
            //_position = position;
            _view.transform.position = _position;
        }

    }
}
