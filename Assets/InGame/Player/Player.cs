using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public enum PlayerColor { R = 0, G, B, Y, }

public class Player : NetworkBehaviour
{
    public delegate void OnSpawnEvent(int playerId, GameObject player);
    public static event OnSpawnEvent OnSpawn;
    public delegate void OnDieEvent(int playerId, GameObject player);
    public static event OnDieEvent OnDie;

    public NetworkIdentity networkId;
    public Rigidbody2D rb;
    public GameObject[] modelsByColor;
    public PlayerItemShooter itemShooter;

    public CameraMove mainCamera;
    
    public AudioClip[] audioclips;

    private AudioSource audiosource;
    private int serverPlayerId;

    void Awake()
    {
        // DontDestroyOnLoad(transform.gameObject);
        networkId = GetComponent<NetworkIdentity>();
        rb = GetComponent<Rigidbody2D>();
        audiosource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (networkId != null && networkId.isLocalPlayer)
        {
            var playerLocal = gameObject.AddComponent<PlayerLocal>();
            playerLocal.player = this;
            var inputProcessor = gameObject.AddComponent<PlayerInputProcessor>();
            inputProcessor.player = this;
            SetCamera();
            int id;
            try
            {
                id = Lobby.playerId;
            }
            catch
            {
                id = 0;
            }
            CmdBroadcastId(id);
        }
    }

    [Command]
    void CmdBroadcastId(int id)
    {
        HostGameManager.instance.OnPlayerSpawn(id, gameObject);
        RpcInitialize(id);
    }

    [ClientRpc]
    void RpcInitialize(int id)
    {
        serverPlayerId = id;
        SetColor(id);

        if (OnSpawn != null) OnSpawn(id, this.gameObject);
    }

    void OnDestroy()
    {
        if (OnDie != null) OnDie(serverPlayerId, this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // tag "Player"
        if (coll.gameObject.tag == "Player")
        {
            audiosource.PlayOneShot(audioclips[0], 1.0f);
            SetTackleEffect();
        }
        // layer number 12 : terrain
        else if (coll.gameObject.layer == 12)
        {
            audiosource.PlayOneShot(audioclips[1], 1.0f);
            SetTackleEffect();
        }
    }
    void SetTackleEffect()
    {
        GameObject obj = EffectSpawner.instance.GetEffect("tackle");
        obj.transform.position = transform.position + Vector3.back * 0.6f;
        obj.SetActive(true);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        // layer number 9 : item
        if (coll.gameObject.layer == 9)
        {
            audiosource.PlayOneShot(audioclips[2], 0.4f);
        }
        // layer number 11 : projectile (missile)
        else if (coll.gameObject.layer == 11)
        {
            audiosource.PlayOneShot(audioclips[3], 1.0f);
        }
    }

    public void SetColorObject(PlayerColor color)
    {
        if (modelsByColor.Length <= (int)color)
        {
            Debug.LogError("model for color not found: " + color);
            return;
        }

        foreach (var model in modelsByColor)
            model.SetActive(false);
        modelsByColor[(int)color].SetActive(true);
    }

    [Command]
    public void CmdDie()
    {
        // TODO:
        // effect
        HostGameManager.instance.OnPlayerDie(serverPlayerId, gameObject);
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    public void CmdImpulse(Vector2 impulse)
    {
        RpcImpulse(impulse);
    }

    [ClientRpc]
    private void RpcImpulse(Vector2 impulse)
    {
        if (!localPlayerAuthority) return;
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    [Command]
    public void CmdGetAndDestroyItem(NetworkIdentity netId)
    {
        if (netId == null) return;
        var itemBox = netId.GetComponent<ItemBox>();
        RpcGetItem(serverPlayerId, itemBox.itemType);
        RpcPlayParticle(itemBox.transform.position, "ItemGet");
        NetworkServer.Destroy(netId.gameObject);
    }

    [ClientRpc]
    public void RpcPlayParticle(Vector3 pos, string effectName)
    {
        var getEffect = EffectSpawner.instance.GetEffect(effectName);
        getEffect.transform.position = pos;
        getEffect.SetActive(true);
    }

    [ClientRpc]
    private void RpcGetItem(int playerId, ItemType itemType)
    {
        PlayerItemManager.inst.Set(playerId, itemType);
        Debug.Log("item: " + itemType);
    }

    [Command]
    public void CmdRequestFire()
    {
        var itemType = PlayerItemManager.inst.Find(serverPlayerId);
        if (!itemType.HasValue) return;
        itemShooter.ShootServerSide(itemType.Value);
        RpcResponseFire(serverPlayerId, itemType.Value);
    }

    [ClientRpc]
    private void RpcResponseFire(int playerId, ItemType itemType)
    {
        var orgItemType = PlayerItemManager.inst.FindAndUnSet(playerId);
        if (itemType != orgItemType) Debug.LogWarning("item type does not match");
        if (localPlayerAuthority) itemShooter.ShootMySide(itemType);
        itemShooter.ShootClientSide(itemType);
    }

	void SetCamera(){
		mainCamera = Camera.main.GetComponent<CameraMove>();
		mainCamera.target = this.transform;
    }

    void SetColor(int id)
    {
		switch(id){
		case 0: SetColorObject(PlayerColor.R); break;
		case 1: SetColorObject(PlayerColor.B); break;
		case 2: SetColorObject(PlayerColor.G); break;
		case 3: SetColorObject(PlayerColor.Y); break;
		}
	}
}
