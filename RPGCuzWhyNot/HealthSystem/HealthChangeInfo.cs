/*--
Copyright (C) 2020 Melvin Ringheim - All Rights Reserved
This file falls under the default laws of exclusive copyright.
Nobody can copy, distribute, or modify this code without explicit permission from the owner.
--*/



namespace StaterZ.Core.HealthSystem {
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
		        if (inflictor?.alignment != null && health.alignment != null && !inflictor.alignment.CanHarm(health.alignment)) return HealthChangeInfoSuccess.AlignmentConflict; //can you hurt me?
		        if (health.IsDead) return HealthChangeInfoSuccess.AlreadyDead; //is the health component dead?
                return HealthChangeInfoSuccess.FullDelivery;
	        }
        }

        public override string ToString() {
			return $"old: {oldHealth}, new: {newHealth}, delta: {Delta}, attemptedDelta: {attemptedDelta}, Success:{Success}";
		}
	}
}