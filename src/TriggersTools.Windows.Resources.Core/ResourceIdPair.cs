using System;

namespace TriggersTools.Windows.Resources {
	/// <summary>
	///  A pair of Ids to identify a unique resource.
	/// </summary>
	public struct ResourceIdPair : IComparable, IComparable<ResourceIdPair>, IEquatable<ResourceIdPair> {
		#region Fields

		/// <summary>
		///  Gets or sets the resource type.
		/// </summary>
		public ResourceId Type { get; set; }
		/// <summary>
		///  Gets or sets the resource name.
		/// </summary>
		public ResourceId Name { get; set; }
		/// <summary>
		///  Gets or sets the resource language Id.
		/// </summary>
		public ushort Language { get; set; }
		/*/// <summary>
		///  Gets or sets if the resource type Id is ignored during comparison.
		/// </summary>
		public bool IgnoreType { get; set; }
		/// <summary>
		///  Gets or sets if the resource language Id is ignored during comparison.
		/// </summary>
		public bool IgnoreLanguage { get; set; }*/

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs a resource Id pair from the existing resource.
		/// </summary>
		/// <param name="resource">The existing resource to get the Ids from.</param>
		public ResourceIdPair(Resource resource) : this(resource.Type, resource.Name, resource.Language) { }
		/*public ResourceIdPair(Resource resource) : this(resource, false, false) { }
		/// <summary>
		///  Constructs a resource Id pair from the existing resource.
		/// </summary>
		/// <param name="resource">The existing resource to get the Ids from.</param>
		/// <param name="ignoreType">True if the type should be ignored during comparison.</param>
		/// <param name="ignoreLanguage">True if langauge should be ignored during comparison.</param>
		public ResourceIdPair(Resource resource, bool ignoreType, bool ignoreLanguage) {
			Type = resource.Type;
			Name = resource.Name;
			Language = resource.Language;
			//IgnoreType = ignoreType;
			//IgnoreLanguage = ignoreLanguage;
		}
		/// <summary>
		///  Constructs a resource Id pair with just a name and ignores the type and langauge.
		/// </summary>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The resource language.</param>
		public ResourceIdPair(ResourceId name) {
			Type = ResourceId.Null;
			Name = name;
			Language = 0;
			//IgnoreType = true;
			//IgnoreLanguage = true;
		}
		/// <summary>
		///  Constructs a resource Id pair with just a type and name and ignores the language.
		/// </summary>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		public ResourceIdPair(ResourceId type, ResourceId name) {
			Type = type;
			Name = name;
			Language = 0;
			IgnoreType = false;
			IgnoreLanguage = true;
		}
		/// <summary>
		///  Constructs a resource Id pair with just a name and language and ignores the type.
		/// </summary>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The resource language.</param>
		public ResourceIdPair(ResourceId name, ushort language) {
			Type = ResourceId.Null;
			Name = name;
			Language = language;
			IgnoreType = true;
			IgnoreLanguage = false;
		}*/
		/// <summary>
		///  Constructs a resource Id pair with type, name, and language.
		/// </summary>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The resource language.</param>
		public ResourceIdPair(ResourceId type, ResourceId name, ushort language) {
			Type = type;
			Name = name;
			Language = language;
			//IgnoreType = false;
			//IgnoreLanguage = false;
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets the string representation of Id pairs's the resource type.
		/// </summary>
		public string TypeName => Type.TypeName;

		#endregion

		#region Object Overrides
		
		/// <summary>
		///  Gets the string representation of the resource Id pair.
		/// </summary>
		/// <returns>The resource Id pair names.</returns>
		public override string ToString() {
			/*const string Any = "<any>";
			string type = (IgnoreType ? Any : TypeName);
			string lang = (IgnoreLanguage ? Any : $"0x{Language:X4}");
			return $"{type} : {Name} : {lang}";*/
			return $"{TypeName} : {Name} : 0x{Language:X4}";
		}

		/// <summary>
		///  Gets the hash code for the resource Id pair. Either returns <see cref="Value"/>.
		/// </summary>
		/// <returns>The resource Id pair hash code.</returns>
		public override int GetHashCode() {
			return Type.GetHashCode() ^ Name.GetHashCode() ^ (Language << 16);
			/*int hashCode = Name.GetHashCode();
			if (!IgnoreType)
				hashCode ^= Type.GetHashCode();
			if (!IgnoreLanguage)
				hashCode ^= (Language << 16);
			return hashCode;*/
		}

		/// <summary>
		///  Checks if the other object is a resource Id pair and is equal to this one.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>True if both resource Id pairs represent the same resource.</returns>
		public override bool Equals(object obj) {
			return (obj is ResourceIdPair idPair && Equals(idPair));
		}
		/// <summary>
		///  Checks if the other resource Id pair is equal to this one.
		/// </summary>
		/// <param name="other">The other resource Id pair to compare.</param>
		/// <returns>True if both resource Id pairs represent the same resource.</returns>
		public bool Equals(ResourceIdPair other) {
			return (Type == other.Type && Name == other.Name && Language == other.Language);
			/*return ((Type     == other.Type     || IgnoreType     || other.IgnoreType) &&
					(Name     == other.Name) &&
					(Language == other.Language || IgnoreLanguage || other.IgnoreLanguage));*/
		}
		/// <summary>
		///  Checks if the other object is a resource Id pair and compares this resource Id pair to it.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>The comparison of both resource Id pairs.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="obj"/> is not a <see cref="ResourceIdPair"/>.
		/// </exception>
		public int CompareTo(object obj) {
			if (obj is ResourceIdPair idPair)
				return CompareTo(idPair);
			throw new ArgumentException($"{nameof(obj)} is not of type {nameof(ResourceIdPair)}!");
		}
		/// <summary>
		///  Compares this resource Id pair the other.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>The comparison of this resource Id pair to the other.</returns>
		public int CompareTo(ResourceIdPair other) {
			int comparison;
			//if (!IgnoreType && !other.IgnoreType && (comparison = Type.CompareTo(other.Type)) != 0)
			//	return comparison;
			if ((comparison = Type.CompareTo(other.Type)) != 0)
				return comparison;
			if ((comparison = Name.CompareTo(other.Name)) != 0)
				return comparison;
			return Language.CompareTo(other.Language);
			/*if (!IgnoreLanguage && !other.IgnoreLanguage)
				return Language.CompareTo(other.Language);
			return 0;*/
		}

		#endregion

		#region Operators

		public static bool operator ==(ResourceIdPair a, ResourceIdPair b) => a.Equals(b);
		public static bool operator !=(ResourceIdPair a, ResourceIdPair b) => !a.Equals(b);

		#endregion
	}
}
