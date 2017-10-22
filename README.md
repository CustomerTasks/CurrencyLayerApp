File Structure:<br />
<h1>CurrencyLayerApp (UI)</h1><br />
1.Abstractions - just interfaces<br />
2.Helpers - Expanded methods, helpers for converting, searching<br />
3.Infrastructure - Main & important operations<br />
	3.1.Converters - WPF converting patterns for turning from one to another type (used between View and ViewModels)<br />
	3.2.DataManagers - Classes which operate with data (save/uploading)<br />
	3.3.Global - Just common data for global access (file/web paths, Settings, Logger)<br />
4.Models - models for ViewModels (MVVM)<br />
5.Resources - strings,pictures ...<br />
	5.1.Pictures (flags & icons)<br />
	5.2.Strings  <h1>!!!You can translate resources in this directory!!!!</h1><br />
	 5.3.Curreccies.TXT - list of currencies<br />
6.ViewModels - main operations<br />
7.Views - UIs<br />


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
		-method (get,post,put....)
		-headers (token, content type ...)
		-request body (converted class into json, form data ...)
