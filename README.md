# NuLuS .NET Client

This is the official C#/.NET client for the NuLuS API (<http://www.mitto.ch>).


## Usage

1. Contact Mitto AG for user account and service enpoints (<https://www.mitto.ch/contacts/>)

2. Add the client to your project

3. Adjust the configuration (web.config / app.config)
	```XML
    <add key="NuLuS.Dns.Api.Address" value="127.0.0.1" />
    <add key="NuLuS.Http.Rest.Api.Url" value="http://localhost:8082/" />
	```

4. Start doing number lookups
	```C#
	await NuLuSApiClientAsync.Instance.LookupAsync(product, number);
	```

## Usage with IoC container or UnitTest frameworks
You can also use the interface `INuLuSApiClientAsync` and the corresponding implementation `NuLuSApiClientAsync`.
This allows you to integrate the NuLuS API client into your favourite IoC framework.
And you can easily mock any dependency to the NuLuS API client in your UnitTests.

## Configuration / AppSettings
* `NuLuS.Api.Channel`: QueryChannel, optional, specifies communication channel
* `NuLuS.Dns.Api.Address`: string, required for Dns lookups, specifies Dns server address
* `NuLuS.Http.Rest.Api.Url`: string, required for Http / Rest lookups, specifies Http Rest API url

## Samples

### Console lookup
*NuLuS.Samples.Console.csproj*

This project shows you how easy you can get data for a desired mobile number.


### Missing features
* NuGet package
* Dns multiple lookups per question
* Dns real async calls