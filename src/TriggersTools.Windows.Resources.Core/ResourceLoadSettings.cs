using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TriggersTools.Windows.Resources {
	/// <summary>
	///  Creates a resource type with the specified module handle, type, name, and language.
	/// </summary>
	/// <param name="hModule">The handle to the module containing this resource.</param>
	/// <param name="type">The type Id of this resource.</param>
	/// <param name="name">The name Id of this resource.</param>
	/// <param name="language">The language Id of this resource.</param>
	/// <returns>The constructed and loaded resource.</returns>
	public delegate Resource CreateResourceDelegate(IntPtr hModule, ResourceId type, ResourceId name, ushort language);
	/// <summary>
	///  Settings to use when loading resources from <see cref="ResourceInfo"/>.
	/// </summary>
	public class ResourceLoadSettings {
		/// <summary>
		///  Gets the dictionary of resource construction delegates for known resource types.
		/// </summary>
		public Dictionary<ResourceId, CreateResourceDelegate> TypeBuilders { get; }
			= new Dictionary<ResourceId, CreateResourceDelegate>();
		/// <summary>
		///  Gets or sets if all resources in the module are loaded during construction.
		///  This is true by default.<para/>
		///  If true, the <see cref="ResourceInfo"/> will close after construction.
		/// </summary>
		public bool LoadAllResources { get; set; } = true;
	}
	/// <summary>
	///  Immutable resource load settings that cannot be changed once created. Used by <see cref="ResourceInfo"/>
	///  internally.
	/// </summary>
	internal sealed class ImmutableResourceLoadSettings {
		/// <summary>
		///  Gets the dictionary of resource construction delegates for known resource types.
		/// </summary>
		public ReadOnlyDictionary<ResourceId, CreateResourceDelegate> TypeBuilders { get; }
		/// <summary>
		///  Gets or sets if all resources in the module are loaded during construction.
		///  This is true by default.<para/>
		///  If true, the <see cref="ResourceInfo"/> will close after construction.
		/// </summary>
		public bool LoadAllResources { get; } = true;

		/// <summary>
		///  Constructs the immutable resource load settings from the mutable settings.
		/// </summary>
		/// <param name="settings">The mutable resource load settings.</param>
		public ImmutableResourceLoadSettings(ResourceLoadSettings settings) {
			Dictionary<ResourceId, CreateResourceDelegate> dictionary =
				new Dictionary<ResourceId, CreateResourceDelegate>(settings.TypeBuilders);
			TypeBuilders = new ReadOnlyDictionary<ResourceId, CreateResourceDelegate>(dictionary);
			LoadAllResources = settings.LoadAllResources;
		}

		/// <summary>
		///  Casts the mutable resource load settings to immutable resource load settings.
		/// </summary>
		/// <param name="settings">The mutable resource load settings.</param>
		public static implicit operator ImmutableResourceLoadSettings(ResourceLoadSettings settings) {
			return new ImmutableResourceLoadSettings(settings);
		}
	}
}
