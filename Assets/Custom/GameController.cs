using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject WallPrf;
    public GameObject SimpleBotPrf;
    public GameObject WallParent;
    public GameObject WallPlane;
    public AstarPath AstarPath;
    public BotGenerator BotGenerator;
    public List<Vector3Int> OpenSpaces;
    public Button StressTestButton;
    public Button ForceScanButton;
    public Button SpawnSimpleBotButton;
    public Button SpawnNotSimpleBotButton;
    public StressTestSeekerController StressTestSeekerController;
    public int BlockScale = 1;

    public int StressTestPathFinderCount = 100;
    public float StressTestPathFinderFindingTimeGapMax = 10f;
    public float StressTestPathFinderFindingTimeGapMin = 3f;
    public int SimpleBotSpawnAmount = 100;
    public int NotSimpleBotSpawnAmount = 100;
    public int MapWidth = 100;
    public int MapHeight = 100;

    // Start is called before the first frame update
    private void Start()
    {
        //스트레스 테스트 기능은 맵이 만들어지고 스캔이 끝난 후 가능하므로 꺼둔다.
        StressTestButton.gameObject.SetActive(false);

        MazeGenerator mazeGenerator = new MazeGenerator();
        var mapData = mazeGenerator.FromDimensions(MapWidth, MapHeight);

        for (int x = 0; x < mapData.GetLength(0); x++)
        {
            for (int z = 0; z < mapData.GetLength(1); z++)
            {
                if (mapData[x, z] == 1)
                {
                    var newWall = Instantiate(WallPrf, new Vector3(x + this.transform.position.x, 0, z + this.transform.position.z) * BlockScale, Quaternion.identity);
                    newWall.transform.SetParent(WallParent.transform);
                    newWall.transform.localScale = new Vector3(1, 4f, 1) * BlockScale;
                    //큰 맵에서 부하를 줄이기 위해 그리기를 꺼준다.
                    newWall.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        // 블럭들을 하나의 메시로 합친다. (그려주기 부담을 줄이자) -> 정점을 합치지 않아서 의미가 없었다. 대신 텍스쳐 한장을 그려서 덮는걸로 변경.
        //MeshMerger.Merge();

        //지형을 텍스쳐로 그려준다.
        WallPlane.transform.position = new Vector3(MapWidth / 2 - BlockScale / 2f, 0.5f, MapHeight / 2 - BlockScale / 2f);
        WallPlane.transform.Rotate(Vector3.up, 180);
        WallPlane.transform.localScale = new Vector3(MapWidth / 10, 1, MapHeight / 10);
        WallPlane.GetComponent<MeshRenderer>().material.mainTexture = WallTextureCreate(mapData);

        //빈방들을 체크해둔다.
        OpenSpaces = GetOpenSpaces(mapData);

        //변경된 미로에 따라 길을 생성한다.
        //bug : 어째서인지, 벽들이 다 생성되기 전에 스캔을 하는 것 같다...
        var gridGraph = AstarPath.graphs.First() as GridGraph;
        gridGraph.SetDimensions(MapWidth, MapHeight, BlockScale);
        gridGraph.center = new Vector3(MapWidth / 2 + BlockScale / 2f, 0, MapHeight / 2 + BlockScale / 2f);

        AstarPath.active.data.recastGraph.forcedBoundsCenter = new Vector3(MapWidth / 2 + BlockScale / 2f, 0, MapHeight / 2 + BlockScale / 2f);
        AstarPath.active.data.recastGraph.forcedBoundsSize = new Vector3(MapWidth, 1, MapHeight);

        AstarPath.Scan();

        //봇들을 미로에 따라 생성한다.
        //BotGenerator.GenerateBots(mapData);

        //스트레스 테스트를 할 수 있게 버튼을 켠다.
        StressTestButton.gameObject.SetActive(true);
        StressTestButton.onClick.AddListener(StressTest);

        //카메라 조정.
        var cam = Camera.main;
        cam.orthographicSize = Mathf.Max(MapWidth, MapHeight) / 2;
        cam.transform.position = new Vector3(MapWidth / 2, 400, MapHeight / 2); //xz 축임에 주의.

        //hack : 위 문제 때문에 강제로 스캔하게 한다.
        ForceScanButton.onClick.AddListener(() => AstarPath.Scan());
        SpawnSimpleBotButton.onClick.AddListener(() => SpawnSimpleBots(SimpleBotSpawnAmount, OpenSpaces));
        SpawnNotSimpleBotButton.onClick.AddListener(() => BotGenerator.GenerateBots(mapData, NotSimpleBotSpawnAmount));
    }

    private void SpawnSimpleBots(int simpleBotSpawnAmount, List<Vector3Int> openSpaces)
    {
        for (int i = 0; i < simpleBotSpawnAmount; i++)
        {
            var spawnSpace = openSpaces[Random.Range(0, openSpaces.Count)];
            var newBot = Instantiate(SimpleBotPrf, spawnSpace, Quaternion.identity);
            //var agent = newBot.GetComponent<SimplePathFindAgent3D>();
            //agent.openSpaces = openSpaces;
        }
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
                    openSpaces.Add(new Vector3Int(x * BlockScale, 0, z * BlockScale));
                }
            }
        }
        return openSpaces;
    }

    public Texture2D WallTextureCreate(int[,] mapData)
    {
        int width = mapData.GetLength(0);
        int height = mapData.GetLength(1);

        var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == 1)
                {
                    texture.SetPixel(x, y, Color.blue);
                }
                else
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }
        }
        texture.Apply();
        return texture;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}