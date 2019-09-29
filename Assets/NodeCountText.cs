using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NodeCountText : MonoBehaviour
{
    private TextMeshProUGUI TMP;

    // Start is called before the first frame update
    private void Start()
    {
        TMP = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Count());
    }

    private IEnumerator Count()
    {
        while (true)
        {
            var aStarPath = AstarPath.active;
            var graphs = aStarPath.graphs;
            var text = "";
            foreach (var graph in graphs)
            {
                text += graph.name;
                text += " : ";
                text += graph.CountNodes();
                text += "  nodes ";
                text += "\r\n";
            }
            TMP.text = text;

            yield return new WaitForSeconds(1);
        }
    }
}