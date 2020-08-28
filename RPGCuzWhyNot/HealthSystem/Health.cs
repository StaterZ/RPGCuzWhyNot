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
	        this.CurrentHealth = currentHealth;
        }

        public void SetHealth(float value, HealthChangeInfo info) {
			float delta = value - CurrentHealth;
			value = ExtraMath.Clamp(value, 0, maxHealth);

			info.health = this;
			info.newHealth = value;
			info.oldHealth = CurrentHealth;
			info.attemptedDelta = delta;

			CurrentHealth = value;
			OnChange?.Invoke(info);
			if (delta > 0) {
				OnHeal?.Invoke(info);
				if (delta != 0 && IsAtMaxHealth) {
					OnFullRecovery?.Invoke(info);
				}
			}
			if (delta < 0) {
				OnDamage?.Invoke(info);
				if (delta != 0 && IsDead) {
					OnDeath?.Invoke(info);
				}
			}
		}

        public void ResoreAllHealth() {
            SetHealth(maxHealth, new HealthChangeInfo());
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
		
		protected HealthChangeInfo ChangeHealth(float amount, IInflictor inflictor) {
            HealthChangeInfo info = new HealthChangeInfo {
                inflictor = inflictor,
                health = this,
                newHealth = CurrentHealth,
                oldHealth = CurrentHealth,
				attemptedDelta = amount
            };

            if (info.Success == HealthChangeInfoSuccess.AlignmentConflict || info.Success == HealthChangeInfoSuccess.AlreadyDead) return info;

            SetHealth(CurrentHealth + amount, info);
	        info.newHealth = CurrentHealth;

            return info;
        }
    }
}
