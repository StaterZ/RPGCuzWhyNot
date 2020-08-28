namespace StaterZ.Core.HealthSystem  {
	public interface IArmor {
		float OnDamageModify(float amount);
		float OnHealModify(float amount);
	}
}