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

    private async void Start()
    {
        /*while (true)
        {
            await Wait();
            RequestPathFind();
        }*/
    }

    private void RequestPathFind()
    {
        //MeshRenderer.material.color = Color.red;
        var start = this.transform.position;
        var end = openSpaces[Random.Range(0, openSpaces.Count)];
        Seeker.StartPath(start, end, Callback);
    }

    private void Callback(Path p)
    {
        // MeshRenderer.material.color = Color.green;
        //Debug.Log($"{this.gameObject.name} found path in {p.duration} ms!");
    }

    private async Task Wait()
    {
        //MeshRenderer.material.color = Color.blue;
        await Task.Delay(TimeSpan.FromSeconds(Random.Range(WaitTimeMin, WaitTimeMax)));
    }

    private void Update()
    {
        if (CloseEnough(_target))
        {
            ResetTarget();
            //MeshRenderer.material.color = Color.green;
        }
        else
        {
            transform.Translate(Vector3.Normalize(_target - transform.position) * Speed);
            //MeshRenderer.material.color = Color.blue;
        }
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