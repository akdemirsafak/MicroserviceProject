using Basket.API.Dtos;
using SharedLibrary.Dtos;
using StackExchange.Redis;

namespace Basket.API.Services;

public class RedisService
{
    private readonly string _hostName;
    private readonly int _port;
    private ConnectionMultiplexer _connectionMultiplexer;
    public RedisService(string host,int port)
    {
        _hostName = host;
        _port = port;
    }
    public void Connect()=> _connectionMultiplexer=ConnectionMultiplexer.Connect($"{_hostName}:{_port}");

    public IDatabase GetDb(int db = 1) => _connectionMultiplexer.GetDatabase(db);

   
}