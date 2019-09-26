using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculatingNotFinishedPathsText : MonoBehaviour
{
    public TextMeshProUGUI Text;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var calculatingNotFinishedPaths = SimplePathFindAgent3D.CalculatingNotFinishedPaths;
        Text.text = $"CalculatingNotFinishedPaths {calculatingNotFinishedPaths}";
    }
}