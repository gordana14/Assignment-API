# Assignment-API -  Web-API-Stream-Message---ASP.Core

I will develop REST service using .NET Core 2.1.1 , C#, Swagger/OpenAPI for .NET Core 2.2.0, Entity framework Core 2.1.1 and Linq.
I will create database with the code-first approach using  data annotation and fluent api. 
The functionality of the REST service are elatively simple:

-          The main functionality is that you can publish messages to different channels. For example, there would be channels such as: traffic, events, public-transportation, weather, etc.

-          You can publish messages to one of the channels (using the appropriate REST method) by using appropriate user rights

-          The message is written to the database

-          REST service should have methods to search/query messages on each channel or on each user also methods the last message from requested user and channel

-          Channels and User are configurable and connect in relationship many to many
