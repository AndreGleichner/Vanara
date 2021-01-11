﻿using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Vanara.PInvoke
{
	/// <summary>The status of a device node (devnode).</summary>
	[PInvokeData("cfg.h")]
	[Flags]
	public enum DN : uint
	{
		DN_ROOT_ENUMERATED = 0x00000001,
		DN_DRIVER_LOADED = 0x00000002,
		DN_ENUM_LOADED = 0x00000004,
		DN_STARTED = 0x00000008,
		DN_MANUAL = 0x00000010,
		DN_NEED_TO_ENUM = 0x00000020,
		DN_NOT_FIRST_TIME = 0x00000040,
		DN_HARDWARE_ENUM = 0x00000080,
		DN_LIAR = 0x00000100,
		DN_HAS_MARK = 0x00000200,
		DN_HAS_PROBLEM = 0x00000400,
		DN_FILTERED = 0x00000800,
		DN_MOVED = 0x00001000,
		DN_DISABLEABLE = 0x00002000,
		DN_REMOVABLE = 0x00004000,
		DN_PRIVATE_PROBLEM = 0x00008000,
		DN_MF_PARENT = 0x00010000,
		DN_MF_CHILD = 0x00020000,
		DN_WILL_BE_REMOVED = 0x00040000,
		DN_NOT_FIRST_TIMEE = 0x00080000,
		DN_STOP_FREE_RES = 0x00100000,
		DN_REBAL_CANDIDATE = 0x00200000,
		DN_BAD_PARTIAL = 0x00400000,
		DN_NT_ENUMERATOR = 0x00800000,
		DN_NT_DRIVER = 0x01000000,
		DN_NEEDS_LOCKING = 0x02000000,
		DN_ARM_WAKEUP = 0x04000000,
		DN_APM_ENUMERATOR = 0x08000000,
		DN_APM_DRIVER = 0x10000000,
		DN_SILENT_INSTALL = 0x20000000,
		DN_NO_SHOW_IN_DM = 0x40000000,
		DN_BOOT_LOG_PROB = 0x80000000,
		DN_NEED_RESTART = DN_LIAR,
		DN_DRIVER_BLOCKED = DN_NOT_FIRST_TIME,
		DN_LEGACY_DRIVER = DN_MOVED,
		DN_CHILD_WITH_INVALID_ID = DN_HAS_MARK,
		DN_DEVICE_DISCONNECTED = DN_NEEDS_LOCKING,
		DN_QUERY_REMOVE_PENDING = DN_MF_PARENT,
		DN_QUERY_REMOVE_ACTIVE = DN_MF_CHILD,
	}
}