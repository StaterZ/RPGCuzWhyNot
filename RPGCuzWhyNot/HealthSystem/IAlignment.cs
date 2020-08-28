/*--
Copyright (C) 2020 Melvin Ringheim - All Rights Reserved
This file falls under the default laws of exclusive copyright.
Nobody can copy, distribute, or modify this code without explicit permission from the owner.
--*/



namespace StaterZ.Core.HealthSystem {
	public abstract class Alignment {
		public abstract bool CanHarm(Alignment other);
	}
}