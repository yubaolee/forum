﻿using ECommon.Components;
using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Domain;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    [Component]
    public class AccountCommandHandler :
        ICommandHandler<RegisterNewAccountCommand>
    {
        private readonly AggregateRootFactory _factory;
        private readonly ILockService _lockService;
        private readonly RegisterAccountIndexService _registerAccountIndexService;

        public AccountCommandHandler(AggregateRootFactory factory, ILockService lockService, RegisterAccountIndexService registerAccountIndexService)
        {
            _factory = factory;
            _lockService = lockService;
            _registerAccountIndexService = registerAccountIndexService;
        }

        public void Handle(ICommandContext context, RegisterNewAccountCommand command)
        {
            _lockService.ExecuteInLock(typeof(Account).Name, () =>
            {
                var account = _factory.CreateAccount(command.Name, command.Password);
                _registerAccountIndexService.RegisterAccountIndex(command.Id, account.Id, command.Name);
                context.Add(account);
            });
        }
    }
}
