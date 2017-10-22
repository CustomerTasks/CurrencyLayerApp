File Structure:
1. CurrencyLayerApp (UI)
-Abstractions - just interfaces
-Helpers - Expanded methods, helpers for converting, searching
-Infrastructure - Main & important operations
	-Converters - WPF converting patterns for turning from one to another type (used between View and ViewModels)
	-DataManagers - Classes which operate with data (save/uploading)
	-Global - Just common data for global access (file/web paths, Settings, Logger)
-Models - models for ViewModels (MVVM)
-Resources - strings,pictures ...
	-Pictures (flags & icons)
	-Strings (						!!!You can translate resources in this directory!!!!					)
	 Curreccies.TXT - list of currencies
-ViewModels - main operations
-Views - UIs


1) Pattern 'Repository' & 'UnitOfWork'
https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
2) Disposable Pattern
https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/dispose-pattern
3) Singleton Pattern
https://msdn.microsoft.com/en-us/library/ff650316.aspx?f=255&MSPPError=-2147217396
4) Strategy Pattern
https://www.wikiwand.com/en/Strategy_pattern
5) Each Http Request contains: 
		-host url + url(also parameters in url)
		-method (het,post,put....)
		-headers (token, content type ...)
		-request body (converted class into json, form data ...)
