namespace RPGCuzWhyNot.Systems.HealthSystem {
	public struct HealthChangeInfo {
		public Health health;
		public float oldHealth;
		public float newHealth;
		public float attemptedDelta;
        public IInflictor inflictor;
        public float Delta => newHealth - oldHealth;

		public HealthChangeInfoSuccess Success {
	        get {
		        if (attemptedDelta != Delta) return HealthChangeInfoSuccess.PartialDelivery;
		        if (inflictor?.Alignment != null && health.alignment != null && !inflictor.Alignment.CanHarm(health.alignment)) return HealthChangeInfoSuccess.AlignmentConflict; //can you hurt me?
		        if (!health.IsAlive) return HealthChangeInfoSuccess.AlreadyDead; //is the health component dead?
                return HealthChangeInfoSuccess.FullDelivery;
	        }
        }

        public override string ToString() {
			return $"old: {oldHealth}, new: {newHealth}, delta: {Delta}, attemptedDelta: {attemptedDelta}, Success:{Success}";
		}
	}
}