using UnityEngine;
using System.Collections;

public class PlayerMapController : MonoBehaviour
{
    private PlayerData _player;

    void Awake()
    {
        var controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>();

        BindToData(controller.Player);
    }

    private void BindToData(PlayerData player)
    {
        _player = player;
    }

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(_player.MapPosition.x, _player.MapPosition.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;
        _player.MapPosition = transform.position;
    }
}
