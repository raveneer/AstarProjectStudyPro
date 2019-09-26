using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Pathfinding;
using Random = UnityEngine.Random;

public class SimplePathFindAgent3D : MonoBehaviour
{
    private Vector3 _target;
    public float Speed = 1;
    public static float CloseTargetThreshold = 1f;
    public MeshRenderer MeshRenderer;
    public Seeker Seeker;
    public float WaitTimeMin = 3;
    public float WaitTimeMax = 10;
    public List<Vector3Int> openSpaces;

    private async void Start()
    {
        while (true)
        {
            await Wait();
            RequestPathFind();
        }
    }

    private void RequestPathFind()
    {
        var start = this.transform.position;
        var end = openSpaces[Random.Range(0, openSpaces.Count)];
        Seeker.StartPath(start, end, Callback);
    }

    private void Callback(Path p)
    {
        //Debug.Log($"{this.gameObject.name} found path in {p.duration} ms!");
    }

    private async Task Wait()
    {
        await Task.Delay(TimeSpan.FromSeconds(Random.Range(WaitTimeMin, WaitTimeMax)));
    }

    //실제로 이동하는 부분은 너무 느리다. 업데이트 연산량이 과도함.
    //1000마리 뿌리면 그냥 이동하는 것 만으로도 40프레임까지 떨어짐.
    //우선 길찾기 함수 호출의 부하만 측정하고 싶으므로, 업데이트를 하지 않고 코루틴만 쓰도록 수정한다.
    /*private void Update()
    {
        /*if (CloseEnough(_target))
        {
            ResetTarget();
            MeshRenderer.material.color = Color.green;
        }
        else
        {
            transform.Translate(Vector3.Normalize(_target - transform.position) * Speed);
            MeshRenderer.material.color = Color.blue;
        }#1#
    }*/

    private bool CloseEnough(Vector3 target)
    {
        return Vector3.Magnitude(_target - transform.position) <= CloseTargetThreshold;
    }

    private void ResetTarget()
    {
        _target = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
    }
}