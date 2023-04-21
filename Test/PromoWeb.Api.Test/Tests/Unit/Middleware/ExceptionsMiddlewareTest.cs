using Microsoft.AspNetCore.Http;
using PromoWeb.Api.Middlewares;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Extensions;
using PromoWeb.Common.Responses;
using System.Text.Json;

namespace PromoWeb.Api.Test.Tests.Unit.Middleware
{
	[TestFixture]
	public class ExceptionsMiddlewareTest
	{

		MemoryStream bodyStream;
		DefaultHttpContext context;
		RequestDelegate next;


		[SetUp]
		public void CreateHttpContext()
		{
			bodyStream = new MemoryStream();
			context = new DefaultHttpContext();
			context.Response.Body = bodyStream;
		}


		[Test]
		public async Task ThrowProcessException_ReturnsJsonType()
		{
			next = (HttpContext ctx) => throw new ProcessException("we have error");	

			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			Assert.That(context.Response.ContentType, Is.EqualTo("application/json"));
		}

		[Test]
		public async Task ThrowProcessException_ReturnsWeHaveErrorBodyContent()
		{
			ErrorResponse expectedResponse = new ErrorResponse { Message = "we have error" };

			next = (HttpContext ctx) => throw new ProcessException("we have error");


			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			string response;
			bodyStream.Seek(0, SeekOrigin.Begin);
			using (var stringReader = new StreamReader(bodyStream))
			{
				response = await stringReader.ReadToEndAsync();
			}

			Assert.That(response, Is.EqualTo(JsonSerializer.Serialize(expectedResponse))); 
		}

		[Test]
		public async Task ThrowProcessException_Returns400Code()
		{
			next = (HttpContext ctx) => throw new ProcessException("we have error");	

			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			Assert.That(context.Response.StatusCode, Is.EqualTo(400));
		}

		[Test]
		public async Task ThrowException_ReturnsJsonType()
		{
			next = (HttpContext ctx) => throw new Exception("we have error");

			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			Assert.That(context.Response.ContentType, Is.EqualTo("application/json"));
		}

		[Test]
		public async Task ThrowException_ReturnsWeHaveErrorBodyContent()
		{
			ErrorResponse expectedResponse = new ErrorResponse { Message = "we have error" , ErrorCode = -1};

			next = (HttpContext ctx) => throw new Exception("we have error");


			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			string response;
			bodyStream.Seek(0, SeekOrigin.Begin);
			using (var stringReader = new StreamReader(bodyStream))
			{
				response = await stringReader.ReadToEndAsync();
			}

			Assert.That(response, Is.EqualTo(JsonSerializer.Serialize(expectedResponse)));
		}

		[Test]
		public async Task ThrowException_Returns500Code()
		{
			next = (HttpContext ctx) => throw new Exception("we have error");

			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			Assert.That(context.Response.StatusCode, Is.EqualTo(500));
		}

		[Test]
		public async Task Returns200Code()
		{
			next = (HttpContext ctx) => Task.CompletedTask;

			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			Assert.That(context.Response.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public async Task ReturnsJsonType()
		{
			next = (HttpContext ctx) => Task.CompletedTask;

			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			Assert.IsNull(context.Response.ContentType);
		}

		[Test]
		public async Task ReturnsEmptyBodyContent()
		{
			next = (HttpContext ctx) => Task.CompletedTask;

			var middleware = new ExceptionsMiddleware(next);
			await middleware.InvokeAsync(context);

			string response;
			bodyStream.Seek(0, SeekOrigin.Begin);
			using (var stringReader = new StreamReader(bodyStream))
			{
				response = await stringReader.ReadToEndAsync();
			}

			Assert.IsEmpty(response);
		}
	}
}
