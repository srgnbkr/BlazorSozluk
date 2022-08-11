using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // user kontrolü yaptık.
            var existUser = await userRepository.GetSingleAsync(x => x.EmailAddress == request.EmailAddress);
            if (existUser is not null)
                throw new DatabaseValidationException("User already exists");

            //Yeni bir user nesnesi oluşturduk
            var user = mapper.Map<Domain.Models.User>(request);

            //Oluşturduğumuz nesneyi async olarak ekledil
            var rows = await userRepository.AddAsync(user);
            
            //Email changed/created
            if(rows > 0)
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


            }

            return user.Id;

            
        }

        
    }
}
