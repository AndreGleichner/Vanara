﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using Vanara.InteropServices;
using static Vanara.Extensions.BitHelper;
using static Vanara.PInvoke.AdvApi32;
using static Vanara.PInvoke.ComCtl32;
using static Vanara.PInvoke.Kernel32;
using static Vanara.PInvoke.Ole32;
using static Vanara.PInvoke.ShlwApi;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable UnusedMethodReturnValue.Global

namespace Vanara.PInvoke
{
	[SuppressUnmanagedCodeSecurity]
	public static partial class Shell32
	{
		// Defined in wingdi.h
		private const int LF_FACESIZE = 32;

		/// <summary>
		/// <para>
		/// [This interface is supported through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be unsupported in
		/// subsequent versions of Windows.]
		/// </para>
		/// <para>
		/// Defines the prototype for the callback function used by the system folder view object. This function essentially duplicates the
		/// functionality of <c>IShellFolderViewCB</c>.
		/// </para>
		/// </summary>
		/// <param name="psvOuter">
		/// <para>Type: <c><c>IShellView</c>*</c></para>
		/// <para>A pointer to the owning instance of <c>IShellView</c>, if applicable. This parameter can be <c>NULL</c>.</para>
		/// </param>
		/// <param name="psf">
		/// <para>Type: <c><c>IShellFolder</c>*</c></para>
		/// <para>A pointer to the instance of <c>IShellFolder</c> the message applies to.</para>
		/// </param>
		/// <param name="hwndMain">
		/// <para>Type: <c>HWND</c></para>
		/// <para>The handle of the window that contains the view that receives the message.</para>
		/// </param>
		/// <param name="uMsg">
		/// <para>Type: <c>UINT</c></para>
		/// <para>One of the following notifications.</para>
		/// </param>
		/// <param name="wParam">
		/// <para>Type: <c>WPARAM</c></para>
		/// <para>Additional information dependent on the value in uMsg. See the individual notification pages for specific requirements.</para>
		/// </param>
		/// <param name="lParam">
		/// <para>Type: <c>LPARAM</c></para>
		/// <para>Additional information dependent on the value in uMsg. See the individual notification pages for specific requirements.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function pointer succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// typedef HRESULT ( CALLBACK *LPFNVIEWCALLBACK)( _In_ IShellView *psvOuter, _In_ IShellFolder *psf, _In_ HWND hwndMain, UINT uMsg,
		// WPARAM wParam, LPARAM lParam); https://msdn.microsoft.com/en-us/library/windows/desktop/bb776771(v=vs.85).aspx
		[PInvokeData("Shlobj_core.h", MSDNShortId = "bb776771")]
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		public delegate HRESULT LPFNVIEWCALLBACK(IShellView psvOuter, IShellFolder psf, IntPtr hwndMain, uint uMsg, IntPtr wParam, IntPtr lParam);

		/// <summary>A flag that controls how PifMgr_CloseProperties operates.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "fd50d4f8-87c8-4162-9e88-3c8592b929fa")]
		public enum CLOSEPROPS
		{
			/// <summary>No options specified.</summary>
			CLOSEPROPS_NONE = 0x0000,

			/// <summary>Abandon cached data.</summary>
			CLOSEPROPS_DISCARD = 0x0001
		}

		/// <summary>
		/// <para>
		/// Represents information about the effects of a drag-and-drop operation. The <c>DoDragDrop</c> function and many of the methods in
		/// the <c>IDropSource</c> and <c>IDropTarget</c> use the values of this enumeration.
		/// </para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// Your application should always mask values from the <c>DROPEFFECT</c> enumeration to ensure compatibility with future
		/// implementations. Presently, only some of the positions in a <c>DROPEFFECT</c> value have meaning. In the future, more
		/// interpretations for the bits will be added. Drag sources and drop targets should carefully mask these values appropriately before
		/// comparing. They should never compare a <c>DROPEFFECT</c> against, say, DROPEFFECT_COPY by doing the following:
		/// </para>
		/// <para>Instead, the application should always mask for the value or values being sought as using one of the following techniques:</para>
		/// <para>This allows for the definition of new drop effects, while preserving backward compatibility with existing code.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/com/dropeffect-constants
		[PInvokeData("OleIdl.h", MSDNShortId = "d8e46899-3fbf-4012-8dd3-67fa627526d5")]
		// public static extern
		public enum DROPEFFECT : uint
		{
			/// <summary>Drop target cannot accept the data.</summary>
			DROPEFFECT_NONE = 0,

			/// <summary>Drop results in a copy. The original data is untouched by the drag source.</summary>
			DROPEFFECT_COPY = 1,

			/// <summary>Drag source should remove the data.</summary>
			DROPEFFECT_MOVE = 2,

			/// <summary>Drag source should create a link to the original data.</summary>
			DROPEFFECT_LINK = 4,

			/// <summary>
			/// Scrolling is about to start or is currently occurring in the target. This value is used in addition to the other values.
			/// </summary>
			DROPEFFECT_SCROLL = 0x80000000,
		}

		/// <summary>A flag that controls the action of SHGetSetFolderCustomSettings.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "38b78a4b-ba68-4dff-812d-d4c7421eb202")]
		[Flags]
		public enum FCS
		{
			/// <summary>Retrieve the custom folder settings in pfcs.</summary>
			FCS_READ = 0x00000001,

			/// <summary>Use pfcs to set the custom folder's settings regardless of whether the values are already present.</summary>
			FCS_FORCEWRITE = 0x00000002,

			/// <summary>Use pfcs to set the custom folder's settings if the values are not already present.</summary>
			FCS_WRITE = (FCS_READ | FCS_FORCEWRITE),
		}

		/// <summary>Flags used by SHFOLDERCUSTOMSETTINGS.dwMask.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "a6357372-80ef-4719-b53f-87eb3fdc1b0d")]
		[Flags]
		public enum FOLDERCUSTOMSETTINGSMASK : uint
		{
			/// <summary>Deprecated. pvid contains the folder's GUID.</summary>
			FCSM_VIEWID = 0x0001,

			/// <summary>Deprecated. pszWebViewTemplate contains a pointer to a buffer containing the path to the folder's WebView template.</summary>
			FCSM_WEBVIEWTEMPLATE = 0x0002,

			/// <summary>pszInfoTip contains a pointer to a buffer containing the folder's info tip.</summary>
			FCSM_INFOTIP = 0x0004,

			/// <summary>pclsid contains the folder's CLSID.</summary>
			FCSM_CLSID = 0x0008,

			/// <summary>pszIconFile contains the path to the file containing the folder's icon.</summary>
			FCSM_ICONFILE = 0x0010,

			/// <summary>pszLogo contains the path to the file containing the folder's thumbnail icon.</summary>
			FCSM_LOGO = 0x0020,

			/// <summary>Not used.</summary>
			FCSM_FLAGS = 0x0040,
		}

		/// <summary>Flags used by SHGetPathFromIDListEx.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "80270c59-275d-4b13-b16c-0c07bb79ed8e")]
		[Flags]
		public enum GPFIDL_FLAGS
		{
			/// <summary>Win32 file names, servers, and root drives are included.</para>
			GPFIDL_DEFAULT = 0x0000,

			/// <summary>Uses short file names.</para>
			GPFIDL_ALTNAME = 0x0001,

			/// <summary>Include UNC printer names items.</para>
			GPFIDL_UNCPRINTER = 0x0002,
		}

		/// <summary>Flags used by Shell_MergeMenus.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "f9e005fd-b1f2-4a5f-ad36-9c44998dc4eb")]
		[Flags]
		public enum MM
		{
			/// <summary>
			/// Add a separator between the items from the two menus if one does not exist already. If you are inserting the entries from
			/// hmSrc into the middle of hmDst, a separator is added above and below the hmSrc material.
			/// </summary>
			MM_ADDSEPARATOR = 0x00000001,

			/// <summary>Do not remove any existing separators in the two menus. Note that this could result in two separators in a row.</summary>
			MM_SUBMENUSHAVEIDS = 0x00000002,

			/// <summary>Set this flag if the submenus have IDs which should be adjusted.</summary>
			MM_DONTREMOVESEPS = 0x00000004,
		}

		/// <summary>Used for options in SHOpenFolderAndSelectItems.</summary>
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762232")]
		public enum OFASI : uint
		{
			/// <summary>No options.</summary>
			OFASI_NONE = 0,

			/// <summary>
			/// Select an item and put its name in edit mode. This flag can only be used when a single item is being selected. For multiple
			/// item selections, it is ignored.
			/// </summary>
			OFASI_EDIT = 1,

			/// <summary>
			/// Select the item or items on the desktop rather than in a Windows Explorer window. Note that if the desktop is obscured behind
			/// open windows, it will not be made visible.
			/// </summary>
			OFASI_OPENDESKTOP = 2
		}

		/// <summary>A flag that controls how PifMgr_OpenProperties operates.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "0bc11528-7278-4765-b3cb-671ba82c9155")]
		public enum OPENPROPS
		{
			/// <summary>No options specified.</summary>
			OPENPROPS_NONE = 0x0000,

			/// <summary>
			/// Ignore any existing .pif files and get the properties from win.ini or _Default.pif. This flag is ignored on Windows NT,
			/// Windows 2000, and Windows XP.
			/// </summary>
			OPENPROPS_INHIBITPIF = 0x8000
		}

		/// <summary>Return values for PathCleanupSpec.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "593fd2b7-44ae-4309-a185-97e42f3cc0fa")]
		[Flags]
		public enum PCS : uint
		{
			/// <summary>The cleaned path is not a valid file name. This flag is always returned in conjunction with PCS_PATHTOOLONG.</summary>
			PCS_FATAL = 0x80000000,

			/// <summary>Replaced one or more invalid characters.</summary>
			PCS_REPLACEDCHAR = 0x00000001,

			/// <summary>Removed one or more invalid characters.</summary>
			PCS_REMOVEDCHAR = 0x00000002,

			/// <summary>The returned path is truncated.</summary>
			PCS_TRUNCATED = 0x00000004,

			/// <summary>
			/// The function failed because the input path specified at is too long to allow the formation of a valid file name from . When
			/// this flag is returned, it is always accompanied by the PCS_FATAL flag.
			/// </summary>
			PCS_PATHTOOLONG = 0x00000008,
		}

		/// <summary>Flags for PathResolve.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "84bf0b56-513f-4ac6-b2cf-11f0c471da1e")]
		[Flags]
		public enum PRF
		{
			/// <summary>Return TRUE if the file's existence is verified; otherwise FALSE.</summary>
			PRF_VERIFYEXISTS = 0x0001,

			/// <summary>Look for the specified path with the following extensions appended: .pif, .com, .bat, .cmd, .lnk, and .exe.</summary>
			PRF_TRYPROGRAMEXTENSIONS = 0x0002 | PRF_VERIFYEXISTS,

			/// <summary>Look first in the directory or directories specified by dirs.</summary>
			PRF_FIRSTDIRDEF = 0x0004,

			/// <summary>Ignore .lnk files.</summary>
			PRF_DONTFINDLNK = 0x0008,

			/// <summary>Require an absolute (full) path.</summary>
			PRF_REQUIREABSOLUTE = 0x0010
		}

		/// <summary>
		/// Flags that direct the handling of the item from which you're retrieving the info tip text. This value is commonly zero (QITIPF_DEFAULT).
		/// </summary>
		[PInvokeData("Shlobj.h", MSDNShortId = "bb761357")]
		public enum QITIP
		{
			/// <summary>No special handling.</summary>
			QITIPF_DEFAULT = 0x00000000,

			/// <summary>Provide the name of the item in ppwszTip rather than the info tip text.</summary>
			QITIPF_USENAME = 0x00000001,

			/// <summary>If the item is a shortcut, retrieve the info tip text of the shortcut rather than its target.</summary>
			QITIPF_LINKNOTARGET = 0x00000002,

			/// <summary>If the item is a shortcut, retrieve the info tip text of the shortcut's target.</summary>
			QITIPF_LINKUSETARGET = 0x00000004,

			/// <summary>Search the entire namespace for the information. This value can result in a delayed response time.</summary>
			QITIPF_USESLOWTIP = 0x00000008, // Flag says it's OK to take a long time generating tip

			/// <summary><c>Windows Vista and later.</c> Put the info tip on a single line.</summary>
			QITIPF_SINGLELINE = 0x00000010,
		}

		/// <summary>
		/// <para>Indicates whether to enable or disable Async Register and Deregister for SHChangeNotifyRegisterThread.</para>
		/// </summary>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/ne-shlobj_core-scnrt_status typedef enum SCNRT_STATUS {
		// SCNRT_ENABLE , SCNRT_DISABLE } ;
		[PInvokeData("shlobj_core.h", MSDNShortId = "31fd993b-d8cb-40cc-9f31-15711dba1b10")]
		public enum SCNRT_STATUS
		{
			/// <summary>Enable Async Register and Deregister for SHChangeNotifyRegisterThread.</summary>
			SCNRT_ENABLE,

			/// <summary>Disable Async Register and Deregister for SHChangeNotifyRegisterThread.</summary>
			SCNRT_DISABLE,
		}

		/// <summary>
		/// Indicates the interpretation of the data passed by SHAddToRecentDocs in its pv parameter to identify the item whose usage
		/// statistics are being tracked.
		/// </summary>
		[PInvokeData("Shlobj.h", MSDNShortId = "dd378453")]
		public enum SHARD
		{
			/// <summary>
			/// <c>Windows 7 and later.</c> The pv parameter points to a SHARDAPPIDINFO structure that pairs an IShellItem that identifies
			/// the item with an AppUserModelID that associates it with a particular process or application.
			/// </summary>
			SHARD_APPIDINFO = 4,

			/// <summary>
			/// <c>Windows 7 and later.</c> The pv parameter points to a SHARDAPPIDINFOIDLIST structure that pairs an absolute PIDL that
			/// identifies the item with an AppUserModelID that associates it with a particular process or application.
			/// </summary>
			SHARD_APPIDINFOIDLIST = 5,

			/// <summary>
			/// <c>Windows 7 and later.</c> The pv parameter points to a SHARDAPPIDINFOLINK structure that pairs an IShellLink that
			/// identifies the item with an AppUserModelID that associates it with a particular process or application.
			/// </summary>
			SHARD_APPIDINFOLINK = 7,

			/// <summary><c>Windows 7 and later.</c> The pv parameter is an interface pointer to an IShellLink object.</summary>
			SHARD_LINK = 6,

			/// <summary>The pv parameter points to a null-terminated ANSI string with the path and file name of the object.</summary>
			SHARD_PATHA = 2,

			/// <summary>The pv parameter points to a null-terminated Unicode string with the path and file name of the object.</summary>
			SHARD_PATHW = 3,

			/// <summary>
			/// The pv parameter points to a PIDL that identifies the document's file object. PIDLs that identify non-file objects are not accepted.
			/// </summary>
			SHARD_PIDL = 1,

			/// <summary><c>Windows 7 and later.</c> The pv parameter is an interface pointer to an IShellItem object.</summary>
			SHARD_SHELLITEM = 8
		}

		/// <summary>Events used in SHChangeNotify.</summary>
		[PInvokeData("Shlobj_core.h")]
		[Flags]
		public enum SHCNE : uint
		{
			/// <summary>
			/// The name of a nonfolder item has changed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the
			/// previous PIDL or name of the item. dwItem2 contains the new PIDL or name of the item.
			/// </summary>
			SHCNE_RENAMEITEM = 0x00000001,

			/// <summary>
			/// A nonfolder item has been created. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the item that was
			/// created. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_CREATE = 0x00000002,

			/// <summary>
			/// A nonfolder item has been deleted. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the item that was
			/// deleted. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_DELETE = 0x00000004,

			/// <summary>
			/// A folder has been created. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the folder that was
			/// created. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_MKDIR = 0x00000008,

			/// <summary>
			/// A folder has been removed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the folder that was
			/// removed. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_RMDIR = 0x00000010,

			/// <summary>
			/// Storage media has been inserted into a drive. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the
			/// root of the drive that contains the new media. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_MEDIAINSERTED = 0x00000020,

			/// <summary>
			/// Storage media has been removed from a drive. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the
			/// root of the drive from which the media was removed. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_MEDIAREMOVED = 0x00000040,

			/// <summary>
			/// A drive has been removed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the root of the drive that
			/// was removed. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_DRIVEREMOVED = 0x00000080,

			/// <summary>
			/// A drive has been added. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the root of the drive that
			/// was added. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_DRIVEADD = 0x00000100,

			/// <summary>
			/// A folder on the local computer is being shared via the network. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
			/// dwItem1 contains the folder that is being shared. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_NETSHARE = 0x00000200,

			/// <summary>
			/// A folder on the local computer is no longer being shared via the network. SHCNF_IDLIST or SHCNF_PATH must be specified in
			/// uFlags. dwItem1 contains the folder that is no longer being shared. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_NETUNSHARE = 0x00000400,

			/// <summary>
			/// The attributes of an item or folder have changed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains
			/// the item or folder that has changed. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_ATTRIBUTES = 0x00000800,

			/// <summary>
			/// The contents of an existing folder have changed, but the folder still exists and has not been renamed. SHCNF_IDLIST or
			/// SHCNF_PATH must be specified in uFlags. dwItem1 contains the folder that has changed. dwItem2 is not used and should be NULL.
			/// If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or SHCNE_RENAMEFOLDER, respectively.
			/// </summary>
			SHCNE_UPDATEDIR = 0x00001000,

			/// <summary>
			/// An existing item (a folder or a nonfolder) has changed, but the item still exists and has not been renamed. SHCNF_IDLIST or
			/// SHCNF_PATH must be specified in uFlags. dwItem1 contains the item that has changed. dwItem2 is not used and should be NULL.
			/// If a nonfolder item has been created, deleted, or renamed, use SHCNE_CREATE, SHCNE_DELETE, or SHCNE_RENAMEITEM, respectively, instead.
			/// </summary>
			SHCNE_UPDATEITEM = 0x00002000,

			/// <summary>
			/// The computer has disconnected from a server. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the
			/// server from which the computer was disconnected. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_SERVERDISCONNECT = 0x00004000,

			/// <summary>
			/// An image in the system image list has changed. SHCNF_DWORD must be specified in uFlags. dwItem2 contains the index in the
			/// system image list that has changed. dwItem1 is not used and should be NULL.
			/// </summary>
			SHCNE_UPDATEIMAGE = 0x00008000,

			/// <summary>
			/// A drive has been added. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the root of the drive that
			/// was added. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_DRIVEADDGUI = 0x00010000,

			/// <summary>
			/// The name of a folder has changed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the previous PIDL
			/// or name of the folder. dwItem2 contains the new PIDL or name of the folder.
			/// </summary>
			SHCNE_RENAMEFOLDER = 0x00020000,

			/// <summary>
			/// The amount of free space on a drive has changed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1 contains the
			/// root of the drive on which the free space changed. dwItem2 is not used and should be NULL.
			/// </summary>
			SHCNE_FREESPACE = 0x00040000,

			/// <summary>Not currently used.</summary>
			SHCNE_EXTENDED_EVENT = 0x04000000,

			/// <summary>
			/// A file type association has changed. SHCNF_IDLIST must be specified in the uFlags parameter. dwItem1 and dwItem2 are not used
			/// and must be NULL. This event should also be sent for registered protocols.
			/// </summary>
			SHCNE_ASSOCCHANGED = 0x08000000,

			/// <summary>All disk related events.</summary>
			SHCNE_DISKEVENTS = 0x0002381F,

			/// <summary>All global events.</summary>
			SHCNE_GLOBALEVENTS = 0x0C0581E0,

			/// <summary>All events.</summary>
			SHCNE_ALLEVENTS = 0x7FFFFFFF,

			/// <summary>
			/// The presence of this flag indicates that the event was generated by an interrupt. It is stripped out before the clients of
			/// SHCNNotify_ see it.
			/// </summary>
			SHCNE_INTERRUPT = 0x80000000,
		}

		/// <summary>Flags used in SHChangeNotify.</summary>
		[PInvokeData("Shlobj_core.h")]
		[Flags]
		public enum SHCNF : uint
		{
			/// <summary>
			/// dwItem1 and dwItem2 are the addresses of ITEMIDLIST structures that represent the item(s) affected by the change. Each
			/// ITEMIDLIST must be relative to the desktop folder.
			/// </summary>
			SHCNF_IDLIST = 0x0000,

			/// <summary>
			/// dwItem1 and dwItem2 are the addresses of null-terminated strings of maximum length MAX_PATH that contain the full path names
			/// of the items affected by the change.
			/// </summary>
			SHCNF_PATHA = 0x0001,

			/// <summary>
			/// dwItem1 and dwItem2 are the addresses of null-terminated strings that represent the friendly names of the printer(s) affected
			/// by the change.
			/// </summary>
			SHCNF_PRINTERA = 0x0002,

			/// <summary>The dwItem1 and dwItem2 parameters are DWORD values.</summary>
			SHCNF_DWORD = 0x0003,

			/// <summary>
			/// dwItem1 and dwItem2 are the addresses of null-terminated strings of maximum length MAX_PATH that contain the full path names
			/// of the items affected by the change.
			/// </summary>
			SHCNF_PATHW = 0x0005,

			/// <summary>
			/// dwItem1 and dwItem2 are the addresses of null-terminated strings that represent the friendly names of the printer(s) affected
			/// by the change.
			/// </summary>
			SHCNF_PRINTERW = 0x0006,

			/// <summary>Indicates that a type is defined.</summary>
			SHCNF_TYPE = 0x00FF,

			/// <summary>
			/// The function should not return until the notification has been delivered to all affected components. As this flag modifies
			/// other data-type flags, it cannot be used by itself.
			/// </summary>
			SHCNF_FLUSH = 0x1000,

			/// <summary>
			/// The function should begin delivering notifications to all affected components but should return as soon as the notification
			/// process has begun. As this flag modifies other data-type flags, it cannot by used by itself. This flag includes SHCNF_FLUSH.
			/// </summary>
			SHCNF_FLUSHNOWAIT = 0x3000,

			/// <summary>Notify clients registered for all children.</summary>
			SHCNF_NOTIFYRECURSIVE = 0x10000
		}

		/// <summary>Flags used by SHChangeNotifyRegister.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "73143865-ca2f-4578-a7a2-2ba4833eddd8")]
		[Flags]
		public enum SHCNRF
		{
			/// <summary>Interrupt level notifications from the file system.</summary>
			SHCNRF_InterruptLevel = 0x0001,

			/// <summary>Shell-level notifications from the shell.</summary>
			SHCNRF_ShellLevel = 0x0002,

			/// <summary>
			/// Interrupt events on the whole subtree. This flag must be combined with the SHCNRF_InterruptLevel flag. When using this flag,
			/// notifications must also be made recursive by setting the fRecursive member of the corresponding SHChangeNotifyEntry structure
			/// referenced by pshcne to TRUE. Use of SHCNRF_RecursiveInterrupt on a single level view—for example, a PIDL that is relative
			/// and contains only one SHITEMID—will block event notification at the highest level and thereby prevent a recursive, child
			/// update. Thus, an icon dragged into the lowest level of a folder hierarchy may fail to appear in the view as expected.
			/// </summary>
			SHCNRF_RecursiveInterrupt = 0x1000,

			/// <summary>
			/// Messages received use shared memory. Call SHChangeNotification_Lock to access the actual data. Call
			/// SHChangeNotification_Unlock to release the memory when done. <note type="note">We recommend this flag because it provides a
			/// more robust delivery method. All clients should specify this flag.</note>
			/// </summary>
			SHCNRF_NewDelivery = 0x8000,
		}

		/// <summary>Receives a value that determines what type the item is in <see cref="SHDESCRIPTIONID"/>.</summary>
		[PInvokeData("Shlobj_core.h", MSDNShortId = "bb759775")]
		public enum SHDID
		{
			/// <summary>The item is a registered item on the desktop.</summary>
			SHDID_ROOT_REGITEM = 1,

			/// <summary>The item is a file.</summary>
			SHDID_FS_FILE = 2,

			/// <summary>The item is a folder.</summary>
			SHDID_FS_DIRECTORY = 3,

			/// <summary>The item is an unidentified item in the file system.</summary>
			SHDID_FS_OTHER = 4,

			/// <summary>The item is a 3.5-inch floppy drive.</summary>
			SHDID_COMPUTER_DRIVE35 = 5,

			/// <summary>The item is a 5.25-inch floppy drive.</summary>
			SHDID_COMPUTER_DRIVE525 = 6,

			/// <summary>The item is a removable disk.</summary>
			SHDID_COMPUTER_REMOVABLE = 7,

			/// <summary>The item is a fixed hard disk.</summary>
			SHDID_COMPUTER_FIXED = 8,

			/// <summary>The item is a drive that is mapped to a network share.</summary>
			SHDID_COMPUTER_NETDRIVE = 9,

			/// <summary>The item is a CD-ROM drive.</summary>
			SHDID_COMPUTER_CDROM = 10,

			/// <summary>The item is a RAM disk.</summary>
			SHDID_COMPUTER_RAMDISK = 11,

			/// <summary>The item is an unidentified system device.</summary>
			SHDID_COMPUTER_OTHER = 12,

			/// <summary>The item is a network domain.</summary>
			SHDID_NET_DOMAIN = 13,

			/// <summary>The item is a network server.</summary>
			SHDID_NET_SERVER = 14,

			/// <summary>The item is a network share.</summary>
			SHDID_NET_SHARE = 15,

			/// <summary>Not currently used.</summary>
			SHDID_NET_RESTOFNET = 16,

			/// <summary>The item is an unidentified network resource.</summary>
			SHDID_NET_OTHER = 17,

			/// <summary>Windows XP and later. Not currently used.</summary>
			SHDID_COMPUTER_IMAGING = 18,

			/// <summary>Windows XP and later. Not currently used.</summary>
			SHDID_COMPUTER_AUDIO = 19,

			/// <summary>Windows XP and later. The item is the system shared documents folder.</summary>
			SHDID_COMPUTER_SHAREDDOCS = 20,

			/// <summary>Windows Vista and later. The item is a mobile device, such as a personal digital assistant (PDA).</summary>
			SHDID_MOBILE_DEVICE = 21,
		}

		/// <summary>Format ID for SHFormatDrive.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "4aa255fa-c407-47db-9b1f-d449e0a0e94f")]
		public enum SHFMT_ID
		{
			/// <summary>The default format ID.</summary>
			SHFMT_ID_DEFAULT = 0xFFFF
		}

		/// <summary>Format options for SHFormatDrive.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "4aa255fa-c407-47db-9b1f-d449e0a0e94f")]
		[Flags]
		public enum SHFMT_OPT
		{
			/// <summary>If this flag is set, then the Quick Format option is selected.</summary>
			SHFMT_OPT_FULL = 0x0001,

			/// <summary>Selects the Create an MS-DOS startup disk option, creating a system boot disk.</summary>
			SHFMT_OPT_SYSONLY = 0x0002
		}

		/// <summary>The format in which the data is being requested.</summary>
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762174")]
		public enum SHGetDataFormat
		{
			/// <summary>Format used for file system objects. The pv parameter is the address of a WIN32_FIND_DATA structure.</summary>
			[CorrespondingType(typeof(WIN32_FIND_DATA), CorrepsondingAction.Get)]
			SHGDFIL_FINDDATA = 1,

			/// <summary>Format used for network resources. The pv parameter is the address of a NETRESOURCE structure.</summary>
			// TODO: Define NETRESOURCE (https://msdn.microsoft.com/en-us/library/windows/desktop/aa385353(v=vs.85).aspx)
			//[CorrespondingType(typeof(NETRESOURCE), CorrepsondingAction.Get)]
			SHGDFIL_NETRESOURCE = 2,

			/// <summary>Version 4.71. Format used for network resources. The pv parameter is the address of an SHDESCRIPTIONID structure.</summary>
			[CorrespondingType(typeof(SHDESCRIPTIONID), CorrepsondingAction.Get)]
			SHGDFIL_DESCRIPTIONID = 3
		}

		/// <summary>Flags used by <see cref="SHGetFolderPath"/>.</summary>
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762181")]
		public enum SHGFP
		{
			/// <summary>Retrieve the folder's current path.</summary>
			SHGFP_TYPE_CURRENT = 0,

			/// <summary>Retrieve the folder's default path.</summary>
			SHGFP_TYPE_DEFAULT = 1
		}

		/// <summary>Used by SHGetImageList.</summary>
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762185")]
		public enum SHIL
		{
			/// <summary>
			/// The image size is normally 32x32 pixels. However, if the Use large icons option is selected from the Effects section of the
			/// Appearance tab in Display Properties, the image is 48x48 pixels.
			/// </summary>
			SHIL_LARGE = 0,

			/// <summary>These images are the Shell standard small icon size of 16x16, but the size can be customized by the user.</summary>
			SHIL_SMALL = 1,

			/// <summary>
			/// These images are the Shell standard extra-large icon size. This is typically 48x48, but the size can be customized by the user.
			/// </summary>
			SHIL_EXTRALARGE = 2,

			/// <summary>
			/// These images are the size specified by GetSystemMetrics called with SM_CXSMICON and GetSystemMetrics called with SM_CYSMICON.
			/// </summary>
			SHIL_SYSSMALL = 3,

			/// <summary>Windows Vista and later. The image is normally 256x256 pixels.</summary>
			SHIL_JUMBO = 4,
		}

		/// <summary>Object type options for SHObjectProperties.</summary>
		[PInvokeData("shlobj_core.h", MSDNShortId = "7517c461-955b-446e-85d7-a707c9bd183a")]
		[Flags]
		public enum SHOP
		{
			/// <summary>Contains the friendly name of a printer.</summary>
			SHOP_PRINTERNAME = 0x00000001,

			/// <summary>Contains a fully qualified file name.</summary>
			SHOP_FILEPATH = 0x00000002,

			/// <summary>
			/// Contains either (a) a volume name of the form \?\Volume{GUID}, where {GUID} is a globally unique identifier (for example,
			/// "\?\Volume{2eca078d-5cbc-43d3-aff8-7e8511f60d0e})", or (b) a drive path (for example, "C:").
			/// </summary>
			SHOP_VOLUMEGUID = 0x00000004,
		}

		/// <summary>
		/// Used by the <c>SHGetSetSettings</c> function to specify which members of its <c>SHELLSTATE</c> structure should be set or retrived.
		/// </summary>
		// https://msdn.microsoft.com/en-us/2a883110-fdc3-4451-9e47-e58894600e3b
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762591")]
		[Flags]
		public enum SSF : uint
		{
			/// <summary>The fShowAllObjects member is being requested.</summary>
			SSF_SHOWALLOBJECTS = 0x00000001,

			/// <summary>The fShowExtensions member is being requested.</summary>
			SSF_SHOWEXTENSIONS = 0x00000002,

			/// <summary>Not used.</summary>
			SSF_HIDDENFILEEXTS = 0x00000004,

			/// <summary>Not used.</summary>
			SSF_SERVERADMINUI = 0x00000004,

			/// <summary>The fShowCompColor member is being requested.</summary>
			SSF_SHOWCOMPCOLOR = 0x00000008,

			/// <summary>The lParamSort and iSortDirection members are being requested.</summary>
			SSF_SORTCOLUMNS = 0x00000010,

			/// <summary>The fShowSysFiles member is being requested.</summary>
			SSF_SHOWSYSFILES = 0x00000020,

			/// <summary>The fDoubleClickInWebView member is being requested.</summary>
			SSF_DOUBLECLICKINWEBVIEW = 0x00000080,

			/// <summary>
			/// The fShowAttribCol member is being requested.
			/// <para>Windows Vista: Not used.</para>
			/// </summary>
			SSF_SHOWATTRIBCOL = 0x00000100,

			/// <summary>
			/// The fDesktopHTML member is being requested. Set is not available. Instead, for versions of Windows prior to Windows XP,
			/// enable desktop HTML by IActiveDesktop. The use of IActiveDesktop for this purpose, however, is not recommended for Windows XP
			/// and later versions of Windows, and is deprecated in Windows Vista.
			/// </summary>
			SSF_DESKTOPHTML = 0x00000200,

			/// <summary>The fWin95Classic member is being requested.</summary>
			SSF_WIN95CLASSIC = 0x00000400,

			/// <summary>The fDontPrettyPath member is being requested.</summary>
			SSF_DONTPRETTYPATH = 0x00000800,

			/// <summary>The fMapNetDrvBtn member is being requested.</summary>
			SSF_MAPNETDRVBUTTON = 0x00001000,

			/// <summary>The fShowInfoTip member is being requested.</summary>
			SSF_SHOWINFOTIP = 0x00002000,

			/// <summary>The fHideIcons member is being requested.</summary>
			SSF_HIDEICONS = 0x00004000,

			/// <summary>The fNoConfirmRecycle member is being requested.</summary>
			SSF_NOCONFIRMRECYCLE = 0x00008000,

			/// <summary>
			/// The fFilter member is being requested.
			/// <para>Windows Vista: Not used.</para>
			/// </summary>
			SSF_FILTER = 0x00010000,

			/// <summary>The fWebView member is being requested.</summary>
			SSF_WEBVIEW = 0x00020000,

			/// <summary>The fShowSuperHidden member is being requested.</summary>
			SSF_SHOWSUPERHIDDEN = 0x00040000,

			/// <summary>The fSepProcess member is being requested.</summary>
			SSF_SEPPROCESS = 0x00080000,

			/// <summary>Windows XP and later. The fNoNetCrawling member is being requested.</summary>
			SSF_NONETCRAWLING = 0x00100000,

			/// <summary>Windows XP and later. The fStartPanelOn member is being requested.</summary>
			SSF_STARTPANELON = 0x00200000,

			/// <summary>Not used.</summary>
			SSF_SHOWSTARTPAGE = 0x00400000,

			/// <summary>Windows Vista and later. The fAutoCheckSelect member is being requested.</summary>
			SSF_AUTOCHECKSELECT = 0x00800000,

			/// <summary>Windows Vista and later. The fIconsOnly member is being requested.</summary>
			SSF_ICONSONLY = 0x01000000,

			/// <summary>Windows Vista and later. The fShowTypeOverlay member is being requested.</summary>
			SSF_SHOWTYPEOVERLAY = 0x02000000,

			/// <summary>Windows 8 and later: The fShowStatusBar member is being requested.</summary>
			SSF_SHOWSTATUSBAR = 0x04000000,
		}

		/// <summary>
		/// CSIDL (constant special item ID list) values provide a unique system-independent way to identify special folders used frequently
		/// by applications, but which may not have the same name or location on any given system. For example, the system folder may be
		/// "C:\Windows" on one system and "C:\Winnt" on another. These constants are defined in Shlobj.h.
		/// </summary>
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762494")]
		internal enum CSIDL
		{
			CSIDL_ADMINTOOLS = 0x0030,
			CSIDL_CDBURN_AREA = 0x003b,
			CSIDL_COMMON_ADMINTOOLS = 0x002f,
			CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,
			CSIDL_COMMON_DOCUMENTS = 0x002e,
			CSIDL_COMMON_MUSIC = 0x0035,
			CSIDL_COMMON_OEM_LINKS = 0x003a,
			CSIDL_COMMON_PICTURES = 0x0036,
			CSIDL_COMMON_PROGRAMS = 0X0017,
			CSIDL_COMMON_STARTMENU = 0x0016,
			CSIDL_COMMON_STARTUP = 0x0018,
			CSIDL_COMMON_TEMPLATES = 0x002d,
			CSIDL_COMMON_VIDEO = 0x0037,
			CSIDL_FLAG_CREATE = 0x8000, // force folder creation in SHGetFolderPath
			CSIDL_FLAG_DONT_VERIFY = 0x4000, // return an unverified folder path
			CSIDL_FONTS = 0x0014, // windows\fonts
			CSIDL_MYVIDEO = 0x000e, // "My Videos" folder
			CSIDL_NETHOOD = 0x0013, // %APPDATA%\Microsoft\Windows\Network Shortcuts
			CSIDL_PRINTHOOD = 0x001b, // %APPDATA%\Microsoft\Windows\Printer Shortcuts
			CSIDL_PROFILE = 0x0028, // %USERPROFILE% (%SystemDrive%\Users\%USERNAME%)
			CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c, // x86 Program Files\Common on RISC
			CSIDL_PROGRAM_FILESX86 = 0x002a, // x86 C:\Program Files on RISC
			CSIDL_RESOURCES = 0x0038, // %windir%\Resources
			CSIDL_RESOURCES_LOCALIZED = 0x0039, // %windir%\resources\0409 (code page)
			CSIDL_SYSTEMX86 = 0x0029, // %windir%\system32
			CSIDL_WINDOWS = 0x0024, // GetWindowsDirectory()
		}

		/// <summary>
		/// <para>Retrieves the value for a given property key using the file association information provided by the Namespace Extensions.</para>
		/// </summary>
		/// <param name="psf">
		/// <para>Type: <c>IShellFolder*</c></para>
		/// <para>A pointer to the shell folder for which the details of the property key of the file association are being retrieved.</para>
		/// </param>
		/// <param name="pidl">
		/// <para>Type: <c>PCUITEMID_CHILD</c></para>
		/// <para>The PIDL of the child item for which the file associations are being requested.</para>
		/// </param>
		/// <param name="pkey">
		/// <para>Type: <c>PROPERTYKEY*</c></para>
		/// <para>A pointer to the property key that is being retrieved.</para>
		/// </param>
		/// <param name="pv">
		/// <para>Type: <c>VARIANT*</c></para>
		/// <para>When this function returns, contains the details of the given property key.</para>
		/// </param>
		/// <param name="pfFoundPropKey">
		/// <para>Type: <c>BOOL*</c></para>
		/// <para>When this function returns, contains a flag that is <c>TRUE</c> if the property key was found, otherwise <c>FALSE</c>.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This function is to be used only by implementers of IShellFolder Namespace Extensions. Other calling applications should use
		/// IShellFolder2::GetDetailsEx to get a value for a PROPERTYKEY. This function is to be used by implementers of <c>IShellFolder</c>
		/// Namespace Extensions.
		/// </para>
		/// <para>The provided namespace extension must support the use of this API in one of the following three ways.</para>
		/// <list type="number">
		/// <item>
		/// If the provided Namespace Extensions supports retrieving an IQueryAssociations interface for the item by implementing
		/// IShellFolder::GetUIObjectOf(..., <c>IID_IQueryAssociations</c>, ...), then <c>AssocGetDetailsOfPropKey</c> will use the provided
		/// file associations API to retrieve the value for the property key.
		/// </item>
		/// <item>
		/// If the provided namespace extension returns <c>SFGAO_FILESYSTEM</c> for the item from IShellFolder::GetAttributesOf and provides
		/// a parsing name for the item, then <c>AssocGetDetailsOfPropKey</c> will use the standard file system associations to retrieve the
		/// value for the property key.
		/// </item>
		/// <item>
		/// If the provided namespace extension returns <c>SFGAO_FOLDER</c> | <c>SFGAO_BROWSABLE</c> for the item from
		/// IShellFolder::GetAttributesOf, then <c>AssocGetDetailsOfPropKey</c> will use the file association for folders (
		/// <c>ASSOCCLASS_FOLDER</c>) to retrieve the value for the property key.
		/// </item>
		/// </list>
		/// <para>
		/// If the ShellFolder being implemented contains items that are extensible through the file associations mechanism, then you can use
		/// this function to retrieve
		/// </para>
		/// <para>PropertyKeys</para>
		/// <para>
		/// that are declared for a given file association. For example, if a given Shell folder drives a details pane and you want the
		/// properties displayed in that pane to be governed by third party file name extensions, then you can use this function to return
		/// </para>
		/// <para>PKEY_PropList_PreviewDetails</para>
		/// <para>
		/// . This key has a value that is declared in the registry for that file name extension with a semicolon delimited list of
		/// properties. There is a list of file name extension defined properties in the registry. This list includes but is not limited to
		/// the following:
		/// </para>
		/// <list type="bullet">
		/// <item><c>PKEY_PropList_PreviewDetails</c></item>
		/// <item><c>PKEY_PropList_PreviewTitle</c></item>
		/// <item><c>PKEY_PropList_FullDetails</c></item>
		/// <item><c>PKEY_PropList_TileInfo</c></item>
		/// <item><c>PKEY_PropList_ExtendedTileInfo</c></item>
		/// <item><c>PKEY_PropList_InfoTip</c></item>
		/// <item><c>PKEY_PropList_QuickTip</c></item>
		/// <item><c>PKEY_PropList_FileOperationPrompt</c></item>
		/// <item><c>PKEY_PropList_ConflictPrompt</c></item>
		/// <item><c>PKEY_PropList_SetDefaultsFor</c></item>
		/// <item><c>PKEY_PropList_NonPersonal</c></item>
		/// <item><c>PKEY_NewMenuPreferredTypes</c></item>
		/// <item><c>PKEY_NewMenuAllowedTypes</c></item>
		/// </list>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-assocgetdetailsofpropkey SHSTDAPI
		// AssocGetDetailsOfPropKey( IShellFolder *psf, PCUITEMID_CHILD pidl, const PROPERTYKEY *pkey, VARIANT *pv, BOOL *pfFoundPropKey );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "f13af5f4-1b6a-419c-a042-e05c9ec51d02")]
		public static extern HRESULT AssocGetDetailsOfPropKey(IShellFolder psf, PIDL pidl, ref PROPERTYKEY pkey, ref object pv, [MarshalAs(UnmanagedType.Bool)] ref bool pfFoundPropKey);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows.]
		/// </para>
		/// <para>Creates an <c>Open</c> dialog box so that the user can specify the drive, directory, and name of a file to open.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>
		/// A handle to the window that owns the dialog box. This member can be any valid window handle, or it can be <c>NULL</c> if the
		/// dialog box has no owner.
		/// </para>
		/// </param>
		/// <param name="pszFilePath">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains a file name used to initialize the File Name edit control. This string corresponds
		/// to the OPENFILENAME structure's <c>lpstrFile</c> member and is used in exactly the same way.
		/// </para>
		/// </param>
		/// <param name="cchFilePath">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The number of characters in , including the terminating null character.</para>
		/// </param>
		/// <param name="pszWorkingDir">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// The fully qualified file path of the initial directory. This string corresponds to the OPENFILENAME structure's
		/// <c>lpstrInitialDir</c> member and is used in exactly the same way.
		/// </para>
		/// </param>
		/// <param name="pszDefExt">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains the default file name extension. This extension is added to if the user does not
		/// specify an extension. The string should not contain any '.' characters. If this string is <c>NULL</c> and the user fails to type
		/// an extension, no extension is appended.
		/// </para>
		/// </param>
		/// <param name="pszFilters">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that defines the filter. This string corresponds to the OPENFILENAME structure's
		/// <c>lpstrFilter</c> member and is used in exactly the same way.
		/// </para>
		/// </param>
		/// <param name="pszTitle">
		/// <para>TBD</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>
		/// If the user specifies a file name and clicks <c>OK</c>, the return value is <c>TRUE</c>. The buffer that points to contains the
		/// full path and file name that the user specifies. If the user cancels or closes the <c>Open</c> dialog box or an error occurs, the
		/// return value is <c>FALSE</c>.
		/// </para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj/nf-shlobj-getfilenamefrombrowse BOOL GetFileNameFromBrowse( HWND hwnd,
		// PWSTR pszFilePath, UINT cchFilePath, PCWSTR pszWorkingDir, PCWSTR pszDefExt, PCWSTR pszFilters, PCWSTR pszTitle );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj.h", MSDNShortId = "1f075051-18c8-4ec2-b010-f983ba2d3303")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetFileNameFromBrowse(HandleRef hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszFilePath, uint cchFilePath, [MarshalAs(UnmanagedType.LPWStr)] string pszWorkingDir, [MarshalAs(UnmanagedType.LPWStr)] string pszDefExt, [MarshalAs(UnmanagedType.LPWStr)] string pszFilters, [MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		/// <summary>
		/// <para>Appends or prepends an SHITEMID structure to an ITEMIDLIST structure.</para>
		/// </summary>
		/// <param name="pidl">
		/// <para>Type: <c>PIDLIST_RELATIVE</c></para>
		/// <para>A pointer to an ITEMIDLIST structure. When the function returns, the SHITEMID structure specified by is appended or prepended.</para>
		/// </param>
		/// <param name="pmkid">
		/// <para>Type: <c>LPSHITEMID</c></para>
		/// <para>A pointer to a SHITEMID structure to be appended or prepended to .</para>
		/// </param>
		/// <param name="fAppend">
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Value that is set to <c>TRUE</c> to append to . Set this value to <c>FALSE</c> to prepend to .</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>PIDLIST_RELATIVE</c></para>
		/// <para>Returns the ITEMIDLIST structure specified by , with appended or prepended. Returns <c>NULL</c> on failure.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-ilappendid PIDLIST_RELATIVE ILAppendID(
		// PIDLIST_RELATIVE pidl, LPCSHITEMID pmkid, BOOL fAppend );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "d1bb5993-fe23-42d4-a2c5-8e54e6e37d09")]
		public static extern IntPtr ILAppendID(IntPtr pidl, ref SHITEMID pmkid, [MarshalAs(UnmanagedType.Bool)] bool fAppend);

		/// <summary>
		/// <para>Determines whether a specified ITEMIDLIST structure is the child of another <c>ITEMIDLIST</c> structure.</para>
		/// </summary>
		/// <param name="pidlParent">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>A pointer to the parent ITEMIDLIST structure.</para>
		/// </param>
		/// <param name="pidlChild">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>A pointer to the child ITEMIDLIST structure.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>PUIDLIST_RELATIVE</c></para>
		/// <para>
		/// Returns a pointer to the child's simple ITEMIDLIST structure if is a child of . The returned structure consists of , minus the
		/// SHITEMID structures that make up . Returns <c>NULL</c> if is not a child of .
		/// </para>
		/// <para>
		/// <c>Note</c> The returned pointer is a pointer into the existing parent structure. It is an alias for . No new memory is allocated
		/// in association with the returned pointer. It is not the caller's responsibility to free the returned value.
		/// </para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-ilfindchild PUIDLIST_RELATIVE ILFindChild(
		// PIDLIST_ABSOLUTE pidlParent, PCIDLIST_ABSOLUTE pidlChild );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "4f07e989-ae74-4cf4-b3d9-0f59f2653095")]
		public static extern IntPtr ILFindChild(IntPtr pidlParent, IntPtr pidlChild);

		/// <summary>
		/// <para>[</para>
		/// <para>ILLoadFromStreamEx(IStream*, PIDLIST_ABSOLUTE*)</para>
		/// <para>
		/// is available for use in the operating systems specified in the Requirements section. It may be altered or unavailable in
		/// subsequent versions.]
		/// </para>
		/// <para>Loads an absolute ITEMIDLIST from an IStream.</para>
		/// </summary>
		/// <param name="pstm">
		/// <para>Type: <c>IStream*</c></para>
		/// <para>A pointer to the IStream interface from which the absolute ITEMIDLIST loads.</para>
		/// </param>
		/// <param name="pidl">
		/// <para>TBD</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>For use where STRICT_TYPED_ITEMIDS is defined.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-illoadfromstreamex SHSTDAPI ILLoadFromStreamEx(
		// IStream *pstm, PIDLIST_RELATIVE *pidl );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "6fb735b6-a8c3-439e-9f20-4fda8f008b28")]
		public static extern HRESULT ILLoadFromStreamEx(IStream pstm, out IntPtr pidl);

		/// <summary>
		/// <para>Saves an ITEMIDLIST structure to a stream.</para>
		/// </summary>
		/// <param name="pstm">
		/// <para>Type: <c>IStream *</c></para>
		/// <para>A pointer to the IStream interface where the ITEMIDLIST is saved.</para>
		/// </param>
		/// <param name="pidl">
		/// <para>Type: <c>PCUIDLIST_RELATIVE</c></para>
		/// <para>A pointer to the ITEMIDLIST structure to be saved.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>Returns S_OK if successful, or a COM error value otherwise.</para>
		/// </returns>
		/// <remarks>
		/// <para>The stream must be opened for writing, or <c>ILSaveToStream</c> returns an error.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-ilsavetostream SHSTDAPI ILSaveToStream( IStream
		// *pstm, PCUIDLIST_RELATIVE pidl );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "40d5ce57-58dc-4c79-8fe6-5412e3d7dc64")]
		public static extern HRESULT ILSaveToStream(IStream pstm, IntPtr pidl);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows. Use
		/// </para>
		/// <para>GetDriveType</para>
		/// <para>or</para>
		/// <para>WNetGetConnection</para>
		/// <para>instead.]</para>
		/// <para>Tests whether a drive is a network drive.</para>
		/// </summary>
		/// <param name="iDrive">
		/// <para>Type: <c>int</c></para>
		/// <para>An integer that indicates which drive letter you want to test. Set it to 0 for A:, 1 for B:, and so on.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>This function returns one of the following values.</para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return value</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>0</term>
		/// <term>The specified drive is not a network drive.</term>
		/// </item>
		/// <item>
		/// <term>1</term>
		/// <term>The specified drive is a network drive that is properly connected.</term>
		/// </item>
		/// <item>
		/// <term>2</term>
		/// <term>The specified drive is a network drive that is disconnected or in an error state.</term>
		/// </item>
		/// </list>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-isnetdrive int IsNetDrive( int iDrive );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "44e02665-648a-4cf0-9dc0-038e54d08a49")]
		public static extern int IsNetDrive(int iDrive);

		/// <summary>
		/// <para>
		/// [IsUserAnAdmin is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Tests whether the current user is a member of the Administrator's group.</para>
		/// </summary>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if the user is a member of the Administrator's group; otherwise, <c>FALSE</c>.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This function is a wrapper for CheckTokenMembership. It is recommended to call that function directly to determine Administrator
		/// group status rather than calling <c>IsUserAnAdmin</c>.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-isuseranadmin BOOL IsUserAnAdmin( );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "fe698d32-32f6-4b2b-ad0c-5d9ec815177f")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsUserAnAdmin();

		/// <summary>
		/// <para>
		/// [OpenRegStream is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions. Instead, use SHOpenRegStream2 or SHOpenRegStream.]
		/// </para>
		/// <para>Opens a registry value and supplies an IStream interface that can be used to read from or write to the value.</para>
		/// </summary>
		/// <param name="hkey">
		/// <para>Type: <c>HKEY</c></para>
		/// <para>A handle to the key that is currently open.</para>
		/// </param>
		/// <param name="pszSubkey">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>A null-terminated Unicode string that specifies the name of the subkey.</para>
		/// </param>
		/// <param name="pszValue">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>A null-terminated Unicode string that specifies the value to be accessed.</para>
		/// </param>
		/// <param name="grfMode">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>The type of access for the stream. This can be one of the following values.</para>
		/// <para>STGM_READ</para>
		/// <para>Open the stream for reading.</para>
		/// <para>STGM_WRITE</para>
		/// <para>Open the stream for writing.</para>
		/// <para>STGM_READWRITE</para>
		/// <para>Open the stream for reading and writing.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>IStream*</c></para>
		/// <para>Returns the address of an IStream interface if successful, or <c>NULL</c> otherwise.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-openregstream IStream * OpenRegStream( HKEY hkey,
		// PCWSTR pszSubkey, PCWSTR pszValue, DWORD grfMode );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "e1e35c94-84ac-4aa1-b2a1-47b37a7f224e")]
		public static extern IStream OpenRegStream(IntPtr hkey, [MarshalAs(UnmanagedType.LPWStr)] string pszSubkey, [MarshalAs(UnmanagedType.LPWStr)] string pszValue, STGM grfMode);

		/// <summary>
		/// <para>
		/// [PathCleanupSpec is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>
		/// Removes illegal characters from a file or directory name. Enforces the 8.3 filename format on drives that do not support long
		/// file names.
		/// </para>
		/// </summary>
		/// <param name="pszDir">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated buffer that contains the fully qualified path of the directory that will contain the file or
		/// directory named at . The path must not exceed MAX_PATH characters in length, including the terminating null character. This path
		/// is not altered.
		/// </para>
		/// <para>This value can be <c>NULL</c>.</para>
		/// </param>
		/// <param name="pszSpec">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated buffer that contains the file or directory name to be cleaned. In the case of a file, include the
		/// file's extension. Note that because '' is considered an invalid character and will be removed, this buffer cannot contain a path
		/// more than one directory deep.
		/// </para>
		/// <para>On exit, the buffer contains a null-terminated string that includes the cleaned name.</para>
		/// <para>This buffer should be at least MAX_PATH characters in length to avoid the possibility of a buffer overrun.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns one or more of the following values.</para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return code</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>PCS_REPLACEDCHAR</term>
		/// <term>Replaced one or more invalid characters.</term>
		/// </item>
		/// <item>
		/// <term>PCS_REMOVEDCHAR</term>
		/// <term>Removed one or more invalid characters.</term>
		/// </item>
		/// <item>
		/// <term>PCS_TRUNCATED</term>
		/// <term>The returned path is truncated.</term>
		/// </item>
		/// <item>
		/// <term>PCS_PATHTOOLONG</term>
		/// <term>
		/// The function failed because the input path specified at is too long to allow the formation of a valid file name from . When this
		/// flag is returned, it is always accompanied by the PCS_FATAL flag.
		/// </term>
		/// </item>
		/// <item>
		/// <term>PCS_FATAL</term>
		/// <term>The cleaned path is not a valid file name. This flag is always returned in conjunction with PCS_PATHTOOLONG.</term>
		/// </item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// <para>The following are considered invalid characters in all names.</para>
		/// <para>
		/// Control characters are also considered invalid. If long file names are not supported, the semi-colon (;) and comma (,) characters
		/// are also invalid.
		/// </para>
		/// <para>
		/// The drive named in is checked to determine whether its file system supports long file names. If it does not, the name at is
		/// truncated to the 8.3 format and the PCS_TRUNCATED value returned. If is <c>NULL</c>, the drive on which Windows is installed is
		/// used to determine long file name support.
		/// </para>
		/// <para>
		/// If the full path—the number of characters in the path at plus the number of characters in the cleaned name at —exceeds MAX_PATH –
		/// 1 (to account for the terminating null character), the function returns PCS_PATHTOOLONG.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pathcleanupspec int PathCleanupSpec( PCWSTR
		// pszDir, PWSTR pszSpec );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "593fd2b7-44ae-4309-a185-97e42f3cc0fa")]
		public static extern PCS PathCleanupSpec([MarshalAs(UnmanagedType.LPWStr)] string pszDir, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszSpec);

		/// <summary>
		/// <para>
		/// [PathGetShortPath is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Retrieves the short path form of a specified input path.</para>
		/// </summary>
		/// <param name="pszLongPath">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated, Unicode string that contains the long path. When the function returns, it contains the equivalent
		/// short path.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>This function does not return a value.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pathgetshortpath void PathGetShortPath( PWSTR
		// pszLongPath );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "f374a575-3fbf-4bed-aa76-76ed81e01d60")]
		public static extern void PathGetShortPath([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszLongPath);

		/// <summary>
		/// <para>
		/// [PathIsExe is available for use in the operating systems specified in the Requirements section. It may be altered or unavailable
		/// in subsequent versions.]
		/// </para>
		/// <para>Determines whether a file is an executable by examining the file name extension.</para>
		/// </summary>
		/// <param name="pszPath">
		/// <para>TBD</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if the file name extension is .cmd, .bat, .pif, .scf, .exe, .com, or .scr; otherwise, <c>FALSE</c>.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pathisexe BOOL PathIsExe( PCWSTR pszPath );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "54e9dae7-f9c4-48b8-9b91-32ed21365fb7")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PathIsExe([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

		/// <summary>
		/// <para>
		/// [PathIsSlow is available for use in the operating systems specified in the Requirements section. It may be altered or unavailable
		/// in subsequent versions.]
		/// </para>
		/// <para>Determines whether a file path is a high-latency network connection.</para>
		/// </summary>
		/// <param name="pszFile">
		/// <para>Type: <c>LPCTSTR</c></para>
		/// <para>A pointer to a null-terminated string that contains the fully qualified path of the file.</para>
		/// </param>
		/// <param name="dwAttr">
		/// <para>TBD</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if the connection is high-latency; otherwise, <c>FALSE</c>.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// A path is considered slow if the MultinetGetConnectionPerformance function returns a dwSpeed of 400 or less in its
		/// NETCONNECTINFOSTRUCT structure—this is the speed of the media to the network resource, in 100 bits-per-second (bps)—or if
		/// FILE_ATTRIBUTE_OFFLINE is set on the file.
		/// </para>
		/// <para>Note that network conditions can impact function performance time.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj/nf-shlobj-pathisslowa BOOL PathIsSlowA( LPCSTR pszFile, DWORD dwAttr );
		[DllImport(Lib.Shell32, SetLastError = false, CharSet = CharSet.Auto)]
		[PInvokeData("shlobj.h", MSDNShortId = "f848a098-9248-453b-a957-77c35d70e528")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PathIsSlow(string pszFile, uint dwAttr);

		/// <summary>
		/// <para>Creates a unique path name from a template.</para>
		/// </summary>
		/// <param name="pszUniqueName">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// A buffer that receives a null-terminated Unicode string that contains the unique path name. It should be at least MAX_PATH
		/// characters in length.
		/// </para>
		/// </param>
		/// <param name="cchMax">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The number of characters in the buffer pointed to by .</para>
		/// </param>
		/// <param name="pszTemplate">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains a template that is used to construct the unique name. This template is used for
		/// drives that require file names with the 8.3 format. This string should be no more than MAX_PATH characters in length, including
		/// the terminating null character.
		/// </para>
		/// </param>
		/// <param name="pszLongPlate">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains a template that is used to construct the unique name. This template is used for
		/// drives that support long file names. This string should be no more than MAX_PATH characters in length, including the terminating
		/// null character.
		/// </para>
		/// </param>
		/// <param name="pszDir">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated string that contains the directory in which the new file resides. This string should be no more than MAX_PATH
		/// characters in length, including the terminating null character.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if successful; otherwise, <c>FALSE</c>.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This function generates a new unique file name based on the templates specified by , for drives that require the 8.3 format, and
		/// for drives that support long file names. For example, if you specify "My New Filename" for , <c>PathMakeUniqueName</c> returns
		/// names such as "My New Filename (1)", "My New Filename (2)", and so on.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pathmakeuniquename BOOL PathMakeUniqueName( PWSTR
		// pszUniqueName, UINT cchMax, PCWSTR pszTemplate, PCWSTR pszLongPlate, PCWSTR pszDir );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "8456ae0c-e83c-43d0-a86a-1861a373d237")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PathMakeUniqueName([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszUniqueName, uint cchMax, [MarshalAs(UnmanagedType.LPWStr)] string pszTemplate, [MarshalAs(UnmanagedType.LPWStr)] string pszLongPlate, [MarshalAs(UnmanagedType.LPWStr)] string pszDir);

		/// <summary>
		/// <para>
		/// [PathResolve is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Converts a relative or unqualified path name to a fully qualified path name.</para>
		/// </summary>
		/// <param name="pszPath">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains the path to resolve. When the function returns, the string contains the
		/// corresponding fully qualified path. This buffer should be at least MAX_PATH characters long.
		/// </para>
		/// </param>
		/// <param name="dirs">
		/// <para>Type: <c>PZPCWSTR</c></para>
		/// <para>
		/// A pointer to an optional null-terminated array of directories to be searched first in the case that the path cannot be resolved
		/// from . This value can be <c>NULL</c>.
		/// </para>
		/// </param>
		/// <param name="fFlags">
		/// <para>Type: <c>UINT</c></para>
		/// <para>Flags that specify how the function operates.</para>
		/// <para>PRF_VERIFYEXISTS</para>
		/// <para>Return <c>TRUE</c> if the file's existence is verified; otherwise <c>FALSE</c>.</para>
		/// <para>PRF_TRYPROGRAMEXTENSIONS</para>
		/// <para>Look for the specified path with the following extensions appended: .pif, .com, .bat, .cmd, .lnk, and .exe.</para>
		/// <para>PRF_FIRSTDIRDEF</para>
		/// <para>Look first in the directory or directories specified by .</para>
		/// <para>PRF_DONTFINDLNK</para>
		/// <para>Ignore .lnk files.</para>
		/// <para>PRF_REQUIREABSOLUTE</para>
		/// <para>Require an absolute (full) path.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>
		/// Returns <c>TRUE</c>, unless PRF_VERIFYEXISTS is set. If that flag is set, the function returns <c>TRUE</c> if the file is
		/// verified to exist and <c>FALSE</c> otherwise. It also sets an ERROR_FILE_NOT_FOUND error code that you can retrieve by calling GetLastError.
		/// </para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// A <c>FALSE</c> return value does not necessarily mean that the file does not exist. It might mean that the function is simply
		/// unable to find the file from the supplied information.
		/// </para>
		/// <para>If <c>PathResolve</c> cannot resolve the path specified in , it calls PathFindOnPath using and as the parameters.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pathresolve int PathResolve( PWSTR pszPath,
		// PZPCWSTR dirs, UINT fFlags );
		[DllImport(Lib.Shell32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "84bf0b56-513f-4ac6-b2cf-11f0c471da1e")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PathResolve(StringBuilder pszPath, string[] dirs, PRF fFlags);

		/// <summary>
		/// <para>Creates a unique filename based on an existing filename.</para>
		/// </summary>
		/// <param name="pszUniqueName">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// A string buffer that receives a null-terminated Unicode string that contains the fully qualified path of the unique file name.
		/// This buffer should be at least MAX_PATH characters long to avoid causing a buffer overrun.
		/// </para>
		/// </param>
		/// <param name="pszPath">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains the fully qualified path of folder that will contain the new file. If is set to
		/// <c>NULL</c>, this string must contain a full destination path, ending with the long file name that the new file name will be base on.
		/// </para>
		/// </param>
		/// <param name="pszShort">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains the short file name that the unique name will be based on. Set this value to
		/// <c>NULL</c> to create a name based on the long file name.
		/// </para>
		/// </param>
		/// <param name="pszFileSpec">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>A null-terminated Unicode string that contains the long file name that the unique name will be based on.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if a unique name was successfully created; otherwise <c>FALSE</c>.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// If the generated path exceeds MAX_PATH characters, this function may return a truncated string in
		/// <c>PathYetAnotherMakeUniqueName</c>. In that case, the function returns <c>FALSE</c>.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pathyetanothermakeuniquename BOOL
		// PathYetAnotherMakeUniqueName( PWSTR pszUniqueName, PCWSTR pszPath, PCWSTR pszShort, PCWSTR pszFileSpec );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true, CharSet = CharSet.Unicode)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "1f76ecfa-6f2f-4dde-b05e-4252c92660d9")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PathYetAnotherMakeUniqueName(StringBuilder pszUniqueName, string pszPath, string pszShort, string pszFileSpec);

		/// <summary>
		/// <para>
		/// [PickIconDlg is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>
		/// Displays a dialog box that allows the user to choose an icon from the selection available embedded in a resource such as an
		/// executable or DLL file.
		/// </para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>The handle of the parent window. This value can be <c>NULL</c>.</para>
		/// </param>
		/// <param name="pszIconPath">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// A pointer to a string that contains the null-terminated, fully qualified path of the default resource that contains the icons. If
		/// the user chooses a different resource in the dialog, this buffer contains the path of that file when the function returns. This
		/// buffer should be at least MAX_PATH characters in length, or the returned path may be truncated. You should verify that the path
		/// is valid before using it.
		/// </para>
		/// </param>
		/// <param name="cchIconPath">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The number of characters in , including the terminating <c>NULL</c> character.</para>
		/// </param>
		/// <param name="piIconIndex">
		/// <para>Type: <c>int*</c></para>
		/// <para>
		/// A pointer to an integer that on entry specifies the index of the initial selection and, when this function returns successfully,
		/// receives the index of the icon that was selected.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns 1 if successful; otherwise, 0.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pickicondlg int PickIconDlg( HWND hwnd, PWSTR
		// pszIconPath, UINT cchIconPath, int *piIconIndex );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "3dfcda10-26d8-495d-8c92-7ff16da098c1")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PickIconDlg(HandleRef hwnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, uint cchIconPath, ref int piIconIndex);

		/// <summary>
		/// [PifMgr_CloseProperties is available for use in the operating systems specified in the Requirements section.It may be altered or
		/// unavailable in subsequent versions.]
		/// <para>Closes application properties that were opened with PifMgr_OpenProperties.</para>
		/// </summary>
		/// <param name="hProps">
		/// A handle to the application's properties. This parameter should be set to the value that is returned by PifMgr_OpenProperties.
		/// </param>
		/// <param name="flOpt">A flag that specifies how the function operates.</param>
		/// <returns>
		/// Returns NULL if successful. If unsuccessful, the functions returns the handle to the application properties that was passed as hProps.
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pifmgr_closeproperties HANDLE
		// PifMgr_CloseProperties(HANDLE hProps, UINT flOpt);
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "fd50d4f8-87c8-4162-9e88-3c8592b929fa")]
		public static extern IntPtr PifMgr_CloseProperties(IntPtr hProps, CLOSEPROPS flOpt);

		/// <summary>
		/// [PifMgr_GetProperties is available for use in the operating systems specified in the Requirements section.It may be altered or
		/// unavailable in subsequent versions.]
		/// <para>Returns a specified block of data from a .pif file.</para>
		/// </summary>
		/// <param name="hProps">
		/// A handle to an application's properties. This parameter should be set to the value that is returned by PifMgr_OpenProperties.
		/// </param>
		/// <param name="pszGroup">
		/// A null-terminated string that contains the property group name. It can be one of the following, or any other name that
		/// corresponds to a valid .pif extension.
		/// </param>
		/// <param name="lpProps">When this function returns, contains a pointer to a PROPPRG structure.</param>
		/// <param name="cbProps">The size of the buffer, in bytes, pointed to by lpProps.</param>
		/// <param name="flOpt">Set this parameter to GETPROPS_NONE.</param>
		/// <returns>
		/// Returns NULL if successful. If unsuccessful, the function returns the handle to the application properties that were passed as hProps.
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pifmgr_getproperties int PifMgr_GetProperties(
		// HANDLE hProps, PCSTR pszGroup, void* lpProps, int cbProps, UINT flOpt );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true, CharSet = CharSet.Ansi)]
		[PInvokeData("shlobj_core.h")]
		public static extern int PifMgr_GetProperties(IntPtr hProps, string pszGroup, IntPtr lpProps, int cbProps, uint flOpt = 0);

		/// <summary>
		/// <para>
		/// [PifMgr_OpenProperties is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Opens the .pif file associated with a Microsoft MS-DOS application, and returns a handle to the application's properties.</para>
		/// </summary>
		/// <param name="pszApp">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>A null-terminated Unicode string that contains the application's name.</para>
		/// </param>
		/// <param name="pszPIF">
		/// <para>TBD</para>
		/// </param>
		/// <param name="hInf">
		/// <para>Type: <c>UINT</c></para>
		/// <para>
		/// A handle to the application's .inf file. Set this value to zero if there is no .inf file. Set this value to -1 to prevent the
		/// .inf file from being processed.
		/// </para>
		/// </param>
		/// <param name="flOpt">
		/// <para>Type: <c>UINT</c></para>
		/// <para>A flag that controls how the function operates.</para>
		/// <para>OPENPROPS_INHIBITPIF</para>
		/// <para>
		/// Ignore any existing .pif files and get the properties from win.ini or _Default.pif. This flag is ignored on Windows NT, Windows
		/// 2000, and Windows XP.
		/// </para>
		/// <para>OPENPROPS_NONE</para>
		/// <para>No options specified.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HANDLE</c></para>
		/// <para>Returns a handle to the application's properties. Use this handle when you call the related .pif functions.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// You should not think of <c>PifMgr_OpenProperties</c> as a function that opens a file somewhere. The .pif file does not remain
		/// open after this call. It is more useful to think of the function as a property structure allocator that you can initialize using
		/// disk data. The primary reason why this function fails is because of low memory or inability to open the specified .pif file.
		/// </para>
		/// <para>
		/// If no .pif file exists, the function still allocates a data block in memory and initializes it with data from _Default.pif or its
		/// internal defaults. If the function looks for a .pif file name but does not find it, it constructs a name and saves it in its
		/// internal .pif data structure. This guarantees that if PifMgr_SetProperties is called, the data is saved to disk.
		/// </para>
		/// <para>If the function does not find the .pif file, it searches for it in the following order.</para>
		/// <list type="number">
		/// <item>Searches the current directory.</item>
		/// <item>Searches the specified directory.</item>
		/// <item>Searches in .pif directory.</item>
		/// <item>Searches the folders specified by the PATH environment variable.</item>
		/// </list>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pifmgr_openproperties HANDLE
		// PifMgr_OpenProperties( PCWSTR pszApp, PCWSTR pszPIF, UINT hInf, UINT flOpt );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "0bc11528-7278-4765-b3cb-671ba82c9155")]
		public static extern IntPtr PifMgr_OpenProperties([MarshalAs(UnmanagedType.LPWStr)] string pszApp, [MarshalAs(UnmanagedType.LPWStr)] string pszPIF, uint hInf, OPENPROPS flOpt);

		/// <summary>
		/// [PifMgr_SetProperties is available for use in the operating systems specified in the Requirements section.It may be altered or
		/// unavailable in subsequent versions.]
		/// <para>Assigns values to a block of data from a .pif file.</para>
		/// </summary>
		/// <param name="hProps">
		/// A handle to the application's properties. This parameter should be set to the value that is returned by PifMgr_OpenProperties.
		/// </param>
		/// <param name="pszGroup">
		/// A null-terminated ANSI string containing the property group name. It can be one of the following, or any other name that
		/// corresponds to a valid .pif extension.
		/// </param>
		/// <param name="lpProps">A property group record buffer that holds the data.</param>
		/// <param name="cbProps">The size of the buffer, in bytes, pointed to by lpProps.</param>
		/// <param name="flOpt">Always SETPROPS_NONE.</param>
		/// <returns>Returns the amount of information transferred, in bytes. Returns zero if the group cannot be found or an error occurs.</returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-pifmgr_setproperties int
		// PifMgr_SetProperties(HANDLE hProps, PCSTR pszGroup, const void* lpProps, int cbProps, UINT flOpt );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true, CharSet = CharSet.Ansi)]
		[PInvokeData("shlobj_core.h")]
		public static extern int PifMgr_SetProperties(IntPtr hProps, string pszGroup, IntPtr lpProps, int cbProps, uint flOpt = 0);

		/// <summary>
		/// <para>
		/// [ReadCabinetState is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Fills a CABINETSTATE structure with information from the registry.</para>
		/// </summary>
		/// <param name="pcs">
		/// <para>Type: <c>CABINETSTATE*</c></para>
		/// <para>
		/// When this function returns, contains a pointer to a CABINETSTATE structure that contains either information pulled from the
		/// registry or default information.
		/// </para>
		/// </param>
		/// <param name="cLength">
		/// <para>Type: <c>int</c></para>
		/// <para>The size of the structure pointed to by , in bytes.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>
		/// Returns <c>TRUE</c> if the returned structure contains information from the registry. Returns <c>FALSE</c> if the structure
		/// contains default information.
		/// </para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-readcabinetstate BOOL ReadCabinetState(
		// CABINETSTATE *pcs, int cLength );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "0f0c6a10-588f-4c79-b73b-cf0bf9336ffc")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReadCabinetState(ref CABINETSTATE pcs, int cLength);

		/// <summary>
		/// <para>
		/// [RealDriveType is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Determines the drive type based on the drive number.</para>
		/// </summary>
		/// <param name="iDrive">
		/// <para>Type: <c>int</c></para>
		/// <para>The number of the drive that you want to test. "A:" corresponds to 0, "B:" to 1, and so on.</para>
		/// </param>
		/// <param name="fOKToHitNet">
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Reserved. Must be set to 0.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns one of the following values.</para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return code</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>DRIVE_UNKNOWN</term>
		/// <term>The drive type cannot be determined.</term>
		/// </item>
		/// <item>
		/// <term>DRIVE_NO_ROOT_DIR</term>
		/// <term>The root path is invalid. For example, no volume is mounted at the path.</term>
		/// </item>
		/// <item>
		/// <term>DRIVE_REMOVABLE</term>
		/// <term>The disk can be removed from the drive.</term>
		/// </item>
		/// <item>
		/// <term>DRIVE_FIXED</term>
		/// <term>The disk cannot be removed from the drive.</term>
		/// </item>
		/// <item>
		/// <term>DRIVE_REMOTE</term>
		/// <term>The drive is a remote (network) drive.</term>
		/// </item>
		/// <item>
		/// <term>DRIVE_CDROM</term>
		/// <term>The drive is a CD-ROM drive.</term>
		/// </item>
		/// <item>
		/// <term>DRIVE_RAMDISK</term>
		/// <term>The drive is a RAM disk.</term>
		/// </item>
		/// </list>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-realdrivetype int RealDriveType( int iDrive, BOOL
		// fOKToHitNet ); public static extern int RealDriveType(int iDrive, [MarshalAs(UnmanagedType.Bool)] bool fOKToHitNet);
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "c4e55b50-637a-446f-aa9c-7d8c71d8071c")]
		public static extern DRIVE_TYPE RealDriveType(int iDrive, [MarshalAs(UnmanagedType.Bool)] bool fOKToHitNet);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows.]
		/// </para>
		/// <para>
		/// Displays a dialog box that prompts the user to restart Windows. When the user clicks the button, the function calls ExitWindowsEx
		/// to attempt to restart Windows.
		/// </para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>TBD</para>
		/// </param>
		/// <param name="pszPrompt">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>A null-terminated Unicode string that contains the text that displays in the dialog box which prompts the user.</para>
		/// </param>
		/// <param name="dwReturn">
		/// <para>TBD</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns the identifier of the button that was pressed to close the dialog box.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-restartdialog int RestartDialog( HWND hwnd, PCWSTR
		// pszPrompt, DWORD dwReturn );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "ec1e3c11-9960-482c-8461-72c4d41dff3c")]
		public static extern int RestartDialog(HandleRef hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszPrompt, uint dwReturn);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows.]
		/// </para>
		/// <para>
		/// Displays a dialog box that asks the user to restart Windows. When the user clicks the button, the function calls ExitWindowsEx to
		/// attempt to restart Windows.
		/// </para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>TBD</para>
		/// </param>
		/// <param name="pszPrompt">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>A null-terminated string that contains the text that displays in the dialog box to prompt the user.</para>
		/// </param>
		/// <param name="dwReturn">
		/// <para>TBD</para>
		/// </param>
		/// <param name="dwReasonCode">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>
		/// <c>Windows XP:</c> Specifies the reason for initiating the shutdown. For more information, see System Shutdown Reason Codes.
		/// </para>
		/// <para><c>Windows 2000:</c> This parameter is ignored.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns the identifier of the button that was pressed to close the dialog box.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-restartdialogex int RestartDialogEx( HWND hwnd,
		// PCWSTR pszPrompt, DWORD dwReturn, DWORD dwReasonCode );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "32bc232f-6cc4-4f19-9d33-ba7ad28dfd59")]
		public static extern int RestartDialogEx(HandleRef hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszPrompt, uint dwReturn, uint dwReasonCode);

		/// <summary>
		/// Notifies the system that an item has been accessed, for the purposes of tracking those items used most recently and most
		/// frequently. This function can also be used to clear all usage data.
		/// </summary>
		/// <param name="uFlags">A value from the SHARD enumeration that indicates the form of the information pointed to by the pv parameter.</param>
		/// <param name="pv">
		/// A pointer to data that identifies the item that has been accessed. The item can be specified in this parameter in one of the
		/// following forms:
		/// <list type="bullet">
		/// <item><definition>A null-terminated string that contains the path and file name of the item.</definition></item>
		/// <item><definition>A PIDL that identifies the item's file object.</definition></item>
		/// <item>
		/// <definition>Windows 7 and later only. A SHARDAPPIDINFO, SHARDAPPIDINFOIDLIST, or SHARDAPPIDINFOLINK structure that identifies the
		/// item through an AppUserModelID. See Application User Model IDs (AppUserModelIDs) for more information.</definition>
		/// </item>
		/// <item><definition>Windows 7 and later only. An IShellLink object that identifies the item through a shortcut.</definition></item>
		/// </list>
		/// <para>Set this parameter to NULL to clear all usage data on all items.</para>
		/// </param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762105")]
		public static extern void SHAddToRecentDocs(SHARD uFlags, IShellLinkW pv);

		/// <summary>
		/// Notifies the system that an item has been accessed, for the purposes of tracking those items used most recently and most
		/// frequently. This function can also be used to clear all usage data.
		/// </summary>
		/// <param name="uFlags">A value from the SHARD enumeration that indicates the form of the information pointed to by the pv parameter.</param>
		/// <param name="pv">
		/// A pointer to data that identifies the item that has been accessed. The item can be specified in this parameter in one of the
		/// following forms:
		/// <list type="bullet">
		/// <item><definition>A null-terminated string that contains the path and file name of the item.</definition></item>
		/// <item><definition>A PIDL that identifies the item's file object.</definition></item>
		/// <item>
		/// <definition>Windows 7 and later only. A SHARDAPPIDINFO, SHARDAPPIDINFOIDLIST, or SHARDAPPIDINFOLINK structure that identifies the
		/// item through an AppUserModelID. See Application User Model IDs (AppUserModelIDs) for more information.</definition>
		/// </item>
		/// <item><definition>Windows 7 and later only. An IShellLink object that identifies the item through a shortcut.</definition></item>
		/// </list>
		/// <para>Set this parameter to NULL to clear all usage data on all items.</para>
		/// </param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762105")]
		public static extern void SHAddToRecentDocs(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		/// <summary>
		/// Notifies the system that an item has been accessed, for the purposes of tracking those items used most recently and most
		/// frequently. This function can also be used to clear all usage data.
		/// </summary>
		/// <param name="uFlags">A value from the SHARD enumeration that indicates the form of the information pointed to by the pv parameter.</param>
		/// <param name="pv">
		/// A pointer to data that identifies the item that has been accessed. The item can be specified in this parameter in one of the
		/// following forms:
		/// <list type="bullet">
		/// <item><definition>A null-terminated string that contains the path and file name of the item.</definition></item>
		/// <item><definition>A PIDL that identifies the item's file object.</definition></item>
		/// <item>
		/// <definition>Windows 7 and later only. A SHARDAPPIDINFO, SHARDAPPIDINFOIDLIST, or SHARDAPPIDINFOLINK structure that identifies the
		/// item through an AppUserModelID. See Application User Model IDs (AppUserModelIDs) for more information.</definition>
		/// </item>
		/// <item><definition>Windows 7 and later only. An IShellLink object that identifies the item through a shortcut.</definition></item>
		/// </list>
		/// <para>Set this parameter to NULL to clear all usage data on all items.</para>
		/// </param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762105")]
		public static extern void SHAddToRecentDocs(SHARD uFlags, PIDL pv);

		/// <summary>
		/// <para>
		/// Given a Shell namespace item specified in the form of a folder, and an item identifier list relative to that folder, this
		/// function binds to the parent of the namespace item and optionally returns a pointer to the final component of the item identifier list.
		/// </para>
		/// </summary>
		/// <param name="psfRoot">
		/// <para>Type: <c>IShellFolder*</c></para>
		/// <para>A pointer to a Shell folder object. If is <c>NULL</c>, indicates that the IDList passed is relative to the desktop.</para>
		/// </param>
		/// <param name="pidl">
		/// <para>Type: <c>PCUIDLIST_RELATIVE</c></para>
		/// <para>A PIDL to bind to, relative to . If is <c>NULL</c>, this is an absolute IDList relative to the desktop folder.</para>
		/// </param>
		/// <param name="riid">
		/// <para>Type: <c>REFIID</c></para>
		/// <para>
		/// Reference to the desired interface ID. This is typically IID_IShellFolder or IID_IShellFolder2, but can be anything supported by
		/// the target folder.
		/// </para>
		/// </param>
		/// <param name="ppv">
		/// <para>Type: <c>void**</c></para>
		/// <para>
		/// When this function returns, contains the interface pointer requested in . This is typically IShellFolder or IShellFolder2, but
		/// can be anything supported by the target folder.
		/// </para>
		/// </param>
		/// <param name="ppidlLast">
		/// <para>Type: <c>PCUITEMID_CHILD*</c></para>
		/// <para>
		/// A pointer to the last ID of the parameter, and is a child ID relative to the parent folder returned in . This value can be <c>NULL</c>.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// <c>Note</c> Calling the <c>SHBindToFolderIDListParent</c> function is equivalent to calling the SHBindToFolderIDListParentEx
		/// function with <c>NULL</c> as the bind context.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shbindtofolderidlistparent SHSTDAPI
		// SHBindToFolderIDListParent( IShellFolder *psfRoot, PCUIDLIST_RELATIVE pidl, REFIID riid, void **ppv, PCUITEMID_CHILD *ppidlLast );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "72a79d1b-15ed-475e-9ebd-03345579a06a")]
		public static extern HRESULT SHBindToFolderIDListParent(IShellFolder psfRoot, PIDL pidl, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv, IntPtr ppidlLast);

		/// <summary>
		/// <para>Extends the SHBindToFolderIDListParent function by allowing the caller to specify a bind context.</para>
		/// </summary>
		/// <param name="psfRoot">
		/// <para>Type: <c>IShellFolder*</c></para>
		/// <para>A pointer to a Shell folder object. If is <c>NULL</c>, indicates that the IDList passed is relative to the desktop.</para>
		/// </param>
		/// <param name="pidl">
		/// <para>Type: <c>PCUIDLIST_RELATIVE</c></para>
		/// <para>A PIDL to bind to, relative to . If is <c>NULL</c>, this is an absolute IDList relative to the desktop folder.</para>
		/// </param>
		/// <param name="ppbc">
		/// <para>Type: <c>IBindCtx*</c></para>
		/// <para>
		/// A pointer to IBindCtx interface on a bind context object to be used during this operation. If this parameter is not used, set it
		/// to <c>NULL</c>, which is equivalent to calling the SHBindToFolderIDListParent function. Because support for is optional for
		/// folder object implementations, some folders may not support the use of bind contexts.
		/// </para>
		/// </param>
		/// <param name="riid">
		/// <para>Type: <c>REFIID</c></para>
		/// <para>
		/// Reference to the desired interface ID. This is typically IID_IShellFolder or IID_IShellFolder2, but can be anything supported by
		/// the target folder.
		/// </para>
		/// </param>
		/// <param name="ppv">
		/// <para>Type: <c>void**</c></para>
		/// <para>
		/// When this function returns, contains the interface pointer requested in . This is typically IShellFolder or IShellFolder2, but
		/// can be anything supported by the target folder.
		/// </para>
		/// </param>
		/// <param name="ppidlLast">
		/// <para>Type: <c>PCUITEMID_CHILD*</c></para>
		/// <para>
		/// A pointer to the last ID of the parameter, and is a child ID relative to the parent folder returned in . This value can be <c>NULL</c>.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shbindtofolderidlistparentex SHSTDAPI
		// SHBindToFolderIDListParentEx( IShellFolder *psfRoot, PCUIDLIST_RELATIVE pidl, IBindCtx *ppbc, REFIID riid, void **ppv,
		// PCUITEMID_CHILD *ppidlLast );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "4f9b68cb-d0ae-45f7-90f5-2db1da3ab599")]
		public static extern HRESULT SHBindToFolderIDListParentEx(IShellFolder psfRoot, PIDL pidl, IBindCtx ppbc, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv, IntPtr ppidlLast);

		/// <summary>
		/// <para>Retrieves and binds to a specified object by using the Shell namespace IShellFolder::BindToObject method.</para>
		/// </summary>
		/// <param name="psf">
		/// <para>Type: <c>IShellFolder*</c></para>
		/// <para>
		/// A pointer to IShellFolder. This parameter can be <c>NULL</c>. If is <c>NULL</c>, this indicates parameter is relative to the
		/// desktop. In this case, must specify an absolute ITEMIDLIST.
		/// </para>
		/// </param>
		/// <param name="pidl">
		/// <para>Type: <c>PCUIDLIST_RELATIVE</c></para>
		/// <para>
		/// A pointer to a constant ITEMIDLIST to bind to that is relative to . If is <c>NULL</c>, this is an absolute <c>ITEMIDLIST</c>
		/// relative to the desktop folder.
		/// </para>
		/// </param>
		/// <param name="pbc">
		/// <para>Type: <c>IBindCtx*</c></para>
		/// <para>
		/// A pointer to IBindCtx interface on a bind context object to be used during this operation. If this parameter is not used, set it
		/// to <c>NULL</c>. Because support for is optional for folder object implementations, some folders may not support the use of bind contexts.
		/// </para>
		/// </param>
		/// <param name="riid">
		/// <para>Type: <c>REFIID</c></para>
		/// <para>Identifier of the interface to return.</para>
		/// </param>
		/// <param name="ppv">
		/// <para>Type: <c>void**</c></para>
		/// <para>
		/// When this method returns, contains the interface pointer as specified in to the bound object. If an error occurs, contains a
		/// <c>NULL</c> pointer.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para><c>Note</c> This is a helper function that gets the desktop object by calling SHGetDesktopFolder.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shbindtoobject SHSTDAPI SHBindToObject(
		// IShellFolder *psf, PCUIDLIST_RELATIVE pidl, IBindCtx *pbc, REFIID riid, void **ppv );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "acc16097-8301-4118-8cb5-00aa2705306a")]
		public static extern HRESULT SHBindToObject(IShellFolder psf, PIDL pidl, IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		/// <summary>
		/// <para>
		/// Takes a pointer to a fully qualified item identifier list (PIDL), and returns a specified interface pointer on the parent object.
		/// </para>
		/// </summary>
		/// <param name="pidl">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>The item's PIDL.</para>
		/// </param>
		/// <param name="riid">
		/// <para>Type: <c>REFIID</c></para>
		/// <para>The <c>REFIID</c> of one of the interfaces exposed by the item's parent object.</para>
		/// </param>
		/// <param name="ppv">
		/// <para>Type: <c>VOID**</c></para>
		/// <para>A pointer to the interface specified by riid. You must release the object when you are finished.</para>
		/// </param>
		/// <param name="ppidlLast">
		/// <para>Type: <c>PCUITEMID_CHILD*</c></para>
		/// <para>
		/// The item's PIDL relative to the parent folder. This PIDL can be used with many of the methods supported by the parent folder's
		/// interfaces. If you set to <c>NULL</c>, the PIDL is not returned.
		/// </para>
		/// <para>
		/// <c>Note</c><c>SHBindToParent</c> does not allocate a new PIDL; it simply receives a pointer through this parameter. Therefore,
		/// you are not responsible for freeing this resource.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shbindtoparent SHSTDAPI SHBindToParent(
		// PCIDLIST_ABSOLUTE pidl, REFIID riid, void **ppv, PCUITEMID_CHILD *ppidlLast );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "1cb283a6-3ebf-4986-9f32-5f6ab8d977ad")]
		public static extern HRESULT SHBindToParent(PIDL pidl, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv, ref IntPtr ppidlLast);

		/// <summary>Displays a dialog box that enables the user to select a Shell folder.</summary>
		/// <param name="lpbi">A pointer to a BROWSEINFO structure that contains information used to display the dialog box.</param>
		/// <returns>
		/// Returns a PIDL that specifies the location of the selected folder relative to the root of the namespace. If the user chooses the
		/// Cancel button in the dialog box, the return value is NULL.
		/// </returns>
		[DllImport(Lib.Shell32, CharSet = CharSet.Auto)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762115")]
		public static extern PIDL SHBrowseForFolder(ref BROWSEINFO lpbi);

		/// <summary>
		/// <para>Locks the shared memory associated with a Shell change notification event.</para>
		/// </summary>
		/// <param name="hChange">
		/// <para>Type: <c>HANDLE</c></para>
		/// <para>A handle to a window received as a in the specified Shell change notification message.</para>
		/// </param>
		/// <param name="dwProcId">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>The process ID ( in the message callback).</para>
		/// </param>
		/// <param name="pppidl">
		/// <para>Type: <c>PIDLIST_ABSOLUTE**</c></para>
		/// <para>
		/// The address of a pointer to a PIDLIST_ABSOLUTE that, when this function returns successfully, receives the list of affected PIDLs.
		/// </para>
		/// </param>
		/// <param name="plEvent">
		/// <para>Type: <c>LONG*</c></para>
		/// <para>
		/// A pointer to a LONG value that, when this function returns successfully, receives the Shell change notification ID of the event
		/// that took place.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HANDLE</c></para>
		/// <para>Returns a handle (HLOCK) to the locked memory. Pass this value to SHChangeNotification_Unlock when finished.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shchangenotification_lock HANDLE
		// SHChangeNotification_Lock( HANDLE hChange, DWORD dwProcId, PIDLIST_ABSOLUTE **pppidl, LONG *plEvent );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "8e22d5d0-64be-403c-982d-c23705d85223")]
		public static extern IntPtr SHChangeNotification_Lock(IntPtr hChange, uint dwProcId, IntPtr pppidl, ref int plEvent);

		/// <summary>
		/// <para>Unlocks shared memory for a change notification.</para>
		/// </summary>
		/// <param name="hLock">
		/// <para>Type: <c>HANDLE</c></para>
		/// <para>A handle to the memory lock. This is the handle returned by SHChangeNotification_Lock when it locked the memory.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> on success; otherwise, <c>FALSE</c>.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shchangenotification_unlock BOOL
		// SHChangeNotification_Unlock( HANDLE hLock );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "967ede1f-ee9c-46ee-a371-dcfc3a57d824")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHChangeNotification_Unlock(IntPtr hLock);

		/// <summary>
		/// Notifies the system of an event that an application has performed. An application should use this function if it performs an
		/// action that may affect the Shell.
		/// </summary>
		/// <param name="wEventId">
		/// Describes the event that has occurred. Typically, only one event is specified at a time. If more than one event is specified, the
		/// values contained in the dwItem1 and dwItem2 parameters must be the same, respectively, for all specified events.
		/// </param>
		/// <param name="uFlags">Flags that, when combined bitwise with SHCNF_TYPE, indicate the meaning of the dwItem1 and dwItem2 parameters.</param>
		/// <param name="dwItem1">Optional. First event-dependent value.</param>
		/// <param name="dwItem2">Optional. Second event-dependent value.</param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h")]
		public static extern void SHChangeNotify(SHCNE wEventId, SHCNF uFlags, [Optional] IntPtr dwItem1, [Optional] IntPtr dwItem2);

		/// <summary>
		/// <para>Unregisters the client's window process from receiving SHChangeNotify messages.</para>
		/// </summary>
		/// <param name="ulID">
		/// <para>Type: <c>ULONG</c></para>
		/// <para>A value of type <c>ULONG</c> that specifies the registration ID returned by SHChangeNotifyRegister.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if the specified client was found and removed; otherwise <c>FALSE</c>.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// See the Change Notify Watcher Sample in the Windows Software Development Kit (SDK) for a full example that demonstrates the use
		/// of this function.
		/// </para>
		/// <para>
		/// The <c>NTSHChangeNotifyDeregister</c> function, which is no longer available for use as of Windows Vista, was equivalent to <c>SHChangeNotifyDeregister</c>.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shchangenotifyderegister BOOL
		// SHChangeNotifyDeregister( ULONG ulID );
		[DllImport(Lib.Shell32, SetLastError = false, CharSet = CharSet.Auto)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "fad021dc-8199-4384-b623-c98bc618799f")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHChangeNotifyDeregister(uint ulID);

		/// <summary>
		/// <para>Registers a window to receive notifications from the file system or Shell, if the file system supports notifications.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>A handle to the window that receives the change or notification messages.</para>
		/// </param>
		/// <param name="fSources">
		/// <para>Type: <c>int</c></para>
		/// <para>One or more of the following values that indicate the type of events for which to receive notifications.</para>
		/// <para>
		/// <c>Note</c> In earlier versions of the SDK, these flags are not defined in a header file and implementers must define these
		/// values themselves or use their numeric values directly. As of Windows Vista, these flags are defined in Shlobj.h.
		/// </para>
		/// <para>SHCNRF_InterruptLevel (0x0001)</para>
		/// <para>Interrupt level notifications from the file system.</para>
		/// <para>SHCNRF_ShellLevel (0x0002)</para>
		/// <para>Shell-level notifications from the shell.</para>
		/// <para>SHCNRF_RecursiveInterrupt (0x1000)</para>
		/// <para>
		/// Interrupt events on the whole subtree. This flag must be combined with the <c>SHCNRF_InterruptLevel</c> flag. When using this
		/// flag, notifications must also be made recursive by setting the <c>fRecursive</c> member of the corresponding SHChangeNotifyEntry
		/// structure referenced by to <c>TRUE</c>. Use of <c>SHCNRF_RecursiveInterrupt</c> on a single level view—for example, a PIDL that
		/// is relative and contains only one SHITEMID—will block event notification at the highest level and thereby prevent a recursive,
		/// child update. Thus, an icon dragged into the lowest level of a folder hierarchy may fail to appear in the view as expected.
		/// </para>
		/// <para>SHCNRF_NewDelivery (0x8000)</para>
		/// <para>
		/// Messages received use shared memory. Call SHChangeNotification_Lock to access the actual data. Call SHChangeNotification_Unlock
		/// to release the memory when done.
		/// </para>
		/// <para>
		/// <c>Note</c> We recommend this flag because it provides a more robust delivery method. All clients should specify this flag.
		/// </para>
		/// </param>
		/// <param name="fEvents">
		/// <para>Type: <c>LONG</c></para>
		/// <para>
		/// Change notification events for which to receive notification. See the SHCNE flags listed in SHChangeNotify for possible values.
		/// </para>
		/// </param>
		/// <param name="wMsg">
		/// <para>Type: <c>UINT</c></para>
		/// <para>Message to be posted to the window procedure.</para>
		/// </param>
		/// <param name="cEntries">
		/// <para>Type: <c>int</c></para>
		/// <para>Number of entries in the array.</para>
		/// </param>
		/// <param name="pshcne">
		/// <para>Type: <c>const SHChangeNotifyEntry*</c></para>
		/// <para>
		/// Array of SHChangeNotifyEntry structures that contain the notifications. This array should always be set to one when calling
		/// <c>SHChangeNotifyRegister</c> or SHChangeNotifyDeregister will not work properly.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>ULONG</c></para>
		/// <para>Returns a positive integer registration ID. Returns 0 if out of memory or in response to invalid parameters.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// See the Change Notify Watcher Sample in the Windows Software Development Kit (SDK) for a full example that demonstrates the use
		/// of this function.
		/// </para>
		/// <para>When a change notification event is raised, the message indicated by is delivered to the window specified by the parameter.</para>
		/// <list type="bullet">
		/// <item>
		/// If SHCNRF_NewDelivery is specified, the and values in the message should be passed to SHChangeNotification_Lock as the and
		/// parameters respectively.
		/// </item>
		/// <item>
		/// If SHCNRF_NewDelivery is not specified, is a pointer to two PIDLIST_ABSOLUTE pointers, and specifies the event. The two
		/// PIDLIST_ABSOLUTE pointers can be <c>NULL</c>, depending on the event being sent.
		/// </item>
		/// </list>
		/// <para>When a relevant file system event takes place and the</para>
		/// <para>hwnd</para>
		/// <para>parameter is not</para>
		/// <para>NULL</para>
		/// <para>, then the message indicated by</para>
		/// <para>wMsg</para>
		/// <para>is posted to the specified window. Otherwise, if the</para>
		/// <para>pshcne</para>
		/// <para>parameter is not</para>
		/// <para>NULL</para>
		/// <para>, then that notification entry is used.</para>
		/// <para>
		/// For performance reasons, multiple notifications can be combined into a single notification. For example, if a large number of
		/// SHCNE_UPDATEITEM notifications are generated for files in the same folder, they can be joined into a single SHCNE_UPDATEDIR notification.
		/// </para>
		/// <para>
		/// The <c>NTSHChangeNotifyRegister</c> function, which is no longer available as of Windows Vista, was equivalent to
		/// <c>SHChangeNotifyRegister</c> with the SHCNRF_NewDelivery flag.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shchangenotifyregister ULONG
		// SHChangeNotifyRegister( HWND hwnd, int fSources, LONG fEvents, UINT wMsg, int cEntries, const SHChangeNotifyEntry *pshcne );
		[DllImport(Lib.Shell32, SetLastError = false, CharSet = CharSet.Auto)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "73143865-ca2f-4578-a7a2-2ba4833eddd8")]
		public static extern uint SHChangeNotifyRegister(HandleRef hwnd, SHCNRF fSources, SHCNE fEvents, uint wMsg, int cEntries, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] SHChangeNotifyEntry[] pshcne);

		/// <summary>
		/// <para>Enables asynchronous register and deregister of a thread.</para>
		/// </summary>
		/// <param name="status">
		/// <para>Type: <c>SCNRT_STATUS</c></para>
		/// <para>Indicates whether the function is being used to register or deregister the thread. One of the values of SCNRT_STATUS.</para>
		/// </param>
		/// <returns>
		/// <para>This function does not return a value.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj/nf-shlobj-shchangenotifyregisterthread void
		// SHChangeNotifyRegisterThread( SCNRT_STATUS status );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj.h", MSDNShortId = "170afefc-b4de-4661-9c12-1341656b0fdb")]
		public static extern void SHChangeNotifyRegisterThread(SCNRT_STATUS status);

		/// <summary>
		/// <para>Creates a data object in a parent folder.</para>
		/// </summary>
		/// <param name="pidlFolder">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>A pointer to an ITEMIDLIST (PIDL) of the parent folder that contains the data object.</para>
		/// </param>
		/// <param name="cidl">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The number of file objects or subfolders specified in the parameter.</para>
		/// </param>
		/// <param name="apidl">
		/// <para>Type: <c>PCUITEMID_CHILD_ARRAY</c></para>
		/// <para>
		/// An array of pointers to constant ITEMIDLIST structures, each of which uniquely identifies a file object or subfolder relative to
		/// the parent folder. Each item identifier list must contain exactly one SHITEMID structure followed by a terminating zero.
		/// </para>
		/// </param>
		/// <param name="pdtInner">
		/// <para>Type: <c>IDataObject*</c></para>
		/// <para>
		/// A pointer to interface IDataObject. This parameter can be <c>NULL</c>. Specify only if the data object created needs to support
		/// additional FORMATETC clipboard formats beyond the default formats it is assigned at creation. Alternatively, provide support for
		/// populating the created data object using non-default clipboard formats by calling method IDataObject::SetData and specifying the
		/// format in the <c>FORMATETC</c> structure passed in parameter .
		/// </para>
		/// </param>
		/// <param name="riid">
		/// <para>Type: <c>REFIID</c></para>
		/// <para>A reference to the IID of the interface to retrieve through . This must be IID_IDataObject.</para>
		/// </param>
		/// <param name="ppv">
		/// <para>Type: <c>void**</c></para>
		/// <para>When this method returns successfully, contains the IDataObject interface pointer requested in .</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This function is typically called when implementing method IShellFolder::GetUIObjectOf. When an interface pointer of interface ID
		/// IID_IDataObject is requested (using parameter ), the implementer can return the interface pointer on the object created with
		/// <c>SHCreateDataObject</c> in response.
		/// </para>
		/// <para>
		/// This function supports the CFSTR_SHELLIDLIST (also known as HIDA) clipboard format and also has generic support for arbitrary
		/// clipboard formats through IDataObject::SetData. For more information on clipboard formats, see Shell Clipboard Formats.
		/// </para>
		/// <para>
		/// The new data object is intended to be used in operations such as drag-and-drop, in which the data is stored in the clipboard with
		/// a given format.
		/// </para>
		/// <para>
		/// We recommend that you use the IID_PPV_ARGS macro, defined in Objbase.h, to package the and parameters. This macro provides the
		/// correct IID based on the interface pointed to by the value in , which eliminates the possibility of a coding error in that could
		/// lead to unexpected results.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreatedataobject SHSTDAPI SHCreateDataObject(
		// PCIDLIST_ABSOLUTE pidlFolder, UINT cidl, PCUITEMID_CHILD_ARRAY apidl, IDataObject *pdtInner, REFIID riid, void **ppv );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "d56cdafe-9463-43a5-8ef0-6cfaf0c524a8")]
		public static extern HRESULT SHCreateDataObject(PIDL pidlFolder, uint cidl, [In, MarshalAs(UnmanagedType.LPArray)] PIDL[] apidl, IDataObject pdtInner, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IDataObject ppv);

		/// <summary>
		/// <para>Creates an object that represents the Shell's default context menu implementation.</para>
		/// </summary>
		/// <param name="pdcm">
		/// <para>Type: <c>const DEFCONTEXTMENU*</c></para>
		/// <para>A pointer to a constant DEFCONTEXTMENU structure.</para>
		/// </param>
		/// <param name="riid">
		/// <para>Type: <c>REFIID</c></para>
		/// <para>
		/// Reference to the interface ID of the interface on which to base the object. This is typically the IID of IContextMenu,
		/// IContextMenu2, or IContextMenu3.
		/// </para>
		/// </param>
		/// <param name="ppv">
		/// <para>Type: <c>void**</c></para>
		/// <para>When this method returns, contains the interface pointer requested in riid.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This function is typically used in the implementation of IShellFolder::GetUIObjectOf. <c>GetUIObjectOf</c> creates a context menu
		/// that merges IContextMenu handlers specified by the DEFCONTEXTMENU structure, and can optionally provide default context menu verb
		/// implementations such as open, explore, delete, and copy.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreatedefaultcontextmenu SHSTDAPI
		// SHCreateDefaultContextMenu( const DEFCONTEXTMENU *pdcm, REFIID riid, void **ppv );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "055ff0a0-9ba7-463d-9684-3fd072b190da")]
		public static extern HRESULT SHCreateDefaultContextMenu(ref DEFCONTEXTMENU pdcm, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		/// <summary>
		/// <para>
		/// [SHCreateDirectory is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Creates a new file system folder.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>A handle to a parent window. This parameter can be set to <c>NULL</c> if no user interface is displayed.</para>
		/// </param>
		/// <param name="pszPath">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated Unicode string that contains the fully qualified path of the directory. This string should have no
		/// more than MAX_PATH characters, including the terminating null character.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>
		/// Returns <c>ERROR_SUCCESS</c> if successful. If the operation fails, other error codes can be returned, including those listed
		/// here. For values not specifically listed, see System Error Codes.
		/// </para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return code</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>ERROR_BAD_PATHNAME</term>
		/// <term>The parameter was set to a relative path.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_FILENAME_EXCED_RANGE</term>
		/// <term>The path pointed to by is too long.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_FILE_EXISTS</term>
		/// <term>The directory exists.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_ALREADY_EXISTS</term>
		/// <term>The directory exists.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_CANCELLED</term>
		/// <term>The user canceled the operation.</term>
		/// </item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This function creates a file system folder whose fully qualified path is given by . If one or more of the intermediate folders do
		/// not exist, it creates them.
		/// </para>
		/// <para>To set security attributes on a new folder, use SHCreateDirectoryEx.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreatedirectory int SHCreateDirectory( HWND
		// hwnd, PCWSTR pszPath );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "4927429c-f457-4dda-aa0d-236eb236795c")]
		public static extern Win32Error SHCreateDirectory([Optional] HandleRef hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszPath);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows.]
		/// </para>
		/// <para>Creates a new file system folder, with optional security attributes.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>A handle to a parent window. This parameter can be set to <c>NULL</c> if no user interface will be displayed.</para>
		/// </param>
		/// <param name="pszPath">
		/// <para>Type: <c>LPCTSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated string specifying the fully qualified path of the directory. This string is of maximum length of
		/// 248 characters, including the terminating null character.
		/// </para>
		/// </param>
		/// <param name="psa">
		/// <para>Type: <c>const SECURITY_ATTRIBUTES*</c></para>
		/// <para>
		/// A pointer to a SECURITY_ATTRIBUTES structure with the directory's security attribute. Set this parameter to <c>NULL</c> if no
		/// security attributes need to be set.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>
		/// Returns <c>ERROR_SUCCESS</c> if successful. If the operation fails, other error codes can be returned, including those listed
		/// here. For values not specifically listed, see System Error Codes.
		/// </para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return code</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>ERROR_BAD_PATHNAME</term>
		/// <term>The parameter was set to a relative path.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_FILENAME_EXCED_RANGE</term>
		/// <term>The path pointed to by is too long.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_PATH_NOT_FOUND</term>
		/// <term>The system cannot find the path pointed to by . The path may contain an invalid entry.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_FILE_EXISTS</term>
		/// <term>The directory exists.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_ALREADY_EXISTS</term>
		/// <term>The directory exists.</term>
		/// </item>
		/// <item>
		/// <term>ERROR_CANCELLED</term>
		/// <term>The user canceled the operation.</term>
		/// </item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This function creates a file system folder whose fully qualified path is given by . If one or more of the intermediate folders do
		/// not exist, they are created as well. <c>SHCreateDirectoryEx</c> also verifies that the files are visible. If they are not
		/// visible, expect one of the following:
		/// </para>
		/// <list type="bullet">
		/// <item>
		/// If is set to a valid window handle, a message box is displayed warning the user that he or she might not be able to access the
		/// files. If the user chooses not to proceed, the function returns <c>ERROR_CANCELLED</c>.
		/// </item>
		/// <item>If is set to <c>NULL</c>, no user interface is displayed and the function returns <c>ERROR_CANCELLED</c>.</item>
		/// </list>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreatedirectoryexa int SHCreateDirectoryEx( HWND
		// hwnd, LPCTSTR pszPath, const SECURITY_ATTRIBUTES *psa );
		[DllImport(Lib.Shell32, SetLastError = false, CharSet = CharSet.Auto)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "7f44f907-cd12-4156-91c0-76e577ae25f6")]
		public static extern Win32Error SHCreateDirectoryEx([Optional] HandleRef hwnd, string pszPath, [In] SECURITY_ATTRIBUTES psa);

		/// <summary>
		/// <para>[</para>
		/// <para>SHCreateFileExtractIcon</para>
		/// <para>
		/// is available for use in the operating systems specified in the Requirements section. It may be altered or unavailable in
		/// subsequent versions.]
		/// </para>
		/// <para>
		/// Creates a default IExtractIcon handler for a file system object. Namespace extensions that display file system objects typically
		/// use this function. The extension and file attributes derive all that is needed for a simple icon extractor.
		/// </para>
		/// </summary>
		/// <param name="pszFile">
		/// <para>Type: <c>LPCTSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated string that specifies the file system object. The buffer must not exceed MAX_PATH characters in length.
		/// </para>
		/// </param>
		/// <param name="dwFileAttributes">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>
		/// A combination of one or more file attribute flags (FILE_ATTRIBUTE_* values as defined in Winnt.h) that specify the type of object.
		/// </para>
		/// </param>
		/// <param name="riid">
		/// <para>Type: <c>REFIID</c></para>
		/// <para>
		/// Reference to the desired interface ID of the icon extractor interface to create. This must be either IID_IExtractIconA or IID_IExtractIconW.
		/// </para>
		/// </param>
		/// <param name="ppv">
		/// <para>Type: <c>void**</c></para>
		/// <para>When this function returns, contains the interface pointer requested in . This is typically IExtractIcon.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreatefileextracticonw SHSTDAPI
		// SHCreateFileExtractIconW( LPCWSTR pszFile, DWORD dwFileAttributes, REFIID riid, void **ppv );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "af3beb0a-892b-43e5-b5b8-8005f497b6e5")]
		public static extern HRESULT SHCreateFileExtractIconW([MarshalAs(UnmanagedType.LPWStr)] string pszFile, FileFlagsAndAttributes dwFileAttributes, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		/// <summary>
		/// <para>
		/// [SHCreatePropSheetExtArray is available for use in the operating systems specified in the Requirements section. It may be altered
		/// or unavailable in subsequent versions.]
		/// </para>
		/// <para>Loads all the Shell property sheet extension handlers located under a specified registry key.</para>
		/// </summary>
		/// <param name="hKey">
		/// <para>TBD</para>
		/// </param>
		/// <param name="pszSubKey">
		/// <para>TBD</para>
		/// </param>
		/// <param name="max_iface">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The maximum number of property sheet handlers to be returned.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HPSXA</c></para>
		/// <para>
		/// Returns a handle to an array of property sheet handlers. Pass this value to SHAddFromPropSheetExtArray. You do not access this
		/// value directly.
		/// </para>
		/// </returns>
		/// <remarks>
		/// <para>When you are finished with the returned HPSXA handle, destroy it by calling SHDestroyPropSheetExtArray.</para>
		/// <para>This function loads up to property sheet extensions into an array that is then passed to SHAddFromPropSheetExtArray.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj/nf-shlobj-shcreatepropsheetextarray WINSHELLAPI HPSXA
		// SHCreatePropSheetExtArray( HKEY hKey, PCWSTR pszSubKey, UINT max_iface );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj.h", MSDNShortId = "88a72529-325d-431e-bc26-bddca787e62b")]
		public static extern IntPtr SHCreatePropSheetExtArray(IntPtr hKey, [MarshalAs(UnmanagedType.LPWStr)] string pszSubKey, uint max_iface);

		/// <summary>
		/// <para>Creates a new instance of the default Shell folder view object (DefView).</para>
		/// </summary>
		/// <param name="pcsfv">
		/// <para>Type: <c>const SFV_CREATE*</c></para>
		/// <para>
		/// Pointer to a SFV_CREATE structure that describes the particulars used in creating this instance of the Shell folder view object.
		/// </para>
		/// </param>
		/// <param name="ppsv">
		/// <para>Type: <c>IShellView**</c></para>
		/// <para>
		/// When this function returns successfully, contains an interface pointer to the new IShellView object. On failure, this value is <c>NULL</c>.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// <c>SHCreateShellFolderView</c> is recommended over SHCreateShellFolderViewEx because of the greater flexibility of its elements
		/// to participate in various scenarios, provide new functionality to the view, and interact with other objects.
		/// </para>
		/// <para>
		/// When dealing with several instances of IShellView, you might want to verify which is the default Shell folder view object. To do
		/// so, call QueryInterface on the object using the IID_CDefView IID. This call succeeds only when made on the default Shell folder
		/// view object.
		/// </para>
		/// <para>Data sources that use the default Shell folder view object must implement these interfaces:</para>
		/// <list type="bullet">
		/// <item>IShellFolder</item>
		/// <item>IShellFolder2</item>
		/// <item>IPersistFolder</item>
		/// <item>IPersistFolder2</item>
		/// </list>
		/// <para>Optionally, they can also implement</para>
		/// <para>IPersistFolder3</para>
		/// <para>.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreateshellfolderview SHSTDAPI
		// SHCreateShellFolderView( const SFV_CREATE *pcsfv, IShellView **ppsv );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "f2948a6d-84a5-456b-b328-ba76dba46e9d")]
		public static extern HRESULT SHCreateShellFolderView(ref SFV_CREATE pcsfv, out IShellView ppsv);

		/// <summary>
		/// <para>
		/// Creates a new instance of the default Shell folder view object. It is recommended that you use SHCreateShellFolderView rather
		/// than this function.
		/// </para>
		/// </summary>
		/// <param name="pcsfv">
		/// <para>Type: <c>CSFV*</c></para>
		/// <para>Pointer to a structure that describes the details used in creating this instance of the Shell folder view object.</para>
		/// </param>
		/// <param name="ppsv">
		/// <para>Type: <c>IShellView**</c></para>
		/// <para>
		/// The address of an IShellView interface pointer that, when this function returns successfully, points to the new view object. On
		/// failure, this value is <c>NULL</c>.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// SHCreateShellFolderView is recommended over <c>SHCreateShellFolderViewEx</c> because of the greater flexibility of its elements
		/// to participate in various scenarios, provide new functionality to the view, and interact with other objects.
		/// </para>
		/// <para>
		/// When dealing with several instances of IShellView, you might want to verify which is the default Shell folder view object. To do
		/// so, call QueryInterface on the object using IID_CDefView. This call succeeds only on the default Shell folder view object.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreateshellfolderviewex SHSTDAPI
		// SHCreateShellFolderViewEx( CSFV *pcsfv, IShellView **ppsv );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "7edd6786-7d74-4065-8cf1-cbb489007a46")]
		public static extern HRESULT SHCreateShellFolderViewEx(ref CSFV pcsfv, out IShellView ppsv);

		/// <summary>
		/// <para>Creates an IShellItem object.</para>
		/// <para><c>Note</c> It is recommended that you use SHCreateItemWithParent or SHCreateItemFromIDList instead of this function.</para>
		/// </summary>
		/// <param name="pidlParent">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>A PIDL to the parent. This value can be <c>NULL</c>.</para>
		/// </param>
		/// <param name="psfParent">
		/// <para>Type: <c>IShellFolder*</c></para>
		/// <para>A pointer to the parent IShellFolder. This value can be <c>NULL</c>.</para>
		/// </param>
		/// <param name="pidl">
		/// <para>Type: <c>PCUITEMID_CHILD</c></para>
		/// <para>A PIDL to the requested item. If parent information is not included in or , this must be an absolute PIDL.</para>
		/// </param>
		/// <param name="ppsi">
		/// <para>Type: <c>IShellItem**</c></para>
		/// <para>When this method returns, contains the interface pointer to the new IShellItem.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// <c>SHCreateShellItem</c> creates an object that represents a Shell namespace item. The caller must provide parent information in
		/// or ; alternatively, the caller can provide an absolute IDList in the parameter.
		/// </para>
		/// <para>There are three valid calling patterns for this function:</para>
		/// <list type="number">
		/// <item>
		/// The parent folder is identified by an absolute IDList . The parameter points to a child IDList that identifies the item within
		/// the folder identified by .
		/// </item>
		/// <item>
		/// The parent folder is identified by . The parameter points to a child IDList that identifies the item within the folder identified
		/// by .
		/// </item>
		/// <item>The item is identified by an absolute IDList passed to the parameter.</item>
		/// </list>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreateshellitem SHSTDAPI SHCreateShellItem(
		// PCIDLIST_ABSOLUTE pidlParent, IShellFolder *psfParent, PCUITEMID_CHILD pidl, IShellItem **ppsi );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "d4371cdf-a8f4-4a39-ba66-97fd40ed46ae")]
		public static extern HRESULT SHCreateShellItem(PIDL pidlParent, IShellFolder psfParent, PIDL pidl, out IShellItem ppsi);

		/// <summary>
		/// <para>
		/// [SHCreateStdEnumFmtEtc is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Creates an IEnumFORMATETC object from an array of FORMATETC structures.</para>
		/// </summary>
		/// <param name="cfmt">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The number of entries in the array.</para>
		/// </param>
		/// <param name="afmt">
		/// <para>Type: <c>const FORMATETC[]</c></para>
		/// <para>An array of FORMATETC structures that specifies the clipboard formats of interest.</para>
		/// </param>
		/// <param name="ppenumFormatEtc">
		/// <para>Type: <c>IEnumFORMATETC**</c></para>
		/// <para>When this function returns successfully, receives an IEnumFORMATETC interface pointer. Receives <c>NULL</c> on failure.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shcreatestdenumfmtetc SHSTDAPI
		// SHCreateStdEnumFmtEtc( UINT cfmt, const FORMATETC [] afmt, IEnumFORMATETC **ppenumFormatEtc );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "c391c8c8-6062-4e70-9a1f-de0eb610250d")]
		public static extern HRESULT SHCreateStdEnumFmtEtc(uint cfmt, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] FORMATETC[] afmt, out IEnumFORMATETC ppenumFormatEtc);

		/// <summary>Provides a default handler to extract an icon from a file.</summary>
		/// <param name="pszIconFile">
		/// A pointer to a null-terminated buffer that contains the path and name of the file from which the icon is extracted.
		/// </param>
		/// <param name="iIndex">
		/// The location of the icon within the file named in pszIconFile. If this is a positive number, it refers to the zero-based position
		/// of the icon in the file. For instance, 0 refers to the 1st icon in the resource file and 2 refers to the 3rd. If this is a
		/// negative number, it refers to the icon's resource ID.
		/// </param>
		/// <param name="uFlags">A flag that controls the icon extraction.</param>
		/// <param name="phiconLarge">
		/// A pointer to an HICON that, when this function returns successfully, receives the handle of the large version of the icon
		/// specified in the LOWORD of nIconSize. This value can be NULL.
		/// </param>
		/// <param name="phiconSmall">
		/// A pointer to an HICON that, when this function returns successfully, receives the handle of the small version of the icon
		/// specified in the HIWORD of nIconSize.
		/// </param>
		/// <param name="nIconSize">
		/// A value that contains the large icon size in its LOWORD and the small icon size in its HIWORD. Size is measured in pixels. Pass 0
		/// to specify default large and small sizes.
		/// </param>
		[DllImport(Lib.Shell32, CharSet = CharSet.Auto)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762149")]
		public static extern HRESULT SHDefExtractIcon(string pszIconFile, int iIndex, uint uFlags, ref IntPtr phiconLarge,
			ref IntPtr phiconSmall, uint nIconSize);

		/// <summary>Provides a default handler to extract an icon from a file.</summary>
		/// <param name="pszIconFile">
		/// A pointer to a null-terminated buffer that contains the path and name of the file from which the icon is extracted.
		/// </param>
		/// <param name="iIndex">
		/// The location of the icon within the file named in pszIconFile. If this is a positive number, it refers to the zero-based position
		/// of the icon in the file. For instance, 0 refers to the 1st icon in the resource file and 2 refers to the 3rd. If this is a
		/// negative number, it refers to the icon's resource ID.
		/// </param>
		/// <param name="uFlags">A flag that controls the icon extraction.</param>
		/// <param name="phiconLarge">
		/// A pointer to an HICON that, when this function returns successfully, receives the handle of the large version of the icon
		/// specified in the LOWORD of nIconSize. This value can be NULL.
		/// </param>
		/// <param name="phiconSmall">
		/// A pointer to an HICON that, when this function returns successfully, receives the handle of the small version of the icon
		/// specified in the HIWORD of nIconSize.
		/// </param>
		/// <param name="nIconSize">
		/// A value that contains the large icon size in its LOWORD and the small icon size in its HIWORD. Size is measured in pixels. Pass 0
		/// to specify default large and small sizes.
		/// </param>
		[DllImport(Lib.Shell32, CharSet = CharSet.Auto)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762149")]
		public static extern HRESULT SHDefExtractIcon(string pszIconFile, int iIndex, uint uFlags, IntPtr phiconLarge,
			ref IntPtr phiconSmall, uint nIconSize);

		/// <summary>
		/// <para>
		/// [SHDestroyPropSheetExtArray is available for use in the operating systems specified in the Requirements section. It may be
		/// altered or unavailable in subsequent versions.]
		/// </para>
		/// <para>Frees property sheet handlers that are pointed to an array created by SHCreatePropSheetExtArray.</para>
		/// </summary>
		/// <param name="hpsxa">
		/// <para>Type: <c>HPSXA</c></para>
		/// <para>The handle of the array that contains pointers to the property sheet handlers to destroy.</para>
		/// </param>
		/// <returns>
		/// <para>This function does not return a value.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shdestroypropsheetextarray WINSHELLAPI void
		// SHDestroyPropSheetExtArray( HPSXA hpsxa );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "beb3c1b1-deef-440d-8cf7-f76b3f396efa")]
		public static extern void SHDestroyPropSheetExtArray(IntPtr hpsxa);

		/// <summary>
		/// <para>Executes a drag-and-drop operation. Supports drag source creation on demand, as well as drag images.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>The handle of the window used to obtain the drag image. This value can be <c>NULL</c>. See Remarks for more details.</para>
		/// </param>
		/// <param name="pdata">
		/// <para>TBD</para>
		/// </param>
		/// <param name="pdsrc">
		/// <para>Type: <c>IDropSource*</c></para>
		/// <para>
		/// A pointer to an implementation of the IDropSource interface, which is used to communicate with the source during the drag operation.
		/// </para>
		/// <para>As of Windows Vista, if this value is <c>NULL</c>, the Shell creates a drop source object for you.</para>
		/// </param>
		/// <param name="dwEffect">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>
		/// The effects that the source allows in the drag-and-drop operation. The most significant effect is whether the drag-and-drop
		/// operation permits a move. For a list of possible values, see DROPEFFECT.
		/// </para>
		/// </param>
		/// <param name="pdwEffect">
		/// <para>Type: <c>DWORD*</c></para>
		/// <para>
		/// A pointer to a value that indicates how the drag-and-drop operation affected the source data. The parameter is set only if the
		/// operation is not canceled. For a list of possible values, see DROPEFFECT.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>This function supports the standard return value E_OUTOFMEMORY, as well as the following values:</para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return code</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>DRAGDROP_S_DROP</term>
		/// <term>The drag-and-drop operation was successful.</term>
		/// </item>
		/// <item>
		/// <term>DRAGDROP_S_CANCEL</term>
		/// <term>The drag-and-drop operation was canceled.</term>
		/// </item>
		/// <item>
		/// <term>E_UNSPEC</term>
		/// <term>Unexpected error occurred.</term>
		/// </item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// <para>
		/// As of Windows Vista, if a drag image is not already stored in the data object and a drag image cannot be obtained from the window
		/// specified by , the Shell provides a generic drag image. A drag image can fail to be obtained from the specified window either
		/// because is <c>NULL</c> or the specified window does not support the DI_GETDRAGIMAGE message.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shdodragdrop SHSTDAPI SHDoDragDrop( HWND hwnd,
		// IDataObject *pdata, IDropSource *pdsrc, DWORD dwEffect, DWORD *pdwEffect );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "76c98516-ede9-47de-b4ad-257a162775b9")]
		public static extern HRESULT SHDoDragDrop(HandleRef hwnd, IDataObject pdata, [Optional] IntPtr pdsrc, DROPEFFECT dwEffect, ref DROPEFFECT pdwEffect);

		/// <summary>
		/// <para>
		/// [Shell_GetCachedImageIndex is available for use in the operating systems specified in the Requirements section. It may be altered
		/// or unavailable in subsequent versions. Instead, use
		/// </para>
		/// <para>Shell_GetCachedImageIndexA</para>
		/// <para>or</para>
		/// <para>Shell_GetCachedImageIndexW</para>
		/// <para>.]</para>
		/// <para>Retrieves the cache index of a cached icon.</para>
		/// </summary>
		/// <param name="pszIconPath">
		/// <para>TBD</para>
		/// </param>
		/// <param name="iIconIndex">
		/// <para>Type: <c>int</c></para>
		/// <para>The index of the image within the file named at .</para>
		/// </param>
		/// <param name="uIconFlags">
		/// <para>Type: <c>UINT</c></para>
		/// <para>Not used.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns the index of the image, or –1 on failure.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// The <c>Shell_GetCachedImageIndexA</c> and <c>Shell_GetCachedImageIndexW</c> versions of this function were added in Windows
		/// Vista. For Unicode strings, call either <c>Shell_GetCachedImageIndexW</c> or <c>Shell_GetCachedImageIndex</c>. For ANSI strings,
		/// you must call <c>Shell_GetCachedImageIndexA</c> explicitly.
		/// </para>
		/// <para>
		/// <c>Windows Server 2003 and Windows XP:</c> Only <c>Shell_GetCachedImageIndex</c> is supported. <c>Shell_GetCachedImageIndex</c>
		/// requires a Unicode string.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shell_getcachedimageindexa int
		// Shell_GetCachedImageIndexA( LPCSTR pszIconPath, int iIconIndex, UINT uIconFlags );
		[DllImport(Lib.Shell32, SetLastError = false, CharSet = CharSet.Auto)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "f0d4dd1f-a41c-4dd0-9713-e3aec48ff101")]
		public static extern int Shell_GetCachedImageIndex(string pszIconPath, int iIconIndex, uint uIconFlags);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows.]
		/// </para>
		/// <para>Retrieves system image lists for large and small icons.</para>
		/// </summary>
		/// <param name="phiml">
		/// <para>Type: <c>HIMAGELIST*</c></para>
		/// <para>A pointer to the handle of an image list which, on success, receives the system image list for large (32 x 32) icons.</para>
		/// </param>
		/// <param name="phimlSmall">
		/// <para>Type: <c>HIMAGELIST*</c></para>
		/// <para>A pointer to the handle of an image list which, on success, receives the system image list for small (16 x 16) icons.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> on success. On failure, returns <c>FALSE</c> and the image lists pointed to by and are unchanged.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// <c>Important</c> The image lists retrieved through this function are global system image lists; do not call ImageList_Destroy
		/// using them.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shell_getimagelists BOOL Shell_GetImageLists(
		// HIMAGELIST *phiml, HIMAGELIST *phimlSmall );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "c3b73616-849c-4149-b04d-a7d389ebf700")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool Shell_GetImageLists(ref IntPtr phiml, ref IntPtr phimlSmall);

		/// <summary>
		/// <para>
		/// [Shell_MergeMenus is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Merges two menus.</para>
		/// </summary>
		/// <param name="hmDst">
		/// <para>Type: <c>HMENU</c></para>
		/// <para>The destination menu to which is added.</para>
		/// </param>
		/// <param name="hmSrc">
		/// <para>Type: <c>HMENU</c></para>
		/// <para>The source menu which is added to .</para>
		/// </param>
		/// <param name="uInsert">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The point in after which the entries in are inserted.</para>
		/// </param>
		/// <param name="uIDAdjust">
		/// <para>Type: <c>UINT</c></para>
		/// <para>
		/// This number is added to each menu's ID to give an adjusted ID. Set to for no adjustment. The value for would typically be the
		/// number of items in . This number can be obtained using the GetMenuItemCount.
		/// </para>
		/// </param>
		/// <param name="uIDAdjustMax">
		/// <para>Type: <c>UINT</c></para>
		/// <para>
		/// The maximum adjusted ID to add to the menu. Any adjusted ID greater than this value is not added. To allow all IDs, set this
		/// parameter to 0xFFFF.
		/// </para>
		/// </param>
		/// <param name="uFlags">
		/// <para>Type: <c>ULONG</c></para>
		/// <para>One or more of the following flags.</para>
		/// <para>MM_ADDSEPARATOR</para>
		/// <para>
		/// Add a separator between the items from the two menus if one does not exist already. If you are inserting the entries from into
		/// the middle of , a separator is added above and below the material.
		/// </para>
		/// <para>MM_DONTREMOVESEPS</para>
		/// <para>Do not remove any existing separators in the two menus. Note that this could result in two separators in a row.</para>
		/// <para>MM_SUBMENUSHAVEIDS</para>
		/// <para>Set this flag if the submenus have IDs which should be adjusted.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>UINT</c></para>
		/// <para>Returns the next open ID at the end of the menu (the maximum adjusted ID + 1).</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shell_mergemenus UINT Shell_MergeMenus( HMENU
		// hmDst, HMENU hmSrc, UINT uInsert, UINT uIDAdjust, UINT uIDAdjustMax, ULONG uFlags );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "f9e005fd-b1f2-4a5f-ad36-9c44998dc4eb")]
		public static extern uint Shell_MergeMenus(IntPtr hmDst, IntPtr hmSrc, uint uInsert, uint uIDAdjust, uint uIDAdjustMax, MM uFlags);

		/// <summary>
		/// <para>
		/// [SHFind_InitMenuPopup is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>
		/// Retrieves the IContextMenu instance for the submenu of options displayed for the <c>Search</c> entry in the Classic style Start menu.
		/// </para>
		/// </summary>
		/// <param name="hmenu">
		/// <para>Type: <c>HMENU</c></para>
		/// <para>The handle of the popup menu.</para>
		/// </param>
		/// <param name="hwndOwner">
		/// <para>TBD</para>
		/// </param>
		/// <param name="idCmdFirst">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The ID of the first menu item.</para>
		/// </param>
		/// <param name="idCmdLast">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The ID of the last menu item.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>IContextMenu*</c></para>
		/// <para>If successful, returns an IContextMenu pointer. On failure, returns <c>NULL</c>.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shfind_initmenupopup IContextMenu *
		// SHFind_InitMenuPopup( HMENU hmenu, HWND hwndOwner, UINT idCmdFirst, UINT idCmdLast );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "ca44bd57-6af0-45b3-9331-914e93360743")]
		public static extern IContextMenu SHFind_InitMenuPopup(IntPtr hmenu, HandleRef hwndOwner, uint idCmdFirst, uint idCmdLast);

		/// <summary>
		/// <para>
		/// [SHFindFiles is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Displays the <c>Search</c> window UI.</para>
		/// </summary>
		/// <param name="pidlFolder">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>
		/// The folder from which to start the search. This folder appears in the <c>Look in:</c> box in the <c>Search</c> window. This
		/// folder and all of its subfolders are searched unless users choose other options in the <c>Search</c> window's <c>More Advanced
		/// Options</c>. This value can be <c>NULL</c>.
		/// </para>
		/// </param>
		/// <param name="pidlSaveFile">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>This parameter is not used and must be set to <c>NULL</c>.</para>
		/// <para>
		/// <c>Windows Server 2003 and Windows XP:</c> A saved search file (.fnd) to load. You can save search parameters to a .fnd file
		/// after the search is begun. This value can be <c>NULL</c>.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if successful in displaying the <c>Search</c> window; otherwise <c>FALSE</c>.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shfindfiles BOOL SHFindFiles( PCIDLIST_ABSOLUTE
		// pidlFolder, PCIDLIST_ABSOLUTE pidlSaveFile );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "c54036c2-e6b9-4b21-b2b2-a6721c502ee5")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHFindFiles(PIDL pidlFolder, IntPtr pidlSaveFile);

		/// <summary>
		/// <para>
		/// [SHFlushSFCache is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Flushes the special folder cache.</para>
		/// </summary>
		/// <returns>
		/// <para>This function does not return a value.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// <c>SHFlushSFCache</c> is called when the path to a special folder is changed. This ensures that the updated path stored in the
		/// registry is used rather than the cached value.
		/// </para>
		/// <para>For more information on special folders, see the section of Getting a Folder's ID.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shflushsfcache void SHFlushSFCache( );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "2e39b6b1-e60c-411c-aabc-5a3511f0693b")]
		public static extern void SHFlushSFCache();

		/// <summary>
		/// <para>
		/// [SHFormatDrive is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Opens the Shell's <c>Format</c> dialog box.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>
		/// The handle of the parent window of the dialog box. The <c>Format</c> dialog box must have a parent window; therefore, this
		/// parameter cannot be <c>NULL</c>.
		/// </para>
		/// </param>
		/// <param name="drive">
		/// <para>Type: <c>UINT</c></para>
		/// <para>
		/// The drive to format. The value of this parameter represents a letter drive starting at 0 for the A: drive. For example, a value
		/// of 2 stands for the C: drive.
		/// </para>
		/// </param>
		/// <param name="fmtID">
		/// <para>Type: <c>UINT</c></para>
		/// <para>The ID of the physical format. Only the following flag is currently defined.</para>
		/// <para>SHFMT_ID_DEFAULT (0xFFFF)</para>
		/// <para>The default format ID.</para>
		/// </param>
		/// <param name="options">
		/// <para>Type: <c>UINT</c></para>
		/// <para>
		/// This value must be 0 or one of the following values that alter the default format options in the dialog box. This value is
		/// regarded as a bitfield and should be treated accordingly.
		/// </para>
		/// <para>SHFMT_OPT_FULL (0x0001)</para>
		/// <para>0x001. If this flag is set, then the <c>Quick Format</c> option is selected.</para>
		/// <para>This function is included in Shlobj.h only in Windows XP with SP1 and later.</para>
		/// <para><c>Windows XP:</c> Prior to Windows XP with SP1, this function is accessible through Shell32.lib.</para>
		/// <para>SHFMT_OPT_SYSONLY (0x0002)</para>
		/// <para>0x002. Selects the <c>Create an MS-DOS startup disk</c> option, creating a system boot disk.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>DWORD</c></para>
		/// <para>
		/// Returns the format ID of the last successful format or one of the following values. The LOWORD of this value can be passed on
		/// subsequent calls as the parameter to repeat the last format.
		/// </para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return code</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>SHFMT_ERROR</term>
		/// <term>An error occurred during the last format. This does not indicate that the drive is unformattable.</term>
		/// </item>
		/// <item>
		/// <term>SHFMT_CANCEL</term>
		/// <term>The last format was canceled.</term>
		/// </item>
		/// <item>
		/// <term>SHFMT_NOFORMAT</term>
		/// <term>The drive cannot be formatted.</term>
		/// </item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// <para>
		/// The format is controlled by the dialog box interface. That is, the user must click the <c>OK</c> button to actually begin the
		/// format—the format cannot be started programmatically.
		/// </para>
		/// <para>Examples</para>
		/// <para>
		/// This call to <c>SHFormatDrive</c> brings up the Shell's Format dialog box for a disk in drive A, with the default formatting
		/// options selected.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shformatdrive DWORD SHFormatDrive( HWND hwnd, UINT
		// drive, UINT fmtID, UINT options );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "4aa255fa-c407-47db-9b1f-d449e0a0e94f")]
		public static extern uint SHFormatDrive(HandleRef hwnd, uint drive, SHFMT_ID fmtID, SHFMT_OPT options);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows. Use CoTaskMemFree instead.]
		/// </para>
		/// <para>Frees the memory allocated by SHAlloc.</para>
		/// </summary>
		/// <param name="pv">
		/// <para>Type: <c>void*</c></para>
		/// <para>A pointer to the memory allocated by SHAlloc.</para>
		/// </param>
		/// <returns>
		/// <para>This function does not return a value.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shfree void SHFree( void *pv );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "c9a532ad-ae24-4505-9e7b-577b90365441")]
		public static extern void SHFree(IntPtr pv);

		/// <summary>
		/// <para>
		/// [SHGetAttributesFromDataObject is available for use in the operating systems specified in the Requirements section. It may be
		/// altered or unavailable in subsequent versions.]
		/// </para>
		/// <para>Retrieves specified pieces of information from a system data object.</para>
		/// </summary>
		/// <param name="pdo">
		/// <para>Type: <c>IDataObject*</c></para>
		/// <para>The data object from which to retrieve the information.</para>
		/// </param>
		/// <param name="dwAttributeMask">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>One or more of the SFGAO flags that indicate which pieces of information the calling application wants to retrieve.</para>
		/// </param>
		/// <param name="pdwAttributes">
		/// <para>Type: <c>DWORD*</c></para>
		/// <para>
		/// A pointer to a <c>DWORD</c> value that, when this function returns successfully, receives one or more SFGAO flags that indicate
		/// the attributes, among those requested, that are common to all items in . This pointer can be <c>NULL</c> if this information is
		/// not needed.
		/// </para>
		/// </param>
		/// <param name="pcItems">
		/// <para>Type: <c>UINT*</c></para>
		/// <para>
		/// A pointer to a <c>UINT</c> that, when this function returns successfully, receives the number of PIDLs in the data object pointed
		/// to by . This pointer can be <c>NULL</c> if this information is not needed.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>This function can return one of these values.</para>
		/// <list type="table">
		/// <listheader>
		/// <term>Return code</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item>
		/// <term>S_OK</term>
		/// <term>Success.</term>
		/// </item>
		/// <item>
		/// <term>S_FALSE</term>
		/// <term>The object is not a system data object. In this case, is set to 0.</term>
		/// </item>
		/// </list>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetattributesfromdataobject HRESULT
		// SHGetAttributesFromDataObject( IDataObject *pdo, DWORD dwAttributeMask, DWORD *pdwAttributes, UINT *pcItems );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "bdc583ef-a5b6-4665-949c-50f79ace39dc")]
		public static extern HRESULT SHGetAttributesFromDataObject(IDataObject pdo, SFGAO dwAttributeMask, out SFGAO pdwAttributes, out uint pcItems);

		/// <summary>Retrieves extended property data from a relative identifier list.</summary>
		/// <param name="psf">
		/// The address of the parent IShellFolder interface. This must be the immediate parent of the ITEMIDLIST structure referenced by the
		/// pidl parameter.
		/// </param>
		/// <param name="pidl">A pointer to an ITEMIDLIST structure that identifies the object relative to the folder specified in psf.</param>
		/// <param name="nFormat">The format in which the data is being requested.</param>
		/// <param name="pv">
		/// A pointer to a buffer that, when this function returns successfully, receives the requested data. The format of this buffer is
		/// determined by nFormat.
		/// <para>
		/// If nFormat is SHGDFIL_NETRESOURCE, there are two possible cases. If the buffer is large enough, the net resource's string
		/// information (fields for the network name, local name, provider, and comments) will be placed into the buffer. If the buffer is
		/// not large enough, only the net resource structure will be placed into the buffer and the string information pointers will be NULL.
		/// </para>
		/// </param>
		/// <param name="cb">Size of the buffer at pv, in bytes.</param>
		/// <remarks>
		/// This function extracts only information that is present in the pointer to an item identifier list (PIDL). Since the content of a
		/// PIDL depends on the folder object that created the PIDL, there is no guarantee that all requested information will be available.
		/// In addition, the information that is returned reflects the state of the object at the time the PIDL was created. The current
		/// state of the object could be different. For example, if you set nFormat to SHGDFIL_FINDDATA, the function might assign meaningful
		/// values to only some of the members of the WIN32_FIND_DATA structure. The remaining members will be set to zero. To retrieve
		/// complete current information on a file system file or folder, use standard file system functions such as GetFileTime or FindFirstFile.
		/// <para>
		/// E_INVALIDARG is returned if the psf, pidl, pv, or cb parameter does not match the nFormat parameter, or if nFormat is not one of
		/// the specific SHGDFIL_ values shown above.
		/// </para>
		/// </remarks>
		[DllImport(Lib.Shell32, CharSet = CharSet.Auto)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762174")]
		public static extern HRESULT SHGetDataFromIDList([In, MarshalAs(UnmanagedType.Interface)]
			IShellFolder psf, [In] PIDL pidl, SHGetDataFormat nFormat, [In, Out] IntPtr pv, int cb);

		/// <summary>Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's namespace.</summary>
		/// <param name="ppv">
		/// When this method returns, receives an IShellFolder interface pointer for the desktop folder. The calling application is
		/// responsible for eventually freeing the interface by calling its IUnknown::Release method.
		/// </param>
		/// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[DllImport(Lib.Shell32, CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762175")]
		public static extern HRESULT SHGetDesktopFolder([MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

		/// <summary>Deprecated. Retrieves the path of a folder as an ITEMIDLIST structure.</summary>
		/// <param name="hwndOwner">Reserved.</param>
		/// <param name="nFolder">
		/// A CSIDL value that identifies the folder to be located. The folders associated with the CSIDLs might not exist on a particular system.
		/// </param>
		/// <param name="hToken">
		/// An access token that can be used to represent a particular user. It is usually set to NULL, but it may be needed when there are
		/// multiple users for those folders that are treated as belonging to a single user. The most commonly used folder of this type is My
		/// Documents. The calling application is responsible for correct impersonation when hToken is non-NULL. It must have appropriate
		/// security privileges for the particular user, and the user's registry hive must be currently mounted.
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetFolderLocation to find
		/// folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user
		/// account is created, and includes special folders such as My Documents and Desktop. Any items added to the Default User folder
		/// also appear in any new user account.
		/// </para>
		/// </param>
		/// <param name="dwReserved">Reserved.</param>
		/// <param name="ppidl">
		/// The address of a pointer to an item identifier list structure that specifies the folder's location relative to the root of the
		/// namespace (the desktop). The ppidl parameter is set to NULL on failure. The calling application is responsible for freeing this
		/// resource by calling CoTaskMemFree.
		/// </param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762180")]
		public static extern HRESULT SHGetFolderLocation(IntPtr hwndOwner, int nFolder, SafeTokenHandle hToken,
			int dwReserved, out PIDL ppidl);

		/// <summary>
		/// Deprecated. Gets the path of a folder identified by a CSIDL value. <note>As of Windows Vista, this function is merely a wrapper
		/// for SHGetKnownFolderPath. The CSIDL value is translated to its associated KNOWNFOLDERID and then SHGetKnownFolderPath is called.
		/// New applications should use the known folder system rather than the older CSIDL system, which is supported only for backward compatibility.</note>
		/// </summary>
		/// <param name="hwndOwner">Reserved.</param>
		/// <param name="nFolder">
		/// A CSIDL value that identifies the folder whose path is to be retrieved. Only real folders are valid. If a virtual folder is
		/// specified, this function fails. You can force creation of a folder by combining the folder's CSIDL with CSIDL_FLAG_CREATE.
		/// </param>
		/// <param name="hToken">
		/// An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function
		/// requests the known folder for the current user.
		/// <para>
		/// Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has
		/// sufficient privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE
		/// rights. In some cases, you also need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of
		/// that specific user must be mounted. See Access Control for further discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find
		/// folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user
		/// account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also
		/// appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <param name="dwFlags">
		/// Flags that specify the path to be returned. This value is used in cases where the folder associated with a KNOWNFOLDERID (or
		/// CSIDL) can be moved, renamed, redirected, or roamed across languages by a user or administrator.
		/// <para>
		/// The known folder system that underlies SHGetFolderPath allows users or administrators to redirect a known folder to a location
		/// that suits their needs. This is achieved by calling IKnownFolderManager::Redirect, which sets the "current" value of the folder
		/// associated with the SHGFP_TYPE_CURRENT flag.
		/// </para>
		/// <para>
		/// The default value of the folder, which is the location of the folder if a user or administrator had not redirected it elsewhere,
		/// is retrieved by specifying the SHGFP_TYPE_DEFAULT flag. This value can be used to implement a "restore defaults" feature for a
		/// known folder.
		/// </para>
		/// <para>
		/// For example, the default value (SHGFP_TYPE_DEFAULT) for FOLDERID_Music (CSIDL_MYMUSIC) is "C:\Users\user name\Music". If the
		/// folder was redirected, the current value (SHGFP_TYPE_CURRENT) might be "D:\Music". If the folder has not been redirected, then
		/// SHGFP_TYPE_DEFAULT and SHGFP_TYPE_CURRENT retrieve the same path.
		/// </para>
		/// </param>
		/// <param name="pszPath">
		/// A pointer to a null-terminated string of length MAX_PATH which will receive the path. If an error occurs or S_FALSE is returned,
		/// this string will be empty. The returned path does not include a trailing backslash. For example, "C:\Users" is returned rather
		/// than "C:\Users\".
		/// </param>
		[DllImport(Lib.Shell32, CharSet = CharSet.Auto)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762181")]
		public static extern HRESULT SHGetFolderPath(IntPtr hwndOwner, int nFolder, [In, Optional] SafeTokenHandle hToken,
			SHGFP dwFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath);

		/// <summary>
		/// <para>Gets the path of a folder and appends a user-provided subfolder path.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>Reserved.</para>
		/// </param>
		/// <param name="csidl">
		/// <para>Type: <c>int</c></para>
		/// <para>
		/// A CSIDL value that identifies the folder whose path is to be retrieved. Only real folders are valid. If a virtual folder is
		/// specified, this function fails. You can force creation of a folder with <c>SHGetFolderPathAndSubDir</c> by combining the folder's
		/// <c>CSIDL</c> with CSIDL_FLAG_CREATE.
		/// </para>
		/// </param>
		/// <param name="hToken">
		/// <para>Type: <c>HANDLE</c></para>
		/// <para>
		/// An access token that represents a particular user. For systems earlier than Windows 2000, set this value to <c>NULL</c>. For
		/// later systems, is usually, but not always, set to <c>NULL</c>. You might need to assign a value to for those folders that can
		/// have multiple users but are treated as belonging to a single user. The most commonly used folder of this type is My Documents.
		/// </para>
		/// </param>
		/// <param name="dwFlags">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>
		/// Specifies whether the path to be returned is the actual path of the folder or the default path. This value is used in cases where
		/// the folder associated with a CSIDL value may be moved or renamed by the user.
		/// </para>
		/// <para>SHGFP_TYPE_CURRENT</para>
		/// <para>Return the folder's current path.</para>
		/// <para>SHGFP_TYPE_DEFAULT</para>
		/// <para>Return the folder's default path.</para>
		/// </param>
		/// <param name="pszSubDir">
		/// <para>Type: <c>LPCTSTR</c></para>
		/// <para>
		/// A pointer to the subpath to be appended to the folder's path. This is a <c>null</c>-terminated string of length MAX_PATH. If you
		/// are not creating a new directory, this must be an existing subdirectory or the function returns an error. This value can be
		/// <c>NULL</c> if no subpath is to be appended.
		/// </para>
		/// </param>
		/// <param name="pszPath">
		/// <para>Type: <c>LPTSTR</c></para>
		/// <para>
		/// When this function returns, this value points to the directory path and appended subpath. This is a <c>null</c>-terminated string
		/// of length MAX_PATH. This string is empty when the function returns an error code.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetfolderpathandsubdira HRESULT
		// SHGetFolderPathAndSubDirA( HWND hwnd, int csidl, HANDLE hToken, DWORD dwFlags, LPCSTR pszSubDir, LPSTR pszPath );
		[DllImport(Lib.Shell32, SetLastError = false, CharSet = CharSet.Auto)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "7e92e136-1036-4c96-931f-6e0129fb839a")]
		public static extern HRESULT SHGetFolderPathAndSubDirA(HandleRef hwnd, int csidl, SafeTokenHandle hToken, SHGFP dwFlags, string pszSubDir, StringBuilder pszPath);

		/// <summary>
		/// Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID. This extends SHGetKnownFolderPath by allowing
		/// you to set the initial size of the string buffer.
		/// </summary>
		/// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder.</param>
		/// <param name="dwFlags">
		/// Flags that specify special retrieval options. This value can be 0; otherwise, one or more of the KNOWN_FOLDER_FLAG values.
		/// </param>
		/// <param name="hToken">
		/// An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function
		/// requests the known folder for the current user.
		/// <para>
		/// Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has
		/// sufficient privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE
		/// rights. In some cases, you also need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of
		/// that specific user must be mounted. See Access Control for further discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find
		/// folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user
		/// account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also
		/// appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <param name="pszPath">
		/// A null-terminated, Unicode string. This buffer must be of size cchPath. When SHGetFolderPathEx returns successfully, this
		/// parameter contains the path for the known folder.
		/// </param>
		/// <param name="cchPath">The size of the ppszPath buffer, in characters.</param>
		[DllImport(Lib.Shell32, CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[PInvokeData("Shlobj.h", MSDNShortId = "mt757093")]
		public static extern HRESULT SHGetFolderPathEx([In, MarshalAs(UnmanagedType.LPStruct)]
			Guid rfid, KNOWN_FOLDER_FLAG dwFlags, [In, Optional] SafeTokenHandle hToken,
			[Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath, uint cchPath);

		/// <summary>
		/// <para>Returns the index of the overlay icon in the system image list.</para>
		/// </summary>
		/// <param name="pszIconPath">
		/// <para>Type: <c>LPCTSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated string of maximum length <c>MAX_PATH</c> that contains the fully qualified path of the file that
		/// contains the icon.
		/// </para>
		/// </param>
		/// <param name="iIconIndex">
		/// <para>Type: <c>int</c></para>
		/// <para>
		/// The icon's index in the file pointed to by . To request a standard overlay icon, set to <c>NULL</c>, and to one of the following:
		/// </para>
		/// <para>IDO_SHGIOI_SHARE (0x0FFFFFFF)</para>
		/// <para>The overlay icon that indicates a shared folder.</para>
		/// <para>IDO_SHGIOI_LINK (0x0FFFFFFE)</para>
		/// <para>The overlay icon that indicates a linked folder or file.</para>
		/// <para>IDO_SHGIOI_SLOWFILE (0x0FFFFFFD)</para>
		/// <para>The overlay icon that indicates a slow file.</para>
		/// <para>IDO_SHGIOI_DEFAULT (0x0FFFFFFC)</para>
		/// <para>
		/// <c>Windows 7 and later</c>. The overlay icon that indicates that the item is the default in a set. One example is the default printer.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns the index of the overlay icon in the system image list if successful, or -1 otherwise.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// Icon overlays are part of the system image list. They have two identifiers. The first is a one-based overlay index that
		/// identifies the overlay relative to other overlays in the image list. The other is an image index that identifies the actual
		/// image. These two indexes are equivalent to the values that you assign to the and parameters, respectively, when you add an icon
		/// overlay to a private image list with ImageList_SetOverlayImage. <c>SHGetIconOverlayIndex</c> returns the overlay index. To
		/// convert an overlay index to its equivalent image index, call INDEXTOOVERLAYMASK.
		/// </para>
		/// <para>
		/// <c>Note</c> After the image has been loaded into the system image list during initialization, it cannot be changed. The file name
		/// and index specified by and are used only to identify the icon overlay. <c>SHGetIconOverlayIndex</c> cannot be used to modify the
		/// system image list.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgeticonoverlayindexa int SHGetIconOverlayIndexA(
		// LPCSTR pszIconPath, int iIconIndex );
		[DllImport(Lib.Shell32, SetLastError = false, CharSet = CharSet.Auto)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "20001ae0-05d0-46a7-8bb8-9bb722f5d795")]
		public static extern int SHGetIconOverlayIndex(string pszIconPath, int iIconIndex);

		/// <summary>Retrieves the pointer to an item identifier list (PIDL) of an object.</summary>
		/// <param name="iUnknown">A pointer to the IUnknown of the object from which to get the PIDL.</param>
		/// <param name="ppidl">When this function returns, contains a pointer to the PIDL of the given object.</param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762184")]
		public static extern HRESULT SHGetIDListFromObject([MarshalAs(UnmanagedType.IUnknown)] object iUnknown,
			out PIDL ppidl);

		/// <summary>Retrieves an image list.</summary>
		/// <param name="iImageList">The image type contained in the list.</param>
		/// <param name="riid">Reference to the image list interface identifier, normally IID_IImageList.</param>
		/// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically IImageList.</param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762185")]
		public static extern HRESULT SHGetImageList(SHIL iImageList, [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
			out IImageList ppv);

		/// <summary>
		/// <para>
		/// Retrieves an interface that allows hosted Shell extensions and other components to prevent their host process from closing
		/// prematurely. The host process is typically Windows Explorer or Windows Internet Explorer, but this function can also be used by
		/// other applications.
		/// </para>
		/// </summary>
		/// <param name="ppunk">
		/// <para>Type: <c>IUnknown**</c></para>
		/// <para>
		/// When this function returns successfully, contains the address of the host process' IUnknown interface pointer. This is a
		/// free-threaded interface used to prevent the host process from terminating. If the function call fails, this value is set to <c>NULL</c>.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// There are a number of components, such as Shell extension handlers, that are implemented as DLLs and run in a host process such
		/// as Windows Explorer (Explorer.exe) or Internet Explorer (Iexplore.exe). Typically, when the user closes the host process, the
		/// component is shut down immediately as well. Such an abrupt termination can create problems for some components. For example, if a
		/// component is using a background thread to download data or run user-interface functions, it might need additional time to safely
		/// shut itself down.
		/// </para>
		/// <para>
		/// <c>SHGetInstanceExplorer</c> allows components that run in a host process to hold a reference on the host process.
		/// <c>SHGetInstanceExplorer</c> increments the host's reference count and returns a pointer to the host's IUnknown interface. By
		/// holding that reference, a component can prevent the host process from closing prematurely. After the component has completed its
		/// necessary processing, it should call (*ppunk)-&gt;Release to release the host's reference and allow the process to terminate.
		/// </para>
		/// <para>
		/// <c>Note</c> If <c>SHGetInstanceExplorer</c> is successful, the component must release the host's reference when it is no longer
		/// needed. Otherwise, all resources associated with the process will remain in memory. The IUnknown interface pointed to by * can
		/// only be used to release this reference. Components cannot use (*ppunk)-&gt;QueryInterface to request other interface pointers.
		/// </para>
		/// <para>SHGetInstanceExplorer</para>
		/// <para>succeeds only if it is called from from an application which had previously called</para>
		/// <para>SHSetInstanceExplorer</para>
		/// <para>to set a process reference.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetinstanceexplorer SHSTDAPI
		// SHGetInstanceExplorer( IUnknown **ppunk );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "ac6d8f7d-2eae-4b22-b493-b4ef740e3c95")]
		public static extern HRESULT SHGetInstanceExplorer([MarshalAs(UnmanagedType.IUnknown)] out object ppunk);

		/// <summary>Retrieves the path of a known folder as an ITEMIDLIST structure.</summary>
		/// <param name="rfid">
		/// A reference to the KNOWNFOLDERID that identifies the folder. The folders associated with the known folder IDs might not exist on
		/// a particular system.
		/// </param>
		/// <param name="dwFlags">
		/// Flags that specify special retrieval options. This value can be 0; otherwise, it is one or more of the KNOWN_FOLDER_FLAG values.
		/// </param>
		/// <param name="hToken">
		/// An access token used to represent a particular user. This parameter is usually set to NULL, in which case the function tries to
		/// access the current user's instance of the folder. However, you may need to assign a value to hToken for those folders that can
		/// have multiple users but are treated as belonging to a single user. The most commonly used folder of this type is Documents.
		/// <para>
		/// The calling application is responsible for correct impersonation when hToken is non-null. It must have appropriate security
		/// privileges for the particular user, including TOKEN_QUERY and TOKEN_IMPERSONATE, and the user's registry hive must be currently
		/// mounted. See Access Control for further discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderIDList to find
		/// folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user
		/// account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also
		/// appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <param name="ppidl">
		/// When this method returns, contains a pointer to the PIDL of the folder. This parameter is passed uninitialized. The caller is
		/// responsible for freeing the returned PIDL when it is no longer needed by calling ILFree.
		/// </param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762187")]
		public static extern HRESULT SHGetKnownFolderIDList([In, MarshalAs(UnmanagedType.LPStruct)]
			Guid rfid, KNOWN_FOLDER_FLAG dwFlags, [In, Optional] SafeTokenHandle hToken, out PIDL ppidl);

		/// <summary>Retrieves an IShellItem object that represents a known folder.</summary>
		/// <param name="rfid">A reference to the KNOWNFOLDERID, a GUID that identifies the folder that contains the item.</param>
		/// <param name="dwFlags">
		/// Flags that specify special options used in the retrieval of the known folder IShellItem. This value can be KF_FLAG_DEFAULT;
		/// otherwise, one or more of the KNOWN_FOLDER_FLAG values.
		/// </param>
		/// <param name="hToken">
		/// An access token used to represent a particular user. This parameter is usually set to NULL, in which case the function tries to
		/// access the current user's instance of the folder. However, you may need to assign a value to hToken for those folders that can
		/// have multiple users but are treated as belonging to a single user. The most commonly used folder of this type is Documents.
		/// <para>
		/// The calling application is responsible for correct impersonation when hToken is non-null. It must have appropriate security
		/// privileges for the particular user, including TOKEN_QUERY and TOKEN_IMPERSONATE, and the user's registry hive must be currently
		/// mounted. See Access Control for further discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderIDList to find
		/// folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user
		/// account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also
		/// appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <param name="riid">A reference to the IID of the interface that represents the item, usually IID_IShellItem or IID_IShellItem2.</param>
		/// <param name="ppv">When this method returns, contains the interface pointer requested in riid.</param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "dd378429")]
		public static extern HRESULT SHGetKnownFolderItem([In, MarshalAs(UnmanagedType.LPStruct)]
			Guid rfid, KNOWN_FOLDER_FLAG dwFlags, [In, Optional] SafeTokenHandle hToken,
			[In, MarshalAs(UnmanagedType.LPStruct)]
			Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		/// <summary>Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.</summary>
		/// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder.</param>
		/// <param name="dwFlags">
		/// Flags that specify special retrieval options. This value can be 0; otherwise, one or more of the KNOWN_FOLDER_FLAG values.
		/// </param>
		/// <param name="hToken">
		/// An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function
		/// requests the known folder for the current user.
		/// <para>
		/// Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has
		/// sufficient privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE
		/// rights. In some cases, you also need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of
		/// that specific user must be mounted. See Access Control for further discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find
		/// folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user
		/// account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also
		/// appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <param name="pszPath">
		/// When this method returns, contains the address of a pointer to a null-terminated Unicode string that specifies the path of the
		/// known folder. The calling process is responsible for freeing this resource once it is no longer needed by calling CoTaskMemFree.
		/// The returned path does not include a trailing backslash. For example, "C:\Users" is returned rather than "C:\Users\".
		/// </param>
		/// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
		/// <remarks>This function replaces SHGetFolderPath. That older function is now simply a wrapper for SHGetKnownFolderPath.</remarks>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762188")]
		public static extern HRESULT SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
			KNOWN_FOLDER_FLAG dwFlags, [In, Optional] SafeTokenHandle hToken, out SafeCoTaskMemHandle pszPath);

		/// <summary>Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.</summary>
		/// <param name="id">A reference to the KNOWNFOLDERID that identifies the folder.</param>
		/// <param name="dwFlags">
		/// Flags that specify special retrieval options. This value can be 0; otherwise, one or more of the KNOWN_FOLDER_FLAG values.
		/// </param>
		/// <param name="hToken">
		/// An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function
		/// requests the known folder for the current user.
		/// <para>
		/// Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has
		/// sufficient privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE
		/// rights. In some cases, you also need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of
		/// that specific user must be mounted. See Access Control for further discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find
		/// folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user
		/// account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also
		/// appear in any new user account. Note that access to the Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <returns>String that specifies the path of the known folder.</returns>
		/// <remarks>This function replaces SHGetFolderPath. That older function is now simply a wrapper for SHGetKnownFolderPath.</remarks>
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762188")]
		public static string SHGetKnownFolderPath(KNOWNFOLDERID id, KNOWN_FOLDER_FLAG dwFlags, SafeTokenHandle hToken = null)
		{
			SHGetKnownFolderPath(id.Guid(), dwFlags, hToken ?? SafeTokenHandle.Null, out SafeCoTaskMemHandle path);
			return path.ToString(-1);
		}

		/// <summary>Retrieves the display name of an item identified by its IDList.</summary>
		/// <param name="pidl">A PIDL that identifies the item.</param>
		/// <param name="sigdnName">A value from the SIGDN enumeration that specifies the type of display name to retrieve.</param>
		/// <param name="ppszName">
		/// A value that, when this function returns successfully, receives the address of a pointer to the retrieved display name.
		/// </param>
		[DllImport(Lib.Shell32, ExactSpelling = true, CharSet = CharSet.Unicode)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762191")]
		public static extern HRESULT SHGetNameFromIDList(PIDL pidl, SIGDN sigdnName, out SafeCoTaskMemHandle ppszName);

		/// <summary>Converts an item identifier list to a file system path.</summary>
		/// <param name="pidl">
		/// The address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).
		/// </param>
		/// <param name="pszPath">
		/// The address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.
		/// </param>
		/// <returns>Returns TRUE if successful; otherwise, FALSE.</returns>
		[DllImport(Lib.Shell32, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762194")]
		public static extern bool SHGetPathFromIDList(PIDL pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

		/// <summary>
		/// <para>
		/// Converts an item identifier list to a file system path. This function extends SHGetPathFromIDList by allowing you to set the
		/// initial size of the string buffer and declare the options below.
		/// </para>
		/// </summary>
		/// <param name="pidl">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>
		/// A pointer to an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).
		/// </para>
		/// </param>
		/// <param name="pszPath">
		/// <para>Type: <c>PWSTR</c></para>
		/// <para>
		/// When this function is called it is passed a null-terminated, Unicode buffer to receive the file system path. This buffer is of
		/// size .
		/// </para>
		/// <para>
		/// When this function returns, contains the address of a null-terminated, Unicode buffer that contains the file system path. This
		/// buffer is of size .
		/// </para>
		/// </param>
		/// <param name="cchPath">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>The size of the buffer pointed to by , in characters.</para>
		/// </param>
		/// <param name="uOpts">
		/// <para>Type: <c>GPFIDL_FLAGS</c></para>
		/// <para>These flags determine the type of path returned.</para>
		/// <para>GPFIDL_DEFAULT (0x0000)</para>
		/// <para>Win32 file names, servers, and root drives are included.</para>
		/// <para>GPFIDL_ALTNAME (0x0001)</para>
		/// <para>Uses short file names.</para>
		/// <para>GPFIDL_UNCPRINTER (0x0002)</para>
		/// <para>Include UNC printer names items.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para>Returns <c>TRUE</c> if successful; otherwise, <c>FALSE</c>.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// Except for UNC printer names, if the location specified by the parameter is not part of the file system, this function fails.
		/// </para>
		/// <para>If the parameter specifies a shortcut, the contains the path to the shortcut, not to the shortcut's target.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetpathfromidlistex BOOL SHGetPathFromIDListEx(
		// PCIDLIST_ABSOLUTE pidl, PWSTR pszPath, DWORD cchPath, GPFIDL_FLAGS uOpts );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "80270c59-275d-4b13-b16c-0c07bb79ed8e")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHGetPathFromIDListEx(PIDL pidl, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath, uint cchPath, GPFIDL_FLAGS uOpts);

		/// <summary>
		/// <para>
		/// [SHGetRealIDL is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Converts a simple pointer to an item identifier list (PIDL) into a full PIDL.</para>
		/// </summary>
		/// <param name="psf">
		/// <para>Type: <c>IShellFolder*</c></para>
		/// <para>A pointer to an instance of IShellFolder whose simple PIDL is to be converted.</para>
		/// </param>
		/// <param name="pidlSimple">
		/// <para>Type: <c>PCUITEMID_CHILD</c></para>
		/// <para>The simple PIDL to be converted.</para>
		/// </param>
		/// <param name="ppidlReal">
		/// <para>Type: <c>PITEMID_CHILD*</c></para>
		/// <para>
		/// When this method returns, contains a pointer to the full converted PIDL. If the function fails, this parameter is set to <c>NULL</c>.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetrealidl SHSTDAPI SHGetRealIDL( IShellFolder
		// *psf, PCUITEMID_CHILD pidlSimple, PITEMID_CHILD *ppidlReal );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "0c0b63c9-7ca7-4f73-be74-9c492f8506fc")]
		public static extern HRESULT SHGetRealIDL(IShellFolder psf, PIDL pidlSimple, out IntPtr ppidlReal);

		/// <summary>
		/// <para>
		/// [SHGetSetFolderCustomSettings is available for use in the operating systems specified in the Requirements section. It may be
		/// altered or unavailable in subsequent versions.]
		/// </para>
		/// <para>Sets or retrieves custom folder settings. This function reads from and writes to Desktop.ini.</para>
		/// </summary>
		/// <param name="pfcs">
		/// <para>Type: <c>LPSHFOLDERCUSTOMSETTINGS</c></para>
		/// <para>A pointer to a SHFOLDERCUSTOMSETTINGS structure that provides or receives the custom folder settings.</para>
		/// </param>
		/// <param name="pszPath">
		/// <para>Type: <c>PCTSTR</c></para>
		/// <para>
		/// A pointer to a null-terminated Unicode string that contains the path to the folder. The length of <c>pszPath</c> must be MAX_PATH
		/// or less, including the terminating null character.
		/// </para>
		/// </param>
		/// <param name="dwReadWrite">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>A flag that controls the action of the function. It may be one of the following values.</para>
		/// <para>FCS_READ (0x00000001)</para>
		/// <para>Retrieve the custom folder settings in .</para>
		/// <para>FCS_FORCEWRITE (0x00000002)</para>
		/// <para>Use to set the custom folder's settings regardless of whether the values are already present.</para>
		/// <para>FCS_WRITE (FCS_READ | FCS_FORCEWRITE)</para>
		/// <para>Use to set the custom folder's settings if the values are not already present.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		/// <remarks>
		/// <para>Only Unicode strings are supported.</para>
		/// <para><c>Windows Server 2003 and Windows XP:</c><c>SHGetSetFolderCustomSettings</c> supports both ANSI and Unicode strings.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetsetfoldercustomsettings SHSTDAPI
		// SHGetSetFolderCustomSettings( LPSHFOLDERCUSTOMSETTINGS pfcs, PCWSTR pszPath, DWORD dwReadWrite );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true, CharSet = CharSet.Unicode)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "38b78a4b-ba68-4dff-812d-d4c7421eb202")]
		public static extern HRESULT SHGetSetFolderCustomSettings(ref SHFOLDERCUSTOMSETTINGS pfcs, [MarshalAs(UnmanagedType.LPWStr)] string pszPath, FCS dwReadWrite);

		/// <summary>
		/// <para>
		/// [SHGetSetSettings is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Sets or retrieves Shell state settings.</para>
		/// </summary>
		/// <param name="lpss">
		/// <para>Type: <c>LPSHELLSTATE</c></para>
		/// <para>A pointer to a SHELLSTATE structure that provides or receives the Shell state settings.</para>
		/// </param>
		/// <param name="dwMask">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>One or more of the SSF flags that indicate which settings should be set or retrieved.</para>
		/// </param>
		/// <param name="bSet">
		/// <para>Type: <c>BOOL</c></para>
		/// <para>
		/// <c>TRUE</c> to indicate that the contents of should be used to set the Shell settings, <c>FALSE</c> to indicate that the Shell
		/// settings should be retrieved to .
		/// </para>
		/// </param>
		/// <returns>
		/// <para>This function does not return a value.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetsetsettings void SHGetSetSettings(
		// LPSHELLSTATE lpss, DWORD dwMask, BOOL bSet );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "d7c2646c-03e0-4d7a-9503-bdf487d43723")]
		public static extern void SHGetSetSettings(ref SHELLSTATE lpss, SSF dwMask, [MarshalAs(UnmanagedType.Bool)] bool bSet);

		/// <summary>
		/// <para>Retrieves the current Shell option settings.</para>
		/// </summary>
		/// <param name="psfs">
		/// <para>TBD</para>
		/// </param>
		/// <param name="dwMask">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>A set of flags that determine which members of are being requested. This can be one or more of the following values.</para>
		/// <para>SSF_DESKTOPHTML</para>
		/// <para>The <c>fDesktopHTML</c> member is being requested.</para>
		/// <para>SSF_DONTPRETTYPATH</para>
		/// <para>The <c>fDontPrettyPath</c> member is being requested.</para>
		/// <para>SSF_DOUBLECLICKINWEBVIEW</para>
		/// <para>The <c>fDoubleClickInWebView</c> member is being requested.</para>
		/// <para>SSF_HIDEICONS</para>
		/// <para>The <c>fHideIcons</c> member is being requested.</para>
		/// <para>SSF_MAPNETDRVBUTTON</para>
		/// <para>The <c>fMapNetDrvBtn</c> member is being requested.</para>
		/// <para>SSF_NOCONFIRMRECYCLE</para>
		/// <para>The <c>fNoConfirmRecycle</c> member is being requested.</para>
		/// <para>SSF_SHOWALLOBJECTS</para>
		/// <para>The <c>fShowAllObjects</c> member is being requested.</para>
		/// <para>SSF_SHOWATTRIBCOL</para>
		/// <para>The <c>fShowAttribCol</c> member is being requested.</para>
		/// <para><c>Windows Vista:</c> Not used.</para>
		/// <para>SSF_SHOWCOMPCOLOR</para>
		/// <para>The <c>fShowCompColor</c> member is being requested.</para>
		/// <para>SSF_SHOWEXTENSIONS</para>
		/// <para>The <c>fShowExtensions</c> member is being requested.</para>
		/// <para>SSF_SHOWINFOTIP</para>
		/// <para>The <c>fShowInfoTip</c> member is being requested.</para>
		/// <para>SSF_SHOWSYSFILES</para>
		/// <para>The <c>fShowSysFiles</c> member is being requested.</para>
		/// <para>SSF_WIN95CLASSIC</para>
		/// <para>The <c>fWin95Classic</c> member is being requested.</para>
		/// </param>
		/// <returns>
		/// <para>This function does not return a value.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetsettings void SHGetSettings( SHELLFLAGSTATE
		// *psfs, DWORD dwMask );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "728a4004-f35d-4592-baf1-456a613a3344")]
		public static extern void SHGetSettings(ref SHELLFLAGSTATE psfs, SSF dwMask);

		/// <summary>
		/// <para>
		/// [SHHandleUpdateImage is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Handles the <c>SHCNE_UPDATEIMAGE</c> Shell change notification.</para>
		/// </summary>
		/// <param name="pidlExtra">
		/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
		/// <para>The index in the system image list that has changed, specified in the parameter of IShellChangeNotify::OnChange.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns -1 on failure or the index of the changed image list entry on success.</para>
		/// </returns>
		/// <remarks>
		/// <para>Use <c>SHHandleUpdateImage</c> only when the parameter received by your change notification callback is non- <c>NULL</c>.</para>
		/// <para>Examples</para>
		/// <para>The following example demonstrates the use of <c>SHHandleUpdateImage</c> in the implementation of IShellChangeNotify::OnChange.</para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shhandleupdateimage int SHHandleUpdateImage(
		// PCIDLIST_ABSOLUTE pidlExtra );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "9d43e28a-bce0-4da4-98c9-5a6a199b4d8e")]
		public static extern int SHHandleUpdateImage(PIDL pidlExtra);

		/// <summary>
		/// <para>
		/// [This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It might be altered or unavailable
		/// in subsequent versions of Windows.]
		/// </para>
		/// <para>Sets limits on valid characters for an edit control.</para>
		/// </summary>
		/// <param name="hwndEdit">
		/// <para>Type: <c>HWND</c></para>
		/// <para>The handle of the edit control.</para>
		/// </param>
		/// <param name="psf">
		/// <para>Type: <c>IShellFolder*</c></para>
		/// <para>
		/// An IShellFolder interface pointer. This object must also implement IItemNameLimits, which supplies a list of invalid characters
		/// and a maximum name length.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shlimitinputedit SHSTDAPI SHLimitInputEdit( HWND
		// hwndEdit, IShellFolder *psf );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "3f03374f-8dfe-4b80-9ecc-12c6548f2865")]
		public static extern HRESULT SHLimitInputEdit(HandleRef hwndEdit, IShellFolder psf);

		/// <summary>
		/// <para>Creates an instance of the specified object class from within the context of the Shell's process.</para>
		/// <para><c>Windows Vista</c> and later: This function has been disabled and returns E_NOTIMPL.</para>
		/// </summary>
		/// <param name="rclsid">
		/// <para>Type: <c>REFCLSID</c></para>
		/// <para>The CLSID of the object class to be created.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>Returns S_OK if successful, or an error value otherwise. In Windows Vista and later versions, always returns E_NOTIMPL.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// <c>Note</c> This function is available through Windows XP Service Pack 2 (SP2) and Windows Server 2003. It is not available in
		/// later versions of Windows, including Windows Vista.
		/// </para>
		/// <para>
		/// This function creates the requested object instance by calling the CoCreateInstance function and immediately releasing the
		/// returned object. The associated DLL is unloaded according to standard Component Object Model (COM) rules when it returns S_OK
		/// from its DllCanUnloadNow function.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shloadinproc SHSTDAPI SHLoadInProc( REFCLSID
		// rclsid );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "307b99d9-2d0a-47c5-8a10-dfdc0a408942")]
		public static extern HRESULT SHLoadInProc([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid);

		/// <summary>
		/// <para>
		/// [ <c>SHMapPIDLToSystemImageListIndex</c> is available for use in the operating systems specified in the Requirements section. It
		/// may be altered or unavailable in subsequent versions.]
		/// </para>
		/// <para>Retrieves the icon index from the system image list that is associated with a folder item.</para>
		/// </summary>
		/// <param name="psf">
		/// <para>Type: <c><c>IShellFolder</c>*</c></para>
		/// <para>An <c>IShellFolder</c> interface pointer for the folder that contains the item.</para>
		/// </param>
		/// <param name="pidl">
		/// <para>Type: <c>PCUITEMID_CHILD</c></para>
		/// <para>A pointer to the item's <c>ITEMIDLIST</c> structure.</para>
		/// </param>
		/// <param name="piIndex">
		/// <para>Type: <c>int*</c></para>
		/// <para>
		/// A pointer to an <c>int</c> that, when this function returns successfully, receives the index of the item's <c>open</c> icon in
		/// the system image list. If the item does not have a special <c>open</c> icon then the index of its normal icon is returned. If the
		/// <c>open</c> icon exists and cannot be obtained, then the value pointed to by piIndex is set to -1. This parameter can be
		/// <c>NULL</c> if the calling application is not interested in the <c>open</c> icon.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>int</c></para>
		/// <para>Returns the index of the item's normal icon in the system image list if successful, or -1 otherwise.</para>
		/// </returns>
		// int SHMapPIDLToSystemImageListIndex( _In_ IShellFolder *psf, _In_ PCUITEMID_CHILD pidl, _Out_opt_ int *piIndex); https://msdn.microsoft.com/en-us/library/windows/desktop/bb762219(v=vs.85).aspx
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("Shlobj_core.h", MSDNShortId = "bb762219")]
		public static extern int SHMapPIDLToSystemImageListIndex(IShellFolder psf, PIDL pidl, out int piIndex);

		/// <summary>
		/// <para>
		/// Displays a merged property sheet for a set of files. Property values common to all the files are shown while those that differ
		/// display the string <c>(multiple values)</c>.
		/// </para>
		/// </summary>
		/// <param name="pdtobj">
		/// <para>Type: <c>IDataObject*</c></para>
		/// <para>
		/// A pointer to a data object that supplies the PIDLs of all of the files for which to display the merged property sheet. The data
		/// object must use the CFSTR_SHELLIDLIST clipboard format. The parent folder's implementation of IShellFolder::GetDisplayNameOf must
		/// return a fully qualified file system path for each item in response to the SHGDN_FORPARSING flag.
		/// </para>
		/// </param>
		/// <param name="dwFlags">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>Reserved. Must be set to 0.</para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>HRESULT</c></para>
		/// <para>If this function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj/nf-shlobj-shmultifileproperties SHSTDAPI SHMultiFileProperties(
		// IDataObject *pdtobj, DWORD dwFlags );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("shlobj.h", MSDNShortId = "7c66fd91-4f7a-45f3-b849-bf210c552511")]
		public static extern HRESULT SHMultiFileProperties(IDataObject pdtobj, uint dwFlags = 0);

		/// <summary>
		/// <para>
		/// [SHObjectProperties is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>Invokes the <c>Properties</c> context menu command on a Shell object.</para>
		/// </summary>
		/// <param name="hwnd">
		/// <para>Type: <c>HWND</c></para>
		/// <para>The handle of the parent window of the dialog box. This value can be <c>NULL</c>.</para>
		/// </param>
		/// <param name="shopObjectType">
		/// <para>Type: <c>DWORD</c></para>
		/// <para>A flag value that specifies the type of object.</para>
		/// <para>SHOP_PRINTERNAME</para>
		/// <para>contains the friendly name of a printer.</para>
		/// <para>SHOP_FILEPATH</para>
		/// <para>contains a fully qualified file name.</para>
		/// <para>SHOP_VOLUMEGUID</para>
		/// <para>
		/// contains either (a) a volume name of the form \?\Volume{GUID}, where {GUID} is a globally unique identifier (for example,
		/// "\?\Volume{2eca078d-5cbc-43d3-aff8-7e8511f60d0e})", or (b) a drive path (for example, "C:").
		/// </para>
		/// </param>
		/// <param name="pszObjectName">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains the object name. The contents of the string are determined by the flag set in .
		/// </para>
		/// </param>
		/// <param name="pszPropertyPage">
		/// <para>Type: <c>PCWSTR</c></para>
		/// <para>
		/// A null-terminated Unicode string that contains the name of the property sheet page to be opened initially. Set this parameter to
		/// <c>NULL</c> to specify the default page.
		/// </para>
		/// </param>
		/// <returns>
		/// <para>Type: <c>BOOL</c></para>
		/// <para><c>TRUE</c> if the command is successfully invoked; otherwise, <c>FALSE</c>.</para>
		/// </returns>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shobjectproperties BOOL SHObjectProperties( HWND
		// hwnd, DWORD shopObjectType, PCWSTR pszObjectName, PCWSTR pszPropertyPage );
		[DllImport(Lib.Shell32, SetLastError = false, ExactSpelling = true, CharSet = CharSet.Unicode)]
		[PInvokeData("shlobj_core.h", MSDNShortId = "7517c461-955b-446e-85d7-a707c9bd183a")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHObjectProperties(HandleRef hwnd, SHOP shopObjectType, string pszObjectName, string pszPropertyPage);

		/// <summary>Opens a Windows Explorer window with specified items in a particular folder selected.</summary>
		/// <param name="pidlFolder">A pointer to a fully qualified item ID list that specifies the folder.</param>
		/// <param name="cidl">
		/// A count of items in the selection array, apidl. If cidl is zero, then pidlFolder must point to a fully specified ITEMIDLIST
		/// describing a single item to select. This function opens the parent folder and selects that item.
		/// </param>
		/// <param name="apidl">
		/// A pointer to an array of PIDL structures, each of which is an item to select in the target folder referenced by pidlFolder.
		/// </param>
		/// <param name="dwFlags">
		/// The optional flags. Under Windows XP this parameter is ignored. In Windows Vista, the following flags are defined.
		/// </param>
		[DllImport(Lib.Shell32, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762232")]
		public static extern HRESULT SHOpenFolderAndSelectItems(PIDL pidlFolder, uint cidl,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl, OFASI dwFlags);

		/// <summary>
		/// Translates a Shell namespace object's display name into an item identifier list and returns the attributes of the object. This
		/// function is the preferred method to convert a string to a pointer to an item identifier list (PIDL).
		/// </summary>
		/// <param name="pszName">A pointer to a zero-terminated wide string that contains the display name to parse.</param>
		/// <param name="pbc">A bind context that controls the parsing operation. This parameter is normally set to NULL.</param>
		/// <param name="ppidl">
		/// The address of a pointer to a variable of type ITEMIDLIST that receives the item identifier list for the object. If an error
		/// occurs, then this parameter is set to NULL.
		/// </param>
		/// <param name="sfgaoIn">
		/// A ULONG value that specifies the attributes to query. To query for one or more attributes, initialize this parameter with the
		/// flags that represent the attributes of interest. For a list of available SFGAO flags, see IShellFolder::GetAttributesOf.
		/// </param>
		/// <param name="psfgaoOut">
		/// A pointer to a ULONG. On return, those attributes that are true for the object and were requested in sfgaoIn are set. An object's
		/// attribute flags can be zero or a combination of SFGAO flags. For a list of available SFGAO flags, see IShellFolder::GetAttributesOf.
		/// </param>
		[DllImport(Lib.Shell32, CharSet = CharSet.Unicode, ExactSpelling = true)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb762236")]
		public static extern HRESULT SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string pszName,
			[In, Optional] IntPtr pbc, out PIDL ppidl, SFGAO sfgaoIn, out SFGAO psfgaoOut);

		/// <summary>
		/// <para>
		/// [CABINETSTATE is available for use in the operating systems specified in the Requirements section. It may be altered or
		/// unavailable in subsequent versions.]
		/// </para>
		/// <para>
		/// Holds the global configuration for Windows Explorer and Windows Internet Explorer. This structure is used in the ReadCabinetState
		/// and WriteCabinetState functions.
		/// </para>
		/// </summary>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/ns-shlobj_core-cabinetstate typedef struct CABINETSTATE { WORD
		// cLength; WORD nVersion; BOOL fFullPathTitle : 1; BOOL fSaveLocalView : 1; BOOL fNotShell : 1; BOOL fSimpleDefault : 1; BOOL
		// fDontShowDescBar : 1; BOOL fNewWindowMode : 1; BOOL fShowCompColor : 1; BOOL fDontPrettyNames : 1; BOOL fAdminsCreateCommonGroups
		// : 1; UINT fUnusedFlags : 7; UINT fMenuEnumFilter; } *LPCABINETSTATE;
		[PInvokeData("shlobj_core.h", MSDNShortId = "4b82b6a8-c4c0-4af2-9612-0551376c1c62")]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct CABINETSTATE
		{
			/// <summary>
			/// <para>Type: <c>WORD</c></para>
			/// <para>The size of the structure, in bytes.</para>
			/// </summary>
			public ushort cLength;

			/// <summary>
			/// <para>Type: <c>WORD</c></para>
			/// </summary>
			public ushort nVersion;

			private ushort fFlags;

			/// <summary>
			/// <para>Type: <c>UINT</c></para>
			/// <para>One or both of the following flags.</para>
			/// <para>SHCONTF_FOLDERS</para>
			/// <para>Display folders.</para>
			/// <para>SHCONTF_NONFOLDERS</para>
			/// <para>Display non-folder items.</para>
			/// </summary>
			public SHCONTF fMenuEnumFilter;

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>TRUE</para>
			/// <para>Display the full path in the title bar.</para>
			/// <para>FALSE</para>
			/// <para>Display only the file name in the title bar.</para>
			/// </summary>
			public bool fFullPathTitle { get => GetBit(ref fFlags, 0); set => SetBit(ref fFlags, 0, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>TRUE</para>
			/// <para>Remember each folder's view settings.</para>
			/// <para>FALSE</para>
			/// <para>Use global settings for all folders.</para>
			/// </summary>
			public bool fSaveLocalView { get => GetBit(ref fFlags, 1); set => SetBit(ref fFlags, 1, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fNotShell { get => GetBit(ref fFlags, 2); set => SetBit(ref fFlags, 2, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fSimpleDefault { get => GetBit(ref fFlags, 3); set => SetBit(ref fFlags, 3, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fDontShowDescBar { get => GetBit(ref fFlags, 4); set => SetBit(ref fFlags, 4, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>TRUE</para>
			/// <para>Display in a new window.</para>
			/// <para>FALSE</para>
			/// <para>Display in the current window.</para>
			/// </summary>
			public bool fNewWindowMode { get => GetBit(ref fFlags, 5); set => SetBit(ref fFlags, 5, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>TRUE</para>
			/// <para>Show encrypted or compressed NTFS files in color.</para>
			/// <para>FALSE</para>
			/// <para>Do not show encrypted or compressed NTFS files in color.</para>
			/// </summary>
			public bool fShowCompColor { get => GetBit(ref fFlags, 6); set => SetBit(ref fFlags, 6, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fDontPrettyNames { get => GetBit(ref fFlags, 7); set => SetBit(ref fFlags, 7, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Used when an administrator installs an application that places an icon in the <c>Start</c> menu.</para>
			/// <para>TRUE</para>
			/// <para>Add the icon to the <c>Start</c> menu for all users (CSIDL_COMMON_STARTMENU). This is the default value.</para>
			/// <para>FALSE</para>
			/// <para>Add the icon to only the current user (CSIDL_STARTMENU).</para>
			/// </summary>
			public bool fAdminsCreateCommonGroups { get => GetBit(ref fFlags, 8); set => SetBit(ref fFlags, 8, value); }
		}

		/// <summary>
		/// Defines the coordinates of a character cell in a console screen buffer. The origin of the coordinate system (0,0) is at the top,
		/// left cell of the buffer.
		/// </summary>
		[PInvokeData("wincon.h")]
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct COORD
		{
			/// <summary>The horizontal coordinate or column value. The units depend on the function call.</summary>
			public short X;

			/// <summary>The vertical coordinate or row value. The units depend on the function call.</summary>
			public short Y;
		}

		/// <summary>
		/// <para>Used with the SHCreateShellFolderViewEx function.</para>
		/// </summary>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/ns-shlobj_core-_csfv typedef struct _CSFV { UINT cbSize;
		// IShellFolder *pshf; IShellView *psvOuter; PCIDLIST_ABSOLUTE pidl; LONG lEvents; LPFNVIEWCALLBACK pfnCallback; FOLDERVIEWMODE fvm;
		// } CSFV, *LPCSFV;
		[PInvokeData("shlobj_core.h", MSDNShortId = "9ec22fd4-1562-4ef0-b932-ebbf06082807")]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct CSFV
		{
			/// <summary>
			/// <para>Type: <c>UINT</c></para>
			/// <para>The size of the <c>CSFV</c> structure, in bytes.</para>
			/// </summary>
			public uint cbSize;

			/// <summary>
			/// <para>Type: <c>IShellFolder*</c></para>
			/// <para>A pointer to the IShellFolder object for which to create the view.</para>
			/// </summary>
			public IShellFolder pshf;

			/// <summary>
			/// <para>Type: <c>IShellView*</c></para>
			/// <para>A pointer to the parent IShellView interface. This parameter can be <c>NULL</c>.</para>
			/// </summary>
			public IShellView psvOuter;

			/// <summary>
			/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
			/// <para>Ignored.</para>
			/// </summary>
			public IntPtr pidl;

			/// <summary>
			/// <para>Type: <c>LONG</c></para>
			/// </summary>
			public int lEvents;

			/// <summary>
			/// <para>Type: <c>LPFNVIEWCALLBACK</c></para>
			/// <para>
			/// A pointer to the LPFNVIEWCALLBACK function used by this folder view to handle callback messages. This parameter can be <c>NULL</c>.
			/// </para>
			/// </summary>
			public LPFNVIEWCALLBACK pfnCallback;

			/// <summary>
			/// <para>Type: <c>FOLDERVIEWMODE</c></para>
			/// </summary>
			public FOLDERVIEWMODE fvm;
		}

		/// <summary>Serves as the header for some of the extra data structures used by IShellLinkDataList.</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb773249")]
		public struct DATABLOCKHEADER
		{
			/// <summary>The size of the extra data block.</summary>
			public uint cbSize;

			/// <summary>A signature that identifies the type of data block that follows the header.</summary>
			public ShellDataBlockSignature dwSignature;
		}

		/// <summary>Contains context menu information used by SHCreateDefaultContextMenu.</summary>
		[StructLayout(LayoutKind.Sequential)]
		[PInvokeData("shlobj_core.h")]
		public struct DEFCONTEXTMENU
		{
			/// <summary>A handle to the context menu. Set this member to the handle returned from CreateMenu.</summary>
			public IntPtr hwnd;

			/// <summary>
			/// A pointer to the IContextMenuCB interface supported by the callback object. This value is optional and can be NULL.
			/// </summary>
			public IContextMenuCB pcmcb;

			/// <summary>
			/// The PIDL of the folder that contains the selected file object(s) or the folder of the context menu if no file objects are
			/// selected. This value is optional and can be NULL, in which case the PIDL is computed from the psf member.
			/// </summary>
			public IntPtr pidlFolder;

			/// <summary>
			/// A pointer to the IShellFolder interface of the folder object that contains the selected file objects, or the folder that
			/// contains the context menu if no file objects are selected.
			/// </summary>
			public IShellFolder psf;

			/// <summary>The count of items in member apidl.</summary>
			public uint cidl;

			/// <summary>
			/// A pointer to a constant array of ITEMIDLIST structures. Each entry in the array describes a child item to which the context
			/// menu applies, for instance, a selected file the user wants to Open.
			/// </summary>
			public IntPtr apidl;

			/// <summary>
			/// A pointer to the IQueryAssociations interface on the object from which to load extensions. This parameter is optional and
			/// thus can be NULL. If this value is NULL and members aKeys and cKeys are also NULL (see Remarks), punkAssociationInfo is
			/// computed from the apidl member and cidl via a request for IQueryAssociations through IShellFolder::GetUIObjectOf. If
			/// IShellFolder::GetUIObjectOf returns E_NOTIMPL, a default implementation is provided based on the SFGAO_FOLDER and
			/// SFGAO_FILESYSTEM attributes returned from IShellFolder::GetAttributesOf.
			/// </summary>
			public IQueryAssociations punkAssociationInfo;

			/// <summary> The count of items in member aKeys. This value can be zero. If the value is zero, the extensions are loaded based
			/// on the object that supports interface IQueryAssociations as specified by member punkAssociationInfo. If the value is
			/// non-NULL, the extensions are loaded based only on member aKeys and not member punkAssociationInfo.
			// Note The maximum number of keys is 16. Callers must enforce this limit as the API does not. Failing to do so can result in
			// memory corruption.
			/// </summary>
			public uint cKeys;

			/// <summary>
			/// A pointer to an HKEY that specifies the registry key from which to load extensions. This parameter is optional and can be
			/// NULL. If the value is NULL, the extensions are loaded based on the object that supports interface IQueryAssociations as
			/// specified in punkAssociationInfo.
			/// </summary>
			public IntPtr aKeys;
		}

		/// <summary>Holds an extra data block used by IShellLinkDataList. It holds the link's Windows Installer ID.</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb773274")]
		public struct EXP_DARWIN_LINK
		{
			/// <summary>
			/// DATABLOCK_HEADER structure stating the size and signature of the EXP_DARWIN_LINK structure. The following is the only
			/// recognized signature value: EXP_DARWIN_ID_SIG
			/// </summary>
			public DATABLOCKHEADER dbh;

			/// <summary>The link's ID in the form of an ANSI string.</summary>
			[MarshalAs(UnmanagedType.LPStr, SizeConst = MAX_PATH)]
			public string szDarwinID;

			/// <summary>The link's ID in the form of an Unicode string.</summary>
			[MarshalAs(UnmanagedType.LPWStr, SizeConst = MAX_PATH)]
			public string szwDarwinID;
		}

		/// <summary>Holds an extra data block used by IShellLinkDataList. It holds special folder information.</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb773279")]
		public struct EXP_SPECIAL_FOLDER
		{
			/// <summary>The size of the EXP_SPECIAL_FOLDER structure.</summary>
			public uint cbSize;

			/// <summary>The structure's signature. It should be set to EXP_SPECIAL_FOLDER_SIG.</summary>
			public ShellDataBlockSignature dwSignature;

			/// <summary>The ID of the special folder that the link points into.</summary>
			public uint idSpecialFolder;

			/// <summary>The offset into the saved PIDL.</summary>
			public uint cbOffset;
		}

		/// <summary>Holds an extra data block used by IShellLinkDataList. It holds expandable environment strings for the icon or target.</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb773282")]
		public struct EXP_SZ_LINK
		{
			/// <summary>The size of the EXP_SZ_LINK structure.</summary>
			public uint cbSize;

			/// <summary>
			/// The structure's signature. It can have one of the following values: EXP_SZ_LINK_SIG = Contains the link's target path;
			/// EXP_SZ_ICON_SIG = Contains the links icon path.
			/// </summary>
			public ShellDataBlockSignature dwSignature;

			/// <summary>The null-terminated ANSI string with the path of the target or icon.</summary>
			[MarshalAs(UnmanagedType.LPStr, SizeConst = MAX_PATH)]
			public string szTarget;

			/// <summary>The null-terminated Unicode string with the path of the target or icon.</summary>
			[MarshalAs(UnmanagedType.LPWStr, SizeConst = MAX_PATH)]
			public string swzTarget;
		}

		/// <summary>Holds an extra data block used by IShellLinkDataList. It holds console properties.</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb773359")]
		public struct NT_CONSOLE_PROPS
		{
			/// <summary>
			/// The DATABLOCK_HEADER structure with the NT_CONSOLE_PROPS structure's size and signature. The signature for an
			/// NT_CONSOLE_PROPS structure is NT_CONSOLE_PROPS_SIG.
			/// </summary>
			public DATABLOCKHEADER dbh;

			/// <summary>Fill attribute for the console.</summary>
			public ushort wFillAttribute;

			/// <summary>Fill attribute for console pop-ups.</summary>
			public ushort wPopupFillAttribute;

			/// <summary>A COORD structure with the console's screen buffer size.</summary>
			public COORD dwScreenBufferSize;

			/// <summary>A COORD structure with the console's window size.</summary>
			public COORD dwWindowSize;

			/// <summary>A COORD structure with the console's window origin.</summary>
			public COORD dwWindowOrigin;

			/// <summary>The font.</summary>
			public uint nFont;

			/// <summary>The input buffer size.</summary>
			public uint nInputBufferSize;

			/// <summary>A COORD structure with the font size.</summary>
			public COORD dwFontSize;

			/// <summary>The font family/</summary>
			public uint uFontFamily;

			/// <summary>The font weight.</summary>
			public uint uFontWeight;

			/// <summary>A character array that contains the font's face name.</summary>
			[MarshalAs(UnmanagedType.LPWStr, SizeConst = LF_FACESIZE)]
			public string FaceName;

			/// <summary>The cursor size.</summary>
			public uint uCursorSize;

			/// <summary>A boolean value that is set to TRUE if the console is in full-screen mode, or FALSE otherwise.</summary>
			[MarshalAs(UnmanagedType.Bool)] public bool bFullScreen;

			/// <summary>A boolean value that is set to TRUE if the console is in quick-edit mode, or FALSE otherwise.</summary>
			[MarshalAs(UnmanagedType.Bool)] public bool bQuickEdit;

			/// <summary>A boolean value that is set to TRUE if the console is in insert mode, or FALSE otherwise.</summary>
			[MarshalAs(UnmanagedType.Bool)] public bool bInsertMode;

			/// <summary>A boolean value that is set to TRUE if the console is in auto-position mode, or FALSE otherwise.</summary>
			[MarshalAs(UnmanagedType.Bool)] public bool bAutoPosition;

			/// <summary>The size of the history buffer.</summary>
			public uint uHistoryBufferSize;

			/// <summary>The number of history buffers.</summary>
			public uint uNumberOfHistoryBuffers;

			/// <summary>A boolean value that is set to TRUE if old duplicate history lists should be discarded, or FALSE otherwise.</summary>
			[MarshalAs(UnmanagedType.Bool)] public bool bHistoryNoDup;

			/// <summary>An array of COLORREF values with the console's color settings.</summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public uint[] ColorTable;
		}

		/// <summary>Holds an extra data block used by IShellLinkDataList. It holds the console's code page.</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb773362")]
		public struct NT_FE_CONSOLE_PROPS
		{
			/// <summary>
			/// The DATABLOCK_HEADER structure with the NT_FE_CONSOLE_PROPS structure's size and signature. The signature for an
			/// NT_FE_CONSOLE_PROPS structure is NT_FE_CONSOLE_PROPS_SIG.
			/// </summary>
			public DATABLOCKHEADER dbh;

			/// <summary>The console's code page.</summary>
			public uint uCodePage;
		}

		/// <summary>
		/// <para>This structure contains information from a .pif file. It is used by PifMgr_GetProperties.</para>
		/// </summary>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/ns-shlobj_core-propprg typedef struct PROPPRG { WORD flPrg; WORD
		// flPrgInit; CHAR achTitle[PIFNAMESIZE]; CHAR achCmdLine[PIFSTARTLOCSIZE + PIFPARAMSSIZE + 1]; CHAR achWorkDir[PIFDEFPATHSIZE]; WORD
		// wHotKey; CHAR achIconFile[PIFDEFFILESIZE]; WORD wIconIndex; DWORD dwEnhModeFlags; DWORD dwRealModeFlags; CHAR
		// achOtherFile[PIFDEFFILESIZE]; CHAR achPIFFile[PIFMAXFILEPATH]; };
		[PInvokeData("shlobj_core.h", MSDNShortId = "603f990b-efb8-4d72-bc96-27bda4ffcbd8")]
		[StructLayout(LayoutKind.Sequential)]
		public struct PROPPRG
		{
			private const int PIFNAMESIZE = 30;
			private const int PIFSTARTLOCSIZE = 63;
			private const int PIFDEFPATHSIZE = 64;
			private const int PIFPARAMSSIZE = 64;
			private const int PIFSHPROGSIZE = 64;
			private const int PIFSHDATASIZE = 64;
			private const int PIFDEFFILESIZE = 80;
			private const int PIFMAXFILEPATH = 260;

			/// <summary>
			/// <para>Type: <c>WORD</c></para>
			/// <para>Flags that describe how the program will run.</para>
			/// <para>PRG_DEFAULT</para>
			/// <para>Use the default options.</para>
			/// <para>PRG_CLOSEONEXIT</para>
			/// <para>Close the application on exit.</para>
			/// </summary>
			public ushort flPrg;

			/// <summary>
			/// <para>Type: <c>WORD</c></para>
			/// <para>Flags that specify the initial conditions for the application.</para>
			/// <para>PRGINIT_DEFAULT</para>
			/// <para>Use the default options.</para>
			/// <para>PRGINIT_MINIMIZED</para>
			/// <para>The application should be minimized.</para>
			/// <para>PRGINIT_MAXIMIZED</para>
			/// <para>The application should be maximized.</para>
			/// <para>PRGINIT_REALMODE</para>
			/// <para>The application should run in real mode.</para>
			/// <para>PRGINIT_REALMODESILENT</para>
			/// <para>The application should run in real mode without being prompted.</para>
			/// <para>PRGINIT_AMBIGUOUSPIF</para>
			/// <para>The data is ambiguous.</para>
			/// <para>PRGINIT_NOPIF</para>
			/// <para>No .pif file was found.</para>
			/// <para>PRGINIT_DEFAULTPIF</para>
			/// <para>A default .pif was found.</para>
			/// <para>PRGINIT_INFSETTINGS</para>
			/// <para>A .inf file was found.</para>
			/// <para>PRGINIT_INHIBITPIF</para>
			/// <para>The .inf file indicates that a .pif file should not be created.</para>
			/// </summary>
			public ushort flPrgInit;

			/// <summary>
			/// <para>Type: <c>__wchar_t</c></para>
			/// <para>A null-terminated string that contains the title.</para>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = PIFNAMESIZE)]
			public byte[] achTitle;

			/// <summary>
			/// <para>Type: <c>__wchar_t</c></para>
			/// <para>A null-terminated string that contains the command line, including arguments.</para>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = PIFSTARTLOCSIZE + PIFPARAMSSIZE + 1)]
			public byte[] achCmdLine;

			/// <summary>
			/// <para>Type: <c>__wchar_t</c></para>
			/// <para>A null-terminated string that contains the working directory.</para>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = PIFDEFPATHSIZE)]
			public byte[] achWorkDir;

			/// <summary>
			/// <para>Type: <c>WORD</c></para>
			/// <para>The key code of the .pif file's hotkey.</para>
			/// </summary>
			public ushort wHotKey;

			/// <summary>
			/// <para>Type: <c>__wchar_t</c></para>
			/// <para>A null-terminated string that contains the name of the file that contains the icon.</para>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = PIFDEFFILESIZE)]
			public byte[] achIconFile;

			/// <summary>
			/// <para>Type: <c>WORD</c></para>
			/// <para>The index of the icon in the file specified by <c>achIconFile</c>.</para>
			/// </summary>
			public ushort wIconIndex;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>Reserved.</para>
			/// </summary>
			public uint dwEnhModeFlags;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>Flags that specify the real mode options.</para>
			/// <para>RMOPT_MOUSE</para>
			/// <para>Requires a real-mode mouse.</para>
			/// <para>RMOPT_EMS</para>
			/// <para>Requires expanded memory.</para>
			/// <para>RMOPT_CDROM</para>
			/// <para>Requires CD-ROM support.</para>
			/// <para>RMOPT_NETWORK</para>
			/// <para>Requires network support.</para>
			/// <para>RMOPT_DISKLOCK</para>
			/// <para>Requires disk locking.</para>
			/// <para>RMOPT_PRIVATECFG</para>
			/// <para>Use a private config.sys or autoexec.bat file.</para>
			/// <para>RMOPT_VESA</para>
			/// <para>Requires a VESA driver.</para>
			/// </summary>
			public uint dwRealModeFlags;

			/// <summary>
			/// <para>Type: <c>__wchar_t</c></para>
			/// <para>A null-terminated string that contains the name of the "other" file in the directory.</para>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = PIFDEFFILESIZE)]
			public byte[] achOtherFile;

			/// <summary>
			/// <para>Type: <c>__wchar_t</c></para>
			/// <para>A null-terminated string that contains the name of the .pif file in the directory.</para>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = PIFMAXFILEPATH)]
			public byte[] achPIFFile;
		}

		/// <summary>This structure is used with the SHCreateShellFolderView function.</summary>
		[StructLayout(LayoutKind.Sequential)]
		[PInvokeData("Shlobj.h")]
		public struct SFV_CREATE
		{
			/// <summary>The size of the SFV_CREATE structure, in bytes.</summary>
			public uint cbSize;

			/// <summary>The IShellFolder interface of the folder for which to create the view.</summary>
			public IShellFolder pshf;

			/// <summary>
			/// A pointer to the parent IShellView interface. This parameter may be NULL. This parameter is used only when the view created
			/// by SHCreateShellFolderView is hosted in a common dialog box.
			/// </summary>
			public IShellView psvOuter;

			/// <summary>
			/// A pointer to the IShellFolderViewCB interface that handles the view's callbacks when various events occur. This parameter may
			/// be NULL.
			/// </summary>
			public IShellFolderViewCB psfvcb;
		}

		/// <summary>
		/// Contains and receives information for change notifications. This structure is used with the SHChangeNotifyRegister function and
		/// the SFVM_QUERYFSNOTIFY notification.
		/// </summary>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/ns-shlobj_core-_shchangenotifyentry typedef struct
		// _SHChangeNotifyEntry { PCIDLIST_ABSOLUTE pidl; BOOL fRecursive; } SHChangeNotifyEntry;
		[PInvokeData("shlobj_core.h", MSDNShortId = "cb11435a-86f0-4b06-bfc6-e0417f2897a1")]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHChangeNotifyEntry
		{
			/// <summary>
			/// <para>Type: <c>PCIDLIST_ABSOLUTE</c></para>
			/// <para>PIDL for which to receive notifications.</para>
			/// </summary>
			public IntPtr pidl;

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// A flag indicating whether to post notifications for children of this PIDL. For example, if the PIDL points to a folder, then
			/// file notifications would come from the folder's children if this flag was <c>TRUE</c>.
			/// </para>
			/// </summary>
			[MarshalAs(UnmanagedType.Bool)]
			public bool fRecursive;
		}

		/// <summary>Receives item data in response to a call to SHGetDataFromIDList.</summary>
		[StructLayout(LayoutKind.Sequential)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb759775")]
		public struct SHDESCRIPTIONID
		{
			/// <summary>Receives a value that determines what type the item is.</summary>
			public SHDID dwDescriptionId;

			/// <summary>Receives the CLSID of the object to which the item belongs.</summary>
			public Guid clsid;
		}

		/// <summary>
		/// <para>Contains a set of flags that indicate the current Shell settings. This structure is used with the SHGetSettings function.</para>
		/// </summary>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/ns-shlobj_core-shellflagstate typedef struct SHELLFLAGSTATE {
		// BOOL fShowAllObjects : 1; BOOL fShowExtensions : 1; BOOL fNoConfirmRecycle : 1; BOOL fShowSysFiles : 1; BOOL fShowCompColor : 1;
		// BOOL fDoubleClickInWebView : 1; BOOL fDesktopHTML : 1; BOOL fWin95Classic : 1; BOOL fDontPrettyPath : 1; BOOL fShowAttribCol : 1;
		// BOOL fMapNetDrvBtn : 1; BOOL fShowInfoTip : 1; BOOL fHideIcons : 1; BOOL fAutoCheckSelect : 1; BOOL fIconsOnly : 1; UINT
		// fRestFlags : 1; UINT fRestFlags : 3; } *LPSHELLFLAGSTATE;
		[PInvokeData("shlobj_core.h", MSDNShortId = "9968c7c9-79d9-4fb1-bda2-d6a2504cd3a3")]
		[StructLayout(LayoutKind.Sequential)]
		public struct SHELLFLAGSTATE
		{
			private uint bits1;

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show all objects, including hidden files and folders. <c>FALSE</c> to hide hidden files and folders.</para>
			/// </summary>
			public bool fShowAllObjects { get => GetBit(ref bits1, 0); set => SetBit(ref bits1, 0, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show file name extensions, <c>FALSE</c> to hide them.</para>
			/// </summary>
			public bool fShowExtensions { get => GetBit(ref bits1, 1); set => SetBit(ref bits1, 1, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>TRUE</c> to show no confirmation dialog box when deleting items to the Recycle Bin, <c>FALSE</c> to display the
			/// confirmation dialog box.
			/// </para>
			/// </summary>
			public bool fNoConfirmRecycle { get => GetBit(ref bits1, 2); set => SetBit(ref bits1, 2, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show system files, <c>FALSE</c> to hide them.</para>
			/// </summary>
			public bool fShowSysFiles { get => GetBit(ref bits1, 3); set => SetBit(ref bits1, 3, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show encrypted or compressed NTFS files in color.</para>
			/// </summary>
			public bool fShowCompColor { get => GetBit(ref bits1, 4); set => SetBit(ref bits1, 4, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to require a double-click to open an item when in web view.</para>
			/// </summary>
			public bool fDoubleClickInWebView { get => GetBit(ref bits1, 5); set => SetBit(ref bits1, 5, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to use Active Desktop, <c>FALSE</c> otherwise.</para>
			/// </summary>
			public bool fDesktopHTML { get => GetBit(ref bits1, 6); set => SetBit(ref bits1, 6, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to enforce Windows 95 Shell behavior and restrictions.</para>
			/// </summary>
			public bool fWin95Classic { get => GetBit(ref bits1, 7); set => SetBit(ref bits1, 7, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to prevent the conversion of the path to all lowercase characters.</para>
			/// </summary>
			public bool fDontPrettyPath { get => GetBit(ref bits1, 8); set => SetBit(ref bits1, 8, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fShowAttribCol { get => GetBit(ref bits1, 9); set => SetBit(ref bits1, 9, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to display a <c>Map Network Drive</c> button.</para>
			/// </summary>
			public bool fMapNetDrvBtn { get => GetBit(ref bits1, 10); set => SetBit(ref bits1, 10, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show a pop-up description for folders and files.</para>
			/// </summary>
			public bool fShowInfoTip { get => GetBit(ref bits1, 11); set => SetBit(ref bits1, 11, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to hide desktop icons, <c>FALSE</c> to show them.</para>
			/// </summary>
			public bool fHideIcons { get => GetBit(ref bits1, 12); set => SetBit(ref bits1, 12, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>Introduced in Windows Vista</c>. <c>TRUE</c> to use the Windows Vista-style checkbox folder views, <c>FALSE</c> to use the
			/// classic views.
			/// </para>
			/// </summary>
			public bool fAutoCheckSelect { get => GetBit(ref bits1, 13); set => SetBit(ref bits1, 13, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>Introduced in Windows Vista</c>. <c>TRUE</c> to show generic icons only, <c>FALSE</c> to show thumbnail-style icons in folders.
			/// </para>
			/// </summary>
			public bool fIconsOnly { get => GetBit(ref bits1, 14); set => SetBit(ref bits1, 14, value); }
		}

		/// <summary>Contains settings for the Shell's state. This structure is used with the <c>SHGetSetSettings</c> function.</summary>
		// typedef struct { BOOL fShowAllObjects :1; BOOL fShowExtensions :1; BOOL fNoConfirmRecycle :1; BOOL fShowSysFiles :1; BOOL
		// fShowCompColor :1; BOOL fDoubleClickInWebView :1; BOOL fDesktopHTML :1; BOOL fWin95Classic :1; BOOL fDontPrettyPath :1; BOOL
		// fShowAttribCol :1; BOOL fMapNetDrvBtn :1; BOOL fShowInfoTip :1; BOOL fHideIcons :1; BOOL fWebView :1; BOOL fFilter :1; BOOL
		// fShowSuperHidden :1; BOOL fNoNetCrawling :1; DWORD dwWin95Unused; UINT uWin95Unused; LONG lParamSort; int iSortDirection; UINT
		// version; UINT uNotUsed; BOOL fSepProcess :1; BOOL fStartPanelOn :1; BOOL fShowStartPage :1; BOOL fAutoCheckSelect :1; BOOL
		// fIconsOnly :1; BOOL fShowTypeOverlay :1; BOOL fShowStatusBar :1; UINT fSpareFlags :9;} SHELLSTATE, *LPSHELLSTATE; https://msdn.microsoft.com/en-us/library/windows/desktop/bb759788(v=vs.85).aspx
		[PInvokeData("Shlobj.h", MSDNShortId = "bb759788")]
		[StructLayout(LayoutKind.Sequential)]
		public struct SHELLSTATE
		{
			private uint bits1;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public uint dwWin95Unused;

			/// <summary>
			/// <para>Type: <c>UINT</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public uint uWin95Unused;

			/// <summary>
			/// <para>Type: <c>LONG</c></para>
			/// <para>The column to sort by.</para>
			/// </summary>
			public int lParamSort;

			/// <summary>
			/// <para>Type: <c>int</c></para>
			/// <para>
			/// Alphabetical sort direction for the column specified by <c>lParamSort</c>. Use 1 for an ascending sort, -1 for a descending sort.
			/// </para>
			/// </summary>
			public int iSortDirection;

			/// <summary>
			/// <para>Type: <c>UINT</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public uint version;

			/// <summary>
			/// <para>Type: <c>UINT</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public uint uNotUsed;

			private uint bits2;

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show all objects, including hidden files and folders. <c>FALSE</c> to hide hidden files and folders.</para>
			/// </summary>
			public bool fShowAllObjects { get => GetBit(ref bits1, 0); set => SetBit(ref bits1, 0, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show file name extensions, <c>FALSE</c> to hide them.</para>
			/// </summary>
			public bool fShowExtensions { get => GetBit(ref bits1, 1); set => SetBit(ref bits1, 1, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>TRUE</c> to show no confirmation dialog box when deleting items to the Recycle Bin, <c>FALSE</c> to display the
			/// confirmation dialog box.
			/// </para>
			/// </summary>
			public bool fNoConfirmRecycle { get => GetBit(ref bits1, 2); set => SetBit(ref bits1, 2, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show system files, <c>FALSE</c> to hide them.</para>
			/// </summary>
			public bool fShowSysFiles { get => GetBit(ref bits1, 3); set => SetBit(ref bits1, 3, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show encrypted or compressed NTFS files in color.</para>
			/// </summary>
			public bool fShowCompColor { get => GetBit(ref bits1, 4); set => SetBit(ref bits1, 4, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to require a double-click to open an item when in web view.</para>
			/// </summary>
			public bool fDoubleClickInWebView { get => GetBit(ref bits1, 5); set => SetBit(ref bits1, 5, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to use Active Desktop, <c>FALSE</c> otherwise.</para>
			/// </summary>
			public bool fDesktopHTML { get => GetBit(ref bits1, 6); set => SetBit(ref bits1, 6, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to enforce Windows 95 Shell behavior and restrictions.</para>
			/// </summary>
			public bool fWin95Classic { get => GetBit(ref bits1, 7); set => SetBit(ref bits1, 7, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to prevent the conversion of the path to all lowercase characters.</para>
			/// </summary>
			public bool fDontPrettyPath { get => GetBit(ref bits1, 8); set => SetBit(ref bits1, 8, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fShowAttribCol { get => GetBit(ref bits1, 9); set => SetBit(ref bits1, 9, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to display a <c>Map Network Drive</c> button.</para>
			/// </summary>
			public bool fMapNetDrvBtn { get => GetBit(ref bits1, 10); set => SetBit(ref bits1, 10, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show a pop-up description for folders and files.</para>
			/// </summary>
			public bool fShowInfoTip { get => GetBit(ref bits1, 11); set => SetBit(ref bits1, 11, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to hide desktop icons, <c>FALSE</c> to show them.</para>
			/// </summary>
			public bool fHideIcons { get => GetBit(ref bits1, 12); set => SetBit(ref bits1, 12, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to display as a web view.</para>
			/// </summary>
			public bool fWebView { get => GetBit(ref bits1, 13); set => SetBit(ref bits1, 13, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fFilter { get => GetBit(ref bits1, 14); set => SetBit(ref bits1, 14, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to show operating system files.</para>
			/// </summary>
			public bool fShowSuperHidden { get => GetBit(ref bits1, 15); set => SetBit(ref bits1, 15, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to disable automatic searching for network folders and printers.</para>
			/// </summary>
			public bool fNoNetCrawling { get => GetBit(ref bits1, 16); set => SetBit(ref bits1, 16, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>TRUE</c> to launch folder windows in separate processes, <c>FALSE</c> to launch in the same process.</para>
			/// </summary>
			public bool fSepProcess { get => GetBit(ref bits2, 0); set => SetBit(ref bits2, 0, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>Windows XP only</c>. <c>TRUE</c> to use the Windows XP-style Start menu, <c>FALSE</c> to use the classic Start menu.
			/// </para>
			/// </summary>
			public bool fStartPanelOn { get => GetBit(ref bits2, 1); set => SetBit(ref bits2, 1, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public bool fShowStartPage { get => GetBit(ref bits2, 2); set => SetBit(ref bits2, 2, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>Introduced in Windows Vista</c>. <c>TRUE</c> to use the Windows Vista-style checkbox folder views, <c>FALSE</c> to use the
			/// classic views.
			/// </para>
			/// </summary>
			public bool fAutoCheckSelect { get => GetBit(ref bits2, 3); set => SetBit(ref bits2, 3, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>Introduced in Windows Vista</c>. <c>TRUE</c> to show generic icons only, <c>FALSE</c> to show thumbnail-style icons in folders.
			/// </para>
			/// </summary>
			public bool fIconsOnly { get => GetBit(ref bits2, 4); set => SetBit(ref bits2, 4, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para>
			/// <c>Introduced in Windows Vista</c>. <c>TRUE</c> indicates a thumbnail should show the application that would be invoked when
			/// opening the item, <c>FALSE</c> indicates that no application will be shown.
			/// </para>
			/// </summary>
			public bool fShowTypeOverlay { get => GetBit(ref bits2, 5); set => SetBit(ref bits2, 5, value); }

			/// <summary>
			/// <para>Type: <c>BOOL</c></para>
			/// <para><c>Introduced in Windows 8</c>. <c>TRUE</c> to show the status bar; otherwise, <c>FALSE</c>.</para>
			/// </summary>
			public bool fShowStatusBar { get => GetBit(ref bits2, 6); set => SetBit(ref bits2, 6, value); }
		}

		/// <summary>
		/// <para>Holds custom folder settings. This structure is used with the SHGetSetFolderCustomSettings function.</para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// In Windows XP Service Pack 2 (SP2) and earlier versions, this structure supported both ANSI and Unicode strings. In Windows Vista
		/// and later versions, only Unicode strings are supported.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/ns-shlobj_core-shfoldercustomsettings typedef struct
		// SHFOLDERCUSTOMSETTINGS { DWORD dwSize; DWORD dwMask; SHELLVIEWID *pvid; LPWSTR pszWebViewTemplate; DWORD cchWebViewTemplate;
		// LPWSTR pszWebViewTemplateVersion; LPWSTR pszInfoTip; DWORD cchInfoTip; CLSID *pclsid; DWORD dwFlags; LPWSTR pszIconFile; DWORD
		// cchIconFile; int iIconIndex; LPWSTR pszLogo; DWORD cchLogo; } *LPSHFOLDERCUSTOMSETTINGS;
		[PInvokeData("shlobj_core.h", MSDNShortId = "a6357372-80ef-4719-b53f-87eb3fdc1b0d")]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct SHFOLDERCUSTOMSETTINGS
		{
			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>The size of the structure, in bytes.</para>
			/// </summary>
			public uint dwSize;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>
			/// A <c>DWORD</c> value specifying which folder attributes to read or write from this structure. Use one or more of the
			/// following values to indicate which structure members are valid:
			/// </para>
			/// <para>FCSM_VIEWID</para>
			/// <para><c>Deprecated</c>. <c>pvid</c> contains the folder's GUID.</para>
			/// <para>FCSM_WEBVIEWTEMPLATE</para>
			/// <para>
			/// <c>Deprecated</c>. <c>pszWebViewTemplate</c> contains a pointer to a buffer containing the path to the folder's WebView template.
			/// </para>
			/// <para>FCSM_INFOTIP</para>
			/// <para><c>pszInfoTip</c> contains a pointer to a buffer containing the folder's info tip.</para>
			/// <para>FCSM_CLSID</para>
			/// <para><c>pclsid</c> contains the folder's CLSID.</para>
			/// <para>FCSM_ICONFILE</para>
			/// <para><c>pszIconFile</c> contains the path to the file containing the folder's icon.</para>
			/// <para>FCSM_LOGO</para>
			/// <para><c>pszLogo</c> contains the path to the file containing the folder's thumbnail icon.</para>
			/// <para>FCSM_FLAGS</para>
			/// <para>Not used.</para>
			/// </summary>
			public FOLDERCUSTOMSETTINGSMASK dwMask;

			/// <summary>
			/// <para>Type: <c>SHELLVIEWID*</c></para>
			/// <para>The folder's GUID.</para>
			/// </summary>
			public IntPtr pvid;

			/// <summary>
			/// <para>Type: <c>LPTSTR</c></para>
			/// <para>A pointer to a null-terminated string containing the path to the folder's WebView template.</para>
			/// </summary>
			public string pszWebViewTemplate;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>
			/// If the SHGetSetFolderCustomSettings parameter is <c>FCS_READ</c>, this is the size of the <c>pszWebViewTemplate</c> buffer,
			/// in characters. If not, this is the number of characters to write from that buffer. Set this parameter to 0 to write the
			/// entire string.
			/// </para>
			/// </summary>
			public uint cchWebViewTemplate;

			/// <summary>
			/// <para>Type: <c>LPTSTR</c></para>
			/// <para>A pointer to a null-terminated buffer containing the WebView template version.</para>
			/// </summary>
			public string pszWebViewTemplateVersion;

			/// <summary>
			/// <para>Type: <c>LPTSTR</c></para>
			/// <para>A pointer to a null-terminated buffer containing the text of the folder's infotip.</para>
			/// </summary>
			public string pszInfoTip;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>
			/// If the SHGetSetFolderCustomSettings parameter is <c>FCS_READ</c>, this is the size of the <c>pszInfoTip</c> buffer, in
			/// characters. If not, this is the number of characters to write from that buffer. Set this parameter to 0 to write the entire string.
			/// </para>
			/// </summary>
			public uint cchInfoTip;

			/// <summary>
			/// <para>Type: <c>CLSID*</c></para>
			/// <para>
			/// A pointer to a CLSID used to identify the folder in the Windows registry. Further folder information is stored in the
			/// registry under that CLSID entry.
			/// </para>
			/// </summary>
			public IntPtr pclsid;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>Not used.</para>
			/// </summary>
			public uint dwFlags;

			/// <summary>
			/// <para>Type: <c>LPTSTR</c></para>
			/// <para>A pointer to a null-terminated buffer containing the path to file containing the folder's icon.</para>
			/// </summary>
			public string pszIconFile;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>
			/// If the SHGetSetFolderCustomSettings parameter is <c>FCS_READ</c>, this is the size of the <c>pszIconFile</c> buffer, in
			/// characters. If not, this is the number of characters to write from that buffer. Set this parameter to 0 to write the entire string.
			/// </para>
			/// </summary>
			public uint cchIconFile;

			/// <summary>
			/// <para>Type: <c>int</c></para>
			/// <para>The index of the icon within the file named in <c>pszIconFile</c>.</para>
			/// </summary>
			public int iIconIndex;

			/// <summary>
			/// <para>Type: <c>LPTSTR</c></para>
			/// <para>
			/// A pointer to a null-terminated buffer containing the path to the file containing the folder's logo image. This is the image
			/// used in thumbnail views.
			/// </para>
			/// </summary>
			public string pszLogo;

			/// <summary>
			/// <para>Type: <c>DWORD</c></para>
			/// <para>
			/// If the SHGetSetFolderCustomSettings parameter is <c>FCS_READ</c>, this is the size of the <c>pszLogo</c> buffer, in
			/// characters. If not, this is the number of characters to write from that buffer. Set this parameter to 0 to write the entire string.
			/// </para>
			/// </summary>
			public uint cchLogo;
		}

		/*[StructLayout(LayoutKind.Sequential)]
		[PInvokeData("Shlobj.h", MSDNShortId = "bb773399")]
		public struct SFV_CREATE
		{
			public uint cbSize;
			[MarshalAs(UnmanagedType.Interface)] public IShellFolder pshf;
			[MarshalAs(UnmanagedType.Interface)] public IShellView psvOuter;
			[MarshalAs(UnmanagedType.Interface)] public IShellFolderViewCB psfbcb;
		}*/
	}
}