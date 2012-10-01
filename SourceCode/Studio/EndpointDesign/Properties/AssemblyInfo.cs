#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

#endregion

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle(@"")]
[assembly: AssemblyDescription(@"")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(@"NServiceBus")]
[assembly: AssemblyProduct(@"NServiceBus.Modeling")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: System.Resources.NeutralResourcesLanguage("en")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion(@"1.0.0.0")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]

//
// Make the Dsl project internally visible to the DslPackage assembly
//
[assembly: InternalsVisibleTo(@"NServiceBus.Modeling.DslPackage, PublicKey=0024000004800000940000000602000000240000525341310004000001000100C1BFD96FE137E1BD00974510D4E2CBA5EB909B52FE534A460A2C0BEBBC4A3C15F86F4B7A9C53D9E8ADD90EC3C59F0627DD841A924D51C4B9CE41261F06D0E251B922C4412D25852DF48C200C680480BE371877DAD7039ADFFA8F2618590DD9D3ECE8AD8F95EB4E5B7A355318A5C7A7BE47FB18A0256115C81BAAB5DBE3AD82AD")]