using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: AssemblyTitle( "Lagash.Core" )]
[assembly: AssemblyDescription( "Lagash Validation Framework - Core" )]

[assembly: CLSCompliant( true )]
[assembly: ComVisible( false )]
[assembly: EnvironmentPermissionAttribute( SecurityAction.RequestMinimum, Read = "COMPUTERNAME;USERNAME;USERDOMAIN" )]

[assembly: Guid( "1498f768-d7ce-470b-9eca-79956e7cc9e1" )]