namespace StaterZ.Core.HealthSystem {
	public interface IArmor {
		int OnDamageModify(int amount);
		int OnHealModify(int amount);
	}
}