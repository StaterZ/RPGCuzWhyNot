namespace RPGCuzWhyNot.Systems.HealthSystem {
	public interface IHealthChangeModifier {
		int OnDamageModify(int amount);
		int OnHealModify(int amount);
	}
}