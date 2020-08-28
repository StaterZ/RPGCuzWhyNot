/*--
Copyright (C) 2020 Melvin Ringheim - All Rights Reserved
This file falls under the default laws of exclusive copyright.
Nobody can copy, distribute, or modify this code without explicit permission from the owner.
--*/



using System.Collections.Generic;

namespace StaterZ.Core.HealthSystem {
	public class BasicAlignment : Alignment {
		public List<BasicAlignment> enemies;

        public override bool CanHarm(Alignment other) {
			return other is BasicAlignment otherAlignment && enemies.Contains(otherAlignment);
		}
	}
}