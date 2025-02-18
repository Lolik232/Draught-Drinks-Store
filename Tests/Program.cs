// See https://aka.ms/new-console-template for more information
using DAL.Abstractions.Interfaces.Payment;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Payment.YooKassa;
using System.Linq.Expressions;
using System.Net;

Console.WriteLine("Hello, World!");

YooKasssaPayment paymentService = new();

//await paymentService.CreatePayment(
//    11,
//    11,
//    "https://www.example.com/return_url",
//    new Amount
//    {
//        Value = "10.00",
//        Currency = "RUB",
//    },
//    "lox pidr"
//    );

HttpListener httpListener = new HttpListener();
httpListener.Prefixes.Add("http://localhost:8080/notify/");

httpListener.Start();

var context = await httpListener.GetContextAsync();
var request = context.Request;


Console.WriteLine(request.ToString());

Console.WriteLine("created");