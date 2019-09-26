using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Pathfinding;
using Random = UnityEngine.Random;

public class SimplePathFindAgent3D : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    public Seeker Seeker;
    public List<Vector3Int> openSpaces;
    private Vector3 _target;
    private float Speed = 1;
    private static float CloseTargetThreshold = 1f;
    private float WaitTimeMin = 3;
    private float WaitTimeMax = 10;
    public static int CalculatingNotFinishedPaths;
    private bool _isWaitCalc = false;

    private async void Start()
    {
        /*while (true)
        {
            await Wait();
            RequestPathFindAndSetTarget();
        }*/
    }

    private async Task Wait()
    {
        //MeshRenderer.material.color = Color.blue;
        await Task.Delay(TimeSpan.FromSeconds(Random.Range(WaitTimeMin, WaitTimeMax)));
    }

    private void Update()
    {
        //경로계산이 끝날 때 까지 대기시킨다. 움직이지 않고 멍때리는 에이전트가 많을수록 길찾기 부하가 심한 것이다.
        if (_isWaitCalc) return;

        if (CloseEnough(_target))
        {
            RequestPathFindAndSetTarget();
            //MeshRenderer.material.color = Color.green;
        }
        else
        {
            transform.Translate(Vector3.Normalize(_target - transform.position) * Speed);
            //MeshRenderer.material.color = Color.blue;
        }
    }

    private void RequestPathFindAndSetTarget()
    {
        //MeshRenderer.material.color = Color.red;
        var start = this.transform.position;
        var end = openSpaces[Random.Range(0, openSpaces.Count)];
        CalculatingNotFinishedPaths++;
        _isWaitCalc = true;
        Seeker.StartPath(start, end, Callback);

        _target = end;
    }

    private void Callback(Path p)
    {
        CalculatingNotFinishedPaths--;
        _isWaitCalc = false;

        //MeshRenderer.material.color = Color.green;
        //Debug.Log($"{this.gameObject.name} found path in {p.duration} ms!");
    }

    private bool CloseEnough(Vector3 target)
    {
        return Vector3.Magnitude(_target - transform.position) <= CloseTargetThreshold;
    }

    private void ResetTarget()
    {
        _target = new Vector3(Random.Range(-400, 400), 0, Random.Range(-400, 400));
    }
}