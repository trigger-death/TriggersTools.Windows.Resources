using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.Windows.Resources {
	public delegate Resource CreateResourceDelegate();
	public class ResourceLoadSettings {
		public Dictionary<ResourceId, CreateResourceDelegate> CustomResources { get; }
			= new Dictionary<ResourceId, CreateResourceDelegate>();
	}
}
