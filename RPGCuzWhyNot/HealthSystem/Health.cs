/*--
Copyright (C) 2020 Melvin Ringheim - All Rights Reserved
This file falls under the default laws of exclusive copyright.
Nobody can copy, distribute, or modify this code without explicit permission from the owner.
--*/



using System;
using System.Collections.Generic;

namespace StaterZ.Core.HealthSystem {
	public class Health {
        public float maxHealth;
        public Alignment alignment;
        public float CurrentHealth { get; private set; }
        public float Percent => CurrentHealth / maxHealth;
        public bool IsDamaged => CurrentHealth < maxHealth;
        public bool IsDead => CurrentHealth <= 0;
        public bool IsAtMaxHealth => CurrentHealth >= maxHealth;

        public event Action<HealthChangeInfo> OnDeath; //when we run out of health
        public event Action<HealthChangeInfo> OnChange; //if we in any way change the current health
        public event Action<HealthChangeInfo> OnDamage; //if we explicity deal damage
        public event Action<HealthChangeInfo> OnHeal; //if we explicity heal damage
        public event Action<HealthChangeInfo> OnFullRecovery; //when we reach max health

        public List<IArmor> armor = new List<IArmor>(); //damage modifiers

        public Health(int health) : this(health, health) {
        }

        public Health(float maxHealth, float currentHealth) {
	        this.maxHealth = maxHealth;
	        CurrentHealth = currentHealth;
        }

        public HealthChangeInfo SetHealth(float value, IInflictor inflictor) {
			float attemptedDelta = value - CurrentHealth;
			value = ExtraMath.Clamp(value, 0, maxHealth);
			float delta = value - CurrentHealth;

			HealthChangeInfo info = new HealthChangeInfo() {
				health = this,
				oldHealth = CurrentHealth,
                newHealth = value,
				attemptedDelta = attemptedDelta,
                inflictor = inflictor
			};

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
				if (IsDead) {
					OnDeath?.Invoke(info);
				}
			}

			return info;
        }

        public void RestoreAllHealth(IInflictor inflictor) {
            SetHealth(maxHealth, inflictor);
        }

        public HealthChangeInfo TakeDamage(float damage, IInflictor inflictor) {
            if (damage < 0) throw new ArgumentException("can't deal negative damage");

	        foreach (IArmor a in armor) {
		        damage = a.OnDamageModify(damage);
	        }

			HealthChangeInfo info = ChangeHealth(-damage, inflictor);

            return info;
        }

        public HealthChangeInfo Heal(float damage, IInflictor inflictor) {
            if (damage < 0) throw new ArgumentException("can't heal negative damage");

            foreach (IArmor a in armor) {
	            damage = a.OnHealModify(damage);
            }

	        HealthChangeInfo info = ChangeHealth(damage, inflictor);
           
            return info;
        }
		
		protected HealthChangeInfo ChangeHealth(float damage, IInflictor inflictor) {
            return SetHealth(CurrentHealth + damage, inflictor);
        }
    }
}
