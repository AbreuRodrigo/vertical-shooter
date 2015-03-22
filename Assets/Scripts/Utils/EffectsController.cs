using UnityEngine;
using System.Collections;

public class EffectsController : MonoBehaviour {
	public static EffectsController instance;

	public GameObject explosion;
	public GameObject scoreUp;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public void CreateEffect(Vector3 position, EffectType effectType, float msToDestroy){
		GameObject obj = null;

		switch(effectType){
			case EffectType.Explosion:
				if(explosion != null){
					obj = Instantiate(explosion, position, Quaternion.identity) as GameObject;
				}
			break;
			case EffectType.ScoreUp:
				if(scoreUp != null){
					scoreUp.GetComponent<TextMesh>().text = "+" + GameWorld.enemyAwards;
					obj = Instantiate(scoreUp, position, Quaternion.identity) as GameObject;					
				}
			break;
		}

		if(obj != null){
			Destroy (obj, msToDestroy);
		}
	}
}
