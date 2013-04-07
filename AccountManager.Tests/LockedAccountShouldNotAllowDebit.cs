using System;
using System.Collections.Generic;
using MTG.Core;
using AccountManager.Models.Account.Commands;
using AccountManager.Models.Account.Domain;
using AccountManager.Models.Account.Events;

namespace AccountManager.Tests
{
    public class LockedAccountShouldNotAllowDebit : TestBase
    {
        readonly Guid _accountId = Guid.NewGuid();

        public override Dictionary<object, List<object>> GivenTheseEvents()
        {
            return new Dictionary<object, List<object>>
            {
                {_accountId, new List<object>
                    {
                        new AccountRegisteredEvent(_accountId, "John", "abc@example.com", 500),
                        new AccountLockedEvent(_accountId)
                    }
                }
            };
        }

        public override object WhenThisHappens()
        {
            return new DebitAccountCommand { AccountId = _accountId, Amount = 10 };
        }

        public override Exception ThisExceptionShouldOccur()
        {
            return new AccountLockedException(_accountId);
        }

        public override void RegisterHandler(MessageBus bus, IRepository repo)
        {
            var svc = new AccountApplicationService(repo);
            bus.RegisterHandler<DebitAccountCommand>(svc.Handle);
        }
    }
}