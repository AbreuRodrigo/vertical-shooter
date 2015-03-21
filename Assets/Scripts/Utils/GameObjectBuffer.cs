using UnityEngine;
using System.Collections;

public class GameObjectBuffer {	
	public int length;
	private int[] disabledIndexes;
	private GameObject[] buffer;
	
	private int indexNextSpaceForDisabled = -1;
	private int indexNextDisabled = -1;
	public int countDisabled = 0;
	public int countCreated = 0;
	
	public GameObjectBuffer() {}
	
	public GameObjectBuffer(int length) {
		this.length = length;
		buffer = new GameObject[length];
		disabledIndexes = new int[length];
	}
	
	public GameObject Get(int index) {
		return buffer[index];
	}
	
	public void Add(int index, GameObject gameObject) {
		buffer[index] = gameObject;
		countCreated++;
	}
	
	public void Add(GameObject gameObject) {
		Spawner spawn = gameObject.GetComponent<Spawner>();
		spawn.objectId = countCreated;
		buffer[countCreated] = gameObject;
		countCreated++;
	}
	
	public void Disable(int index) {
		if (countCreated < 0) {
			return;
		}
		
		if (countDisabled == length) {
			return;
		}

		indexNextSpaceForDisabled++;
		countDisabled++;
		if (indexNextSpaceForDisabled == length) {
			indexNextSpaceForDisabled = 0;
		}
		disabledIndexes[indexNextSpaceForDisabled] = index;
	}
	
	public GameObject GetNextDisabled() {
		indexNextDisabled++;

		if (indexNextDisabled >= length) {
			indexNextDisabled = 0;
		}

		int index = disabledIndexes[indexNextDisabled];
		
		GameObject objectInstance = Get(index);
		
		Spawner spawn = objectInstance.GetComponent<Spawner>();

		spawn.Spawn();
		
		countDisabled--;
		
		return objectInstance;
	}
	
	public bool IsFull() {
		return countCreated >= length;
	}
	
	public bool hasDisabled() {
		return countDisabled > 0;
	}
}