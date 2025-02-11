using GigaChatBot.Api.Hubs;
using GigaChatBot.Api.Services;
using GigaChatBot.Application;
using GigaChatBot.Application.Common.Interfaces.Services;
using GigaChatBot.Application.Conversation.Commands.SendConversationMessage;
using GigaChatBot.Application.Conversation.Queries.GetTestConversationDetails;
using GigaChatBot.Infrastructure;
using GigaChatBot.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GigaChatBot.Application.Message.Commands.ReactToMessage;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.SetIsOriginAllowed(_ => true);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });
        });
        builder.Services.AddTransient<IConversationNotificationService, ConversationNotificationService>();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();
        builder.Services.AddPersistence(builder.Configuration);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapGet("/conversation/test", async (
            [AsParameters] GetTestConversationDetailsQuery query,
            [FromServices] IMediator mediator) =>
        {
            return await mediator.Send(query);
        });

        app.MapPost("/conversation/{id}/message", async (
            [FromRoute] Guid id,
            [FromBody] SendConversationMessageCommand command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (command.ConversationId != id)
            {
                return Results.BadRequest();
            }
            await mediator.Send(command, cancellationToken);
            return Results.Ok();
        });

        app.MapPost("/message/{id}/react", async (
            [FromRoute] Guid id,
            [FromBody] ReactToMessageCommand command,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (command.MessageId != id)
            {
                return Results.BadRequest();
            }
            await mediator.Send(command, cancellationToken);
            return Results.Ok();
        });

        app.MapHub<ConversationHub>("/conversation/hub");

        app.UseCors();
        app.Run();
    }
}
