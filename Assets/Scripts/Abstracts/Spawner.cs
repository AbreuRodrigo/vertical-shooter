public abstract class Spawner {
	public int objectId;

	protected abstract void OnSpawn();

	protected abstract void OnVanish();

	public void Spawn() {
		this.OnSpawn();
	}

	public void Vanish() {
		this.OnVanish();
	}
}
