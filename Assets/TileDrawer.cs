using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDrawer : MonoBehaviour
{
    public Tilemap Tilemap;
    public Tile BlockTile;

    private void Start()
    {
    }

    private void Update()
    {
        //우클릭일때 타일을 배치하거나, 지움.
        if (Input.GetMouseButtonUp(1))
        {
            var clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var clickIntPos = new Vector3Int((int) clickedPosition.x, (int) clickedPosition.y, 0);

            var drarpos = clickedPosition;
            Debug.Log($"drarpos {drarpos}");
            if (Tilemap.GetTile(clickIntPos) != null)
            {
                Tilemap.SetTile(clickIntPos, null);
            }
            else
            {
                Tilemap.SetTile(clickIntPos, BlockTile);
            }

            var bounds = new Bounds(clickedPosition, new Vector3(3,3,0)); //이 바운드가 아닐 것 같은데 (최소한 중앙이 맞지 않을 것 같아서 3*3으로 크게 잡아줌.)
            if (AstarPath.active != null && Application.isPlaying)
            {
                var guo = new GraphUpdateObject(bounds);
                AstarPath.active.UpdateGraphs(guo);
            }
        }
    }

    private void OnDisable()
    {
        /*if (AstarPath.active != null && Application.isPlaying)
        {
            var guo = new GraphUpdateObject(_boundsCurrent);
            AstarPath.active.UpdateGraphs(guo);
        }*/
    }

    /*
    private void OnMouseDrag()
    {
        //움직이기 시작했을 때만, 기존의 바운드를 저장해둔다.
        if (!_isMoved)
        {
            _boundsPrev = _col.bounds;
        }

        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;
        _isMoved = true;
    }

    private void OnMouseUp()
    {
        if (_isMoved)
        {
            //이동후의 경로 수정해줌
            _boundsCurrent = _col.bounds;
            var guoCurrent = new GraphUpdateObject(_boundsCurrent);
            AstarPath.active.UpdateGraphs(guoCurrent);

            //이동전의 경로 수정해줌.
            var guoPrev = new GraphUpdateObject(_boundsPrev);
            AstarPath.active.UpdateGraphs(guoPrev);

            _isMoved = false;
        }
    }*/
}