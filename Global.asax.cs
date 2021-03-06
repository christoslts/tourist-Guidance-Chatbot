using Autofac;
using System.Web.Http;
using System.Configuration;
using System.Reflection;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace SimpleEchoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //// Bot Storage: This is a great spot to register the private state storage for your bot. 
            //// We provide adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
            //// For samples and documentation, see: https://github.com/Microsoft/BotBuilder-Azure

            //Conversation.UpdateContainer(
            //    builder =>
            //    {
            //        builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

            //        // Using Azure Table Storage
            //       // TableBotDataStore store = new TableBotDataStore(ConfigurationManager.AppSettings["DefaultEndpointsProtocol=https;AccountName=touristbot2a8a1;AccountKey=61T9PZPTwtS3oSWBno6BTfTjRp6pRv0z9nAXhzcjA4VNrttFpfEI+W+qGp05iBpSN/tPoK0FCqXvctmdjFgyww==NULL;"]); // requires Microsoft.BotBuilder.Azure Nuget package 

            //        // To use CosmosDb or InMemory storage instead of the default table storage, uncomment the corresponding line below
            //        // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 
            //         var store = new InMemoryDataStore(); // volatile in-memory store

            //        builder.Register(c => store)
            //            .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
            //            .AsSelf()
            //            .SingleInstance();

            // });
            var store = new InMemoryDataStore();

            Conversation.UpdateContainer(
                       builder =>
                       {
                           builder.Register(c => store)
                                     .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                                     .AsSelf()
                                     .SingleInstance();

                           builder.Register(c => new CachingBotDataStore(store,
                                      CachingBotDataStoreConsistencyPolicy
                                      .ETagBasedConsistency))
                                      .As<IBotDataStore<BotData>>()
                                      .AsSelf()
                                      .InstancePerLifetimeScope();


                       });

            GlobalConfiguration.Configure(WebApiConfig.Register);

        }
    }
}
