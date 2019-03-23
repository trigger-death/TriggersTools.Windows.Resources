using System;
using TriggersTools.Windows.Resources.Dialog;
using TriggersTools.Windows.Resources.Manifest;
using TriggersTools.Windows.Resources.Menu;
using TriggersTools.Windows.Resources.StringTable;

namespace TriggersTools.Windows.Resources {
	/// <summary>
	///  Static methods for seeding <see cref="ResourceLoadSettings"/> with implemented known resource types.
	/// </summary>
	public static class ResourceKnownTypes {
		/// <summary>
		///  Constructs resource load settings with known resource types added.
		/// </summary>
		public static ResourceLoadSettings LoadSettings => AddKnownTypes(new ResourceLoadSettings());

		/// <summary>
		///  Adds built in implemented known resource types to the <see cref="ResourceLoadSettings.TypeBuilders"/>. 
		/// </summary>
		/// <param name="settings">The settings to add the known types to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="settings"/> is null.
		/// </exception>
		public static ResourceLoadSettings AddKnownTypes(ResourceLoadSettings settings) {
			if (settings == null)
				throw new ArgumentNullException(nameof(settings));
			settings.TypeBuilders.Add(ResourceTypes.Manifest, (h,t,n,l) => new ManifestResource(h,n,l));
			settings.TypeBuilders.Add(ResourceTypes.Menu,     (h,t,n,l) => new MenuResource(h,n,l));
			settings.TypeBuilders.Add(ResourceTypes.Dialog,   (h,t,n,l) => new DialogResource(h,n,l));
			settings.TypeBuilders.Add(ResourceTypes.String,   (h,t,n,l) => new StringResource(h,n,l));
			return settings;
		}
	}
}
