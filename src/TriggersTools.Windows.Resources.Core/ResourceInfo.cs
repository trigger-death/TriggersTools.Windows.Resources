//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     ResourceLib Original Code from http://resourcelib.codeplex.com
//     Original Copyright (c) 2008-2009 Vestris Inc.
//     Changes Copyright (c) 2011 Garrett Serack . All rights reserved.
// </copyright>
// <license>
// MIT License
// You may freely use and distribute this software under the terms of the following license agreement.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
// the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE
// </license>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.Windows.Resources.Native;

namespace TriggersTools.Windows.Resources {
	/// <summary>
	///  A class for storing a collection of resources, optionally loaded from a module.
	/// </summary>
	public class ResourceInfo : KeyedCollection<ResourceIdPair, Resource>, IDisposable {
		#region Constants

		/*/// <summary>
		///  The default settings structure to use.
		/// </summary>
		private static readonly ResourceLoadSettings DefaultSettings = new ResourceLoadSettings();*/

		#endregion

		#region Fields

		/// <summary>
		///  The outer exception thrown during native resource enumeration.<para/>
		///  This exists because exceptions cannot be caught through these native calls.
		/// </summary>
		private Exception outerException;

		/*/// <summary>
		///  A dictionary of resources, the key is the resource type, eg. "REGISTRY" or "16" (version).
		/// </summary>
		private readonly Dictionary<ResourceId, ResourceCollection> resourceLists
			= new Dictionary<ResourceId, ResourceCollection>();*/
		/// <summary>
		///  The settings used to load the resources.
		/// </summary>
		private readonly ImmutableResourceLoadSettings settings;

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the empty resource info collection.
		/// </summary>
		public ResourceInfo() { }

		/// <summary>
		///  Constructs the resource info collection from the file name and optionally loads the resources.
		/// </summary>
		/// <param name="filePath">The file path of the module to load.</param>
		/// <param name="loadAllResources">
		///  If true, all resources are loaded, then the module handle is disposed of. The <see cref="ModuleHandle"/>
		///  property will only be stored if this is set to false.
		/// </param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="filePath"/> is null.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		///  <paramref name="filePath"/> does not exist.
		/// </exception>
		/// <exception cref="ResourceIOException">
		///  An error occurred while loading resources.
		/// </exception>
		public ResourceInfo(string filePath, bool loadAllResources) {
			if (filePath == null)
				throw new ArgumentNullException(nameof(filePath));
			settings = new ResourceLoadSettings {
				LoadAllResources = loadAllResources,
			};
			LoadModule(filePath);
		}
		/// <summary>
		///  Constructs the resource info collection from the module handle and optionally loads the resources.
		/// </summary>
		/// <param name="hModule">The handle to the module to load.</param>
		/// <param name="loadAllResources">
		///  If true, all resources are loaded, then the module handle is disposed of. The <see cref="ModuleHandle"/>
		///  property will only be stored if this is set to false.
		/// </param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hModule"/> is null.
		/// </exception>
		/// <exception cref="ResourceIOException">
		///  An error occurred while loading resources.
		/// </exception>
		public ResourceInfo(IntPtr hModule, bool loadAllResources) {
			if (hModule == IntPtr.Zero)
				throw new ArgumentNullException(nameof(hModule));
			settings = new ResourceLoadSettings {
				LoadAllResources = loadAllResources,
			};
			LoadModule(hModule);
		}

		/// <summary>
		///  Constructs the resource info collection from the file name and loads the resources using the load
		///  settings.
		/// </summary>
		/// <param name="filePath">The file path of the module to load.</param>
		/// <param name="settings">The settings for loading the resources.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="filePath"/> or <paramref name="settings"/> is null.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		///  <paramref name="filePath"/> does not exist.
		/// </exception>
		/// <exception cref="ResourceIOException">
		///  An error occurred while loading resources.
		/// </exception>
		public ResourceInfo(string filePath, ResourceLoadSettings settings) {
			if (filePath == null)
				throw new ArgumentNullException(nameof(filePath));
			this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
			LoadModule(filePath);
		}
		/// <summary>
		///  Constructs the resource info collection from the module handle and loads the resources using the load
		///  settings.
		/// </summary>
		/// <param name="hModule">The handle to the module to load.</param>
		/// <param name="settings">The settings for loading the resources.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hModule"/> or <paramref name="settings"/> is null.
		/// </exception>
		/// <exception cref="ResourceIOException">
		///  An error occurred while loading resources.
		/// </exception>
		public ResourceInfo(IntPtr hModule, ResourceLoadSettings settings) {
			if (hModule == IntPtr.Zero)
				throw new ArgumentNullException(nameof(hModule));
			this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
			LoadModule(hModule);
		}

		#endregion

		#region KeyedCollection Overrides

		/// <summary>
		///  Gets the key from the <see cref="Resource"/>.
		/// </summary>
		/// <param name="item">The resource from which to extract the key.</param>
		/// <returns>The key for the specified resource.</returns>
		protected override ResourceIdPair GetKeyForItem(Resource item) => item.IdPair;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the collection of all currently loaded resource types.
		/// </summary>
		public IEnumerable<ResourceId> ResourceTypes {
			get {
				HashSet<ResourceId> encounteredTypes = new HashSet<ResourceId>();
				foreach (Resource resource in this) {
					if (encounteredTypes.Add(resource.Type))
						yield return resource.Type;
				}
			}
		}
		/*/// <summary>
		///  Gets the total number of stored resources.
		/// </summary>
		public int Count => resourceLists.Values.Sum(r => r.Count);*/

		/// <summary>
		///  Gets the currently loaded module handle.
		/// </summary>
		public IntPtr ModuleHandle { get; private set; }
		/// <summary>
		///  Gets if the resource info is still open and the <see cref="ModuleHandle"/> is non-null.
		/// </summary>
		public bool IsOpen => ModuleHandle != IntPtr.Zero;

		/// <summary>
		///  Gets an enumerable containing resources of the specified type.
		/// </summary>
		/// <param name="type">The type of the resource collection to get.</param>
		/// <returns>An enumerable of resources of a given type.</returns>
		public IEnumerable<Resource> this[ResourceId type] {
			get {
				foreach (Resource resource in this) {
					if (resource.Type == type)
						yield return resource;
				}
			}
		}
		/*/// <summary>
		///  Gets an enumerable containing resources of the specified type.
		/// </summary>
		/// <param name="type">The type of the resource collection to get.</param>
		/// <returns>An enumerable of resources of a given type.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> does not match <see cref="ResourceCollection.Type"/>.
		/// </exception>
		public ResourceCollection this[ResourceId type] {
			get => resourceLists[type];
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				if (value.Type != type)
					throw new ArgumentException($"{nameof(ResourceCollection)}.{nameof(ResourceCollection.Type)} " +
												$"{value.Type} must be the same type as {nameof(type)} {type}!");
				resourceLists[type] = value;
			}
		}*/

		#endregion

		#region IEnumerable Implementation

		/*/// <summary>
		///  Enumerates all resources within this resource info collection.
		/// </summary>
		/// <returns>The resource enumerator.</returns>
		public IEnumerator<Resource> GetEnumerator() {
			var resourceTypesEnumerator = resourceLists.GetEnumerator();
			while (resourceTypesEnumerator.MoveNext()) {
				var resourceEnumerator = resourceTypesEnumerator.Current.Value.GetEnumerator();
				while (resourceEnumerator.MoveNext()) {
					yield return resourceEnumerator.Current;
				}
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();*/

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///  Closes the resource info if the module handle is still open. This is the same as <see cref="Dispose"/>.
		/// </summary>
		public void Close() {
			Dispose();
		}
		/// <summary>
		///  Disposes of the resource info if loadResources was false when constructing from a file path.
		/// </summary>
		public void Dispose() {
			if (ModuleHandle != IntPtr.Zero) {
				Kernel32.FreeLibrary(ModuleHandle);
				ModuleHandle = IntPtr.Zero;
			}
		}

		#endregion

		#region Collection Mutators

		/*/// <summary>
		///  Adds the resource collection to the resource info.
		/// </summary>
		/// <param name="collection">The collection to add.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="collection"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  A resource collection with <see cref="ResourceCollection.Type"/> already exists in the resource info.
		/// </exception>
		public void AddCollection(ResourceCollection collection) {
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			if (resourceLists.ContainsKey(collection.Type)) {
				throw new ArgumentException($"Already contains a {nameof(ResourceCollection)} of type " +
											$"{collection.Type}!");
			}
			resourceLists.Add(collection.Type, collection);
		}
		/// <summary>
		///  Removes the resource collection from the resource info.
		/// </summary>
		/// <param name="collection">The collection to remove.</param>
		/// <returns>True if this collection existed in the resource info and was removed.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="collection"/> is null.
		/// </exception>
		public bool RemoveCollection(ResourceCollection collection) {
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			var pair = new KeyValuePair<ResourceId, ResourceCollection>(collection.Type, collection);
			return ((ICollection<KeyValuePair<ResourceId, ResourceCollection>>) resourceLists).Remove(pair);
		}
		/// <summary>
		///  Removes the resource collection with the specified type from the resource info.
		/// </summary>
		/// <param name="type">The type of the collection to remove.</param>
		/// <returns>True if this collection type existed in the resource info and was removed.</returns>
		public bool RemoveCollection(ResourceId type) {
			return resourceLists.Remove(type);
		}*/

		#endregion

		#region Resource Mutators

		/// <summary>
		///  Loads the resource with the specified information.
		/// </summary>
		/// <param name="idPair">The pair of Id's of the resource.</param>
		/// <returns>The loaded resource.</returns>
		/// 
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public Resource Load(ResourceIdPair idPair) {
			return Load(idPair.Type, idPair.Name, idPair.Language);
		}
		/// <summary>
		///  Loads the resource with the specified information.
		/// </summary>
		/// <param name="type">The type of the resource.</param>
		/// <param name="name">The name of the resource.</param>
		/// <param name="language">The language of the resource.</param>
		/// <returns>The loaded resource.</returns>
		/// 
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public Resource Load(ResourceId type, ResourceId name, ushort language) {
			if (!IsOpen)
				throw new ObjectDisposedException($"{nameof(ResourceInfo)} is closed!");
			Resource resource = CreateResource(ModuleHandle, type, name, language);
			Add(resource);
			return resource;
		}
		/// <summary>
		///  Loads the resource with the specified information.
		/// </summary>
		/// <param name="idPair">The pair of Id's of the resource.</param>
		/// <returns>The loaded resource.</returns>
		/// 
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public GenericResource LoadGeneric(ResourceIdPair idPair) {
			return LoadGeneric(idPair.Type, idPair.Name, idPair.Language);
		}
		/// <summary>
		///  Loads the resource with the specified information.
		/// </summary>
		/// <param name="type">The type of the resource.</param>
		/// <param name="name">The name of the resource.</param>
		/// <param name="language">The language of the resource.</param>
		/// <returns>The loaded resource.</returns>
		/// 
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public GenericResource LoadGeneric(ResourceId type, ResourceId name, ushort language) {
			if (!IsOpen)
				throw new ObjectDisposedException($"{nameof(ResourceInfo)} is closed!");
			GenericResource resource = new GenericResource(ModuleHandle, type, name, language);
			Add(resource);
			return resource;
		}
		/*/// <summary>
		///  Adds the resource to the resource info and its proper collection. Adds a collection if one does not exist.
		/// </summary>
		/// <param name="resource">The resource to add.</param>
		/// <returns>True if the resource was added and did not exist in the collection.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="resource"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  A resource with <see cref="Resource.IdPair"/> already exists in the collection.
		/// </exception>
		public void Add(Resource resource) {
			if (resource == null)
				throw new ArgumentNullException(nameof(resource));
			if (!resourceLists.TryGetValue(resource.Type, out var resources)) {
				resources = new ResourceCollection(resource.Type);
				resourceLists[resource.Type] = resources;
			}
			resources.Add(resource);
		}
		public bool Remove(Resource resource) {
			if (resource == null)
				throw new ArgumentNullException(nameof(resource));
			if (!resourceLists.TryGetValue(resource.Type, out var resources))
				return false;
			return resources.Remove(resource);
		}*/

		#endregion

		#region Save

		/// <summary>
		///  Save resource to a file.
		/// </summary>
		/// <param name="filePath">Target filename.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="filePath"/> is null.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		///  <paramref name="filePath"/> does not exist.
		/// </exception>
		/// <exception cref="ResourceIOException">
		///  A resource IO exception occurred.
		/// </exception>
		public void Save(string filePath) {
			Resource.SaveTo(filePath, this);
		}
		
		#endregion

		#region Private Load

		/// <summary>
		///  Load a module file and optionally reads its resources.
		/// </summary>
		/// <param name="filePath">Source filename.</param>
		private void LoadModule(string filePath) {
			if (!File.Exists(filePath))
				throw new FileNotFoundException($"Could not find file \"{filePath}\"!");

			// load DLL
			IntPtr hModule = Kernel32.LoadLibraryEx(
				filePath,
				IntPtr.Zero,
				LoadLibraryExFlags.DontResolveDllReferences | LoadLibraryExFlags.LoadLibraryAsDatafile);

			if (IntPtr.Zero == hModule) {
				throw new ResourceIOException($"An error occurred while opening \"{filePath}\"!",
					new Win32Exception());
			}
			try {
				if (!settings.LoadAllResources) {
					ModuleHandle = hModule;
					return;
				}
				// enumerate resource types
				// for each type, enumerate resource names
				// for each name, enumerate resource languages
				// for each resource language, enumerate actual resources
				if (!Kernel32.EnumResourceTypes(hModule, EnumResourceTypesImpl, IntPtr.Zero)) {
					outerException?.Rethrow();
					throw new Win32Exception();
				}
			} catch (Exception ex) {
				outerException?.Rethrow();
				throw;
			} finally {
				if (settings.LoadAllResources)
					Kernel32.FreeLibrary(hModule);
			}
		}
		/// <summary>
		///  Load a module handle and optionally reads its resources.
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		private void LoadModule(IntPtr hModule) {
			try {
				if (!settings.LoadAllResources) {
					ModuleHandle = hModule;
					return;
				}
				// enumerate resource types
				// for each type, enumerate resource names
				// for each name, enumerate resource languages
				// for each resource language, find the resource and add it
				if (!Kernel32.EnumResourceTypes(hModule, EnumResourceTypesImpl, IntPtr.Zero)) {
					outerException?.Rethrow();
					throw new Win32Exception();
				}
			} catch (Exception ex) {
				outerException?.Rethrow();
				throw;
			}
		}

		#endregion

		#region CreateResource

		/// <summary>
		///  Create a resource of a given type.
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="hGlobal">Pointer to the resource in memory.</param>
		/// <param name="type">Resource type.</param>
		/// <param name="name">Resource name.</param>
		/// <param name="language">Language ID.</param>
		/// <param name="size">Size of resource.</param>
		/// <returns>A specialized or a generic resource.</returns>
		protected Resource CreateResource(IntPtr hModule, ResourceId type, ResourceId name, ushort language) {
			// Check if we have a resource registered for this type.
			if (settings.TypeBuilders.TryGetValue(type, out var createResource)) {
				return createResource(hModule, type, name, language);
			}
			return new GenericResource(hModule, type, name, language);
		}

		#endregion

		#region Private Enumerate

		/// <summary>
		///  Enumerate resource types.
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="lpszType">Resource type.</param>
		/// <param name="lParam">Additional parameter.</param>
		/// <returns>TRUE if successful.</returns>
		private bool EnumResourceTypesImpl(IntPtr hModule, IntPtr lpszType, IntPtr lParam) {
			// enumerate resource names
			if (!Kernel32.EnumResourceNames(hModule, lpszType, EnumResourceNamesImpl, IntPtr.Zero)) {
				outerException?.Rethrow();
				throw new Win32Exception();
			}

			return true;
		}
		/// <summary>
		///  Enumerate resource names within a resource by type
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="lpszType">Resource type.</param>
		/// <param name="lpszName">Resource name.</param>
		/// <param name="lParam">Additional parameter.</param>
		/// <returns>TRUE if successful.</returns>
		private bool EnumResourceNamesImpl(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam) {
			if (!Kernel32.EnumResourceLanguages(hModule, lpszType, lpszName, EnumResourceLanguages, IntPtr.Zero)) {
				outerException?.Rethrow();
				throw new Win32Exception();
			}

			return true;
		}
		/// <summary>
		///  Enumerate resource languages within a resource by name
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="lpszType">Resource type.</param>
		/// <param name="lpszName">Resource name.</param>
		/// <param name="wIDLanguage">Language ID.</param>
		/// <param name="lParam">Additional parameter.</param>
		/// <returns>TRUE if successful.</returns>
		private bool EnumResourceLanguages(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, ushort wIDLanguage, IntPtr lParam) {
			ResourceId type = lpszType;
			ResourceId name = lpszName;
			try {
				/*if (!resourceLists.TryGetValue(type, out var resources)) {
					resources = new ResourceCollection(type);
					resourceLists[type] = resources;
				}*/
				//resources.Add(CreateResource(hModule, type, name, wIDLanguage));
				Add(CreateResource(hModule, type, name, wIDLanguage));
			} catch (Exception ex) {
				outerException = ex;
				throw;
			}

			return true;
		}

		#endregion
	}
}