using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using TMPro;
using UnityEngine;

public class AgentCountText : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUgui;

    // Start is called before the first frame update
    private void Start()
    {
    }

    //hack : 에이전트 개수를 세는 건 너무 느림. (특히 인터페이스로 세면 모든 모노 비헤이비어 검색을 해야 하므로 극도로 느림)
    //CustomAiDestinationSetter 가 custom ai 당 1개씩 붙어야 하므로 개수를 역산하여 체크함.
    private void Update()
    {
        TextMeshProUgui.text = $"Agents: {CustomAiDestinationSetter.Amount}";
    }
}