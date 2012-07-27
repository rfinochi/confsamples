using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: AssemblyTitle( "Lagash.Core" )]
[assembly: AssemblyDescription( "Lagash Validation Framework - Unit Tests" )]

[assembly: CLSCompliant( true )]
[assembly: ComVisible( false )]
[assembly: EnvironmentPermissionAttribute( SecurityAction.RequestMinimum, Read = "COMPUTERNAME;USERNAME;USERDOMAIN" )]

[assembly: Guid( "cf7cc5c0-e24a-4b54-a008-6adf6b88041d" )]