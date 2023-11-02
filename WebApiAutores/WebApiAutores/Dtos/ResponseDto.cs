﻿namespace WebApiAutores.Dtos;

public class ResponseDto<T>
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}