using System;
using System.Collections.Generic;
using RPGCuzWhyNot.Systems.HealthSystem.Alignments;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.HealthSystem {
	public class Health {
		public int maxHealth;
		public IAlignment alignment;
		public int CurrentHealth { get; private set; }
		public float Percent => (float)CurrentHealth / maxHealth;
		public bool IsDamaged => CurrentHealth < maxHealth;
		public bool IsAlive => CurrentHealth > 0;
		public bool IsAtMaxHealth => CurrentHealth >= maxHealth;

		public event Action<HealthChangeInfo> OnDeath; //when we run out of health
		public event Action<HealthChangeInfo> OnChange; //if we in any way change the current health
		public event Action<HealthChangeInfo> OnDamage; //if we explicity deal damage
		public event Action<HealthChangeInfo> OnHeal; //if we explicity heal damage
		public event Action<HealthChangeInfo> OnFullRecovery; //when we reach max health

		public List<IHealthChangeModifier> armor = new List<IHealthChangeModifier>(); //damage modifiers

		public Health(int health) : this(health, health) {
		}

		public Health(int maxHealth, int currentHealth) {
			this.maxHealth = maxHealth;
			CurrentHealth = currentHealth;
		}

		public HealthChangeInfo SetHealth(int value, IInflictor inflictor) {
			int attemptedDelta = value - CurrentHealth;
			value = Utils.Clamp(value, 0, maxHealth);
			int delta = value - CurrentHealth;

			HealthChangeInfo info = new HealthChangeInfo(this, CurrentHealth, value, attemptedDelta, inflictor);

			CurrentHealth = value;
			OnChange?.Invoke(info);
			if (delta > 0) {
				OnHeal?.Invoke(info);
				if (IsAtMaxHealth) {
					OnFullRecovery?.Invoke(info);
				}
			}
			if (delta < 0) {
				OnDamage?.Invoke(info);
				if (!IsAlive) {
					OnDeath?.Invoke(info);
				}
			}

			return info;
		}

		public void RestoreAllHealth(IInflictor inflictor) {
			SetHealth(maxHealth, inflictor);
		}

		public HealthChangeInfo DealDamage(int damage, IInflictor inflictor) {
			foreach (IHealthChangeModifier a in armor) {
				damage = a.OnDamageModify(damage);
			}

			if (damage == 0) return new HealthChangeInfo(this, CurrentHealth, CurrentHealth, 0, inflictor);
			if (damage < 0) throw new ArgumentException("can't deal negative damage");

			HealthChangeInfo info = ChangeHealth(-damage, inflictor);

			return info;
		}

		public HealthChangeInfo Heal(int damage, IInflictor inflictor) {
			foreach (IHealthChangeModifier a in armor) {
				damage = a.OnHealModify(damage);
			}

			if (damage == 0) return new HealthChangeInfo(this, CurrentHealth, CurrentHealth, 0, inflictor);
			if (damage < 0) throw new ArgumentException("can't heal negative damage");

			HealthChangeInfo info = ChangeHealth(damage, inflictor);

			return info;
		}

		protected HealthChangeInfo ChangeHealth(int damage, IInflictor inflictor) {
			return SetHealth(CurrentHealth + damage, inflictor);
		}
	}
}
