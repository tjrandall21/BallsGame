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

    [SerializeField] List<PlayerData> players = new List<PlayerData>();

    List<Vector2> spawnLocations = new List<Vector2>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void LoadBattleScene()
    {
        //empty spawn locations
        spawnLocations = new List<Vector2>();
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

    public void AddSpawnLocation(Vector2 location)
    {
        spawnLocations.Add(location);
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            GameObject ball = Instantiate(players[i].ballPrefab, spawnLocations[i], Quaternion.identity);
            GameObject weapon = Instantiate(players[i].weaponPrefab, ball.transform);
            ball.layer = LayerMask.NameToLayer("Player"+(i+1).ToString());
            weapon.layer = ball.layer;
            ball.GetComponent<BallController>().SetUpgrades(players[i].upgrades);
            weapon.GetComponent<Weapon>().SetUpgrades(players[i].weaponUpgrades);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
