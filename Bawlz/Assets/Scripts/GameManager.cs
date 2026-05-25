using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance {get{return instance;}}
    
    [SerializeField] PlayerData blankPlayerData = null;

    [SerializeField] bool isBattleScene = false;
    [SerializeField] int playerCount = 4;
    public int PlayerCount {get{return playerCount;}}

    public List<PlayerData> players = new List<PlayerData>();
    
    public List<int> playerDeathOrder = new List<int>();

    List<Vector2> spawnLocations = new List<Vector2>{Vector2.zero,Vector2.zero,Vector2.zero,Vector2.zero};
    List<BallController> mainBalls = new List<BallController>();
    [SerializeField] List<Upgrade> passiveStatUpgradesPerRound = new List<Upgrade>();

    bool queueGameOverCheck = false;

    bool gameOver = false;

    int[] placementPoints = {3,2,1,0};

    public int coinsPerRound = 10;
    public int buyPrice = 3;

    public int roundNumber = 1;
    public int maxRounds = 5;

    [SerializeField] bool clearPlayerDataOnStart = false;

    public EndBattlePanel endBattlePanel = null;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public int GetLevelUpThreshold(int level)
    {
        switch (level)
        {
            case 0:
                return 2;
            case 1:
                return 3;
            default:
                Debug.Log("Gamemanager.GetLevelUpThreshold: invalid level provided");
                return 0;
        }
    }

    public void LoadBattleScene()
    {
        //empty spawn locations
        playerDeathOrder = new List<int>();
        spawnLocations = new List<Vector2>{Vector2.zero,Vector2.zero,Vector2.zero,Vector2.zero};
        if (playerCount == 2)
        {
            SceneManager.LoadScene(3); //small arena
        }
        else
        {
            SceneManager.LoadScene(1); //regular arena
        }
        
    }



    public void StartShopWithPlayerCount(int count)
    {
        playerCount = count;
        SceneManager.LoadScene(2);
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
        else if (clearPlayerDataOnStart)
        {
            foreach (PlayerData player in players)
            {
                player.upgrades = new List<Upgrade>();
                player.weaponUpgrades = new List<WeaponUpgrade>();
                player.placementsByRound = new List<int>();
                player.roundsWon = 0;
                player.coins = coinsPerRound;
            }
        }
    }

    public void AddSpawnLocation(Vector2 location, int index)
    {
        spawnLocations[index] = location;
    }

    void SpawnPlayers()
    {
        mainBalls.Clear();
        gameOver = false;
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
            //add passive round upgrades to player upgrades
            List<Upgrade> upgrades  = new List<Upgrade>();
            foreach (Upgrade upgrade in players[i].upgrades)
            {
                upgrades.Add(upgrade);
            }
            for (int j = 0; j < roundNumber-1; j++)
            {
                foreach (Upgrade upgrade in passiveStatUpgradesPerRound)
                {
                    upgrades.Add(upgrade);
                }
            }

            BallController ballController = ball.GetComponent<BallController>();
            ballController.Init(upgrades, players[i].playerNum, Random.Range(0.0f, 360.0f), players[i].playerSprite);
            ballController.isMainBall = true;
            mainBalls.Add(ballController);
        }
    }

    public int GetMainBallCount()
    {
        return mainBalls.Count;
    }

    public List<BallController> GetMainBalls()
    {
        return mainBalls;
    }

    public BallController GetMainBallByNumber(int playerNum)
    {
        mainBalls.RemoveAll(item => item == null);
        foreach (BallController ball in mainBalls)
        {
            if (ball.playerNum == playerNum)
            {
                return ball;
            }
        }
        return null;
    }

    public void ResetPlayers()
    {
        players.Clear(); //reset playerdata
        for (int i = 0; i < playerCount; i++)
        { //generate empty playerdata
            PlayerData player = Instantiate(blankPlayerData);
            player.coins = coinsPerRound;
            player.playerNum = i+1;
            players.Add(player);
        }
    }

    public void ResetPlayers(int playerAmount)
    {
        playerCount = playerAmount;
        ResetPlayers();
    }

    public void CheckGameOver()
    {
        mainBalls.RemoveAll(item => item == null);
        if (!gameOver)
        {
            if (mainBalls.Count == 0) // Draw
            {
                int index = playerDeathOrder.Count-1;
                for (int i = 0; i < 2; i++)
                {
                    int playerNum = playerDeathOrder[playerDeathOrder.Count-1];
                    playerDeathOrder.Remove(playerNum);
                    players[playerNum-1].placementsByRound.Add(1);
                    players[playerNum-1].roundsWon++;
                    index--;
                }

                UpdatePoints();
                gameOver = true;
                endBattlePanel.SetUpPanelForDraw();
                ShowEndScreen();
            }
            else if (mainBalls.Count == 1) // single winner
            {
                int winnerNum = mainBalls[0].playerNum;
                players[winnerNum-1].roundsWon ++;
                players[winnerNum-1].placementsByRound.Add(1);
                for (int i = 0; i < playerDeathOrder.Count; i++)
                {
                    int playerNum = playerDeathOrder[i];
                    players[playerNum-1].placementsByRound.Add(playerCount-i);
                }

                UpdatePoints();
                gameOver = true;
                endBattlePanel.SetUpPanel();
                ShowEndScreen();
            }
        }
    }

    public int GetPointsByPlacement(int placement)
    {
        return placementPoints[placement-1]-(4-playerCount);
    }

    void UpdatePoints()
    {
        foreach (PlayerData player in players)
        {
            player.winPoints = 0;
            foreach (int placement in player.placementsByRound)
            {
                player.winPoints += GetPointsByPlacement(placement);
            }
        }
    }

    void ShowEndScreen()
    {
        endBattlePanel.gameObject.SetActive(true);
    }

    public void EndBattleScene()
    {
        
        if (roundNumber > maxRounds)
        {
            ReturnToMainMenu();
        }
        else
        {
            roundNumber++;

            foreach (PlayerData player in players)
            {
                player.coins = coinsPerRound;
            }
            
            SceneManager.LoadScene(2);
        }
    }

    void ReturnToMainMenu()
    {
        instance = null;
        SceneManager.LoadScene(0);
        Destroy(this);
    }

    void Update()
    {
        if (queueGameOverCheck)
        {
            queueGameOverCheck = false;
            CheckGameOver();
        }
    }

    public void MainBallDied(BallController deadBall)
    {
        mainBalls.Remove(deadBall);
        queueGameOverCheck = true; //delay checking until next frame in case theres a draw
        playerDeathOrder.Add(deadBall.playerNum);
    }
}
