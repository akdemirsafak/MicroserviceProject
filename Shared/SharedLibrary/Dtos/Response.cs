﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharedLibrary.Dtos { 
public class Response<T>
{
    public T Data { get;private set; }
    [JsonIgnore]
    public int StatusCode { get; private set; } //Dönüş tipinden faydalanıcaz ama response'un içinde olmasına gerek duymuyoruz.
    [JsonIgnore] //Client bu bilgiyi görmeyecek
    public bool IsSuccessfull { get; private set; }
    public List<string> Errors { get; private set; }
    //--Static Factory Method-- Static methodlarla geriye bir nesne dönülüyorsa. 
    public static Response<T> Success(T data,int statusCode) 
    {
        return new Response<T> { 
            Data = data,
            StatusCode = statusCode,
            IsSuccessfull=true };
    }
    public static Response<T> Success(int statusCode) {  
        return new Response<T> { 
            Data=default(T),
            StatusCode = statusCode,
            IsSuccessfull=true }; 
    }
    public static Response<T> Fail(List<string> errors,int statusCode) 
    {
        return new Response<T> {
            Errors=errors,
            StatusCode = statusCode,
            IsSuccessfull=false };
    }
    public static Response<T> Fail(string error, int statusCode)
    {
        return new Response<T> { 
            Errors=new List<string>() { error },
            StatusCode = statusCode,
            IsSuccessfull = false };
    }
  

}
}
