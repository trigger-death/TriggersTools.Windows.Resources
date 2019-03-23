using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		///  The dictionary of resource construction delegates for known resource types.
		/// </summary>
		public Dictionary<ResourceId, CreateResourceDelegate> TypeBuilders { get; }
			= new Dictionary<ResourceId, CreateResourceDelegate>();
	}
}
