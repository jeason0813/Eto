using Eto.Drawing;
using Eto.Forms;
using MonoMac.AppKit;

#if IOS
using NSView = MonoTouch.UIKit.UIView;
using NSControl = MonoTouch.UIKit.UIControl;
#endif
namespace Eto.Platform.Mac.Forms
{
	public static class MacControlExtensions
	{
		public static SizeF GetPreferredSize(this Control control, SizeF availableSize)
		{
			if (control == null)
				return Size.Empty;
			var mh = control.GetMacAutoSizing();
			if (mh != null)
			{
				return mh.GetPreferredSize(availableSize);
			}
			
			var c = control.ControlObject as NSControl;
			if (c != null)
			{
				c.SizeToFit();
				return c.Frame.Size.ToEto();
			}
			var child = control.ControlObject as Control;
			return child == null ? SizeF.Empty : child.GetPreferredSize(availableSize);

		}

		public static IMacContainer GetMacContainer(this Control control)
		{
			if (control == null)
				return null;
			var container = control.Handler as IMacContainer;
			if (container != null)
				return container;
			var child = control.ControlObject as Control;
			return child == null ? null : child.GetMacContainer();
		}

		public static IMacAutoSizing GetMacAutoSizing(this Control control)
		{
			if (control == null)
				return null;
			var container = control.Handler as IMacAutoSizing;
			if (container != null)
				return container;
			var child = control.ControlObject as Control;
			return child == null ? null : child.GetMacAutoSizing();
		}

		public static NSView GetContainerView(this Control control)
		{
			if (control == null)
				return null;
			var containerHandler = control.Handler as IMacContainerControl;
			if (containerHandler != null)
				return containerHandler.ContainerControl;
			var childControl = control.ControlObject as Control;
			if (childControl != null)
				return childControl.GetContainerView();
			return control.ControlObject as NSView;
		}
	}
}
