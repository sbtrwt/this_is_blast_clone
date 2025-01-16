using UnityEngine;

namespace Blaster.Grid
{
    public class TileController
    {
        public TileView _tileView { get; private set; }
        private Transform _container;

        public TileController(TileView tileView, Transform container)
        {
            _tileView = GameObject.Instantiate(tileView, container);
            _tileView.Controller = this;
            _container = container;
        }

        public void SetPosition(Vector2 position)
        {
            _tileView.transform.position = new Vector3(position.x, position.y, 0);
        }

        public Vector2 GetPosition()
        {
            return _tileView.transform.position;
        }
        public Transform GetTransform()
        {
            return _tileView.transform;
        }
    }

}
