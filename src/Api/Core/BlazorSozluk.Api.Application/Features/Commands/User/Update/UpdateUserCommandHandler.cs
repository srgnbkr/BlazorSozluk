using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand,Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.Id);
            var dbEmailAddress = user.EmailAddress;
            //email kontrol ediyoruz. Büyük küçük harf duyarlılığı yok.
            var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

            if (user is null)
                throw new DatabaseValidationException("User already exist");

            mapper.Map(request, user);

            var rows = await userRepository.UpdateAsync(user);

            //Check if  email changed

            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = user.EmailAddress
                };
                QueueFactory.SendMessageToExchange
                    (
                    exchangeName: SozlukConstants.UserExchangeName,
                    exchangeType: SozlukConstants.DefaultExchangeType,
                    queueName: SozlukConstants.UserEmailChangedQueueName,
                    obj: @event
                    );
                user.EmailConfirmed = false;
                await userRepository.UpdateAsync(user);

            }

            return user.Id;
        }
    }
}
