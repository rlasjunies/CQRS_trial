using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccountManager.Models.Account.Commands;
using AccountManager.Models.Account.Domain;
using AccountManager.Models.Account.Events;
using AccountManager.Models.Account.ReadModel;
using AccountManager.Models.Account.ReadModel.Admin;
using AccountManager.Models.Account.ReadModel.Public;
using AccountManager.Models.News.ReadModel;

namespace MTG.Core
{
    public class Configuration
    {
        /* in memory event store */
        private Dictionary<object, List<object>> inMemDict = new Dictionary<object, List<object>>();

        /* application bus */        
        private readonly MessageBus _bus;
        public MessageBus Bus { 
            get { 
                return _bus; 
            } 
        }
        
        /* account domain init */
        private readonly AccountReadModelFacade _AccountReadModel;
        public AccountReadModelFacade AccountReadModel { 
            get { 
                return _AccountReadModel; 
            } 
        }

        /* news domain init */
        private readonly NewsReadModelFacade _NewsReadModel;
        public NewsReadModelFacade NewsReadModel { 
            get { 
                return _NewsReadModel; 
            } 
        }

        private static readonly Configuration Config = new Configuration();
        public static Configuration Instance()
        {
            return Config;
        }

        private static readonly AccountManager.Models.RazorContext _Context = new AccountManager.Models.RazorContext();
        public static AccountManager.Models.RazorContext Context()
        {
            return _Context;
        }

        private Configuration()
        {
    /* bus intialisation */
            _bus = new MessageBus();
            //var eventStore = new SqlServerEventStore(_bus);
            //var eventStore = new SqlLiteEventStore(_bus);
            var eventStore = new InMemoryEventStore(_bus, inMemDict );
            var repository = new DomainRepository(eventStore);

    /* Account Domain */
            var commandService = new AccountApplicationService(repository);
            _bus.RegisterHandler<RegisterAccountCommand>(commandService.Handle);
            _bus.RegisterHandler<DebitAccountCommand>(commandService.Handle);
            _bus.RegisterHandler<UnlockAccountCommand>(commandService.Handle);

            var infoProjection = new AccountInfoProjection();
            _bus.RegisterHandler<AccountRegisteredEvent>(infoProjection.Handle);
            _bus.RegisterHandler<AccountLockedEvent>(infoProjection.Handle);
            _bus.RegisterHandler<AccountUnlockedEvent>(infoProjection.Handle);

            var balanceProjection = new AccountBalanceProjection();
            _bus.RegisterHandler<AccountRegisteredEvent>(balanceProjection.Handle);
            _bus.RegisterHandler<AccountDebitedEvent>(balanceProjection.Handle);

            var notification = new NotificationProjection();
            _bus.RegisterHandler<AccountRegisteredEvent>(notification.Handle);
            _bus.RegisterHandler<AccountDebitedEvent>(notification.Handle);
            _bus.RegisterHandler<AccountLockedEvent>(notification.Handle);
            _bus.RegisterHandler<AccountUnlockedEvent>(notification.Handle);
            _bus.RegisterHandler<AccountOverdrawAttemptedEvent>(notification.Handle);

            _AccountReadModel = new AccountReadModelFacade(balanceProjection, infoProjection, notification);

    /*  News Domain*/
            //var newsEventStore = new SqlServerEventStore(_bus); 
            //var newsEventStore = new SqlLiteEventStore(_bus);
            var newsEventStore = new InMemoryEventStore(_bus, inMemDict); 
            var newsRepository = new DomainRepository(eventStore);

            /* Register command on the News bounded context */
            var newsCommandService = new AccountManager.Models.News.Domain.NewsApplicationService(newsRepository);
            _bus.RegisterHandler<AccountManager.Models.News.Commands.CreateNewsCommand>(newsCommandService.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Commands.PublishNewsCommand>(newsCommandService.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Commands.UnpublishNewsCommand>(newsCommandService.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Commands.UpdateNewsCommand>(newsCommandService.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Commands.DeleteNewsCommand>(newsCommandService.Handle);

            var _newsProjection = new AccountManager.Models.News.ReadModel.NewsProjection();
            _bus.RegisterHandler<AccountManager.Models.News.Events.NewsCreatedEvent>(_newsProjection.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Events.NewsPublishedEvent>(_newsProjection.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Events.NewsUnpublishedEvent>(_newsProjection.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Events.NewsUpdatedEvent>(_newsProjection.Handle);
            _bus.RegisterHandler<AccountManager.Models.News.Events.NewsDeletedEvent>(_newsProjection.Handle);
            

            _NewsReadModel = new NewsReadModelFacade(_newsProjection);

    /* rehydrate */
            var events = eventStore.GetAllEventsEver();
            _bus.Publish(events);
        }
    }
}