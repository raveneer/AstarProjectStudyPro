using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject BlockPrf;
    public AstarPath AstarPath;
    public BotGenerator BotGenerator;
    public List<Vector3Int> OpenSpaces;
    public Button StressTestButton;
    public Button ForceScanButton;
    public StressTestSeekerController StressTestSeekerController;
    public int BlockMapScale = 2;

    // Start is called before the first frame update
    void Start()
    {
        //스트레스 테스트 기능은 맵이 만들어지고 스캔이 끝난 후 가능하므로 꺼둔다.
        StressTestButton.gameObject.SetActive(false);

        MazeGenerator mazeGenerator = new MazeGenerator();
        var mapData = mazeGenerator.FromDimensions(100, 100);

        for (int x = 0; x < mapData.GetLength(0); x++)
        {
            for (int z = 0; z < mapData.GetLength(1); z++)
            {
                if (mapData[x, z] == 1)
                {
                    var newBlock = Instantiate(BlockPrf, new Vector3(x + this.transform.position.x, 0, z + this.transform.position.z ) * BlockMapScale, Quaternion.identity);
                    newBlock.transform.localScale = new Vector3(1, 4f, 1) * BlockMapScale;
                }
            }
        }


        //빈방들을 체크해둔다.
        OpenSpaces = GetOpenSpaces(mapData);

        //변경된 미로에 따라 길을 생성한다.
        //bug : 어째서인지, 벽들이 다 생성되기 전에 스캔을 하는 것 같다...
        AstarPath.Scan();

        //봇들을 미로에 따라 생성한다.
        BotGenerator.GenerateBots(mapData);

        //스트레스 테스트를 할 수 있게 버튼을 켠다.
        StressTestButton.gameObject.SetActive(true);
        StressTestButton.onClick.AddListener(StressTest);

        //hack : 위 문제 때문에 강제로 스캔하게 한다.
        ForceScanButton.onClick.AddListener(() => AstarPath.Scan());
        
    }

    /// <summary>
    /// 임의의 길찾기를 연속 수행하여 그 결과를 출력한다.
    /// </summary>
    /// <returns></returns>
    private void StressTest()
    {
        Debug.Assert(OpenSpaces.Count > 0);
        var start = OpenSpaces[Random.Range(0, OpenSpaces.Count)];
        var end = OpenSpaces[Random.Range(0, OpenSpaces.Count)];
        StressTestSeekerController.ForceSeek(start, end);
    }


    public List<Vector3Int> GetOpenSpaces(int[,] mapData)
    {
        var openSpaces = new List<Vector3Int>();
        for (int x = 0; x < mapData.GetLength(0); x++)
        {
            for (int z = 0; z < mapData.GetLength(1); z++)
            {
                if (mapData[x, z] == 0)
                {
                    openSpaces.Add(new Vector3Int(x* BlockMapScale, 0, z* BlockMapScale));
                }
            }
        }
        return openSpaces;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


