using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public PlayerBattleController Player;
    public EnemyController Enemy;

    public Canvas GameOverCanvas;
    public Canvas BattleFinishedCanvas;
    public Canvas[] PlayerActionCanvases;

    private Canvas _activePlayerCanvas;

    public Canvas UpgradeCanvas;
    public Text UpgradeText;

    public Button SuperAttackButton;

    private WorldController _world;

    public void Awake()
    {
        _world = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>();
    }

	// Use this for initialization
	void Start ()
	{
	    var battle = _world.GetNextTemplate();

	    var enemyGameObject = Instantiate(battle.Template.EnemyPreFab);

	    Enemy = enemyGameObject.GetComponent<EnemyController>();

	    var playerTemplate = _world.GetPlayerTemplate();
	    var playerGameObject = Instantiate(playerTemplate.PlayerPreFab);

	    Player = playerGameObject.GetComponent<PlayerBattleController>();

	    foreach (var canvas in PlayerActionCanvases)
	    {
	        if (canvas.name == playerTemplate.CanvasName)
	        {
	            _activePlayerCanvas = canvas;

	        }
	    }
        _activePlayerCanvas.gameObject.SetActive(true);

	    if (!string.IsNullOrEmpty(battle.UpgradeText))
	    {
	        UpgradeCanvas.gameObject.SetActive(true);
	        UpgradeText.text = battle.UpgradeText;

            _activePlayerCanvas.gameObject.SetActive(false);
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFinishedUpgrade()
    {
        UpgradeCanvas.gameObject.SetActive(false);
        _activePlayerCanvas.gameObject.SetActive(true);
    }

    public void OnPlayerAttack()
    {
        _activePlayerCanvas.gameObject.SetActive(false);

        if (!Enemy.Defend(Player.Attack()))
        {
            Enemy.gameObject.SetActive(false);
            BattleFinishedCanvas.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(EnemyActions());
        }
    }

    public void OnPlayerSuperAttack()
    {
        SetSuperAttackCooldown();
        _activePlayerCanvas.gameObject.SetActive(false);

        if (!Enemy.Defend(Player.SuperAttack()))
        {
            Enemy.gameObject.SetActive(false);
            BattleFinishedCanvas.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(EnemyActions());
        }
    }

    private int _cooldownTurns = 0;

    private void SetSuperAttackCooldown()
    {
        _cooldownTurns = 3;
        ShowSuperAttackCooldownText();
    }

    private void SuperAttackCooldownTick()
    {
        if (_cooldownTurns > 0)
        {
            _cooldownTurns -= 1;

            if (_cooldownTurns == 0)
            {
                SuperAttackButton.enabled = true;
                ResetSuperAttackText();
            }
            else
            {
                SuperAttackButton.enabled = false;
                ShowSuperAttackCooldownText();
            }
        }
    }

    private void ResetSuperAttackText()
    {
        SuperAttackButton.enabled = true;

        var buttonText = SuperAttackButton.GetComponentInChildren<Text>();
        buttonText.text = "Big Attack";

        var enabledColor = buttonText.color;

        enabledColor.a = 1f;
        buttonText.color = enabledColor;
    }

    private void ShowSuperAttackCooldownText()
    {
        SuperAttackButton.enabled = false;

        var buttonText = SuperAttackButton.GetComponentInChildren<Text>();
        buttonText.text = "Big Attack (" + _cooldownTurns + ")";

        var disabledColor = buttonText.color;

        disabledColor.a = .5f;
        buttonText.color = disabledColor;
    }

    public void OnPlayerHeal()
    {
        _activePlayerCanvas.gameObject.SetActive(false);

        Player.Heal(4f);

        StartCoroutine(EnemyActions());
    }

    public void OnContinue()
    {
        var battle = _world.GetNextTemplate();

        if (battle.Template.Name == "LevelOneBoss")
        {
            SceneManager.LoadScene("Epilogue");
        }
        else
        {
            SceneManager.LoadScene("Map");
        }
    }

    public void OnRestart()
    {
        _world.Reset();
        SceneManager.LoadScene("Map");
    }

    private IEnumerator EnemyActions()
    {
        yield return new WaitForSeconds(1);

        var isPlayerAlive = Player.Defend(Enemy.Attack());

        yield return new WaitForSeconds(1);

        if (!isPlayerAlive)
        {
            GameOverCanvas.gameObject.SetActive(true);
        }
        else
        {
            SuperAttackCooldownTick();
            _activePlayerCanvas.gameObject.SetActive(true);
        }
    }
}
