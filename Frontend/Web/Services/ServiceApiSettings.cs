﻿namespace Web.Services;

public class ServiceApiSettings
{
    public string IdentityBaseUri { get; set; } //IdentityServer 
    public string GatewayBaseUri { get; set; }
    public string PhotoStockUri { get; set; }
    public ServiceApi Catalog { get; set; }
}
public class ServiceApi
{
    public string Path { get; set; }
}
