using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    public PlayerData Player;
    private PlayerData _backupPlayer;

    public BattleTemplate[] BattleTemplates;
    private PendingBattle _nextTemplate;

    public PlayerTemplate[] PlayerTemplates;

    public void Awake()
    {
        BackupPlayer();

        DontDestroyOnLoad(gameObject);
    }

    private void BackupPlayer()
    {
        _backupPlayer = new PlayerData(Player);
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnStartGame()
    {
        QueueNextBattle("LevelOne");

        SceneManager.LoadScene("Battle");
    }

    public void Reset()
    {
        Player = new PlayerData(_backupPlayer);
    }

    public void QueueNextBattle(string encounterName, string upgradeText = null)
    {
        var template = BattleTemplates.First(bt => bt.Name == encounterName);
        
        _nextTemplate = new PendingBattle()
        {
            Template = template,
            UpgradeText = upgradeText
        };
    }

    public PendingBattle GetNextTemplate()
    {
        return _nextTemplate;
    }

    public PlayerTemplate GetPlayerTemplate()
    {
        return PlayerTemplates.First(pt => pt.Name == Player.PlayerTemplateName);
    }
}
