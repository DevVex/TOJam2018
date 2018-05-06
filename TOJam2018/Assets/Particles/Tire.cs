using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TOJAM;

[System.Serializable]
public class TireEffect
{
    public string layer;
    public GameObject effect;

    public TireEffect(string layer, GameObject effect)
    {
        this.layer = layer;
        this.effect = effect;
    }
}


public class Tire : MonoBehaviour {

    public TireEffect defaultTireEffect;
    public TireEffect[] tireEffects;
    private Dictionary<int, GameObject> effects;
    private TireEffect currentEffect;
    private bool jumping;

    [SerializeField] private Player _playerRef;

	// Use this for initialization
	void Start () {
        effects = new Dictionary<int, GameObject>();
		foreach(TireEffect tireEffect in tireEffects)
        {
            effects.Add(LayerMask.NameToLayer(tireEffect.layer), tireEffect.effect);
        }
        SetDefault();

    }
	
	// Update is called once per frame
	void Update () {
        if (!jumping && !_playerRef.CanJump)
        {
            jumping = true;
            currentEffect.effect.GetComponent<ParticleSystem>().Stop();
        }

        if(jumping && _playerRef.CanJump)
        {
            jumping = false;
            currentEffect.effect.GetComponent<ParticleSystem>().Play();
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //hit a new tire layer
        if (effects.ContainsKey(other.gameObject.layer))
        {
            SetEffect(LayerMask.LayerToName(other.gameObject.layer), effects[other.gameObject.layer]);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (effects.ContainsKey(other.gameObject.layer))
        {
            SetDefault();
        }
    }

    void SetDefault()
    {
        SetEffect(defaultTireEffect.layer, defaultTireEffect.effect);
    }

    void SetEffect(string layer, GameObject effect)
    {
        if (currentEffect != null && currentEffect.effect != null)
        {
            currentEffect.effect.GetComponent<ParticleSystem>().Stop();
            Destroy(currentEffect.effect, 0.8f);
        }
        GameObject newEffect = Instantiate(effect);
        newEffect.GetComponent<FollowTarget>().SetTarget(this.gameObject);
        currentEffect = new TireEffect(layer, newEffect);
    }
}
