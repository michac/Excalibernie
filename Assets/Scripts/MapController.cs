using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public int WorldHeight;
    public int WorldWidth;

    public GameObject TilePreFab;
    public GameObject CastlePreFab;

    public PlayerMapController Player;

    private GameObject[] _tiles;


    public void Awake()
    {
        _world = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>();
    }

    // Use this for initialization
    void Start()
    {
        InitializeTiles();
    }

    private void InitializeTiles()
    {
        var tileCount = WorldHeight * WorldWidth;

        _tiles = new GameObject[tileCount];
        for (var i = 0; i < tileCount - 1; i++)
        {
            var position = VectorFromTileIndex(i);

            _tiles[i] = (GameObject)Instantiate(TilePreFab, new Vector3(position.x, position.y, 0.0f), Quaternion.identity);
        }

        _tiles[tileCount - 1] = (GameObject)Instantiate(CastlePreFab, new Vector3(WorldWidth - 1, WorldHeight - 1, 0.0f), Quaternion.identity);
    }

    private Vector2 VectorFromTileIndex(int i)
    {
        var x = i % WorldWidth;
        var y = i / WorldHeight;

        return new Vector2((float)x, (float)y);
    }

    public bool IsMoveCommand()
    {
        return Input.GetKeyUp(KeyCode.W) ||
               Input.GetKeyUp(KeyCode.A) ||
               Input.GetKeyUp(KeyCode.S) ||
               Input.GetKeyUp(KeyCode.D);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsMoveCommand())
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector2 direction;

        if (Input.GetKeyUp(KeyCode.W))
        {
            direction = new Vector2(0, 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            direction = new Vector2(-1, 0);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            direction = new Vector2(0, -1);
        }
        else
        {
            direction = new Vector2(1, 0);
        }

        if (IsValidDestination(Player.transform.position, direction))
        {
            Player.Move(direction);
            EncountersOnMove(Player.transform.position);
        }
    }

    private bool IsValidDestination(Vector3 position, Vector2 direction)
    {
        return position.x + direction.x < WorldWidth &&
               position.y + direction.y < WorldHeight;
    }

    private float _chanceOfRandomEncounter = 0.1f;
    private WorldController _world;

    private void EncountersOnMove(Vector3 position)
    {
        if (IsCastlePosition(position))
        {
            _world.QueueNextBattle("LevelOneBoss",
                "Finally! It feels right to have a hilt again. With my battle magic flowing we should have no trouble with this one!");
            _world.Player.PlayerTemplateName = "Excalibur";
            _world.Player.BaseDamage = 2f;

            SceneManager.LoadScene("Battle");

            return;
        }

        if (Random.value < _chanceOfRandomEncounter)
        {
            _chanceOfRandomEncounter = .2f;
            _world.QueueNextBattle("LevelOne");

            SceneManager.LoadScene("Battle");
        }
        else
        {
            _chanceOfRandomEncounter += .2f;
        }
    }

    private bool IsCastlePosition(Vector3 position)
    {
        return position.x >= (WorldWidth - 1) && position.y >= (WorldHeight - 1);
    }
}
