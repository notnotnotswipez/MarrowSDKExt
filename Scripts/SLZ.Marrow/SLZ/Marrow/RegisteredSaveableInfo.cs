using System;
using System.Runtime.CompilerServices;

namespace SLZ.Marrow
{
	[Serializable]
	public class RegisteredSaveableInfo
	{
		public Saveable Saveable
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public string UniqueId
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public RegisteredSaveableInfo(Saveable saveable, string uniqueId)
		{
		}
	}
}
