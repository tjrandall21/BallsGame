using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance {get{return instance;}}
    
    [SerializeField] bool isBattleScene = false;
    [SerializeField] int playerCount = 4;
    public int PlayerCount {get{return playerCount;}}

    public List<PlayerData> players = new List<PlayerData>();

    List<Vector2> spawnLocations = new List<Vector2>{Vector2.zero,Vector2.zero,Vector2.zero,Vector2.zero};
    List<BallController> mainBalls = new List<BallController>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            spawnLocations.Capacity = 4;
        }
    }



    public void LoadBattleScene()
    {
        //empty spawn locations
        spawnLocations = new List<Vector2>{Vector2.zero,Vector2.zero,Vector2.zero,Vector2.zero};
        spawnLocations.Capacity = 4;
        SceneManager.LoadScene(1);
    }

    public void StartBattleWithPlayerCount(int count)
    {
        playerCount = count;
        LoadBattleScene();
    }
    void Start()
    {
        if (isBattleScene)
        {
            Instance.SpawnPlayers();
        }
        if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void AddSpawnLocation(Vector2 location, int index)
    {
        spawnLocations[index] = location;
    }

    void SpawnPlayers()
    {
        mainBalls.Clear();
        for (int i = 0; i < playerCount; i++)
        {
            GameObject ball = Instantiate(players[i].ballPrefab, spawnLocations[i], Quaternion.identity);
            ball.layer = LayerMask.NameToLayer("Player"+(i+1).ToString());
            if (players[i].weaponPrefab != null)
            {
                GameObject weapon = Instantiate(players[i].weaponPrefab, ball.transform);
                weapon.layer = ball.layer;
                weapon.GetComponent<Weapon>().SetUpgrades(players[i].weaponUpgrades);
            }
            BallController ballController = ball.GetComponent<BallController>();
            ballController.Init(players[i].upgrades, players[i].playerNum, Random.Range(0.0f, 360.0f), players[i].playerSprite);

            mainBalls.Add(ballController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
