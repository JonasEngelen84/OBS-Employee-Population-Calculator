// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

//TODO: unnötige unterdrückung???
[assembly: SuppressMessage("Design", "ASP0000:Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'", Justification = "<Ausstehend>", Scope = "member", Target = "~M:OBS.Dashboard.Map.Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
