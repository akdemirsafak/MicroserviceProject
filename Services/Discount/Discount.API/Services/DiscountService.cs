using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;
using SharedLibrary.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Discount.API.Services;

public class DiscountService : IDiscountService
{
    //Tip güvenli class üzerinden de bağlanılabilir fakat farklı yöntemleri görmek açısından bu şekilde yaptık.

    private readonly IConfiguration _configuration;
     private readonly IDbConnection _dbConnection; //Herhangi bir db ile iletişime geçerken kullanırız.Dapper'e özgü değildir.
    public DiscountService(IConfiguration configuration)
    {
        _configuration = configuration;
        _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
    }

    public async Task<Response<List<DiscountModel>>> GetAll()
    {
        var discounts = await _dbConnection.QueryAsync<DiscountModel>("Select * from discount");
        return Response<List<DiscountModel>>.Success(discounts.ToList(),200);
    }
    public async Task<Response<DiscountModel>> GetById(int discountId)
    {
        var discount = await _dbConnection.QuerySingleOrDefaultAsync<DiscountModel>("Select * from discount where id=@myDiscountId",new { myDiscountId = discountId});
        if (discount == null)
        {
            return Response<DiscountModel>.Fail("Discount not found.",404);
        }
        return Response<DiscountModel>.Success(discount,200);
    }

    public async Task<Response<DiscountModel>> GetByCodeAndUserId(string code, string userId)
    {
        var discount = await _dbConnection.QueryAsync<DiscountModel>("Select * from discount where userid=@UserId and code=@Code");
        var parameters= new DynamicParameters();
        parameters.Add("UserId", userId, DbType.String);
        parameters.Add("Code", code, DbType.String);
        var hasDiscount = discount.FirstOrDefault();

        if (hasDiscount==null)
        {
            return Response<DiscountModel>.Fail("Discount not found",404);
        }
        return Response<DiscountModel>.Success(hasDiscount,200);

    }

    public async Task<Response<NoContent>> Save(DiscountModel discountModel)
    {
        var affectedRowCount = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES (@UserId,@Rate,@Code)");
        var parameters = new DynamicParameters();
        parameters.Add("UserId",discountModel.UserId,DbType.String);
        parameters.Add("Rate",discountModel.Rate,DbType.Int32);
        parameters.Add("Code", discountModel.Code, DbType.String);
        if (affectedRowCount > 0)
        {
            return Response<NoContent>.Success(201);
        }
        return Response<NoContent>.Fail("UnSuccessfull while adding",500);
    }

    public async Task<Response<NoContent>> Update(DiscountModel discountModel)
    {
        //var existRecord = await _dbConnection.QuerySingleAsync("Select * from discount where id=@discoutdId", new {discountId=discountModel.Id});
        //if (existRecord == null)
        //{
        //    return Response<NoContent>.Fail("Record is not found", 404);
        //}
        var affectedRowCount = await _dbConnection.ExecuteAsync("Update discount set userid=@UserId,rate=@Rate,code=@Code where id=@discountId");

        var parameters = new DynamicParameters();
       
        parameters.Add("UserId", discountModel.UserId, DbType.String);
        parameters.Add("Rate", discountModel.Rate, DbType.Int32);
        parameters.Add("Code", discountModel.Code, DbType.String);
        parameters.Add("discountId",discountModel.Id,DbType.Int32);
        if (affectedRowCount > 0)
        {
            return Response<NoContent>.Success(204);
        }
        return Response<NoContent>.Fail("UnSuccessfull.Discount Not found.", 404);
    }

    public async Task<Response<NoContent>> Delete(int discoundId)
    {
        //Eğer veritabanına bağlantıda sıkıntı varsa hata throw edilir.
        var affectedRowCount = await _dbConnection.ExecuteAsync("Delete from discount where id=@Id",new { Id=discoundId });
        if (affectedRowCount > 0)
        {
            return Response<NoContent>.Success(204);
        }
        return Response<NoContent>.Fail("Discount Not found", 404);
    }

   

 
}
