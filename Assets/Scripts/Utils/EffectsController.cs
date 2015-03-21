using UnityEngine;
using System.Collections;

public class EffectsController : MonoBehaviour {
	public static EffectsController instance;

	public GameObject explosion;

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
		}

		if(obj != null){
			Destroy (obj, msToDestroy);
		}
	}

	/*public void CreateEffect(Vector3 position, Quaternion quaternion, GameObject prefab, Transform container, GameObjectBuffer buffer) {
		if (buffer.hasDisabled()) {
			GameObject obj = buffer.GetNextDisabled();

			obj.transform.position = position;
		} else if (!buffer.IsFull()) {
			GameObject obj = Instantiate(prefab, position, quaternion) as GameObject;

			if (container != null) {
				obj.transform.parent = container;
			}

			buffer.Add(obj);
		}
	}*/
}
