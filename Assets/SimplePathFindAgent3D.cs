using UnityEngine;

public class SimplePathFindAgent3D : MonoBehaviour
{
    private Vector3 _target;
    public float Speed = 1;
    public static float CloseTargetThreshold = 1f;
    public MeshRenderer MeshRenderer;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (CloseEnough(_target))
        {
            ResetTarget();
            MeshRenderer.material.color = Color.green;
        }
        else
        {
            transform.Translate(Vector3.Normalize(_target - transform.position) * Speed);
            MeshRenderer.material.color = Color.blue;
        }
    }

    private bool CloseEnough(Vector3 target)
    {
        return Vector3.Magnitude(_target - transform.position) <= CloseTargetThreshold;
    }

    private void ResetTarget()
    {
        _target = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
    }
}