using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    private bool _isMoved;
    private Collider _col;
    private Bounds _boundsCurrent;
    private Bounds _boundsPrev;
    private Vector3 screenPoint;
    private Vector3 offset;

    // Start is called before the first frame update
    private void Start()
    {
        _col = GetComponent<Collider>();
        Debug.Assert(_col != null);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDisable()
    {
        if (AstarPath.active != null && Application.isPlaying)
        {
            var guo = new GraphUpdateObject(_boundsCurrent);
            AstarPath.active.UpdateGraphs(guo);
        }
    }

    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

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
    }
}