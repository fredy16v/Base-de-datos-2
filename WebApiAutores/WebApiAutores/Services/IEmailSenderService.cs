﻿namespace WebApiAutores.Services;

public interface IEmailSenderService
{
    Task SendEmailAsync(string email, string subjet, string message);
}