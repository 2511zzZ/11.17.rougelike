using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;

    public float turnDelay = .1f;

    public static GameManager instance = null;

    private BoardManager boardManager;

    public int playerFoodPoints = 100;

    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 1;

    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardManager = GetComponent<BoardManager>();

        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day" + level;
        levelImage.SetActive(true);
        Invoke(nameof(HideLevelImage), levelStartDelay);

        enemies.Clear();
        boardManager.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level +" days, you starved";
        levelImage.SetActive(true);
        enabled = false;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(turnDelay);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
